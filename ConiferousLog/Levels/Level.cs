//
//  Level.cs
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
using ConiferousLog.Core;
using ConiferousLog.Formatters;
using static ConiferousLog.Internals.Validation;

namespace ConiferousLog.Levels
{
	public abstract class Level
	{
		readonly ISink _sink;
		readonly LogLevel _level;
		readonly IMessageFormatter _formatter;

		internal Level(LogLevel level, ISink sink, IMessageFormatter formatter)
		{
			_level = level;
			_sink = IsNotNull(sink, nameof(sink));
			_formatter = IsNotNull(formatter, nameof(formatter));
		}

		internal Level(LogLevel level, ISink sink)
			: this(level, sink, new DefaultFormatter(DateTime.UtcNow))
		{
		}

		public void Push(String text)
		{
			Absorb(DateTime.UtcNow, text);
		}

		public void Push<TFirst>(String textFormat, TFirst first)
		{
			Absorb(_formatter.TimeStamp, _formatter.Format(textFormat, first));
		}

		public void Push<TFirst, TSecond>(String textFormat, TFirst first, TSecond second)
		{
			Absorb(_formatter.TimeStamp, _formatter.Format(textFormat, first, second));
		}

		public void Push<TFirst, TSecond, TThird>(String textFormat, TFirst first, TSecond second, TThird third)
		{
			Absorb(_formatter.TimeStamp, _formatter.Format(textFormat, first, second, third));
		}

		public void Push(String textFormat, params Object[] parameters)
		{
			Absorb(_formatter.TimeStamp, _formatter.Format(textFormat, parameters));
		}

		protected void PushImpl(Exception e)
		{
			Absorb(_formatter.TimeStamp, _formatter.Format(e));
		}

		private void Absorb(DateTime timeStamp, String message)
		{
			_sink.Absorb(new LogMessage(_level, timeStamp, message));
		}
	}
}
