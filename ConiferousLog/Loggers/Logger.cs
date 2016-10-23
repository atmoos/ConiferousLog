//
//  Logger.cs
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
using ConiferousLog.Sinks;
using static ConiferousLog.Internals.Validation;

namespace ConiferousLog.Loggers
{
	internal sealed class Logger : ILogger
	{
		readonly LogSinks _sinks;
		readonly Info _info;
		readonly Error _error;
		readonly Warning _warning;
		readonly Verbose _verbose;
		readonly IMessageFormatter _formatter;

		internal Logger(LogSinks sinks, IMessageFormatter formatter)
		{
			_sinks = IsNotNull(sinks, nameof(sinks));
			_formatter = IsNotNull(formatter, nameof(formatter));
			_info = MakeLevel(sinks.InfoSink, s => new Info(s, _formatter));
			_error = MakeLevel(sinks.ErrorSink, s => new Error(s, _formatter));
			_warning = MakeLevel(sinks.WarningSink, s => new Warning(s, _formatter));
			_verbose = MakeLevel(sinks.VerboseSink, s => new Verbose(s, _formatter));
		}

		public void Trace(Action<Verbose> level)
		{
			Trace(level, _verbose);
		}

		public void Trace(Action<Info> level)
		{
			Trace(level, _info);
		}

		public void Trace(Action<Warning> level)
		{
			Trace(level, _warning);
		}

		public void Trace(Action<Error> level)
		{
			Trace(level, _error);
		}

		public ILogger Fork(string childName)
		{
			return new Logger(_sinks.Fork(childName), _formatter);
		}

		private static void Trace<TLevel>(Action<TLevel> log, TLevel level)
			where TLevel : Level
		{
			// if log is null, we assume the client has a programming error
			IsNotNull(log, nameof(log));
			if (level != null) {
				log(level);
			}
		}

		private static TLevel MakeLevel<TLevel>(AggregateSink sink, Func<ISink, TLevel> generate)
			where TLevel : Level
		{
			if (sink == null || sink.Count <= 0) {
				return null;
			}
			return generate(sink);
		}
	}
}
