using UnityEngine;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace IsoTools.Tiled.Internal {
	class TiledMapPropertiesTests {

		[Test]
		public void Test00() {
			var props = new TiledMapProperties(null);
			Assert.AreEqual(props.Count, 0);
			Assert.False(props.Has(null));
			Assert.False(props.Has(string.Empty));
			Assert.False(props.Has("prop1"));
		}

		[Test]
		public void Test01() {
			var prop_list = new List<string>{
				"prop1", "val1",
				"prop2", "val2",
				"prop3", "val3",
				"prop4", "",
				"fakep"
			};
			var props = new TiledMapProperties(prop_list);
			Assert.AreEqual(props.Count, 4);

			Assert.True(props.Has("prop1"));
			Assert.True(props.Has("prop2"));
			Assert.True(props.Has("prop3"));
			Assert.True(props.Has("prop4"));

			Assert.False(props.Has(null));
			Assert.False(props.Has("val2"));
			Assert.False(props.Has("prop5"));
			Assert.False(props.Has(string.Empty));
		}

		[Test]
		public void Test02() {
			var props = new TiledMapProperties(null);
			Assert.Throws<UnityException>(() => { props.GetAsBool  (""); });
			Assert.Throws<UnityException>(() => { props.GetAsShort (""); });
			Assert.Throws<UnityException>(() => { props.GetAsInt   (""); });
			Assert.Throws<UnityException>(() => { props.GetAsLong  (""); });
			Assert.Throws<UnityException>(() => { props.GetAsFloat (""); });
			Assert.Throws<UnityException>(() => { props.GetAsDouble(""); });
			Assert.Throws<UnityException>(() => { props.GetAsString(""); });
		}

		[Test]
		public void Test03() {
			var props = new TiledMapProperties(null);
			Assert.Throws<IndexOutOfRangeException>(() => {
				props.GetKeyByIndex(0); });
			Assert.Throws<IndexOutOfRangeException>(() => {
				props.GetValueByIndex(0); });
			var prop_list = new List<string>{
				"prop1", "val1",
				"prop2", "val2",
				"prop3", "val3",
				"prop4", "",
				"fakep"
			};
			props = new TiledMapProperties(prop_list);
			Assert.AreEqual(props.GetKeyByIndex  (0), "prop1");
			Assert.AreEqual(props.GetKeyByIndex  (1), "prop2");
			Assert.AreEqual(props.GetValueByIndex(0), "val1");
			Assert.AreEqual(props.GetValueByIndex(1), "val2");
			Assert.Throws<IndexOutOfRangeException>(() => {
				props.GetKeyByIndex(-1); });
			Assert.Throws<IndexOutOfRangeException>(() => {
				props.GetValueByIndex(4); });
		}

		[Test]
		public void Test04() {
			var props = new TiledMapProperties(null);
			bool   v0;
			short  v1;
			int    v2;
			long   v3;
			float  v4;
			double v5;
			string v6;
			Assert.False(props.TryGetAsBool  ("", out v0));
			Assert.False(props.TryGetAsShort ("", out v1));
			Assert.False(props.TryGetAsInt   ("", out v2));
			Assert.False(props.TryGetAsLong  ("", out v3));
			Assert.False(props.TryGetAsFloat ("", out v4));
			Assert.False(props.TryGetAsDouble("", out v5));
			Assert.False(props.TryGetAsString("", out v6));
		}

		[Test]
		public void Test05() {
			var prop_list = new List<string>{
				"bool"   , "true",
				"short"  , "64",
				"int"    , "128",
				"long"   , "1024",
				"float"  , "1.2",
				"double" , "1.23",
				"string1", "hello",
				"string2", ""
			};
			var props = new TiledMapProperties(prop_list);

			bool   v0;
			short  v1;
			int    v2;
			long   v3;
			float  v4;
			double v5;
			string v6;
			string v7;

			Assert.True(props.TryGetAsBool  ("bool"   , out v0));
			Assert.True(props.TryGetAsShort ("short"  , out v1));
			Assert.True(props.TryGetAsInt   ("int"    , out v2));
			Assert.True(props.TryGetAsLong  ("long"   , out v3));
			Assert.True(props.TryGetAsFloat ("float"  , out v4));
			Assert.True(props.TryGetAsDouble("double" , out v5));
			Assert.True(props.TryGetAsString("string1", out v6));
			Assert.True(props.TryGetAsString("string2", out v7));

			Assert.AreEqual(v0, true);
			Assert.AreEqual(v1, 64);
			Assert.AreEqual(v2, 128);
			Assert.AreEqual(v3, 1024);
			Assert.AreEqual(v4, 1.2f);
			Assert.AreEqual(v5, 1.23);
			Assert.AreEqual(v6, "hello");
			Assert.AreEqual(v7, "");

			Assert.AreEqual(props.GetAsBool  ("bool"   ), v0);
			Assert.AreEqual(props.GetAsShort ("short"  ), v1);
			Assert.AreEqual(props.GetAsInt   ("int"    ), v2);
			Assert.AreEqual(props.GetAsLong  ("long"   ), v3);
			Assert.AreEqual(props.GetAsFloat ("float"  ), v4);
			Assert.AreEqual(props.GetAsDouble("double" ), v5);
			Assert.AreEqual(props.GetAsString("string1"), v6);
			Assert.AreEqual(props.GetAsString("string2"), v7);
		}
	}
}
