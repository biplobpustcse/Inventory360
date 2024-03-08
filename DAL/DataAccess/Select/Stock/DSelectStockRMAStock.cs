using Inventory360DataModel;
using Inventory360Entity;
using DAL.Interface.Select.Stock;
using System;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Stock
{
    public class DSelectStockRMAStock : ISelectStockRMAStock
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectStockRMAStock(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Stock_RMAStock> SelectRMAStockAll()
        {
            return _db.Stock_RMAStock
                .Where(x => x.CompanyId == _companyId);
        }
        public object SelectRMAProductBySerialFromComplainReceive(long companyId, long LocationId, string serial, long ProductId, Guid complainReceiveId)
        {
            var value = (from rma in _db.Stock_RMAStock
                         join product in _db.Setup_Product.WhereIf(ProductId != 0, x => x.ProductId == ProductId)
                         on rma.ProductId equals product.ProductId
                         join RMAStockSerial in _db.Stock_RMAStockSerial.WhereIf(!string.IsNullOrEmpty(serial), x => x.Serial.ToLower().Contains(serial.ToLower()))
                         on rma.RMAStockId equals RMAStockSerial.RMAStockId
                         join cr in _db.Task_ComplainReceiveDetail
                         .WhereIf(complainReceiveId != Guid.Empty, x => x.Task_ComplainReceive.ReceiveId == complainReceiveId)
                          on RMAStockSerial.Serial equals cr.Serial into complainRcv
                         from complainReceive in complainRcv.DefaultIfEmpty()
                         select new
                         {
                             Item = RMAStockSerial.Serial.ToString(),
                             Value = RMAStockSerial.Serial.ToString(),
                             ProductName = "[" + product.Code + "] " + product.Name.ToString(),
                             ProductId = rma.ProductId.ToString(),
                             product.Code,
                             complainReceiveId = complainReceive.ReceiveId,
                             rma.Cost,
                             rma.Cost1,
                             rma.Cost2
                         }).ToList();

            return value;
        }
    }
}