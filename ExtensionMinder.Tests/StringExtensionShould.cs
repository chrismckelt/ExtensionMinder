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
    }
}
