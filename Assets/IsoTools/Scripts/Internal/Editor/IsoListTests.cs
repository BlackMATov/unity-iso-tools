using System;
using NUnit.Framework;

namespace IsoTools.Internal {
	class IsoListTests {

		[Test]
		public void Test00() {
			var list1 = new IsoList<int>();
			Assert.AreEqual(list1.Count, 0);
			Assert.AreEqual(list1.Capacity, 0);

			var list2 = new IsoList<int>(0);
			Assert.AreEqual(list2.Count, 0);
			Assert.AreEqual(list2.Capacity, 0);

			var list3 = new IsoList<int>(1);
			Assert.AreEqual(list3.Count, 0);
			Assert.AreEqual(list3.Capacity, 1);

			var list4 = new IsoList<int>();
			list4.Push(1);
			Assert.AreEqual(list4.Count, 1);
			Assert.AreEqual(list4.Capacity, 4);
		}

		[Test]
		public void Test01() {
			var list1 = new IsoList<int>(1);
			list1.Push(1);
			Assert.AreEqual(list1.Count, 1);
			Assert.AreEqual(list1.Capacity, 1);
			Assert.AreEqual(list1[0], 1);
			list1.Push(2);
			Assert.AreEqual(list1.Count, 2);
			Assert.AreEqual(list1.Capacity, 2);
			Assert.AreEqual(list1[0], 1);
			Assert.AreEqual(list1[1], 2);
			list1.Push(3);
			Assert.AreEqual(list1.Count, 3);
			Assert.AreEqual(list1.Capacity, 4);
			Assert.AreEqual(list1[0], 1);
			Assert.AreEqual(list1[1], 2);
			Assert.AreEqual(list1[2], 3);
		}

		[Test]
		public void Test02() {
			var list1 = new IsoList<int>();
			list1.Push(1);
			list1.Push(2);
			list1.Push(3);
			Assert.AreEqual(list1.Peek(), 3);
			Assert.AreEqual(list1.Pop(), 3);
			Assert.AreEqual(list1.Count, 2);
			Assert.AreEqual(list1.Capacity, 4);
			Assert.AreEqual(list1[0], 1);
			Assert.AreEqual(list1[1], 2);
			Assert.AreEqual(list1.Peek(), 2);
			Assert.AreEqual(list1.Pop(), 2);
			Assert.AreEqual(list1.Count, 1);
			Assert.AreEqual(list1.Capacity, 4);
			Assert.AreEqual(list1[0], 1);
			Assert.AreEqual(list1.Peek(), 1);
			Assert.AreEqual(list1.Pop(), 1);
			Assert.AreEqual(list1.Count, 0);
			Assert.AreEqual(list1.Capacity, 4);
		}

		[Test]
		public void Test03() {
			var list1 = new IsoList<int>();
			list1.Push(1);
			list1.Push(2);
			list1.Push(3);
			list1.Clear();
			Assert.AreEqual(list1.Count, 0);
			Assert.AreEqual(list1.Capacity, 4);
		}

		[Test]
		public void Test04() {
			var list1 = new IsoList<int>();
			list1.Push(1);
			list1.Push(2);
			list1.Push(3);
			list1.Push(4);
			Assert.AreEqual(list1.UnorderedRemoveAt(1), 4);
			Assert.AreEqual(list1[0], 1);
			Assert.AreEqual(list1[1], 4);
			Assert.AreEqual(list1[2], 3);
			Assert.AreEqual(list1.Count, 3);
			Assert.AreEqual(list1.Capacity, 4);
			Assert.AreEqual(list1.UnorderedRemoveAt(0), 3);
			Assert.AreEqual(list1[0], 3);
			Assert.AreEqual(list1[1], 4);
			Assert.AreEqual(list1.Count, 2);
			Assert.AreEqual(list1.Capacity, 4);
			Assert.AreEqual(list1.UnorderedRemoveAt(1), 4);
			Assert.AreEqual(list1[0], 3);
			Assert.AreEqual(list1.Count, 1);
			Assert.AreEqual(list1.Capacity, 4);
			Assert.AreEqual(list1.UnorderedRemoveAt(0), 3);
			Assert.AreEqual(list1.Count, 0);
			Assert.AreEqual(list1.Capacity, 4);
		}

		[Test]
		public void Test05() {
			var list = new IsoList<int>();
			list.Capacity = 10;
			Assert.AreEqual(list.Count, 0);
			Assert.AreEqual(list.Capacity, 10);
			list.Push(1);
			list.Push(2);
			list.Capacity = 20;
			Assert.AreEqual(list[0], 1);
			Assert.AreEqual(list[1], 2);
			Assert.AreEqual(list.Count, 2);
			Assert.AreEqual(list.Capacity, 20);
			list.Capacity = 2;
			Assert.AreEqual(list[0], 1);
			Assert.AreEqual(list[1], 2);
			Assert.AreEqual(list.Count, 2);
			Assert.AreEqual(list.Capacity, 2);
		}

		[Test]
		public void Test06() {
			Assert.Throws<ArgumentOutOfRangeException>(() => {
				new IsoList<int>(-1);
			});
			Assert.Throws<InvalidOperationException>(() => {
				var list = new IsoList<int>();
				list.Pop();
			});
			Assert.Throws<InvalidOperationException>(() => {
				var list = new IsoList<int>();
				list.Peek();
			});
			Assert.Throws<IndexOutOfRangeException>(() => {
				var list = new IsoList<int>();
				list.UnorderedRemoveAt(0);
			});
			Assert.Throws<IndexOutOfRangeException>(() => {
				var list = new IsoList<int>();
				Assert.AreEqual(list[0], 100500);
			});
			Assert.Throws<ArgumentOutOfRangeException>(() => {
				var list = new IsoList<int>();
				list.Push(1);
				list.Push(2);
				list.Capacity = 1;
			});
		}
	}
}
