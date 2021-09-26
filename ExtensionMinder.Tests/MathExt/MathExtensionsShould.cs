using ExtensionMinder.MathExt;
using FluentAssertions;
using Xunit;

namespace ExtensionMinder.Tests.MathExt
{
    public class MathExtensionsShould
    {
        [Fact]
        public void Multiply_by_100_should_set_correct_value()
        {
            decimal x = 10;
            decimal y = x.MultiplyBy(100);
            y.Should().Be(1000);
        }

        [Fact]
        public void Divide_by_100_should_set_correct_value()
        {
            decimal x = 1000;
            decimal y = x.DivideBy(100);
            y.Should().Be(10);
        }

        [Fact]
        public void Round_up_should_round_decimal_value_up()
        {
            var value = 123.3333333M;
            var result = value.RoundUp(2);
            result.Should().Be(123.34M);
        }

        [Fact]
        public void Round_up_should_round_double_value_up()
        {
            var value = 123.3333333;
            var result = value.RoundUp(2);
            result.Should().Be(123.34);
        }

        [Fact]
        public void Round_up_should_round_edge_case_decmial_up()
        {
            var value = 123.000001M;
            var result = value.RoundUp(2);
            result.Should().Be(123.01M);
        }

        [Fact]
        public void Round_up_should_round_edge_case_double_up()
        {
            var value = 123.000001;
            var result = value.RoundUp(2);
            result.Should().Be(123.01);
        }

        [Fact]
        public void Is_between_correctly_identifies_between_value()
        {
            var x = 10;
            var result = x.IsBetween(9, 11);
            result.Should().Be(true);
        }

        [Fact]
        public void Is_between_for_boundary_case()
        {
            var x = 10;
            var result = x.IsBetween(10, 12, true);
            result.Should().Be(true);
        }

        [Fact]
        public void Is_between_for_boundary_case_when_excluding_boundaries()
        {
            var x = 10;
            var result = x.IsBetween(10, 12, false);
            result.Should().Be(false);
        }

        [Fact]
        public void Is_between_when_value_not_between()
        {
            var x = 10;
            var result = x.IsBetween(100, 110);
            result.Should().Be(false);
        }

        [Fact]
        public void Truncate_for_simple_decimal()
        {
            var x = 15.123456M;
            var result = x.TruncateDecimal(2);
            result.Should().Be(15.12M);
        }

        [Fact]
        public void Truncate_for_simple_decimal_and_does_not_round_up()
        {
            var x = 15.1999M;
            var result = x.TruncateDecimal(1);
            result.Should().Be(15.1M);
        }

        [Fact]
        public void Truncate_for_decimal_with_zero_integral()
        {
            var x = 0.0047568M;
            var result = x.TruncateDecimal(4);
            result.Should().Be(0.0047M);
        }

        [Fact]
        public void Round_to_nearest_100()
        {
            var x = 792m;
            var result = x.RoundDownToNearest(100);

            result.Should().Be(700);
        }

        [Fact]
        public void Round_to_nearest_1000()
        {
            var x = 792m;
            var result = x.RoundDownToNearest(1000);

            result.Should().Be(0);
        }

        [Fact]
        public void Round_to_nearest_10()
        {
            var x = 792m;
            var result = x.RoundDownToNearest(10);

            result.Should().Be(790);

        }

        [Fact]
        public void Round_to_nearest_5()
        {
            var x = 797m;
            var result = x.RoundDownToNearest(5);

            result.Should().Be(795);

        }

        [Fact]
        public void Round_to_nearest_exact()
        {
            var x = 795m;
            var result = x.RoundDownToNearest(5);

            result.Should().Be(795);

        }

        [Fact]
        public void Round_up_to_nearest_100()
        {
            var x = 792m;
            var result = x.RoundUpToNearest(100);

            result.Should().Be(800);
        }

        [Fact]
        public void Round_up_to_nearest_1000()
        {
            var x = 792m;
            var result = x.RoundUpToNearest(1000);

            result.Should().Be(1000);
        }

        [Fact]
        public void Round_up_to_nearest_10()
        {
            var x = 792m;
            var result = x.RoundUpToNearest(10);

            result.Should().Be(800);

        }

        [Fact]
        public void Round_up_to_nearest_5()
        {
            var x = 792m;
            var result = x.RoundUpToNearest(5);

            result.Should().Be(795);

        }

        [Fact]
        public void Round_up_to_nearest_exact()
        {
            var x = 795m;
            var result = x.RoundUpToNearest(5);

            result.Should().Be(795);

        }

        [Fact]
        public void Add_with_func__by_adding_one()
        {
            var x = 1.2m;
            var result = x.Add(a => a%1 == 0 ? 0 : 1);
            result.Should().Be(2.2m);
        }

        [Fact]
        public void Add_with_func__by_adding_zero()
        {
            var x = 1.0m;
            var result = x.Add(a => a % 1 == 0 ? 0 : 1);
            result.Should().Be(1m);
        }

        [Fact]
        public void Percent_valid()
        {
            double d = 100d;
            d.Percent(20).Should().Be(20);

        }
    }
}
