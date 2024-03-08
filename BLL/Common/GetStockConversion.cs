using Inventory360DataModel;
using DAL.DataAccess.Select.Setup;
using DAL.Interface.Select.Setup;
using System;
using System.Linq;

namespace BLL.Common
{
    public static class GetStockConversion
    {
        public static CommonConvertedStock ConvertedStockQtyAndCost(long productId, long unitTypeId, decimal stockQty, decimal stockCost, long companyId)
        {
            try
            {
                CommonConvertedStock commonConvertedStock = new CommonConvertedStock();

                ISelectSetupProduct iSelectSetupProduct = new DSelectSetupProduct(companyId);
                var productInfo = iSelectSetupProduct.SelectProductAll()
                    .Where(x => x.ProductId == productId)
                    .Select(s => new
                    {
                        s.PrimaryUnitTypeId,
                        s.SecondaryUnitTypeId,
                        s.TertiaryUnitTypeId,
                        s.SecondaryConversionRatio,
                        s.TertiaryConversionRatio
                    })
                    .FirstOrDefault();

                if (productInfo != null)
                {
                    if (productInfo.PrimaryUnitTypeId == unitTypeId)
                    {
                        commonConvertedStock.PrimaryStockQty = stockQty;
                        commonConvertedStock.PrimaryStockCost = stockCost;
                        commonConvertedStock.SecondaryStockQty = (productInfo.SecondaryConversionRatio == 0 ? 0 : (stockQty / (productInfo.SecondaryConversionRatio == 0 ? 1 : productInfo.SecondaryConversionRatio)));
                        commonConvertedStock.TertiaryStockQty = (productInfo.TertiaryConversionRatio == 0 ? 0 : (stockQty / ((productInfo.SecondaryConversionRatio == 0 ? 1 : productInfo.SecondaryConversionRatio) * (productInfo.TertiaryConversionRatio == 0 ? 1 : productInfo.TertiaryConversionRatio))));
                        commonConvertedStock.UnitLevel = 1;
                    }
                    else if (productInfo.SecondaryUnitTypeId == unitTypeId)
                    {
                        commonConvertedStock.PrimaryStockQty = stockQty * productInfo.SecondaryConversionRatio;
                        commonConvertedStock.PrimaryStockCost = stockCost / (productInfo.SecondaryConversionRatio == 0 ? 1 : productInfo.SecondaryConversionRatio);
                        commonConvertedStock.SecondaryStockQty = stockQty;
                        commonConvertedStock.TertiaryStockQty = (productInfo.TertiaryConversionRatio == 0 ? 0 : (stockQty / (productInfo.TertiaryConversionRatio == 0 ? 1 : productInfo.TertiaryConversionRatio)));
                        commonConvertedStock.UnitLevel = 2;
                    }
                    else if (productInfo.TertiaryUnitTypeId == unitTypeId)
                    {
                        commonConvertedStock.PrimaryStockQty = stockQty * productInfo.SecondaryConversionRatio * productInfo.TertiaryConversionRatio;
                        commonConvertedStock.PrimaryStockCost = stockCost / (productInfo.SecondaryConversionRatio * productInfo.TertiaryConversionRatio == 0 ? 1 : (productInfo.SecondaryConversionRatio * productInfo.TertiaryConversionRatio));
                        commonConvertedStock.SecondaryStockQty = stockQty * productInfo.TertiaryConversionRatio;
                        commonConvertedStock.TertiaryStockQty = stockQty;
                        commonConvertedStock.UnitLevel = 3;
                    }
                }

                return commonConvertedStock;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static CommonConvertedStock ConvertedStockQtyAndCost(long unitTypeId, long primaryUnitTypeId, long secondaryUnitTypeId, long tertiaryUnitTypeId, decimal secondaryConversionRatio, decimal tertiaryConversionRatio, decimal stockQty, decimal stockCost)
        {
            try
            {
                CommonConvertedStock commonConvertedStock = new CommonConvertedStock();

                if (primaryUnitTypeId == unitTypeId)
                {
                    commonConvertedStock.PrimaryStockQty = stockQty;
                    commonConvertedStock.PrimaryStockCost = stockCost;
                    commonConvertedStock.SecondaryStockQty = (secondaryConversionRatio == 0 ? 0 : (stockQty / (secondaryConversionRatio == 0 ? 1 : secondaryConversionRatio)));
                    commonConvertedStock.TertiaryStockQty = (tertiaryConversionRatio == 0 ? 0 : (stockQty / ((secondaryConversionRatio == 0 ? 1 : secondaryConversionRatio) * (tertiaryConversionRatio == 0 ? 1 : tertiaryConversionRatio))));
                    commonConvertedStock.UnitLevel = 1;
                }
                else if (secondaryUnitTypeId == unitTypeId)
                {
                    commonConvertedStock.PrimaryStockQty = stockQty * secondaryConversionRatio;
                    commonConvertedStock.PrimaryStockCost = stockCost / (secondaryConversionRatio == 0 ? 1 : secondaryConversionRatio);
                    commonConvertedStock.SecondaryStockQty = stockQty;
                    commonConvertedStock.TertiaryStockQty = (tertiaryConversionRatio == 0 ? 0 : (stockQty / (tertiaryConversionRatio == 0 ? 1 : tertiaryConversionRatio)));
                    commonConvertedStock.UnitLevel = 2;
                }
                else if (tertiaryUnitTypeId == unitTypeId)
                {
                    commonConvertedStock.PrimaryStockQty = stockQty * secondaryConversionRatio * tertiaryConversionRatio;
                    commonConvertedStock.PrimaryStockCost = stockCost / (secondaryConversionRatio * tertiaryConversionRatio == 0 ? 1 : (secondaryConversionRatio * tertiaryConversionRatio));
                    commonConvertedStock.SecondaryStockQty = stockQty * tertiaryConversionRatio;
                    commonConvertedStock.TertiaryStockQty = stockQty;
                    commonConvertedStock.UnitLevel = 3;
                }

                return commonConvertedStock;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}