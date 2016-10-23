//
//  Validation.cs
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

namespace ConiferousLog.Internals
{
	public static class Validation
	{
		public static TClass IsNotNull<TClass>(TClass instance, Func<TClass> defaultCreator)
			where TClass : class
		{
			if (instance != null) {
				return instance;
			}
			return IsNotNull(IsNotNull(defaultCreator, nameof(defaultCreator))(), "defaultValue");
		}

		public static TClass IsNotNull<TClass>(TClass instance, String paramName = null)
			where TClass : class
		{
			if (instance != null) {
				return instance;
			}
			paramName = paramName ?? nameof(instance);
			String msg = String.Format("Parameter {0} of type {1} may not be null.", paramName, typeof(TClass).Name);
			throw new ArgumentNullException(paramName, msg);
		}

		public static String IsNotNullOrEmpty(String instance, String paramName = null)
		{
			if (String.IsNullOrEmpty(instance)) {
				paramName = paramName ?? nameof(instance);
				String msg = String.Format("String parameter {0} may not be null nor empty.", paramName);
				if (instance == null) {
					throw new ArgumentNullException(paramName, msg);
				}
				throw new ArgumentOutOfRangeException(paramName, msg);
			}
			return instance;
		}

		public static String IsNotNullOrWhiteSpace(String instance, String paramName = null)
		{
			if (String.IsNullOrWhiteSpace(instance)) {
				paramName = paramName ?? nameof(instance);
				String msg = String.Format("String parameter {0} may not be null, empty nor consist entirely of whitespace.", paramName);
				if (instance == null) {
					throw new ArgumentNullException(paramName, msg);
				}
				throw new ArgumentOutOfRangeException(paramName, msg);
			}
			return instance;
		}
	}
}
