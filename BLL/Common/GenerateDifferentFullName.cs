using DAL.DataAccess.Select.Configuration;
using DAL.DataAccess.Select.Setup;
using DAL.Interface.Select.Configuration;
using DAL.Interface.Select.Setup;
using System;
using System.Linq;

namespace BLL.Common
{
    public static class GenerateDifferentFullName
    {
        public static string GenerateProductDimensionFull(long? productDimensionId, long companyId)
        {
            try
            {
                string dimensionFull = string.Empty;

                if (productDimensionId == null || productDimensionId == 0)
                    return dimensionFull;

                ISelectSetupProductDimension iSelectSetupProductDimension = new DSelectSetupProductDimension(companyId);
                var dimensionInfo = iSelectSetupProductDimension.SelectProductDimensionAll()
                    .Where(x => x.ProductDimensionId == productDimensionId)
                    .Select(s => new
                    {
                        Measurement = s.MeasurementId == null ? string.Empty : s.Setup_Measurement.Name,
                        Size = s.SizeId == null ? string.Empty : s.Setup_Size.Name,
                        Style = s.StyleId == null ? string.Empty : s.Setup_Style.Name,
                        Color = s.ColorId == null ? string.Empty : s.Setup_Color.Name
                    })
                    .FirstOrDefault();

                dimensionFull += string.IsNullOrEmpty(dimensionInfo.Measurement) ? string.Empty : "Measurement : " + dimensionInfo.Measurement;
                dimensionFull += string.IsNullOrEmpty(dimensionInfo.Size) ? string.Empty :
                    ((string.IsNullOrEmpty(dimensionFull) ? string.Empty : " # ") + "Size : " + dimensionInfo.Size);
                dimensionFull += string.IsNullOrEmpty(dimensionInfo.Style) ? string.Empty :
                    ((string.IsNullOrEmpty(dimensionFull) ? string.Empty : " # ") + "Style : " + dimensionInfo.Style);
                dimensionFull += string.IsNullOrEmpty(dimensionInfo.Color) ? string.Empty :
                    ((string.IsNullOrEmpty(dimensionFull) ? string.Empty : " # ") + "Color : " + dimensionInfo.Color);

                return dimensionFull;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GenerateCustomerFull(long customerId, long companyId)
        {
            try
            {
                string customerFull = string.Empty;
                ISelectConfigurationCode iSelectConfigurationCode = new DSelectConfigurationCode(companyId);

                var codeInfo = iSelectConfigurationCode.SelectCodeAll()
                    .Where(x => x.FormName.ToLower().Equals("Customer".ToLower()))
                    .Select(s => new
                    {
                        s.IsCodeVisible
                    })
                    .FirstOrDefault();

                ISelectSetupCustomer iSelectSetupCustomer = new DSelectSetupCustomer(companyId);
                var customerInfo = iSelectSetupCustomer.SelectCustomerAll()
                    .Where(x => x.CustomerId == customerId)
                    .Select(s => new
                    {
                        s.Name,
                        s.PhoneNo,
                        s.Code
                    })
                    .FirstOrDefault();

                customerFull += codeInfo.IsCodeVisible ? customerInfo.Code : string.Empty;
                customerFull += (string.IsNullOrEmpty(customerFull) ? string.Empty : " # ") + customerInfo.Name;
                customerFull += (string.IsNullOrEmpty(customerFull) ? string.Empty : " # ") + customerInfo.PhoneNo;

                return customerFull;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GenerateEmployeeFull(long employeeId, long companyId)
        {
            try
            {
                string employeeFull = string.Empty;
                ISelectConfigurationCode iSelectConfigurationCode = new DSelectConfigurationCode(companyId);

                var codeInfo = iSelectConfigurationCode.SelectCodeAll()
                    .Where(x => x.FormName.ToLower().Equals("Employee".ToLower()))
                    .Select(s => new
                    {
                        s.IsCodeVisible
                    })
                    .FirstOrDefault();

                ISelectSetupEmployee iSelectSetupEmployee = new DSelectSetupEmployee(companyId);
                var employeeInfo = iSelectSetupEmployee.SelectEmployeeAll()
                    .Where(x => x.EmployeeId == employeeId)
                    .Select(s => new
                    {
                        s.Name,
                        s.ContactNo,
                        s.Code
                    })
                    .FirstOrDefault();

                employeeFull += codeInfo.IsCodeVisible ? employeeInfo.Code : string.Empty;
                employeeFull += (string.IsNullOrEmpty(employeeFull) ? string.Empty : " # ") + employeeInfo.Name;
                employeeFull += (string.IsNullOrEmpty(employeeFull) ? string.Empty : " # ") + employeeInfo.ContactNo;

                return employeeFull;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GenerateSupplierFull(long supplierId, long companyId)
        {
            try
            {
                string supplierFull = string.Empty;
                ISelectConfigurationCode iSelectConfigurationCode = new DSelectConfigurationCode(companyId);

                var codeInfo = iSelectConfigurationCode.SelectCodeAll()
                    .Where(x => x.FormName.ToLower().Equals("Supplier".ToLower()))
                    .Select(s => new
                    {
                        s.IsCodeVisible
                    })
                    .FirstOrDefault();

                ISelectSetupSupplier iSelectSetupSupplier = new DSelectSetupSupplier(companyId);
                var employeeInfo = iSelectSetupSupplier.SelectSupplierAll()
                    .Where(x => x.SupplierId == supplierId)
                    .Select(s => new
                    {
                        s.Name,
                        s.Phone,
                        s.Code
                    })
                    .FirstOrDefault();

                supplierFull += codeInfo.IsCodeVisible ? employeeInfo.Code : string.Empty;
                supplierFull += (string.IsNullOrEmpty(supplierFull) ? string.Empty : " # ") + employeeInfo.Name;
                supplierFull += (string.IsNullOrEmpty(supplierFull) ? string.Empty : " # ") + employeeInfo.Phone;

                return supplierFull;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}