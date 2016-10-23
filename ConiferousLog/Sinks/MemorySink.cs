//
//  MemorySink.cs
//
//  This file is part of ConiferousLog
//
//  Author: Thomas Kägi <kaegit@gmail.com>
//
//  Copyright (c) 2016 Thomas Kägi
//
//  ConiferousLog is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  ConiferousLog is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with ConiferousLog.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using ConiferousLog.Core;
using static ConiferousLog.Internals.Validation;

namespace ConiferousLog.Sinks
{
	public sealed class MemorySink : ISink, IEnumerable<LogMessage>
	{
		public String Name { get; }

		private readonly List<LogMessage> _messages;
		private readonly List<MemorySink> _children;

		public Int32 Count => _messages.Count + _children.Sum(c => c.Count);

		public MemorySink(String name)
		{
			Name = IsNotNullOrWhiteSpace(name, nameof(name));
			_messages = new List<LogMessage>();
			_children = new List<MemorySink>();
		}

		public void Absorb(LogMessage message)
		{
			_messages.Add(message);
		}

		public ISink Fork(string childName)
		{
			String name = String.Format("{0}.{1}", Name, IsNotNullOrWhiteSpace(childName, nameof(childName)));
			MemorySink child = new MemorySink(name);
			_children.Add(child);
			return child;
		}

		public IEnumerator<LogMessage> GetEnumerator()
		{
			List<LogMessage> allMessages = new List<LogMessage>();
			Populate(ref allMessages);
			allMessages.Sort((l, r) => l.TimeStamp.CompareTo(r.TimeStamp));
			return allMessages.GetEnumerator();
		}

		private void Populate(ref List<LogMessage> allMessages)
		{
			allMessages.AddRange(_messages);
			foreach (MemorySink child in _children) {
				child.Populate(ref allMessages);
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public override String ToString()
		{
			return $"{nameof(MemorySink)}: <{Name}> [{Count:D}]";
		}
	}
}
