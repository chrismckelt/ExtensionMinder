﻿using System;
using System.Linq;
using ExtensionMinder.StringExt;
using FluentAssertions;
using Xunit;

namespace ExtensionMinder.Tests.StringExt
{
    public class StringExtensionShould
    {

        [Fact]
        public void Surround_with_double_quotes()
        {
            string expect = @"""aaa""";
            string actual = "aaa".SurroundWithDoubleQuotes();
            expect.Should().Be(actual);
        }

        [Fact]
        public void Trim_all_strings()
        {
            var to = new TestObject {StringProperty = "   hello   "};
            var result = to.TrimAllStrings();
            Assert.Equal("hello", result.StringProperty);
        }

        [Fact]
        public void Split_sentence_into_words_removes_junk()
        {
            const string test = "hell this ain't the best! words to split 123";
            var words = test.SplitSentenceIntoWords();
            foreach (var word in words) Console.WriteLine(word);

            words.Count().Should().Be(9);
        }

        [Theory]
        [InlineData("abcd&efg-h", "abcdefgh")]
        [InlineData("abc!@#$%&*(", "abc")]
        [InlineData("123!", "123")]
        [InlineData("123! 456", "123 456")]
        [InlineData("aaa    aaa", "aaa aaa")]
        public void Clean_removes_dodgy_characters(string text, string expected)
        {
            text.Clean().Should().Be(expected);
        }

        [Fact]
        public void To_alpha_numeric_only()
        {
            var text = @"a1!b2&c3$";
            text.ToAlphaNumericOnly(" ").Should().Be("a1 b2 c3");
        }

        [Fact]
        public void To_alpha_numeric_only_with_spaces()
        {
            var text = @"abc!123!@#";
            text.ToAlphaNumericOnly(" ").Should().Be("abc 123");
        }

        [Fact]
        public void To_alpha_only()
        {
            var text = @"abc123!@#";
            text.ToAlphaOnly().Should().Be("abc");
        }

        [Fact]
        public void To_alpha_only_with_spaces()
        {
            var text = @"a1b2c3!@#";
            text.ToAlphaOnly(" ").Should().Be("a b c");
        }

        [Fact]
        public void To_numeric_only()
        {
            var text = @"abc123!@#";
            text.ToNumericOnly().Should().Be("123");
        }


        [Fact]
        public void Trim_to_length_should_cut_string_to_specfied_length()
        {
            var s = "123456";
            var x = s.TrimToLength(3);
            x.Length.Should().Be(3);
        }

        [Theory]
        [InlineData("@#$", true)]
        [InlineData("[!]", true)]
        [InlineData("hello", false)]
        [InlineData("I'm ok!", false)]
        public void IsGibberish_should_detect(string text, bool isGibberish)
        {
            text.IsGibberish().Should().Be(isGibberish);
        }

        [Fact]
        public void Be_true_if_more_numbers_than_letters()
        {
            string text = "abc1234";
            text.IsGibberish().Should().Be(true);
        }

        [Fact]
        public void Be_false_if_more_numbers_than_letters()
        {
            string text = "abcd123";
            text.IsGibberish().Should().Be(false);

        }

        [Fact]
        public void Be_false_if_more_number_count_same_as_letter_count()
        {
            string text = "abc123";
            text.IsGibberish().Should().Be(false);
        }
    }
}