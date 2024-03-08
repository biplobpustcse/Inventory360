using System;

namespace DAL.Interface.Update.Task
{
    public interface IUpdateTaskConvertionDetail
    {
        bool UpdateConvertionDetailReference(Guid? GoodsReceiveId, Guid? ImportedStockInId,long? SupplierId,string ReferenceNo,DateTime? ReferenceDate);
    }
}