﻿using Inventory360Entity;
using DAL.Interface.Select.Configuration;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Configuration
{
    public class DSelectConfigurationFormattingTag : ISelectConfigurationFormattingTag
    {
        private Inventory360Entities _db;

        public DSelectConfigurationFormattingTag()
        {
            _db = new Inventory360Entities();
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Configuration_FormattingTag> SelectFormattingTagAll()
        {
            return _db.Configuration_FormattingTag;
        }
    }
}