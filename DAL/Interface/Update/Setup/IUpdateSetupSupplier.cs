using Inventory360DataModel;
using Inventory360DataModel.Setup;

namespace DAL.Interface.Update.Setup
{
    public interface IUpdateSetupSupplier
    {
        bool UpdateSupplier(CommonSetupSupplier entity);
        bool UpdateSupplierOpeningByPaymentInPaidAmountAsIncrease(CurrencyConvertedAmount convertedAmount);
        bool UpdateSupplierOpeningByPaymentInPaidAmountAsDecrease(CurrencyConvertedAmount convertedAmount);
    }
}