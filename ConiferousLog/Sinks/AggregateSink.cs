//
//  AggregateSink.cs
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
using System.Collections;
using System.Collections.Generic;
using ConiferousLog.Core;

namespace ConiferousLog.Sinks
{
	public sealed class AggregateSink : ISink, IEnumerable<ISink>
	{
		readonly List<ISink> _sinks;

		public Int32 Count { get { return _sinks.Count; } }

		public AggregateSink()
		{
			_sinks = new List<ISink>();
		}

		public AggregateSink(IEnumerable<ISink> sinks)
			: this()
		{
			if (sinks == null) {
				throw new ArgumentNullException(nameof(sinks));
			}
			_sinks.AddRange(sinks);
		}

		public void Add(ISink sink)
		{
			_sinks.Add(sink);
		}

		public void AddRange(IEnumerable<ISink> sinks)
		{
			_sinks.AddRange(sinks);
		}

		public void Absorb(LogMessage message)
		{
			foreach (ISink sink in _sinks) {
				sink.Absorb(message);
			}
		}

		public AggregateSink ForkAggregate(String childName)
		{
			AggregateSink fork = new AggregateSink();
			foreach (ISink sink in _sinks) {
				fork.Add(sink.Fork(childName));
			}
			return fork;
		}

		public ISink Fork(String childName)
		{
			return ForkAggregate(childName);
		}

		public IEnumerator<ISink> GetEnumerator()
		{
			return _sinks.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public override String ToString()
		{
			return $"{nameof(AggregateSink)}: [{Count:D}]";
		}
	}
}
