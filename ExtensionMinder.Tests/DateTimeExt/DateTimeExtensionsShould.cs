using System;
using System.Linq;
using System.Threading.Tasks;
using ExtensionMinder.DateTimeExt;
using FluentAssertions;
using Xunit;

namespace ExtensionMinder.Tests.DateTimeExt
{
    public class DateTimeExtensionsShould
    {
        [Fact]
        public void DateTime_IsBetween_Returns_True_For_Valid_Case()
        {
            var start = new DateTime(2000, 01, 01);
            var end = new DateTime(2010, 01, 01);
            var between = new DateTime(2005, 01, 01);

            var result = DateTimeExtensions.IsBetween(between, start, end);

            result.Should().Be(true);
        }

        [Fact]
        public void DateTime_Is_Between_Returns_False_For_Valid_Case()
        {
            var start = new DateTime(2000, 01, 01);
            var end = new DateTime(2010, 01, 01);
            var between = new DateTime(2012, 01, 01);

            var result = DateTimeExtensions.IsBetween(between, start, end);

            result.Should().Be(false);
        }

        [Fact]
        public void DateTimeIsBetweenReturnsTrueForEdgeCase()
        {
            var start = new DateTime(2000, 01, 01);
            var end = new DateTime(2010, 01, 01);
            var between = new DateTime(2010, 01, 01);

            var result = DateTimeExtensions.IsBetween(between, start, end, true);

            result.Should().Be(true);
        }

        [Fact]
        public void DateTime_Is_Between_Returns_False_For_Edge_Case()
        {
            var start = new DateTime(2000, 01, 01);
            var end = new DateTime(2010, 01, 01);
            var between = new DateTime(2010, 01, 01);

            var result = DateTimeExtensions.IsBetween(between, start, end, false);

            result.Should().Be(false);
        }

        [Fact]
        public void Financial_Year_Start_Date_Gets_Correct_Year_In_Jan_To_June()
        {
            var date = new DateTime(2010, 01, 01);
            var start = DateTimeExtensions.GetFinancialYearStartDate(date);
            start.Year.Should().Be(2009);
            start.Month.Should().Be(7);
            start.Day.Should().Be(1);
        }

        [Fact]
        public void GetFinancialYearStartYearGetsCorrectYearInJulyToDec()
        {
            var date = new DateTime(2010, 11, 01);
            var end = DateTimeExtensions.GetFinancialYearStartDate(date);
            end.Year.Should().Be(2010);
            end.Month.Should().Be(7);
            end.Day.Should().Be(1);
        }

        [Fact]
        public void EndOfDayFunctionCorrectlyReturnsRightTime()
        {
            var date = new DateTime(2010, 10, 10, 10, 10, 10);
            var endOfDay = new DateTime(2010, 10, 10, 23, 59, 59);
            var result = DateTimeExtensions.EndOfDay(date);
            result.Should().Be(endOfDay);
        }

        [Fact]
        public void CalculateAgeCalculatesAgeCorrectlyWithDobLaterInYear()
        {
            var dob = DateTime.Now.AddDays(1).AddYears(-10);
            var age = DateTimeExtensions.CalculateAge(dob);
            age.Should().Be(9);
        }

        [Fact]
        public void CalculateAge_correct_for_dob_early_in_year()
        {
            var dob = DateTime.Now.AddDays(-1).AddYears(-10);
            var age = DateTimeExtensions.CalculateAge(dob);
            age.Should().Be(10);
        }

        [Fact]
        public void WeekDaysInMonthCount_correct()
        {

            var dt = DateTime.Parse("9/9/2021");
            var days = DateTimeExtensions.WeekDaysInMonthCount(dt);
            days.Should().Be(22);
        }

        [Fact]
        public void WeekDaysInMonthList_correct()
        {
            var dt = DateTime.Parse("9/9/2021");
            var days = DateTimeExtensions.WeekDaysInMonthList(dt);
            days.Count().Should().Be(22);
        }
    }
}
