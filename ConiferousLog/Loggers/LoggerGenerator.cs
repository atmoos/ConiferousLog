//
//  LoggerGenerator.cs
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
using System.Collections.Generic;
using ConiferousLog.Core;
using ConiferousLog.Formatters;
using static ConiferousLog.Internals.Validation;

namespace ConiferousLog.Loggers
{
	/// <summary>Creates a configurable <see cref="ILogger"/>.</summary>
	public sealed class LoggerGenerator
	{
		LogLevel _minimumLevel;
		IMessageFormatter _formatter;
		readonly Dictionary<LogLevel, List<ISink>> _sinks;

		public LoggerGenerator()
		{
			_minimumLevel = LogLevel.Info;
			_sinks = new Dictionary<LogLevel, List<ISink>> {
				[LogLevel.Verbose] = new List<ISink>(),
				[LogLevel.Info] = new List<ISink>(),
				[LogLevel.Warning] = new List<ISink>(),
				[LogLevel.Error] = new List<ISink>(),
			};
			_formatter = new DefaultFormatter(DateTime.UtcNow);
		}

		/// <summary>The minimum log level from which log messages are
		/// to be written.</summary>
		public LoggerGenerator SetMinumumLevel(LogLevel minLevel)
		{
			_minimumLevel = minLevel;
			return this;
		}

		/// <summary>Configures a sink to log over all levels.</summary>
		/// <remarks>Naturally the minimum log level is respected.</remarks>
		public LoggerGenerator SetBroadband(ISink sink)
		{
			IsNotNull(sink, nameof(sink));
			foreach (List<ISink> levelSinks in _sinks.Values) {
				CheckedInsert(sink, levelSinks);
			}
			return this;
		}

		/// <summary>Sets the formatter that formats the raw messages.</summary>
		public LoggerGenerator SetFormatter(IMessageFormatter formatter)
		{
			_formatter = IsNotNull(formatter, nameof(formatter));
			return this;
		}

		/// <summary>Sets the listed sinks to log on the specified <see cref="LogLevel"/> level.</summary>
		public LoggerGenerator SetLevelSinks(LogLevel logLevel, params ISink[] verboseSinks)
		{
			CheckedMerge(verboseSinks, _sinks[logLevel]);
			return this;
		}


		/// <summary>Generate a <see cref="ILogger">logger</see> from the given configuration.</summary>
		/// <exception cref="InvalidOperationException">When no sinks are configured.</exception>
		public ILogger Generate()
		{
			List<ISink> warningSinks = new List<ISink>();
			List<ISink> infoSinks = new List<ISink>();
			List<ISink> verboseSinks = new List<ISink>();
			if (_minimumLevel <= LogLevel.Verbose) {
				verboseSinks.AddRange(_sinks[LogLevel.Verbose]);
			}
			if (_minimumLevel <= LogLevel.Info) {
				infoSinks.AddRange(_sinks[LogLevel.Info]);
				CheckedMerge(verboseSinks, infoSinks);
			}
			if (_minimumLevel <= LogLevel.Warning) {
				warningSinks.AddRange(_sinks[LogLevel.Warning]);
				CheckedMerge(infoSinks, warningSinks);
			}
			List<ISink> errorSinks = new List<ISink>(_sinks[LogLevel.Error]);
			CheckedMerge(warningSinks, errorSinks);
			if (errorSinks.Count == 0) {
				const String msg = "No sinks have been configured for the logger. Please add at least one.";
				throw new InvalidOperationException(msg);
			}
			LogSinks sinks = new LogSinks();
			sinks.ErrorSink.AddRange(errorSinks);
			sinks.WarningSink.AddRange(warningSinks);
			sinks.InfoSink.AddRange(infoSinks);
			sinks.VerboseSink.AddRange(verboseSinks);
			return new Logger(sinks, _formatter);
		}

		static void CheckedMerge(IEnumerable<ISink> source, List<ISink> destination)
		{
			foreach (ISink sink in source) {
				CheckedInsert(sink, destination);
			}
		}

		static void CheckedInsert(ISink sink, List<ISink> destination)
		{
			if (!destination.Contains(sink)) {
				destination.Add(sink);
			}
		}
	}
}
