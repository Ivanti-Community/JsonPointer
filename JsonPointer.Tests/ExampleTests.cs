namespace JsonPointerTests
{
    using System;
    using System.Collections.Generic;
    using JsonPointer;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ExampleTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void Example()
        {
            var obj = @"{ foo: 1, bar: { baz: 2}, qux: [3, 4, 5]}";

            Assert.AreEqual(1, JsonPointer.Get<int>(obj, "/foo"));
            Assert.AreEqual(2, JsonPointer.Get<int>(obj, "/bar/baz"));
            Assert.AreEqual(2, JsonPointer.Get<int>(obj, "/bar/baz"));
            Assert.AreEqual(3, JsonPointer.Get<int>(obj, "/qux/0"));
            Assert.AreEqual(4, JsonPointer.Get<int>(obj, "/qux/1"));
            Assert.AreEqual(5, JsonPointer.Get<int>(obj, "/qux/2"));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void ReplacePropertyUsingSet()
        {
            var obj = @"{ foo: 1, bar: { baz: 2}, qux: [3, 4, 5]}";

            obj = JsonPointer.Set(obj, "/foo", 6);   // sets obj.foo = 6;

            Assert.AreEqual(6, JsonPointer.Get<int>(obj, "/foo"));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AddItemToCollectionUsingSet()
        {
            var obj = @"{ foo: 1, bar: { baz: 2}, qux: [3, 4, 5]}";

            obj = JsonPointer.Set(obj, "/qux/-", 6); // sets obj.qux = [3, 4, 5, 6]

            Assert.AreEqual(3, JsonPointer.Get<int>(obj, "/qux/0"));
            Assert.AreEqual(4, JsonPointer.Get<int>(obj, "/qux/1"));
            Assert.AreEqual(5, JsonPointer.Get<int>(obj, "/qux/2"));
            Assert.AreEqual(6, JsonPointer.Get<int>(obj, "/qux/3"));
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ExpectedException(typeof(ArgumentException))]
        public void MissingPropertyThrowsArgumentException()
        {
            var obj = @"{ foo: 1, bar: { baz: 2}, qux: [3, 4, 5]}";
            JsonPointer.Get<int>(obj, "/quo");     // throws ArgumentException
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Example2Test()
        {
            var sample = @"{
              'books': [
                {
                  'title' : 'The Great Gatsby',
                  'author' : 'F. Scott Fitzgerald'
                },
                {
                  'title' : 'The Grapes of Wrath',
                  'author' : 'John Steinbeck'
                }
              ]
            }";

            var result = JsonPointer.Get<string>(sample, "/books/1/author"); // returns "John Steinbeck"
            Assert.AreEqual("John Steinbeck", result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void ExampleCollectionTest()
        {
            var sample = @"{
              'books': [
                {
                  'title' : 'The Great Gatsby',
                  'author' : 'F. Scott Fitzgerald'
                },
                {
                  'title' : 'The Grapes of Wrath',
                  'author' : 'John Steinbeck'
                }
              ]
            }";

            var result = JsonPointer.Get<List<Book>>(sample, "/books"); 
            Assert.IsTrue(result.Count == 2);

            sample = JsonPointer.Set(sample, "/books/-", new Book { Title = "Jayne Eyre", Author = "Charlotte Brontë" });
            result = JsonPointer.Get<List<Book>>(sample, "/books");
            Assert.IsTrue(result.Count == 3);
        }
    }

    public class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
    }
}
