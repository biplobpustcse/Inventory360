using Inventory360DataModel;
using Inventory360DataModel.Setup;
using BLL.Common;
using DAL.DataAccess.Select.Configuration;
using DAL.DataAccess.Select.Setup;
using DAL.DataAccess.Select.Stock;
using DAL.DataAccess.Select.Task;
using DAL.Interface.Select.Configuration;
using DAL.Interface.Select.Setup;
using DAL.Interface.Select.Stock;
using DAL.Interface.Select.Task;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.DropDown
{
    public class DropDownSetupProduct
    {
        private decimal GetProductPriceForSalesOrder(string currency, long operationTypeId, long productId, long? dimensionId, long unitTypeId, long locationId, long companyId)
        {
            try
            {
                ISelectConfigurationOperationalEventDetail iSelectConfigurationOperationalEventDetail = new DSelectConfigurationOperationalEventDetail(companyId);
                var defaultPriceTypeId = iSelectConfigurationOperationalEventDetail.SelectOperationalEventDetailAll()
                    .Where(x => x.LocationId == locationId
                        && x.OperationTypeId == operationTypeId
                        && x.Configuration_OperationalEvent.EventName == CommonEnum.OperationalEvent.Sales.ToString()
                        && x.Configuration_OperationalEvent.SubEventName == CommonEnum.OperationalSubEvent.SalesOrder.ToString())
                    .Select(s => s.DefaultPriceTypeId)
                    .DefaultIfEmpty(0)
                    .FirstOrDefault();

                var currencyInfo = GetCompanyCurrencyInfo.CompanyCurrencyInfo(companyId);
                ISelectSetupPrice iSelectSetupPrice = new DSelectSetupPrice(companyId);

                return defaultPriceTypeId == 0 ? 0 :
                    (iSelectSetupPrice.SelectProductPriceAll()
                    .Where(x => x.ProductId == productId
                        && x.UnitTypeId == unitTypeId
                        && x.PriceTypeId == defaultPriceTypeId
                        && x.ProductDimensionId == dimensionId)
                    .Select(s =>
                        currencyInfo.BaseCurrency == currency ? s.Price : (currencyInfo.Currency1 == currency ? s.Price1 : s.Price2)
                    )
                    .DefaultIfEmpty(0)
                    .FirstOrDefault());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private decimal GetProductPriceByOperationalEvent(string OperationalEvent, string OperationalSubEvent, string currency, long operationTypeId, long productId, long? dimensionId, long unitTypeId, long locationId, long companyId)
        {
            try
            {
                ISelectConfigurationOperationalEventDetail iSelectConfigurationOperationalEventDetail = new DSelectConfigurationOperationalEventDetail(companyId);
                var defaultPriceTypeId = iSelectConfigurationOperationalEventDetail.SelectOperationalEventDetailAll()
                    .Where(x => x.LocationId == locationId
                        && x.OperationTypeId == operationTypeId
                        && x.Configuration_OperationalEvent.EventName == OperationalEvent //CommonEnum.OperationalEvent.Sales.ToString()
                        && x.Configuration_OperationalEvent.SubEventName == OperationalSubEvent //CommonEnum.OperationalSubEvent.SalesOrder.ToString()
                        )
                    .Select(s => s.DefaultPriceTypeId)
                    .DefaultIfEmpty(0)
                    .FirstOrDefault();

                var currencyInfo = GetCompanyCurrencyInfo.CompanyCurrencyInfo(companyId);
                ISelectSetupPrice iSelectSetupPrice = new DSelectSetupPrice(companyId);

                return defaultPriceTypeId == 0 ? 0 :
                    (iSelectSetupPrice.SelectProductPriceAll()
                    .Where(x => x.ProductId == productId
                        && x.UnitTypeId == unitTypeId
                        && x.PriceTypeId == defaultPriceTypeId
                        && x.ProductDimensionId == dimensionId)
                    .Select(s =>
                        currencyInfo.BaseCurrency == currency ? s.Price : (currencyInfo.Currency1 == currency ? s.Price1 : s.Price2)
                    )
                    .DefaultIfEmpty(0)
                    .FirstOrDefault());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SelectSerialProductOrNotByProductId(long companyId, long productId)
        {
            try
            {
                ISelectSetupProduct iSelectSetupProduct = new DSelectSetupProduct(companyId);
                return iSelectSetupProduct.SelectProductAll()
                    .Where(x => x.ProductId == productId)
                    .Select(s => s.SerialAvailable)
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object SelectSerialByProductForDropdown(long productId, long? dimensionId, long unitTypeId, long locationId, long? warehouseId, long companyId, string query)
        {
            try
            {
                ISelectStockCurrentStockSerial iSelectStockCurrentStockSerial = new DSelectStockCurrentStockSerial(companyId);

                return iSelectStockCurrentStockSerial.SelectCurrentStockSerialAll()
                    .Where(x => x.Stock_CurrentStock.ProductId == productId
                        && x.Stock_CurrentStock.ProductDimensionId == (dimensionId == 0 ? null : dimensionId)
                        && x.Stock_CurrentStock.UnitTypeId == unitTypeId
                        && x.Stock_CurrentStock.LocationId == locationId
                        && x.Stock_CurrentStock.WareHouseId == (warehouseId == 0 ? null : warehouseId)
                        && x.Stock_CurrentStock.CompanyId == companyId
                    )
                    .WhereIf(!string.IsNullOrEmpty(query), x => x.Serial.ToLower().Contains(query.ToLower()))
                    .OrderBy(o => o.Serial)
                    .Take(10)
                    .Select(s => new
                    {
                        Item = s.Serial,
                        Value = s.Serial
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object SelectProductByCompanyId(long companyId, string query)
        {
            try
            {
                ISelectSetupProduct iSelectSetupProduct = new DSelectSetupProduct(companyId);

                return iSelectSetupProduct.SelectProductAll()
                    .Where(x => x.IsActive == true)
                    .WhereIf(!string.IsNullOrEmpty(query), x => x.Name.ToLower().Contains(query.ToLower())
                        || x.Code.ToLower().Contains(query.ToLower()))
                    .OrderBy(o => o.Name)
                    .Take(10)
                    .Select(s => new
                    {
                        Item = "[" + s.Code + "] " + s.Name.ToString(),
                        Value = s.ProductId.ToString(),
                        Code = s.Code,
                        isServiceProduct = s.ProductType == "Service" ? true : false
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public object SelectProductNameFromRMAByCompanyId(long companyId, string query)
        {
            try
            {
                ISelectStockRMAStock iSelectStockRMAStock = new DSelectStockRMAStock(companyId);

                return iSelectStockRMAStock.SelectRMAStockAll()
                    //.Where(x => x.Setup_Product.Name.ToLower() == query.ToLower())                    
                    .WhereIf(!string.IsNullOrEmpty(query), x => x.Setup_Product.Name.ToLower().Contains(query.ToLower()))
                    .OrderBy(x => x.Setup_Product.Name)
                    .Take(10)
                    .Select(s => new
                    {
                        Item = "[" + s.Setup_Product.Code + "] " + s.Setup_Product.Name.ToString(),
                        Value = s.ProductId.ToString(),
                        Code = s.Setup_Product.Code,
                        isServiceProduct = s.Setup_Product.ProductType == "Service" ? true : false
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CommonResultList> SelectProductByCompanyId(long groupId, long subGroupId, long categoryId, long brandId, string model, string query, long companyId)
        {
            try
            {
                List<CommonResultList> initialList = new List<CommonResultList>();
                ISelectSetupProduct iSelectSetupProduct = new DSelectSetupProduct(companyId);

                //initialList.Add(new CommonResultList { Item = "Select One...", Value = "", IsSelected = true });

                List<CommonResultList> result = iSelectSetupProduct.SelectProductAll()
                    .WhereIf(groupId > 0, x => x.ProductGroupId == groupId)
                    .WhereIf(subGroupId > 0, x => x.ProductSubGroupId == subGroupId)
                    .WhereIf(categoryId > 0, x => x.ProductCategoryId == categoryId)
                    .WhereIf(brandId > 0, x => x.BrandId == brandId)
                    .WhereIf(!string.IsNullOrEmpty(model), x => x.Model.ToLower().Contains(model.ToLower()))
                    .WhereIf(!string.IsNullOrEmpty(query), x => x.Name.ToLower().Contains(query.ToLower())
                        || x.Code.ToLower().Contains(query.ToLower()))
                    .OrderBy(o => o.Name)
                    .Select(s => new CommonResultList
                    {
                        Item = "[" + s.Code + "] " + s.Name.ToString(),
                        Value = s.ProductId.ToString()
                    })
                    .ToList();

                initialList.AddRange(result);

                return initialList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CommonResultList> SelectProductGroupExceptExistingProductByCompanyId(long groupId, long subGroupId, long categoryId, long brandId, string model, string query, long companyId, CommonSetupProductSearch entityData)
        {
            try
            {
                List<CommonResultList> initialList = new List<CommonResultList>();
                ISelectSetupProduct iSelectSetupProduct = new DSelectSetupProduct(companyId);

                //initialList.Add(new CommonResultList { Item = "Select One...", Value = "", IsSelected = true });

                List<CommonResultList> result = iSelectSetupProduct.SelectProductAll()
                    .WhereIf(groupId > 0, x => x.ProductGroupId == groupId)
                    .WhereIf(subGroupId > 0, x => x.ProductSubGroupId == subGroupId)
                    .WhereIf(categoryId > 0, x => x.ProductCategoryId == categoryId)
                    .WhereIf(brandId > 0, x => x.BrandId == brandId)
                    .WhereIf(!string.IsNullOrEmpty(model), x => x.Model.ToLower().Contains(model.ToLower()))
                    .WhereIf(!string.IsNullOrEmpty(query), x => x.Name.ToLower().Contains(query.ToLower())
                        || x.Code.ToLower().Contains(query.ToLower()))
                    .OrderBy(o => o.Name)
                    .Select(s => new CommonResultList
                    {
                        Item = "[" + s.Code + "] " + s.Name.ToString(),
                        Value = s.ProductId.ToString()
                    })
                    .ToList();
                if (entityData != null && entityData.CommonSetupProductSearchDetail.Count() > 0)
                {

                    result = result.Where(x => !entityData.CommonSetupProductSearchDetail.Any(a => a.ProductId.ToString() == x.Value && (a.ProductDimensionId == 0 || a.ProductDimensionId == null))).ToList();
                }

                initialList.AddRange(result);

                return initialList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CommonResultList> SelectSaleableProductByCompanyId(long companyId, string query)
        {
            try
            {
                ISelectSetupProduct iSelectSetupProduct = new DSelectSetupProduct(companyId);

                return iSelectSetupProduct.SelectProductAll()
                    .Where(x => x.IsActive == true && x.IsSaleable == true)
                    .WhereIf(!string.IsNullOrEmpty(query), x => x.Name.ToLower().Contains(query.ToLower())
                        || x.Code.ToLower().Contains(query.ToLower()))
                    .OrderBy(o => o.Name)
                    .Take(10)
                    .Select(s => new CommonResultList
                    {
                        Item = "[" + s.Code + "] " + s.Name.ToString(),
                        Value = s.ProductId.ToString()
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object SelectProductWiseAvailableStockPriceDiscount(string currency, long operationTypeId, long productId, long? dimensionId, long unitTypeId, long locationId, long? wareHouseId, long companyId)
        {
            try
            {
                ISelectSetupProduct iSelectSetupProduct = new DSelectSetupProduct(companyId);
                var productInfo = iSelectSetupProduct.SelectProductAll()
                    .Where(x => x.ProductId == productId)
                    .Select(s => new
                    {
                        s.PrimaryUnitTypeId,
                        s.IsStockMaintain,
                        s.SerialAvailable
                    })
                    .FirstOrDefault();

                var availableStockPriceDiscount = new
                {
                    AvailableStock = GetAvailableStock.GetAvailableStockByProductExceptSelectedSalesOrder(productId, unitTypeId, productInfo.PrimaryUnitTypeId, locationId, (wareHouseId == 0 ? null : wareHouseId), companyId, (dimensionId == 0 ? null : dimensionId), null),
                    Price = GetProductPriceForSalesOrder(currency, operationTypeId, productId, (dimensionId == 0 ? null : dimensionId), unitTypeId, locationId, companyId),
                    Discount = 0.0,
                    IsStockMaintain = productInfo.IsStockMaintain,
                    IsSerialProduct = productInfo.SerialAvailable
                };

                return availableStockPriceDiscount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public object SelectProductWiseAvailableStockPriceDiscountByOperationalEvent(string OperationalEvent, string OperationalSubEvent, string currency, long operationTypeId, long productId, long? dimensionId, long unitTypeId, long locationId, long? wareHouseId, long companyId)
        {
            try
            {
                ISelectSetupProduct iSelectSetupProduct = new DSelectSetupProduct(companyId);
                var productInfo = iSelectSetupProduct.SelectProductAll()
                    .Where(x => x.ProductId == productId)
                    .Select(s => new
                    {
                        s.PrimaryUnitTypeId,
                        s.IsStockMaintain,
                        s.SerialAvailable
                    })
                    .FirstOrDefault();

                var availableStockPriceDiscount = new
                {
                    AvailableStock = GetAvailableStock.GetAvailableStockByProductExceptSelectedSalesOrder(productId, unitTypeId, productInfo.PrimaryUnitTypeId, locationId, (wareHouseId == 0 ? null : wareHouseId), companyId, (dimensionId == 0 ? null : dimensionId), null),
                    Price = GetProductPriceByOperationalEvent(OperationalEvent, OperationalSubEvent, currency, operationTypeId, productId, (dimensionId == 0 ? null : dimensionId), unitTypeId, locationId, companyId),
                    Discount = 0.0,
                    IsStockMaintain = productInfo.IsStockMaintain,
                    IsSerialProduct = productInfo.SerialAvailable
                };

                return availableStockPriceDiscount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public object SelectProductCost(long productId, long? dimensionId, long unitTypeId, long locationId, long? wareHouseId, long companyId)
        {
            try
            {
                ISelectStockCurrentStock iSelectStockCurrentStock = new DSelectStockCurrentStock(companyId);
                var costInfo = iSelectStockCurrentStock.SelectCurrentStockAll()
                    .Where(x => x.ProductId == productId && x.UnitTypeId == unitTypeId && x.LocationId == locationId && x.CompanyId == companyId)
                    .WhereIf(dimensionId != 0, x => x.ProductDimensionId == dimensionId || x.ProductDimensionId == null)
                    .WhereIf(wareHouseId != 0, x => x.WareHouseId == wareHouseId || x.WareHouseId == null)
                    .Where(x => x.Quantity > 0)
                    .GroupBy(x => x.ProductId, (key, g) => new
                    {
                        ProductId = key,
                        Cost = Math.Round(g.Sum(x => x.Quantity * x.Cost) / g.Sum(x => x.Quantity), 2),
                        Cost1 = Math.Round(g.Sum(x => x.Quantity * x.Cost1) / g.Sum(x => x.Quantity), 2),
                        Cost2 = Math.Round(g.Sum(x => x.Quantity * x.Cost2) / g.Sum(x => x.Quantity), 2)
                    }).FirstOrDefault();

                return costInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public object SelectProductSerial(long productId, long? dimensionId, long unitTypeId, long locationId, long? wareHouseId, long companyId)
        {
            try
            {
                ISelectStockCurrentStock iSelectStockCurrentStock = new DSelectStockCurrentStock(companyId);
                ISelectStockCurrentStockSerial iSelectStockCurrentStockSerial = new DSelectStockCurrentStockSerial(companyId);
                var costInfo = iSelectStockCurrentStock.SelectCurrentStockAll()
                    .Where(x => x.ProductId == productId && x.UnitTypeId == unitTypeId && x.LocationId == locationId && x.CompanyId == companyId)
                    .WhereIf(dimensionId != 0, x => x.ProductDimensionId == dimensionId || x.ProductDimensionId == null)
                    .WhereIf(wareHouseId != 0, x => x.WareHouseId == wareHouseId || x.WareHouseId == null)
                    .GroupBy(x => x.ProductId, (key, g) => new
                    {
                        ProductId = key,
                        Cost = Math.Round(g.Sum(x => x.Quantity * x.Cost) / g.Sum(x => x.Quantity), 2),
                        Cost1 = Math.Round(g.Sum(x => x.Quantity * x.Cost1) / g.Sum(x => x.Quantity), 2),
                        Cost2 = Math.Round(g.Sum(x => x.Quantity * x.Cost2) / g.Sum(x => x.Quantity), 2)
                    }).FirstOrDefault();
                return costInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public object SelectProductBySerial(long companyId, long locationId, string serial, long productId)
        {
            try
            {
                ISelectStockCurrentStockSerial iSelectStockCurrentStockSerial = new DSelectStockCurrentStockSerial(companyId);

                return iSelectStockCurrentStockSerial.SelectCurrentStockSerialAll()
                    .WhereIf(productId != 0, x => x.Stock_CurrentStock.ProductId == productId)
                    .Where(x => x.Stock_CurrentStock.LocationId == locationId && x.Stock_CurrentStock.CompanyId == companyId)
                    .WhereIf(!string.IsNullOrEmpty(serial), x => x.Serial.ToLower().Contains(serial.ToLower()))
                    .OrderBy(o => o.Serial)
                    .Take(10)
                    .Select(s => new
                    {
                        Item = s.Serial,
                        Value = s.Serial,
                        ProductName = "[" + s.Stock_CurrentStock.Setup_Product.Code + "] " + s.Stock_CurrentStock.Setup_Product.Name.ToString(),
                        ProductId = s.Stock_CurrentStock.ProductId.ToString(),
                        s.Stock_CurrentStock.Setup_Product.Code,
                        s.Stock_CurrentStock.Cost,
                        s.Stock_CurrentStock.Cost1,
                        s.Stock_CurrentStock.Cost2
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public object SelectProductByRMASerial(long companyId, long locationId, string serial, long productId)
        {
            try
            {
                ISelectStockRMAStockSerial iSelectStockRMAStockSerial = new DSelectStockRMAStockSerial(companyId);

                return iSelectStockRMAStockSerial.SelectRMAStockSerialAll()
                    .WhereIf(productId != 0, x => x.Stock_RMAStock.ProductId == productId)
                    .Where(x => x.Stock_RMAStock.LocationId == locationId && x.Stock_RMAStock.CompanyId == companyId
                    )
                    .WhereIf(!string.IsNullOrEmpty(serial), x => x.Serial.ToLower().Contains(serial.ToLower()))
                    .OrderBy(o => o.Serial)
                    .Take(10)
                    .Select(s => new
                    {
                        Item = s.Serial,
                        Value = s.Serial,
                        ProductName = "[" + s.Stock_RMAStock.Setup_Product.Code + "] " + s.Stock_RMAStock.Setup_Product.Name.ToString(),
                        ProductId = s.Stock_RMAStock.ProductId.ToString(),
                        s.Stock_RMAStock.Setup_Product.Code,
                        s.Stock_RMAStock.Cost,
                        s.Stock_RMAStock.Cost1,
                        s.Stock_RMAStock.Cost2
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static Random random = new Random();
        public object GenerateProductSerial(long companyId, long locationId, long productId, int serialLength)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, serialLength).Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public object SelectProductStockInReferenceInfo(long companyId, long productId, string serial, long supplierId)
        {
            try
            {
                ISelectSetupProduct iSelectSetupProduct = new DSelectSetupProduct(companyId);
                var data = iSelectSetupProduct.SelectProductStockInReferenceInfo(companyId, productId, serial, supplierId);
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public object SelectProductNameExceptExistingProductByCompanyId(long companyId, CommonSetupProductSearch entityData)
        {
            try
            {
                ISelectSetupProduct iSelectSetupProduct = new DSelectSetupProduct(companyId);

                var data = iSelectSetupProduct.SelectProductAll()
                    .Where(x => x.IsActive == true)
                    .WhereIf(!string.IsNullOrEmpty(entityData.query), x => x.Name.ToLower().Contains(entityData.query.ToLower())
                        || x.Code.ToLower().Contains(entityData.query.ToLower()))
                     .OrderBy(o => o.Name)
                    .Take(10)
                    .Select(s => new
                    {
                        Item = "[" + s.Code + "] " + s.Name.ToString(),
                        Value = s.ProductId.ToString(),
                        Code = s.Code,
                        s.PrimaryUnitTypeId,
                        s.SecondaryUnitTypeId,
                        s.TertiaryUnitTypeId,
                        s.SecondaryConversionRatio,
                        s.TertiaryConversionRatio,
                        isServiceProduct = s.ProductType == "Service" ? true : false                        
                    })
                    .ToList();
                if (entityData != null && entityData.CommonSetupProductSearchDetail.Count() > 0)
                {

                    data = data.Where(x => !entityData.CommonSetupProductSearchDetail.Any(a => a.ProductId.ToString() == x.Value && (a.ProductDimensionId == 0 || a.ProductDimensionId == null))).ToList();
                }

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}