//
//  LogMessage.cs
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
namespace ConiferousLog.Core
{
	/// <summary>Message to be absorbed by message <see cref="ISink">sinks</see>.</summary>
	public sealed class LogMessage
	{
		static readonly CultureInfo CULTURE = CultureInfo.InvariantCulture;

		public LogLevel LogLevel { get; }
		public DateTime TimeStamp { get; }
		public String Text { get; }

		public LogMessage(LogLevel level, DateTime timeStamp, String text)
		{
			LogLevel = level;
			TimeStamp = timeStamp;
			Text = text ?? "no text";
		}

		public override String ToString()
		{
			const Int32 maxDisplayLength = 24;
			const Int32 shortenedLength = maxDisplayLength - 3;
			String shortendText = Text;
			if (shortendText.Length > maxDisplayLength) {
				shortendText = $"{shortendText.Substring(0, shortenedLength)}...";
			}
			return String.Format(CULTURE, "[{0}-({1:u}): {2}]", LogLevel, TimeStamp, shortendText);
		}
	}
}
