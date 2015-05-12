using Phoenix.Core.Domain.Accounts;
using Phoenix.Core.Domain.Products.Payments;
using Phoenix.Core.Libraries.Extensions;
using Should;
using Xunit;

namespace Phoenix.Core.UnitTests.Libraries.Extensions
{
    public class EnumExtensionsTestFixture
    {
        [Fact]
        public void GetDescription_ReturnsCorrectDescription()
        {
            var x = AccountBankType.AutoCash;
            var str = x.GetDescription();
            str.ShouldEqual("Auto Cash");
        }

        [Fact]
        public void StringToEnum_ShouldConvertStringToEnum()
        {
            var str = "AutoCash";
            var x = str.ToEnum<AccountBankType>();
            x.ShouldEqual(AccountBankType.AutoCash);
        }

        [Fact]
        public void GetEnumFromValue_ReturnsCorrectEnum()
        {
            double value = 12;
            var result = value.GetEnumFromValue<PaymentFrequency>();
            double value2 = 1;
            var result2 = value2.GetEnumFromValue<PaymentFrequency>();

            result.ShouldEqual(PaymentFrequency.Monthly);
            result2.ShouldEqual(PaymentFrequency.Yearly);
        }
    }
}
