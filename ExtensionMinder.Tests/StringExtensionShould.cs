using System;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace ExtensionMinder.Tests
{
  public class StringExtensionShould
  {
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

    [Fact]
    public void Clean_removes_dodgy_characters()
    {
      var text = @"abcd&efg-h";
      text.Clean().Should().Be("abcdefgh");
    }

    [Fact]
    public void To_alpha_numeric_only()
    {
      var text = @"abc123!@#";
      text.ToAlphaNumericOnly().Should().Be("abc123");
    }

    [Fact]
    public void To_alpha_only()
    {
      var text = @"abc123!@#";
      text.ToAlphaOnly().Should().Be("abc");
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
      string s = "123456";
      string x = s.TrimToLength(3);
      x.Length.Should().Be(3);
    }
  }
}