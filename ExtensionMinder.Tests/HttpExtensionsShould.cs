﻿using System.Collections.Specialized;
using FluentAssertions;
using Xunit;

namespace ExtensionMinder.Tests
{
    public class HttpExtensionsShould
    {
      [Fact]
      public void ToQueryString()
      {
        var nv = new NameValueCollection {{"aaa", "111"}, {"bbb", "2222"}, {"ccc", "3333"}};
        nv.ToQueryString(true).Should().Be("?aaa=111&bbb=2222&ccc=3333");

        nv = new NameValueCollection { { "aaa", "111" }, { "bbb", "2222" }, { "ccc", "3333" } };
        nv.ToQueryString().Should().Be("aaa=111&bbb=2222&ccc=3333");
    }
    }
}
