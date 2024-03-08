using Inventory360DataModel;

namespace BLL.Common
{
    public static class GetCurrencyConversion
    {
        public static CurrencyConvertedAmount GetConvertedCurrencyAmount(CommonCompanyCurrency currencyInfo, CommonCurrencyRate currencyRate, long companyId, string currency, decimal amount, decimal exchangeRate)
        {
            // Convert price as necessary currency type
            CurrencyConvertedAmount amountItem = new CurrencyConvertedAmount();

            if (currency.Equals(currencyInfo.BaseCurrency))
            {
                amountItem.BaseAmount = amount;
                amountItem.Currency1Rate = currencyRate.Currency1Rate;
                amountItem.Currency1Amount = (currencyRate.Currency1Rate == 0 ? 0 : (amount / currencyRate.Currency1Rate));
                amountItem.Currency2Rate = currencyRate.Currency2Rate;
                amountItem.Currency2Amount = (currencyRate.Currency2Rate == 0 ? 0 : (amount / currencyRate.Currency2Rate));
            }
            else if (currency.Equals(currencyInfo.Currency1))
            {
                amountItem.BaseAmount = amount * exchangeRate;
                amountItem.Currency1Rate = exchangeRate;
                amountItem.Currency1Amount = amount;
                amountItem.Currency2Rate = currencyRate.Currency2Rate;
                amountItem.Currency2Amount = (currencyRate.Currency2Rate == 0 ? 0 : (amountItem.BaseAmount / currencyRate.Currency2Rate));
            }
            else if (currency.Equals(currencyInfo.Currency2))
            {
                amountItem.BaseAmount = amount * exchangeRate;
                amountItem.Currency1Rate = currencyRate.Currency1Rate;
                amountItem.Currency1Amount = (currencyRate.Currency1Rate == 0 ? 0 : (amountItem.BaseAmount / currencyRate.Currency1Rate));
                amountItem.Currency2Rate = exchangeRate;
                amountItem.Currency2Amount = amount;
            }

            return amountItem;
        }
    }
}