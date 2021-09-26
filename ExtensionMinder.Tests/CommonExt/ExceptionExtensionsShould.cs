using System;
using ExtensionMinder.CommonExt;
using FluentAssertions;
using Xunit;

namespace ExtensionMinder.Tests.CommonExt
{
    public class ExceptionExtensionsShould
    {
      [Fact]
      public void GetInnerMostException_retrieves_nested_exception()
      {
        var ex1 = new ApplicationException("1");
        var ex2 = new ApplicationException("2",ex1);
        var ex3 = new ApplicationException("3", ex2);

        ex3.GetInnerMostException().Message.Should().Be("1");
      }
    }
}
