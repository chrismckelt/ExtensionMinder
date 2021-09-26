using System.Collections.Generic;
using System.Linq;
using ExtensionMinder.CollectionExt;
using FizzWare.NBuilder;
using Xunit;

namespace ExtensionMinder.Tests.CollectionExt
{
    public class CollectionExtensionsShould
    {
        [Fact]
        public void Each_will_perform_action()
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

        [Fact]
        public void Max_by_will_select_max()
        {
            const int i = 1;

            var list = Builder<TestObject>
                .CreateListOfSize(3)
                .All()
                .With(x => x.IntegerProperty = i + 1)
                .Build();

            var max = list.MaxBy<TestObject, int>(a => a.IntegerProperty);

            Assert.Equal(list.Last().IntegerProperty, max.IntegerProperty);

        }

        [Fact]
        public void Max_by_will_select_max_with_specified_comparer()
        {
            const int i = 1;

            var list = Builder<TestObject>
                .CreateListOfSize(3)
                .All()
                .With(x => x.IntegerProperty = i + 1)
                .Build();

            var max = list.MaxBy<TestObject, int>(a => a.IntegerProperty, Comparer<int>.Default);

            Assert.Equal(list.Last().IntegerProperty, max.IntegerProperty);

        }

        [Fact]
        public void Map_by()
        {
            const int i = 1;

            var list = Builder<TestObject>
                .CreateListOfSize(3)
                .All()
                .With(x => x.IntegerProperty = i + 1)
                .Build();

            var result = list.Map<TestObject>(a =>
            {
                var x = a;
                x.IntegerProperty = 44;
                return x;
            });

            result.All(a => a.IntegerProperty == 44);
        }

        [Fact]
        public void Get_duplicates()
        {
            const int i = 1;

            var list = Builder<TestObject>
                .CreateListOfSize(3)
                .TheFirst(2)
                .With(x => x.IntegerProperty = i)
                .All()
                .With(x=>x.IntegerProperty = i+1)
                .Build();

            var duplicateList = new List<TestObject>();

            foreach (var testObject in list)
            {
                duplicateList.Add(testObject);
                if (testObject.IntegerProperty == i)
                {
                    duplicateList.Add(testObject);
                }
            }

            Assert.Equal(2, duplicateList.GetDuplicates().Count());
            
           
        }
    }
}
