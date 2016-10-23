//
//  ConsoleSink.cs
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
using System.Globalization;
using System.Collections.Generic;
using ConiferousLog.Core;
using static ConiferousLog.Internals.Validation;

namespace ConiferousLog.Sinks
{
	public sealed class ConsoleSink : ISink, IEquatable<ConsoleSink>
	{
		const String SEPARATOR = ">";
		static DateTime _today;
		static readonly ConsoleColor _dateColour;
		static readonly Dictionary<LogLevel, ConsoleColor> _configs;

		readonly String _name;
		readonly CultureInfo _culture;

		static ConsoleSink()
		{
			_today = DateTime.UtcNow.Date;
			_dateColour = ConsoleColor.Blue;
			_configs = new Dictionary<LogLevel, ConsoleColor> {
				[LogLevel.Verbose] = ConsoleColor.Cyan,
				[LogLevel.Info] = ConsoleColor.Green,
				[LogLevel.Warning] = ConsoleColor.DarkYellow,
				[LogLevel.Error] = ConsoleColor.Red
			};
			Console.WriteLine("Log Levels:");
			foreach (var pair in _configs) {
				Console.Write(" {0} ", pair.Key);
				Console.ForegroundColor = pair.Value;
				Console.CursorLeft = 12;
				Console.WriteLine(pair.Value);
				Console.ResetColor();
			}
			Console.Write("Today is: ");
			Console.CursorLeft = 12;
			Console.ForegroundColor = _dateColour;
			Console.WriteLine(_today.ToString("d", CultureInfo.CurrentCulture));
			Console.ResetColor();
		}

		public ConsoleSink(String name, CultureInfo culture)
		{
			_name = IsNotNullOrWhiteSpace(name, nameof(name));
			_culture = IsNotNull(culture, nameof(culture));
		}

		public ConsoleSink(String name)
			: this(name, CultureInfo.InvariantCulture)
		{
			// empty :-)
		}

		public void Absorb(LogMessage message)
		{
			lock (_name) {
				UpdateDate(message.TimeStamp, _culture);
				ConsoleColor colour = _configs[message.LogLevel];
				Console.Write(_name);
				Console.WriteLine(SEPARATOR);
				Console.ForegroundColor = colour;
				Console.Write(message.TimeStamp.ToString("T", _culture));
				Console.ResetColor();
				Console.Write(" ");
				Console.WriteLine(message.Text);
			}
		}

		public ISink Fork(String childName)
		{
			return new ConsoleSink($"{_name}{SEPARATOR}{childName}", _culture);
		}

		public static void UpdateDate(DateTime newDate, CultureInfo culture)
		{
			if (_today == newDate.Date) {
				return;
			}
			Console.ForegroundColor = _dateColour;
			Console.WriteLine("[{0} -> {1}]", _today.ToString("d", culture), newDate.ToString("d", culture));
			Console.ResetColor();
			_today = newDate.Date;

		}

		public override Boolean Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return ReferenceEquals(this, obj) || Equals(obj as ConsoleSink);
		}

		public Boolean Equals(ConsoleSink other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return _name.Equals(other._name) && _culture.Equals(other._culture);
		}

		public override int GetHashCode()
		{
			unchecked {
				return _name.GetHashCode() ^ 1621 + _culture.GetHashCode();
			}
		}

		public override String ToString()
		{
			return $"{nameof(ConsoleSink)}: {_name}";
		}

	}
}
