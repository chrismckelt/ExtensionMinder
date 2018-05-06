using System;
using FluentAssertions;
using Xunit;

namespace ExtensionMinder.Tests
{
    public class DateTimeExtensionsShould
    {
        [Fact]
        public void DateTimeIsBetweenReturnsTrueForValidCase()
        {
            var start = new DateTime(2000, 01, 01);
            var end = new DateTime(2010, 01, 01);
            var between = new DateTime(2005, 01, 01);

            var result = between.IsBetween(start, end);

            result.Should().Be(true);
        }

        [Fact]
        public void DateTimeIsBetweenReturnsFalseForValidCase()
        {
            var start = new DateTime(2000, 01, 01);
            var end = new DateTime(2010, 01, 01);
            var between = new DateTime(2012, 01, 01);

            var result = between.IsBetween(start, end);

            result.Should().Be(false);
        }

        [Fact]
        public void DateTimeIsBetweenReturnsTrueForEdgeCase()
        {
            var start = new DateTime(2000, 01, 01);
            var end = new DateTime(2010, 01, 01);
            var between = new DateTime(2010, 01, 01);

            var result = between.IsBetween(start, end, true);

            result.Should().Be(true);
        }

        [Fact]
        public void DateTimeIsBetweenReturnsFalseForEdgeCase()
        {
            var start = new DateTime(2000, 01, 01);
            var end = new DateTime(2010, 01, 01);
            var between = new DateTime(2010, 01, 01);

            var result = between.IsBetween(start, end, false);

            result.Should().Be(false);
        }

        [Fact]
        public void GetFinancialYearStartDateGetsCorrectYearInJanToJune()
        {
            var date = new DateTime(2010, 01, 01);
            var start = date.GetFinancialYearStartDate();
            start.Year.Should().Be(2009);
            start.Month.Should().Be(7);
            start.Day.Should().Be(1);
        }

        [Fact]
        public void GetFinancialYearStartYearGetsCorrectYearInJulyToDec()
        {
            var date = new DateTime(2010, 11, 01);
            var end = date.GetFinancialYearStartDate();
            end.Year.Should().Be(2010);
            end.Month.Should().Be(7);
            end.Day.Should().Be(1);
        }

        [Fact]
        public void EndOfDayFunctionCorrectlyReturnsRightTime()
        {
            var date = new DateTime(2010, 10, 10, 10, 10, 10);
            var endOfDay = new DateTime(2010, 10, 10, 23, 59, 59);
            var result = date.EndOfDay();
            result.Should().Be(endOfDay);
        }

        [Fact]
        public void CalculateAgeCalculatesAgeCorrectlyWithDobLaterInYear()
        {
            var dob = DateTime.Now.AddDays(1).AddYears(-10);
            var age = dob.CalculateAge();
            age.Should().Be(9);
        }

        [Fact]
        public void CalculateAgeCalculatesAgeCorrectlyWithDobEarlierInYear()
        {
            var dob = DateTime.Now.AddDays(-1).AddYears(-10);
            var age = dob.CalculateAge();
            age.Should().Be(10);
        }
    }
}
