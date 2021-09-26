using System;
using System.Diagnostics;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using ExtensionMinder.CommonExt;
using Xunit;

namespace ExtensionMinder.Tests.CommonExt
{
    public class ExtensionsTestShould
    {
        [Fact]
        public async Task Dump_will_print_properties_and_values()
        {
            var to = new TestObject();
            to.CreatedUtcDateTime = DateTime.MaxValue;
            to.DecimalProperty = 2m;
            to.StringProperty = "AAA";

            to.Dump("test");

            


        }
       
    }

     
}