﻿using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Setup
{
    public interface ISelectSetupUnitType
    {
        IQueryable<Setup_UnitType> SelectUnitTypeAll();
    }
}
