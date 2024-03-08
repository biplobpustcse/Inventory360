﻿using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskDeliveryChallan
    {
        IQueryable<Task_DeliveryChallan> SelectDeliveryChallanAll();
    }
}