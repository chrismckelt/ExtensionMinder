using System;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace ExtensionMinder.Tests
{
    public class StringExtensionShould
    {
      [Fact]
      public void TrimAllStrings_trims_all_strings()
      {
        var to = new TestObject {StringProperty = "   hello   "};
        var result = to.TrimAllStrings();
        Assert.Equal("hello", result.StringProperty);
      }

      [Fact]
      public void SplitSentenceIntoWords_removes_junk()
      {
        const string test = "hell this ain't the best! words to split 123";
        var words = test.SplitSentenceIntoWords();
        foreach (var word in words)
        {
          Console.WriteLine(word);
        }

        words.Count().Should().Be(9);
      }

      [Fact]
      public void Clean_removes_dodgy_characters()
      {
        string text = @"abcd&efg-h";
        text.Clean().Should().Be("abcdefgh");
      }
    }
}
