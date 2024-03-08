using Inventory360Entity;
using DAL.Interface.StoredProcedures;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.StoredProcedures
{
    public class DExecuteCustomerOrSupplierWiseChequePerformance : IExecuteCustomerOrSupplierWiseChequePerformance
    {
        private Inventory360Entities _db;
        private long _companyId;
        private string _currency;
        private string _chequeType;
        private long _locationId;
        private long _bankId;
        private long _customerOrSupplierId;
        private DateTime? _dateFrom;
        private DateTime? _dateTo;
        private string _chequeOrTreatementBankOptionValue;
        private string _chequeCollectionOrPaymentDateOptionValue;
        private long _entryBy;
        

        //public DExecuteCustomerOrSupplierWiseChequePerformance(string currency, long entryBy, long companyId, DateTime dateFrom, DateTime dateTo, long supplierId)
        public DExecuteCustomerOrSupplierWiseChequePerformance(long companyId, long entryBy, string currency, string dataPositionOptionValue, string chequeType, long locationId, long bankId, long customerOrSupplierId, DateTime? dateFrom, DateTime? dateTo, string chequeStatusCode, string chequeOrTreatementBankOptionValue, string chequeCollectionOrPaymentDateOptionValue)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
            _entryBy = entryBy;
            _currency = currency;
            _chequeType = chequeType;
            _locationId = locationId;
            _bankId = bankId;
            _customerOrSupplierId = customerOrSupplierId;
            _dateFrom = dateFrom;
            _dateTo = dateTo;
            _chequeOrTreatementBankOptionValue = chequeOrTreatementBankOptionValue;
            _chequeCollectionOrPaymentDateOptionValue = chequeCollectionOrPaymentDateOptionValue;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public void ExecuteCustomerOrSupplierWiseChequePerformance()
        {
            _db.Database.ExecuteSqlCommand("EXEC dbo.SP_CustomerOrSupplierWiseChequePerformance " +
                "@CompanyId = " + _companyId + "," +
                "@Currency = N'" + _currency + "'," +
                "@ChequeType = N'" + _chequeType + "'," +
                "@LocationId = " + _locationId + "," +
                "@BankId = " + _bankId + "," +
                "@CustomerOrSupplierId = " + _customerOrSupplierId + "," +
                "@DateFrom = '" + _dateFrom + "'," +
                "@DateTo = '" + _dateTo + "'," +
                "@ChequeOrTreatementBankOptionValue = N'" + _chequeOrTreatementBankOptionValue + "'," +
                "@ChequeCollectionOrPaymentDateOptionValue = N'" + _chequeCollectionOrPaymentDateOptionValue + "'," +
                "@EntryBy = " + _entryBy + " ");
        }
    }
}