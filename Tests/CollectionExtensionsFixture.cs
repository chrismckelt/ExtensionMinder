using System.Linq;
using FizzWare.NBuilder;
using Xunit;

namespace ExtensionMinder.Tests
{
    public class CollectionExtensionsFixture
    {
        [Fact]
        public void EachWillPerformAction()
        {
            const int i = 77;

            var list = Builder<TestObject>
                .CreateListOfSize(3)
                .All()
                .With(x=>x.IntegerProperty = i+1)
                .Build();

            list.Each(a => a.IntegerProperty = 5);

            Assert.True(list.All(a=>a.IntegerProperty==5));

        }
    }
}
