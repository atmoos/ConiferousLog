//
//  Verbose.cs
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

using ConiferousLog.Core;

namespace ConiferousLog.Levels
{
	public sealed class Verbose : Level
	{
		public Verbose(ISink sink, IMessageFormatter formatter)
			: base(LogLevel.Verbose, sink, formatter)
		{
		}

		public Verbose(ISink sink)
			: base(LogLevel.Verbose, sink)
		{
		}
	}
}
