//
//  EmptyLogger.cs
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
using ConiferousLog.Levels;

namespace ConiferousLog.Loggers
{
	public sealed class EmptyLogger : ILogger
	{
		public void Trace(Action<Verbose> level)
		{
		}

		public void Trace(Action<Info> level)
		{
		}

		public void Trace(Action<Warning> level)
		{
		}

		public void Trace(Action<Error> level)
		{
		}

		public ILogger Fork(string childName)
		{
			return this;
		}
	}
}
