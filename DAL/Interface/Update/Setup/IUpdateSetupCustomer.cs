using Inventory360DataModel;
using Inventory360DataModel.Setup;

namespace DAL.Interface.Update.Setup
{
    public interface IUpdateSetupCustomer
    {
        bool UpdateCustomer(CommonSetupCustomer entity);
        bool UpdateCustomerOpeningByCollectionInCollectedAmountAsIncrease(CurrencyConvertedAmount convertedAmount);
        bool UpdateCustomerOpeningByCollectionInCollectedAmountAsDecrease(CurrencyConvertedAmount convertedAmount);
    }
}