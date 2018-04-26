using System.ComponentModel;

namespace ExtensionMinder.Tests
{
    public enum TestEnum
    {
        [Description("First Value")] FirstValue,
        [Description("Second Value")] SecondValue,
        [Description("ThirdValue")] ThirdValue
    }
}