namespace JsonPointerTests
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json.Linq;
    using JsonPointer;

    [TestClass]
    public class JsonPointerTests
    {
        [TestMethod]

        [TestCategory("Unit")]
        public void SetObjectSubPropertiesDoesNotExist()
        {
            var payload = GetPocoObject();
            payload = JsonPointer.Set(payload, "/Settings/Wait", false);
            Assert.AreEqual(false, JsonPointer.Get<bool>(payload, "/Settings/Wait"));
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ExpectedException(typeof(ArgumentException))]
        public void CannotSetRealObjectSubPropertiesThatDoNotExist()
        {
            var payload = GetTestObject();
            payload = JsonPointer.Set(payload, "/Settings/Wait", false);
            Assert.AreEqual(false, JsonPointer.Get<bool>(payload, "/Settings/Wait"));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void SetObjectSubProperties()
        {
            var payload = GetPocoObject();
            payload = JsonPointer.Set(payload, "/Settings/Launch", false);
            Assert.AreEqual(false, JsonPointer.Get<bool>(payload, "/Settings/Launch"));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void SetObjectCollectionElement()
        {
            var payload = GetPocoObject();
            payload = JsonPointer.Set(payload, "/Collection/1", "last");
            Assert.AreEqual("last", JsonPointer.Get<string>(payload, "/Collection/1"));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void SetAddObjectCollectionElement()
        {
            var payload = GetPocoObject();
            payload = JsonPointer.Set(payload, "/Collection/-", "last");
            Assert.AreEqual("last", JsonPointer.Get<string>(payload, "/Collection/3"));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void SetTestObjectSubProperties()
        {
            var payload = GetTestObject();
            payload = JsonPointer.Set(payload, "/Settings/Launch", false);
            Assert.AreEqual(false, JsonPointer.Get<bool>(payload, "/Settings/Launch"));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void SetTestObjectCollectionElement()
        {
            var payload = GetTestObject();
            payload = JsonPointer.Set(payload, "/Collection/1", "last");
            Assert.AreEqual("last", JsonPointer.Get<string>(payload, "/Collection/1"));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void SetAddTestObjectCollectionElement()
        {
            var payload = GetTestObject();
            payload = JsonPointer.Set(payload, "/Collection/-", "last");
            Assert.AreEqual("last", JsonPointer.Get<string>(payload, "/Collection/3"));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CanHandleObjectProperties()
        {
            var payload = GetPocoObject();
            Assert.AreEqual(false, JsonPointer.Get<bool>(payload, "/Enabled"));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CanHandleObjectSubProperties()
        {
            var payload = GetPocoObject();
            Assert.AreEqual(true, JsonPointer.Get<bool>(payload, "/Settings/Launch"));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CanHandleObjectCollections()
        {
            var payload = GetPocoObject();
            Assert.AreEqual("second", JsonPointer.Get<string>(payload, "/Collection/1"));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CanHandleTestObjectProperties()
        {
            var payload = GetTestObject();
            Assert.AreEqual(false, JsonPointer.Get<bool>(payload, "/Enabled"));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CanHandleTestObjectSubProperties()
        {
            var payload = GetTestObject();
            Assert.AreEqual(true, JsonPointer.Get<bool>(payload, "/Settings/Launch"));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CanHandleTestObjectCollections()
        {
            var payload = GetTestObject();
            Assert.AreEqual("second", JsonPointer.Get<string>(payload, "/Collection/1"));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void PointToRoot()
        {
            var sample = GetJsonObject();
            Assert.IsTrue(JToken.DeepEquals(sample, JsonPointer.Get<JObject>(sample, "")));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void PointToArray()
        {
            var sample = GetJsonObject();
            CollectionAssert.AreEqual(sample["foo"].ToObject<List<string>>(), JsonPointer.Get<List<string>>(sample, "/foo"));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void PointToArrayElement()
        {
            var sample = GetJsonObject();
            Assert.AreEqual(sample["foo"][0], JsonPointer.Get<string>(sample, "/foo/0"));
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ExpectedException(typeof(ArgumentException))]
        public void PointToArrayElementThatDoesNotExistThrowsException()
        {
            var sample = GetJsonObject();
            JsonPointer.Get<string>(sample, "/foo/10");
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ExpectedException(typeof(ArgumentException))]
        public void PointToElementThatDoesNotExistThrowsException()
        {
            var sample = GetJsonObject();
            JsonPointer.Get<string>(sample, "/tee/nope");
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void PointToPropertyWithEmptyKey()
        {
            var sample = GetJsonObject();
            Assert.AreEqual(sample[""], JsonPointer.Get<JToken>(sample, "/"));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void PointToTokenWithEmbeddedSlash()
        {
            var sample = GetJsonObject();
            Assert.AreEqual(1, JsonPointer.Get<int>(sample, "/a~1b"));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void PointToTokenWithEmbeddedPercent()
        {
            var sample = GetJsonObject();
            Assert.AreEqual(2, JsonPointer.Get<int>(sample, "/c%d"));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void PointToTokenWithEncodedPercent()
        {
            var sample = GetJsonObject();
            Assert.AreEqual(2, JsonPointer.Get<int>(sample, "/c%25d"));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void PointToTokenWithEmbeddedCaret()
        {
            var sample = GetJsonObject();
            Assert.AreEqual(3, JsonPointer.Get<int>(sample, "/e^f"));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void PointToTokenWithEncodedCaret()
        {
            var sample = GetJsonObject();
            Assert.AreEqual(3, JsonPointer.Get<int>(sample, "/e%5Ef"));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void PointToTokenWithEmbeddedPipe()
        {
            var sample = GetJsonObject();
            Assert.AreEqual(4, JsonPointer.Get<int>(sample, "/g|h"));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void PointToTokenWithEscapedPipe()
        {
            var sample = GetJsonObject();
            Assert.AreEqual(4, JsonPointer.Get<int>(sample, "/g%7Ch"));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void PointToTokenWithEmbeddedBackSlash()
        {
            var sample = GetJsonObject();
            Assert.AreEqual(5, JsonPointer.Get<int>(sample, "/i\\j"));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void PointToTokenWithEscapedBackSlash()
        {
            var sample = GetJsonObject();
            Assert.AreEqual(5, JsonPointer.Get<int>(sample, "/i%5Cj"));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void PointToTokenWithEmbeddedQuote()
        {
            var sample = GetJsonObject();
            Assert.AreEqual(6, JsonPointer.Get<int>(sample, "/k\"l"));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void PointToTokenWithEscapedQuote()
        {
            var sample = GetJsonObject();
            Assert.AreEqual(6, JsonPointer.Get<int>(sample, "/k%22l"));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void PointToTokenWithSpace()
        {
            var sample = GetJsonObject();
            Assert.AreEqual(7, JsonPointer.Get<int>(sample, "/ "));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void PointToTokenWithEscapedSpace()
        {
            var sample = GetJsonObject();
            Assert.AreEqual(7, JsonPointer.Get<int>(sample, "/%20"));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void PointToTokenWithEmbeddedTilde()
        {
            var sample = GetJsonObject();
            Assert.AreEqual(8, JsonPointer.Get<int>(sample, "/m~0n"));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void PointToTokenTwoLevelsDeep()
        {
            var sample = GetJsonObject();
            Assert.AreEqual("a3", JsonPointer.Get<string>(sample, "/tee/black"));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void PointToTokenThreeLevelsDeep()
        {
            var sample = GetJsonObject();
            Assert.AreEqual("blue", JsonPointer.Get<string>(sample, "/tee/pink/1"));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetStringFromArrayIndexedProperty()
        {
            var sample = GetJsonString();
            Assert.AreEqual("John Steinbeck", JsonPointer.Get<string>(sample, "/books/1/author"));
        }

        private static JToken GetJsonObject()
        {
            return new JObject
            {
                ["foo"] = new JArray("bar", "baz"),
                [""] = 0,
                ["a/b"] = 1,
                ["c%d"] = 2,
                ["e^f"] = 3,
                ["g|h"] = 4,
                ["i\\j"] = 5,
                ["k\"l"] = 6,
                [" "] = 7,
                ["m~n"] = 8,
                ["tee"] =
                new JObject(
                    new JProperty("orange", "a1"),
                    new JProperty("blue", "a2"),
                    new JProperty("black", "a3"),
                    new JProperty("pink", new JArray(new JValue("orange"), new JValue("blue"))))
            };
        }
        private static string GetJsonString()
        {
            return @"{'books':[{'title':'The Great Gatsby','author':'F. Scott Fitzgerald'},{'title':'The Grapes of Wrath','author':'John Steinbeck'}]}";
        }

        private static object GetPocoObject()
        {
            return new
            {
                Enabled = false,
                Settings = new { Launch = true },
                Collection = new Collection<string> { "first", "second", "third" }
            };
        }

        public static TestObject GetTestObject()
        {
            return new TestObject
            {
                Enabled = false,
                Settings = new TestObjectSettings { Launch = true },
                Collection = new Collection<string> { "first", "second", "third" }
            };
        }
    }

    public class TestObject
    {
        public bool Enabled { get; set; }
        public TestObjectSettings Settings { get; set; }
        public Collection<string> Collection { get; set; }
    }

    public class TestObjectSettings
    {
        public bool Launch { get; set; }
    }
}
