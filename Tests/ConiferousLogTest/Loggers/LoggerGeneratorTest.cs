//
//  LoggerGeneratorTest.cs
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
using NUnit.Framework;
using ConiferousLog.Core;
using ConiferousLog.Sinks;
using ConiferousLog.Levels;
using ConiferousLog.Loggers;

namespace ConiferousLogTest.Loggers
{
	[TestFixture]
	[TestOf(typeof(LoggerGenerator))]
	public class LoggerGeneratorTest
	{
		[Test]
		public void Generate_OnUnconfiguredGenerator_Throws()
		{
			LoggerGenerator gen = new LoggerGenerator();
			Assert.Throws<InvalidOperationException>(() => gen.Generate());
		}

		[Test]
		public void SetBroadband_WhenMinimumLevelSetToVerbose_LogsAllMessages()
		{
			BroadMinLevelTestOnSingleSink(LogLevel.Verbose);
		}


		[Test]
		public void SetBroadband_WhenMinimumLevelSetToInfo_LogsAllButVerbose()
		{
			BroadMinLevelTestOnSingleSink(LogLevel.Info);
		}

		[Test]
		public void SetBroadband_WhenMinimumLevelSetToWarning_LogsAllWarningAndError()
		{
			BroadMinLevelTestOnSingleSink(LogLevel.Warning);
		}


		[Test]
		public void SetBroadband_WhenMinimumLevelSetToError_LogsOnlyError()
		{
			BroadMinLevelTestOnSingleSink(LogLevel.Error);
		}

		[Test]
		public void SetAllDedicated_WhenMinimumLevelSetToVerbose_LogsAllMessages()
		{
			SetAllDedicatedSinksMinLevelTest(LogLevel.Verbose);
		}

		[Test]
		public void SetAllDedicated_WhenMinimumLevelSetToInfo_LogsAllButVerbose()
		{
			SetAllDedicatedSinksMinLevelTest(LogLevel.Info);
		}

		[Test]
		public void SetAllDedicated_WhenMinimumLevelSetToWarning_LogsAllWarningAndError()
		{
			SetAllDedicatedSinksMinLevelTest(LogLevel.Warning);
		}

		[Test]
		public void SetAllDedicated_WhenMinimumLevelSetToError_LogsOnlyError()
		{
			SetAllDedicatedSinksMinLevelTest(LogLevel.Error);
		}

		[Test]
		public void SetOnlyOneDedicatedLevelVerbose_OnMinimumLevelSetToVerbose_IgnoresAllOthers()
		{
			SetDedicatedLevelIgnoresAllOthers(LogLevel.Verbose);
		}

		[Test]
		public void SetOnlyOneDedicatedLevelInfo_OnMinimumLevelSetToVerbose_IgnoresAllOthers()
		{
			SetDedicatedLevelIgnoresAllOthers(LogLevel.Info);
		}

		[Test]
		public void SetOnlyOneDedicatedLevelWarning_OnMinimumLevelSetToVerbose_IgnoresAllOthers()
		{
			SetDedicatedLevelIgnoresAllOthers(LogLevel.Warning);
		}

		[Test]
		public void SetOnlyOneDedicatedLevelError_OnMinimumLevelSetToVerbose_IgnoresAllOthers()
		{
			SetDedicatedLevelIgnoresAllOthers(LogLevel.Error);
		}

		private static void BroadMinLevelTestOnSingleSink(LogLevel minLevel)
		{
			Int32 expectedMsgCount = 1 + LogLevel.Error - minLevel;
			MemorySink sink = new MemorySink("sink");
			LoggerGenerator gen = new LoggerGenerator();
			gen.SetMinimumLevel(minLevel);
			EmitAll(gen.SetBroadband(sink).Generate());
			Assert.AreEqual(expectedMsgCount, sink.Count);
		}

		private static void SetAllDedicatedSinksMinLevelTest(LogLevel minLevel)
		{
			MemorySink verboseSink = new MemorySink("verboseSink");
			MemorySink infoSink = new MemorySink("infoSink");
			MemorySink warningSink = new MemorySink("warningSink");
			MemorySink errorSink = new MemorySink("errorSink");
			LoggerGenerator gen = new LoggerGenerator().SetMinimumLevel(minLevel)
													   .SetLevelSinks(LogLevel.Verbose, verboseSink)
													   .SetLevelSinks(LogLevel.Info, infoSink)
													   .SetLevelSinks(LogLevel.Warning, warningSink)
													   .SetLevelSinks(LogLevel.Error, errorSink);
			EmitAll(gen.Generate());
			Int32 expectedCount = 4 - (minLevel - LogLevel.Verbose);
			Assert.AreEqual(LogLevel.Verbose >= minLevel ? expectedCount-- : 0, verboseSink.Count);
			Assert.AreEqual(LogLevel.Info >= minLevel ? expectedCount-- : 0, infoSink.Count);
			Assert.AreEqual(LogLevel.Warning >= minLevel ? expectedCount-- : 0, warningSink.Count);
			Assert.AreEqual(expectedCount, errorSink.Count);
		}

		private static void SetDedicatedLevelIgnoresAllOthers(LogLevel expectedLevel)
		{

			MemorySink sink = new MemorySink("sink");
			LoggerGenerator gen = new LoggerGenerator().SetMinimumLevel(LogLevel.Verbose)
													   .SetLevelSinks(expectedLevel, sink);
			EmitAll(gen.Generate());
			Assert.AreEqual(expectedLevel, sink.First().LogLevel);
		}

		private static void EmitAll(ILogger logger)
		{
			logger.Trace((Verbose v) => v.Push("foo"));
			logger.Trace((Info i) => i.Push("bar"));
			logger.Trace((Warning w) => w.Push("qux"));
			logger.Trace((Error e) => e.Push("baz"));
		}
	}
}
