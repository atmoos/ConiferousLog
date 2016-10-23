//
//  LoggerTest.cs
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
using NUnit.Framework;
using ConiferousLog.Core;
using ConiferousLog.Sinks;
using ConiferousLog.Levels;
using ConiferousLog.Loggers;

namespace ConiferousLogTest.Loggers
{
	[TestFixture]
	[TestOf(typeof(LoggerGenerator))]
	public class LoggerTest
	{
		ILogger _target;

		[SetUp]
		public void SetUp()
		{
			_target = new LoggerGenerator().SetBroadband(new MemorySink("sink")).Generate();
		}

		[Test]
		public void Trace_OnLambdaThatThrows_PropagatesTheExceptionUp()
		{
			Assert.Throws<ArgumentException>(() => _target.Trace((Info i) => { throw new ArgumentException(); }));
		}

		[Test]
		public void Trace_OnNullLambdaAndActiveLevel_ThrowsArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() => _target.Trace(null as Action<Info>));
		}

		[Test]
		public void Trace_OnNullLambdaAndInactiveLevel_ThrowsArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() => _target.Trace(null as Action<Verbose>));
		}
	}
}
