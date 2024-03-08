using System;
using System.Collections.Generic;

namespace DAL.Interface.Delete.Stock
{
    public interface IDeleteTaskCurrentStockSerial
    {
        bool DeleteCurrentStockSerial(List<Guid> serialListIds);
    }
}