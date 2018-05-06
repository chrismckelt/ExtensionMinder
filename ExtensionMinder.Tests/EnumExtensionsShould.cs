﻿using FluentAssertions;
using Xunit;

namespace ExtensionMinder.Tests
{
    public class EnumExtensionsShould
    {
        [Fact]
        public void GetDescription_ReturnsCorrectDescription()
        {
            const TestEnum x = TestEnum.FirstValue;
            var str = x.GetDescription();
            str.Should().Be("First Value");
        }

        [Fact]
        public void StringToEnum_ShouldConvertStringToEnum()
        {
            const string str = "FirstValue";
            var x = str.ToEnum<TestEnum>();
            x.Should().Be(TestEnum.FirstValue);
        }

    }
}
