﻿using System;

namespace ExtensionMinder.MathExt
{
    public static class MathExtensions
    {

        public static decimal MultiplyBy(this decimal dec, int multiple)
        {
            return dec * multiple;
        }

        public static decimal DivideBy(this decimal dec, int divider)
        {
            return dec / divider;
        }

        public static int MultiplyBy(this int i, int multiple)
        {
            return i * multiple;
        }

        public static int DivideBy(this int i, int divider)
        {
            return i / divider;
        }

        public static decimal ToCeiling(this decimal d)
        {
            return System.Math.Ceiling(d);
        }

        public static decimal RoundUp(this decimal d, int dp)
        {
            var multiplier = Convert.ToDecimal(System.Math.Pow(10, dp));
            return System.Math.Ceiling(d*multiplier)/multiplier;
        }

        public static double RoundUp(this double d, int dp)
        {
            // Perform this as a decimal, to avoid some bad edge cases with floating point math
            return (double)Convert.ToDecimal(d).RoundUp(dp);
        }

        public static decimal TruncateDecimal(this decimal d, int dp)
        {
            var multiplier = Convert.ToDecimal(System.Math.Pow(10, dp));
            return System.Math.Truncate(d*multiplier)/multiplier;
        }

        public static bool IsBetween(this int x, int lower, int upper, bool includeBoundaries = true)
        {
            if (upper < lower)
                throw new ArgumentException("IsBetween: Upper limit must be greater then lower limit");

            if (includeBoundaries)
                return x >= lower && x <= upper;

            return x > lower && x < upper;
        }

        public static bool IsBetween(this decimal x, decimal lower, decimal upper, bool includeBoundaries = true)
        {
            if (upper < lower)
                throw new ArgumentException("IsBetween: Upper limit must be greater then lower limit");

            if (includeBoundaries)
                return x >= lower && x <= upper;

            return x > lower && x < upper;
        }

        public static bool IsInIncrementOf(this decimal x, decimal increment)
        {
            return System.Math.Floor(x/increment) == x/increment;
        }

        public static int GetCents(this decimal x)
        {
            return x.GetCents(2);
        }

        public static int GetCents(this decimal x, int places)
        {
            return (int) (Convert.ToDouble(x - Decimal.Truncate(x))*(System.Math.Pow(10, places)));
        }

        public static int RoundDownToNearest(this decimal x, int number)
        {
           return (int) System.Math.Floor(x/number)*number;
        }

        public static int RoundUpToNearest(this decimal x, int number)
        {
            return (int) System.Math.Ceiling(x / number) * number;
        }

        public static int ButNotLessThen(this int x, int number)
        {
            return System.Math.Max(x, number);
        }

        public static decimal Add(this decimal x, decimal number)
        {
            return x + number;
        }

        public static decimal Minus(this decimal x, decimal number)
        {
            return x - number;
        }

        public static int  Minus(this int x, int number)
        {
            return x - number;
        }

        public static decimal Add(this decimal x, Func<decimal, decimal> func)
        {
            return x + func(x);
        }

        public static decimal Minus(this decimal x, Func<decimal, decimal> func)
        {
            return x - func(x);
        }

        public static double Percent(this double number,int percent)
        {
            //return ((double) 80         *       25)/100;
            return ((double)number * percent) / 100;
        }
    }
}
