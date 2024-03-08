using Inventory360DataModel;
using Inventory360DataModel.Setup;
using BLL;
using BLL.DropDown;
using BLL.DropDown.Setup;
using BLL.Grid.Stock;
using BLL.Grid.Task;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Web.Http;

namespace Inventory360API_V2.Controllers
{
    [RoutePrefix("api")]
    public class CommonController : ApiController
    {
        [Authorize]
        [HttpGet]
        //[Route("C00224")]
        [Route("C0087")]
        // Load adjustment nature
        // Example - addition, deduction
        public IHttpActionResult SelectPartyAdjustmentNature()
        {
            try
            {
                var data = new ManagerDefault()
                    .SelectPartyAdjustmentNature();

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0080")]
        //Load only own bank by Company for dropdown
        public IHttpActionResult SelectOwnBankByCompanyIdForDropdown(string query = "")
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownSetupBank()
                    .SelectOwnBankByCompanyIdForDropdown(userInfo.CompanyId, query);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0079")]
        //Load terms & conditions of sales invoice by Company for dropdown
        public IHttpActionResult SelectTemplateHeaderForSalesInvoiceTermsAndConditionsForDropdown(string query = "")
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownSetupTemplateHeader()
                    .SelectTemplateHeaderForSalesInvoiceTermsAndConditionsForDropdown(userInfo.CompanyId, query);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0078")]
        public IHttpActionResult SelectCustomerCollectionType()
        {
            try
            {
                var data = new ManagerDefault()
                    .SelectCustomerCollectionType();

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0077")]
        //Load Product Group by Company for dropdown
        public IHttpActionResult SelectProductGroupByCompanyId(string query, string model, long groupId = 0, long subGroupId = 0, long categoryId = 0, long brandId = 0)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownSetupProduct()
                    .SelectProductByCompanyId(groupId, subGroupId, categoryId, brandId, model, query, userInfo.CompanyId);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0076")]
        //Load Product Group by Company for dropdown
        public IHttpActionResult SelectProductGroupByCompanyId(long groupId = 0, long subGroupId = 0, long categoryId = 0, long brandId = 0)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownSetupProductModel()
                    .SelectProductModelForDropdown(groupId, subGroupId, categoryId, brandId, userInfo.CompanyId);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0075")]
        //Load serial by product for dropdown
        public IHttpActionResult SelectSerialByProductForGrid(long productId, long? dimensionId, long unitTypeId, long? warehouseId, string query = "")
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridCurrentStock()
                    .SelectAllSerialByProductForGrid(productId, dimensionId, unitTypeId, userInfo.LocationId, warehouseId, userInfo.CompanyId, query);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0074")]
        //Load Product wise Unit Type for dropdown
        public IHttpActionResult SelectSerialProductOrNotByProductId(long productId)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownSetupProduct()
                    .SelectSerialProductOrNotByProductId(userInfo.CompanyId, productId);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0072")]
        //Load serial by product for dropdown
        public IHttpActionResult SelectSerialByProductForDropdown(long productId, long? dimensionId, long unitTypeId, long? warehouseId, string query = "")
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownSetupProduct()
                    .SelectSerialByProductForDropdown(productId, dimensionId, unitTypeId, userInfo.LocationId, warehouseId, userInfo.CompanyId, query);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0071")]
        // Purchase payment against like - Advance/Previous/Purchase Order/Bill
        public IHttpActionResult PurchasePaymentAgainst()
        {
            try
            {
                var data = new ManagerDefault()
                    .PurchasePaymentAgainst();

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0070")]
        //Load payment terms of purchase by Company for dropdown
        public IHttpActionResult SelectTemplateHeaderForPurchasePaymentByCompanyIdForDropdown(string query = "")
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownSetupTemplateHeader()
                    .SelectTemplateHeaderForPurchasePaymentForDropdown(userInfo.CompanyId, query);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0069")]
        //Load terms & conditions of purchase by Company for dropdown
        public IHttpActionResult SelectTemplateHeaderForPurchaseTermsAndConditionsByCompanyIdForDropdown(string query = "")
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownSetupTemplateHeader()
                    .SelectTemplateHeaderForPurchaseOrderTermsAndConditionsForDropdown(userInfo.CompanyId, query);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0068")]
        //Load all supplier by Company for dropdown
        public IHttpActionResult SelectAllSupplierByCompanyIdForDropdown(string query = "")
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownSetupSupplier()
                    .SelectAllSupplierByCompanyIdForDropdown(userInfo.CompanyId, query);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0067T")]
        // Load measurement name
        public IHttpActionResult MeasurementNameList()
        {
            try
            {
                var data = new ManagerDefault()
                    .MeasurementNameLists();

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0067")]
        // Load purchase type
        // Example - local, foreign
        public IHttpActionResult PurchaseType()
        {
            try
            {
                var data = new ManagerDefault()
                    .PurchaseType();

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0066")]
        // Sales collection against like - Advance/Previous/Sales Order/Invoice
        public IHttpActionResult SalesCollectionAgainst()
        {
            try
            {
                var data = new ManagerDefault()
                    .SalesCollectionAgainst();

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0065")]
        //Load Location Wise Security User for dropdown
        public IHttpActionResult SelectLocationWiseSecurityUserAll(string query = "")
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new ManagerSecurity()
                    .SelectLocationWiseSecurityUserAll(userInfo.CompanyId, userInfo.LocationId, query);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0064")]
        //Load Product wise available stock, price and discount
        public IHttpActionResult SelectProductWiseAvailableStockPriceDiscount(string currency, long operationTypeId, long productId, long unitTypeId, long locationId, long wareHouseId = 0, long dimensionId = 0)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownSetupProduct()
                    .SelectProductWiseAvailableStockPriceDiscount(currency, operationTypeId, productId, dimensionId, unitTypeId, locationId, wareHouseId, userInfo.CompanyId);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0063")]
        // Example - increase, decrease
        public IHttpActionResult IncreaseDecrease()
        {
            try
            {
                var data = new ManagerDefault()
                    .IncreaseDecrease();

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0062")]
        //Load Product wise Dimension for dropdown
        public IHttpActionResult SelectProductWiseDimensionByProductId(long productId, string query = "")
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownSetupProductDimension()
                    .SelectProductWiseDimensionByProductId(userInfo.CompanyId, productId, query);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0061")]
        //Load Saleable Product Company for dropdown
        public IHttpActionResult SelectSaleableProductByCompanyId(string query = "")
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownSetupProduct()
                    .SelectSaleableProductByCompanyId(userInfo.CompanyId, query);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0060")]
        //Load Product wise Unit Type for dropdown
        public IHttpActionResult SelectProductWiseUnitTypeByProductId(long productId, string query = "")
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownSetupUnitType()
                    .SelectProductWiseUnitTypeByProductId(userInfo.CompanyId, productId, query);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0058")]
        // Load Trial Balance Type
        // Trial Balance, Transactional Trial Balance
        public IHttpActionResult TrialBalanceType()
        {
            try
            {
                var data = new ManagerDefault()
                    .TrialBalanceType();

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0057")]
        // Load Trial Balance Tree
        // Subgroup, Control, Subsidiary, Account
        public IHttpActionResult TrialBalanceTree()
        {
            try
            {
                var data = new ManagerDefault()
                    .TrialBalanceTree();

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0056")]
        // Load price type isdetail by pricetypeid
        public IHttpActionResult SelectPriceTypeIsDetailByPriceTypeIdAndCompanyId(long id)
        {
            var userInfo = GetUserInfoFromIdentity();
            var data = new DropDownSetupPriceType()
                .SelectPriceTypeIsDetailByPriceTypeIdAndCompanyId(id, userInfo.CompanyId);

            return Ok(data);
        }

        [Authorize]
        [HttpGet]
        [Route("C0055")]
        //Load price type for dropdown
        public IHttpActionResult SelectPriceTypeByCompanyId(string query = "")
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownSetupPriceType()
                    .SelectPriceTypeByCompanyId(userInfo.CompanyId, query);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0054")]
        // Load Payment Mode - for sales
        public IHttpActionResult PaymentMode()
        {
            try
            {
                var data = new DropDownConfigurationPaymentMode()
                    .PaymentMode();

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0053E")]
        // Load warehouse by locaion
        public IHttpActionResult SelectWarehouseBySelectedLocationAndCompany(long locationId)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new ManagerSetupLocation()
                    .SelectWarehouseByLocationIdAndCompanyId(locationId, userInfo.CompanyId);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0053")]
        // Load warehouse by locaion
        public IHttpActionResult SelectWarehouseByLoggedLocationAndCompany()
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new ManagerSetupLocation()
                    .SelectWarehouseByLocationIdAndCompanyId(userInfo.LocationId, userInfo.CompanyId);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0052")]
        // Load sales type
        // Example - installment, regular
        public IHttpActionResult SalesType()
        {
            try
            {
                var data = new ManagerDefault()
                    .SalesType();

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0051")]
        //Load company currency exchange rate for dropdown
        public IHttpActionResult SelectCurrencyExchangeRateByCompanyIdAndCurrencyForDropdown(string currency)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownCurrency()
                    .SelectCurrencyExchangeRateByCompanyIdAndCurrencyForDropdown(currency, userInfo.CompanyId);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0050")]
        //Load company currency for dropdown
        public IHttpActionResult SelectCompanyCurrencyByCompanyIdForDropdown()
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();

                var data = new DropDownCurrency()
                    .SelectCompanyCurrencyByCompanyIdForDropdown(userInfo.CompanyId);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0049")]
        //Load transport type for dropdown
        public IHttpActionResult SelectTransportTypeByCompanyId(string query = "")
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownSetupTransportType()
                    .SelectTransportTypeByCompanyId(userInfo.CompanyId, query);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0048")]
        //Load transport for dropdown
        public IHttpActionResult SelectTransportByCompanyId(string query = "")
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownSetupTransport()
                    .SelectTransportByCompanyId(userInfo.CompanyId, query);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0047")]
        // Load shipment mode
        // Example - air, road, sea
        public IHttpActionResult ShipmentMode()
        {
            try
            {
                var data = new ManagerDefault()
                    .ShipmentMode();

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0046")]
        // Load sales shipment type
        // Example - Full, Part
        public IHttpActionResult SalesShipmentType()
        {
            try
            {
                var data = new ManagerDefault()
                    .SalesShipmentType();

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0045")]
        //Load payment terms of sales by Company for dropdown
        public IHttpActionResult SelectTemplateHeaderForSalesPaymentByCompanyIdForDropdown(string query = "")
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownSetupTemplateHeader()
                    .SelectTemplateHeaderForSalesPaymentForDropdown(userInfo.CompanyId, query);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0044")]
        //Load terms & conditions of sales by Company for dropdown
        public IHttpActionResult SelectTemplateHeaderForSalesTermsAndConditionsByCompanyIdForDropdown(string query = "")
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownSetupTemplateHeader()
                    .SelectTemplateHeaderForSalesOrderTermsAndConditionsForDropdown(userInfo.CompanyId, query);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0043")]
        //Load only buyer by Company for dropdown
        public IHttpActionResult SelectOnlyBuyerByCompanyIdForDropdown(string query = "")
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownSetupCustomer()
                    .SelectOnlyBuyerByCompanyIdForDropdown(userInfo.CompanyId, query);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0042")]
        //Load all customer by Company for dropdown
        public IHttpActionResult SelectAllCustomerByCompanyIdForDropdown(string query = "")
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownSetupCustomer()
                    .SelectAllCustomerByCompanyIdForDropdown(userInfo.CompanyId, query);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0041")]
        //Load Template Header for dropdown
        public IHttpActionResult SelectTemplateHeaderByCompanyId(string query = "")
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownSetupTemplateHeader()
                    .SelectTemplateHeaderByCompanyId(userInfo.CompanyId, query);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0040")]
        //Load Operational Sub-Event for dropdown
        public IHttpActionResult SelectOperationalSubEventByCompanyId(string eventName, string query = "")
        {
            try
            {
                var data = new DropDownSetupOperationalEvent()
                    .SelectOperationalSubEventByCompanyId(eventName, query);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0039")]
        //Load Operational Event for dropdown
        public IHttpActionResult SelectOperationalEventByCompanyId(string query = "")
        {
            try
            {
                var data = new DropDownSetupOperationalEvent()
                    .SelectOperationalEventByCompanyId(query);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0036")]
        //Load all employee by Company for dropdown
        public IHttpActionResult SelectAllEmployeeByCompanyIdForDropdown(string query = "")
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownSetupEmployee()
                    .SelectAllEmployeeByCompanyIdForDropdown(userInfo.CompanyId, query);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0035")]
        //Load Product Company for dropdown
        public IHttpActionResult SelectProductByCompanyId(string query = "")
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownSetupProduct()
                    .SelectProductByCompanyId(userInfo.CompanyId, query);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0030")]
        //Load Unit Type by Company for dropdown
        public IHttpActionResult SelectUnitTypeByCompanyId(string query = "")
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownSetupUnitType()
                    .SelectUnitTypeByCompanyId(userInfo.CompanyId, query);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [Authorize]
        [HttpGet]
        [Route("C0399")]
        // Load Product stock type
        // Example - Current, RMA, BAD
        public IHttpActionResult SelectProductStockType()
        {
            try
            {
                var data = new ManagerDefault()
                    .SelectProductStockType();

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0029")]
        //Load Product Brand by Company for dropdown
        public IHttpActionResult SelectBrandByCompanyId(string query = "")
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownSetupBrand()
                    .SelectBrandByCompanyId(userInfo.CompanyId, query);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0028")]
        //Load Product Category by Company and Product Group for dropdown
        public IHttpActionResult SelectProductCategoryByCompanyIdAndGroupId(long productGroupId, string query = "")
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownSetupProductCategory()
                    .SelectProductCategoryByCompanyIdAndGroupId(userInfo.CompanyId, productGroupId, query);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0027")]
        //Load Product Sub Group by Company and Product Group for dropdown
        public IHttpActionResult SelectProductSubGroupByCompanyIdAndGroupId(long productGroupId, string query = "")
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownSetupProductSubGroup()
                    .SelectProductSubGroupByCompanyIdAndGroupId(userInfo.CompanyId, productGroupId, query);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0026")]
        // Load stock type
        // Example - Finished, Raw, WIP
        public IHttpActionResult SelectStockType()
        {
            try
            {
                var data = new ManagerDefault()
                    .SelectStockType();

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0025")]
        // Load product type
        // Example - Asset, Consumable, Inventory, Service
        public IHttpActionResult SelectProductType()
        {
            try
            {
                var data = new ManagerDefault()
                    .SelectProductType();

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0024")]
        // Load customer type
        // Example - Customer, Buyer
        public IHttpActionResult SelectCustomerType()
        {
            try
            {
                var data = new ManagerDefault()
                    .SelectCustomerType();

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0023")]
        //Load sales person from employee by Company for dropdown
        public IHttpActionResult SelectEmployeeSalesPersonByCompanyIdForDropdown(string query = "")
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownSetupEmployee()
                    .SelectEmployeeSalesPersonByCompanyIdForDropdown(userInfo.CompanyId, query);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0022")]
        //Load customer group by Company for dropdown
        public IHttpActionResult SelectCustomerGroupByCompanyIdForDropdown(string query = "")
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownSetupCustomerGroup()
                    .SelectCustomerGroupByCompanyIdForDropdown(userInfo.CompanyId, query);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0020")]
        //Load supplier group by Company for dropdown
        public IHttpActionResult SelectSupplierGroupByCompanyIdForDropdown(string query = "")
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownSetupSupplierGroup()
                    .SelectSupplierGroupByCompanyIdForDropdown(userInfo.CompanyId, query);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0019")]
        // Load Accounts Hierarchy Tree
        // Group, Subgroup, Control, Subsidiary, Account
        public IHttpActionResult AccountsHierarchyTree()
        {
            try
            {
                var data = new ManagerDefault()
                    .AccountsHierarchyTree();

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0018")]
        //Load Product Group by Company for dropdown
        public IHttpActionResult SelectProductGroupByCompanyId(string query = "")
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownSetupProductGroup()
                    .SelectProductGroupByCompanyIdForDropdown(userInfo.CompanyId, query);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0017")]
        //Load bank by Company for dropdown
        public IHttpActionResult SelectBankByCompanyIdForDropdown(string query = "")
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownSetupBank()
                    .SelectAllBankByCompanyIdForDropdown(userInfo.CompanyId, query);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0015")]
        // Load Employee role
        // Example - Sales Person
        public IHttpActionResult SelectEmployeeRole()
        {
            try
            {
                var data = new ManagerDefault()
                    .SelectEmployeeRole();

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0014")]
        // Load Voucher Types
        // Debit, Credit, Contra, Journal
        public IHttpActionResult SelectVoucherTypes()
        {
            try
            {
                var data = new ManagerDefault()
                    .SelectVoucherTypes();

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0013")]
        //Load Project
        public IHttpActionResult SelectProject()
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();

                var data = new ManagerSetupProject()
                    .SelectProject(userInfo.CompanyId);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0012")]
        public IHttpActionResult SelectContraVoucherType()
        {
            try
            {
                var data = new ManagerDefault()
                    .SelectContraVoucherType();

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0011")]
        public IHttpActionResult SelectCreditVoucherType()
        {
            try
            {
                var data = new ManagerDefault()
                    .SelectCreditVoucherType();

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0010")]
        public IHttpActionResult SelectDebitVoucherType()
        {
            try
            {
                var data = new ManagerDefault()
                    .SelectDebitVoucherType();

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C0001")]
        //Load Balance Type
        public IHttpActionResult SelectBalanceType()
        {
            try
            {
                var data = new ManagerDefault()
                    .SelectBalanceType();

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        //Developed By Biplob 08/03//2020
        [Authorize]
        [HttpGet]
        [Route("C00201")]
        //Load Cheque Typpe for dropdown
        public IHttpActionResult SelectChequeType()
        {
            try
            {
                var data = new ManagerDefault()
                    .SelectChequeType();

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        //Developed By Biplob 08/03//2020
        [Authorize]
        [HttpGet]
        [Route("C00202")]
        //Load Cheque Typpe for dropdown
        public IHttpActionResult SelectChequeStatus()
        {
            try
            {
                var data = new ManagerDefault()
                    .SelectChequeStatus();

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [Authorize]
        [HttpGet]
        [Route("C00205")]
        //Load Cheque Typpe for dropdown
        public IHttpActionResult SelectChequeStatusByGroup(string GRP)
        {
            try
            {
                var data = new ManagerDefault()
                    .SelectChequeStatusByGroup(GRP);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [Authorize]
        [HttpGet]
        [Route("C00206")]
        //Load Project
        public IHttpActionResult SelectReportName()
        {
            try
            {
                var data = new ManagerDefault()
                    .SelectReportName();

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [Authorize]
        [HttpGet]
        [Route("C00207")]
        //Load Project
        public IHttpActionResult SelectPositionOptionName()
        {
            try
            {
                var data = new ManagerDefault()
                    .SelectPositionOptionName();

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [Authorize]
        [HttpGet]
        [Route("C00208")]
        //Load Project
        public IHttpActionResult SelectChequeOrTreatementBankOption()
        {
            try
            {
                var data = new ManagerDefault()
                    .SelectChequeOrTreatementBankOption();

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [Authorize]
        [HttpGet]
        [Route("C00209")]
        //Load Project
        public IHttpActionResult SelectChequeCollectionOrPaymentDateOptionByGroup(string GRP)
        {
            try
            {
                var data = new ManagerDefault()
                    .SelectChequeCollectionOrPaymentDateOptionByGroup(GRP);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [Authorize]
        [HttpGet]
        [Route("C00210")]
        //Load all customer By Group for dropdown 
        public IHttpActionResult SelectAllCustomerByCustomerGroupForDropdown(string query ,long customerGroupId)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownSetupCustomer()
                    .SelectAllCustomerByCustomerGroupForDropdown(userInfo.CompanyId, query, customerGroupId);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [Authorize]
        [HttpGet]
        [Route("C00211")]
        //Load all customer Group By CompanyId for dropdown 
        public IHttpActionResult SelectCustomerGroupAllByCompanyIdForDropdown()
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownSetupCustomer()
                    .SelectCustomerGroupAllByCompanyIdForDropdown(userInfo.CompanyId);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [Authorize]
        [HttpGet]
        [Route("C00212")]
        //Load all Invoice No by Company for dropdown
        public IHttpActionResult SelectAllInvoiceNoByCompanyIdForDropdown(string query,long CustomerId, DateTime? dateFrom, DateTime? dateTo)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownSalesInvoice()
                    .SelectAllInvoiceNoByCompanyIdForDropdown(query, CustomerId, dateFrom, dateTo, userInfo.CompanyId, userInfo.LocationId);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [Authorize]
        [HttpGet]
        [Route("C00213")]
        //Load all Invoice No by Company for dropdown
        public IHttpActionResult SelectSalesInvoiceShortInfoByInvoiceId(string id = "")
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridTaskSalesInvoice()
                    .SelectSalesInvoiceShortInfoByInvoiceId((string.IsNullOrEmpty(id) ? Guid.Empty : new Guid(id)), userInfo.CompanyId);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [Authorize]
        [HttpGet]
        [Route("C00214")]
        //Load Product Company for dropdown
        public IHttpActionResult SelectProductNameByInvoice(string query = "",string InvoiceId = "")
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownSalesInvoice()
                    .SelectProductNameByInvoice(userInfo.CompanyId, query, (string.IsNullOrEmpty(InvoiceId) ? Guid.Empty : new Guid(InvoiceId)));

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [Authorize]
        [HttpGet]
        [Route("C00215")]
        //Load all sales Invoice info by product and Company for dropdown
        public IHttpActionResult SelectSalesInvoiceInfoByProduct(long ProductId,string ProductSerial)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridTaskSalesInvoice()
                    .SelectSalesInvoiceInfoByProduct(ProductId, ProductSerial, userInfo.CompanyId);                
                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [Authorize]
        [HttpGet]
        [Route("C00216")]
        //Load all warranty info by sales invoice, product and Company for dropdown
        public IHttpActionResult SelectSalesInvoiceWarrantyInfoByProduct(string InvoiceId = "", long ProductId = 0)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridTaskSalesInvoice()
                    .SelectSalesInvoiceWarrantyInfoByProduct((string.IsNullOrEmpty(InvoiceId) ? Guid.Empty : new Guid(InvoiceId)),ProductId, userInfo.CompanyId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [Authorize]
        [HttpGet]
        [Route("C00217")]
        //Load Product by serial from Invoicesales Company for dropdown
        public IHttpActionResult SelectProductBySerialFromInvoice(string serial,bool isReceiveAgainstPreviousSales, long ProductId,string InvoiceId)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownSalesInvoice()
                    .SelectProductBySerialFromInvoice(userInfo.CompanyId, userInfo.LocationId, serial, isReceiveAgainstPreviousSales, ProductId,(string.IsNullOrEmpty(InvoiceId) ? Guid.Empty : new Guid(InvoiceId)));

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [Authorize]
        [HttpGet]
        [Route("C00218")]
        //Load Product wise available stock, price and discount
        public IHttpActionResult SelectProductCost(long productId, long unitTypeId, long locationId, long wareHouseId = 0, long dimensionId = 0)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownSetupProduct()
                    .SelectProductCost(productId, dimensionId, unitTypeId, (locationId == 0 ? userInfo.LocationId : locationId), wareHouseId, userInfo.CompanyId);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [Authorize]
        [HttpGet]
        [Route("C00219")]
        //Load Product wise available stock, price and discount
        public IHttpActionResult SelectProductSerial(long productId, long unitTypeId, long locationId, long wareHouseId = 0, long dimensionId = 0)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownSetupProduct()
                    .SelectProductSerial(productId, dimensionId, unitTypeId, locationId, wareHouseId, userInfo.CompanyId);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [Authorize]
        [HttpGet]
        [Route("C00220")]
        //Load Product wise available stock, price and discount
        public IHttpActionResult SelectProductWiseAvailableStockPriceDiscountByOperationalEvent(string OperationalEvent, string OperationalSubEvent, string currency, long operationTypeId, long productId, long unitTypeId, long locationId, long wareHouseId = 0, long dimensionId = 0)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownSetupProduct()
                    .SelectProductWiseAvailableStockPriceDiscountByOperationalEvent(OperationalEvent, OperationalSubEvent, currency, operationTypeId, productId, dimensionId, unitTypeId, (locationId == 0 ? userInfo.LocationId: locationId), wareHouseId, userInfo.CompanyId);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        //Developed By Biplob 08/03//2020
        [Authorize]
        [HttpGet]
        [Route("C00221")]
        //Load Customer Delivery Product Option for dropdown
        public IHttpActionResult SelectRMAExchangeProductOption()
        {
            try
            {
                var data = new ManagerDefault()
                    .SelectRMAExchangeProductOption();

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [Authorize]
        [HttpGet]
        [Route("C00222")]
        //Load all Complain Receive info by product and Company
        public IHttpActionResult SelectComplainReceiveInfoByProduct(long ProductId, string ProductSerial,string ComplainReceiveId)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridTaskComplainReceive()
                    .SelectComplainReceiveInfoByProduct(ProductId, ProductSerial, userInfo.CompanyId, (string.IsNullOrEmpty(ComplainReceiveId) ? Guid.Empty : new Guid(ComplainReceiveId)));
                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [Authorize]
        [HttpGet]
        [Route("C00223")]
        //Load Product by serial from Invoicesales Company for dropdown
        public IHttpActionResult SelectProductBySerial(string serial, long ProductId)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownSetupProduct()
                    .SelectProductBySerial(userInfo.CompanyId, userInfo.LocationId, serial, ProductId);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("C00225")]
        //Load Cheque Typpe for dropdown
        public IHttpActionResult SelectSpareType()
        {
            try
            {
                var data = new ManagerDefault()
                    .SelectSpareType();

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [Authorize]
        [HttpGet]
        [Route("C00226")]
        //Load all Invoice No by Company for dropdown
        public IHttpActionResult SelectComplainReceiveShortInfoById(string id = "")
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridTaskComplainReceive()
                    .SelectComplainReceiveShortInfoById((string.IsNullOrEmpty(id) ? Guid.Empty : new Guid(id)), userInfo.CompanyId);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [Authorize]
        [HttpGet]
        [Route("C00227")]
        //Load Product by RMA serial for dropdown
        public IHttpActionResult SelectProductByRMASerial(string serial, long ProductId)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownSetupProduct()
                    .SelectProductByRMASerial(userInfo.CompanyId, userInfo.LocationId, serial, ProductId);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [Authorize]
        [HttpGet]
        [Route("C00228")]
        //Load Product Company for dropdown
        public IHttpActionResult SelectProductNameFromRMAByCompanyId(string query = "")
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownSetupProduct()
                    .SelectProductNameFromRMAByCompanyId(userInfo.CompanyId, query);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [Authorize]
        [HttpGet]
        [Route("C00229")]
        //Generate Product Serial
        public IHttpActionResult GenerateProductSerial(long ProductId,int serialLength)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownSetupProduct()
                    .GenerateProductSerial(userInfo.CompanyId, userInfo.LocationId,ProductId, serialLength);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [Authorize]
        [HttpGet]
        [Route("C00230")]
        //Load Product Stock In Reference Info
        public IHttpActionResult SelectProductStockInReferenceInfo(long productId,string serial,long supplierId)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownSetupProduct()
                    .SelectProductStockInReferenceInfo(userInfo.CompanyId, productId,serial, supplierId);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [Authorize]
        [HttpGet]
        [Route("C00234")]
        //Load all Complain Receive No by Company for dropdown
        public IHttpActionResult SelectComplainReceiveNoForDropdown(string query)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownComplainReceive()
                    .SelectComplainReceiveNoByCompanyIdForDropdown(userInfo.CompanyId, userInfo.LocationId, query);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [Authorize]
        [HttpGet]
        [Route("C00235")]
        //Load all Complain Receive No by Company for dropdown
        public IHttpActionResult SelectReplacementReceiveNumberForDropdown(string query)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownReplacementReceive()
                    .SelectReplacementReceiveNumberForDropdown(userInfo.CompanyId, userInfo.LocationId, query);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [Authorize]
        [HttpPost]
        [Route("C00236")]
        //Load Product Company for dropdown
        //public IHttpActionResult SelectProductNameExceptExistingByCompanyId(CommonSetupProductDimension entityData, string query)
        public IHttpActionResult SelectProductNameExceptExistingProductByCompanyId(CommonSetupProductSearch entityData)
        {
            try
            { 
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownSetupProduct()
                    .SelectProductNameExceptExistingProductByCompanyId(userInfo.CompanyId, entityData);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [Authorize]
        [HttpPost]
        [Route("C00237")]
        //Load Product Group ExceptExistingProduct by Company for dropdown
        public IHttpActionResult SelectProductGroupExceptExistingProductByCompanyId(CommonSetupProductSearch entityData)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownSetupProduct()
                    .SelectProductGroupExceptExistingProductByCompanyId(entityData.groupId, entityData.subGroupId, entityData.categoryId, entityData.brandId, entityData.model, entityData.query, userInfo.CompanyId,entityData);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        private UserIdentityInfo GetUserInfoFromIdentity()
        {
            var identity = (ClaimsIdentity)User.Identity;
            return GetUserIdentityInfo.GetUserInfo(identity);
        }
    }
}