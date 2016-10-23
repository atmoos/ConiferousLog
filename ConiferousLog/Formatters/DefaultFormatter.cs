//
//  DefaultFormatter.cs
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
using System.Diagnostics;
using System.Globalization;
using ConiferousLog.Core;

namespace ConiferousLog.Formatters
{
	public sealed class DefaultFormatter : IMessageFormatter
	{
		private static readonly CultureInfo CULTURE = CultureInfo.InvariantCulture;

		private readonly DateTime _origin;
		private readonly CultureInfo _culture;
		private readonly Stopwatch _timeKeeper;

		public DateTime TimeStamp => _origin.Add(_timeKeeper.Elapsed);

		public DefaultFormatter(DateTime origin, CultureInfo culture = null)
		{
			_origin = origin;
			_timeKeeper = Stopwatch.StartNew();
			_culture = culture ?? CULTURE;
		}

		public String Format(Exception e)
		{
			return e != null ? e.ToString() : "Null";
		}

		public String Format<TFirst>(String textFormat, TFirst first)
		{
			return String.Format(_culture, textFormat, first);
		}

		public String Format<TFirst, TSecond>(String textFormat, TFirst first, TSecond second)
		{
			return String.Format(_culture, textFormat, first, second);
		}

		public String Format<TFirst, TSecond, TThird>(String textFormat, TFirst first, TSecond second, TThird third)
		{
			return String.Format(_culture, textFormat, first, second, third);
		}

		public String Format(String textFormat, params Object[] parameters)
		{
			return String.Format(_culture, textFormat, parameters);
		}
	}
}
