//
//  MemorySinkTest.cs
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

namespace ConiferousLogTest.Sinks
{
	[TestFixture]
	[TestOf(typeof(MemorySink))]
	public class MemorySinkTest
	{
		[Test]
		public void Create_WithValidName_CreatesInstanceWithSameName()
		{
			String expectedName = "foo";
			MemorySink sink = new MemorySink(expectedName);
			Assert.AreSame(expectedName, sink.Name);
		}
		[Test]
		public void Create_WithValidName_CreatesInstanceWithCountEqualToZero()
		{
			MemorySink sink = new MemorySink("bar");
			Assert.AreEqual(0, sink.Count);
		}

		[Test]
		public void Fork_OnParent_CreatesChildWithConcatenatedNames()
		{
			MemorySink parent = new MemorySink("foo");
			MemorySink child = (MemorySink)parent.Fork("bar");
			Assert.AreEqual("foo.bar", child.Name);
		}


		[Test]
		public void Fork_OnNonEmptyParent_CreatesChildWithZeroCount()
		{
			MemorySink parent = new MemorySink("parent");
			parent.Absorb(new LogMessage(LogLevel.Info, DateTime.Now, "qux"));
			MemorySink child = (MemorySink)parent.Fork("child");
			Assert.AreEqual(0, child.Count);
		}

		[Test]
		public void Absorb_OnEmptyInstance_SetsCountToOne()
		{
			MemorySink parent = new MemorySink("parent");
			parent.Absorb(new LogMessage(LogLevel.Info, DateTime.Now, "qux"));
			Assert.AreEqual(1, parent.Count);
		}

		[Test]
		public void Absorb_OnChildInstance_IncreasesParentCountByOne()
		{
			MemorySink parent = new MemorySink("parent");
			parent.Absorb(new LogMessage(LogLevel.Info, DateTime.Now, "qux"));
			Int32 initialCount = parent.Count;
			ISink child = parent.Fork("child");
			child.Absorb(new LogMessage(LogLevel.Error, DateTime.Now, "bit"));
			Assert.AreEqual(initialCount + 1, parent.Count);
		}

		[Test]
		public void Iteration_OnParentWithForkedChildren_HappendInSortedTimeOrder()
		{
			DateTime t0 = DateTime.Now;
			DateTime t1 = t0.AddSeconds(1.2);
			DateTime t2 = t1.AddSeconds(0.2);
			DateTime t3 = t2.AddSeconds(3.8);
			DateTime t4 = t3.AddSeconds(1.7);
			MemorySink parent = new MemorySink("parent");
			ISink childA = parent.Fork("childA");
			ISink childB = parent.Fork("childB");
			parent.Absorb(new LogMessage(LogLevel.Info, t0, "t0"));
			childA.Absorb(new LogMessage(LogLevel.Warning, t1, "t1"));
			childB.Absorb(new LogMessage(LogLevel.Warning, t2, "t2"));
			childA.Absorb(new LogMessage(LogLevel.Warning, t3, "t3"));
			parent.Absorb(new LogMessage(LogLevel.Warning, t4, "t4"));
			DateTime[] expectedTimeStamps = new[] { t0, t1, t2, t3, t4 };
			DateTime[] actualTimeStamps = parent.Select(log => log.TimeStamp).ToArray();
			CollectionAssert.AreEqual(expectedTimeStamps, actualTimeStamps);
		}
	}
}
