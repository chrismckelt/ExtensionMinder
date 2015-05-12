using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Phoenix.Core.Libraries.Extensions;
using Should;
using Xunit;

namespace Phoenix.Core.UnitTests.Libraries.Extensions
{

    public class ExtensionsTestFixture
    {

        [Fact]
        public void TrimToLengthShouldCutStringToSpecfiedLength()
        {
            string s = "123456";
            string x = s.TrimToLength(3);
            x.Length.ShouldEqual(3);
        }

        [Fact]
        public void CalculateAgeCalculatesAgeCorrectlyWithDobLaterInYear()
        {
            var dob = DateTime.Now.AddDays(1).AddYears(-10);
            var age = dob.CalculateAge();
            age.ShouldEqual(9);
        }

        [Fact]
        public void CalculateAgeCalculatesAgeCorrectlyWithDobEarlierInYear()
        {
            var dob = DateTime.Now.AddDays(-1).AddYears(-10);
            var age = dob.CalculateAge();
            age.ShouldEqual(10);
        }

        [Fact]
        public void ShowIf_ReturnsStringWhenFuncIsTrue()
        {
            var result = "This".ShowIf(() => true);

            result.ShouldEqual("This");
        }

        [Fact]
        public void ShowIf_ReturnsEmptyStringWhenFuncIsFalse()
        {
            var result = "This".ShowIf(() => false);

            result.ShouldEqual("");
        }
    }
}
