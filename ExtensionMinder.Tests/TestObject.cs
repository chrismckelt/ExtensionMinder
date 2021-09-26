using System;

namespace ExtensionMinder.Tests
{
    public class TestObject 
    {
        public int IntegerProperty { get; set; }
        public string StringProperty { get; set; }
        public decimal DecimalProperty { get; set; }

        public System.DateTime CreatedUtcDateTime { get; set; }

        public static TestObject Build()
        {
            var rand = new Random();

            return new TestObject()
            {

                CreatedUtcDateTime = DateTime.UtcNow,
                DecimalProperty = Convert.ToDecimal(rand.NextDouble()),
                IntegerProperty = rand.Next(),
                StringProperty = "string_" + rand.Next().ToString()

            };
        }
    }
}
