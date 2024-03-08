﻿using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Setup
{
    public interface ISelectSetupSupplier
    {
        IQueryable<Setup_Supplier> SelectSupplierAll();
        IQueryable<Setup_Supplier> SelectSupplierWithoutCheckingCompany();
    }
}