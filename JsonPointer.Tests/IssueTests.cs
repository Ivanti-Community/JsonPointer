using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JsonPointerTests
{
	using JsonPointer;

	[TestClass]
	public class IssueTests
	{
		[TestMethod]
		public void Issue1()
		{
			var input = "[{\"Id\":\"b4120a48-d5b0-476f-a653-083f3725dfce\"}]";
			var result = JsonPointer.Get<List<Foo>>(input, "/");

			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Count);
		}

		public class Foo
		{
			public Guid Id { get; set; }
		}
	}
}
