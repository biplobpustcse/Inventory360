using DAL.DataAccess.Update;
using DAL.Interface.Update;
using System;

namespace BLL.Update
{
    public class UpdateDatabaseUpdate
    {
        public bool UpdateDatabase()
        {
            try
            {
                IUpdateDatabaseUpdate iUpdateDatabaseUpdate = new DUpdateDatabaseUpdate();
                return iUpdateDatabaseUpdate.UpdateDatabase();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
