using System;
using FluentAssertions;
using Xunit;


namespace ExtensionMinder.Tests
{
    public class ExtensionsTestShould
    {
        [Fact]
        public void TrimToLengthShouldCutStringToSpecfiedLength()
        {
            string s = "123456";
            string x = s.TrimToLength(3);
            x.Length.Should().Be(3);
        }

        [Fact]
        public void ShowIf_ReturnsStringWhenFuncIsTrue()
        {
            var result = "This".ShowIf(() => true);

            result.Should().Be("This");
        }

        [Fact]
        public void ShowIf_ReturnsEmptyStringWhenFuncIsFalse()
        {
            var result = "This".ShowIf(() => false);

            result.Should().Be("");
        }
    }
}