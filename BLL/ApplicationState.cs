using System;
using System.Transactions;

namespace BLL
{
    public static class ApplicationState
    {
        private static TransactionOptions _transactionOptions;
        public const IsolationLevel TRANSACTION_ISOLATION_LEVEL = IsolationLevel.ReadCommitted;

        public static TransactionOptions TransactionOptions
        {
            get
            {
                if (_transactionOptions == null || _transactionOptions.IsolationLevel != TRANSACTION_ISOLATION_LEVEL)
                {
                    _transactionOptions = new TransactionOptions();
                    _transactionOptions.IsolationLevel = TRANSACTION_ISOLATION_LEVEL;
                    _transactionOptions.Timeout = new TimeSpan(1, 0, 30);
                }
                return _transactionOptions;
            }
            set
            {
                _transactionOptions = value;
            }
        }
    }
}
