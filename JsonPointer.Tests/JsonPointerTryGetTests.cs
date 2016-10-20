namespace JsonPointerTests
{
    using JsonPointer;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class JsonPointerTryGetTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void MissingProperty()
        {
            var obj = @"{ foo: 1, bar: { baz: 2}, qux: [3, 4, 5]}";

            int result;
            if (JsonPointer.TryGet(obj, "/quo", out result))
            {
                Assert.Fail("Item doesn't exit");
            }
            else
            {
                Assert.IsTrue(result == default(int));
            }
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void ExistingProperty()
        {
            var obj = @"{ foo: 1, bar: { baz: 2}, qux: [3, 4, 5]}";

            int result;
            if (JsonPointer.TryGet(obj, "/foo", out result))
            {
                Assert.AreEqual(1, result);
            }
            else
            {
                Assert.Fail("Item doesn't exit");
            }
        }
    }
}
