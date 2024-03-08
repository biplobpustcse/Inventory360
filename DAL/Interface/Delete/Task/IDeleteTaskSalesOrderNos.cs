namespace DAL.Interface.Delete.Task
{
    public interface IDeleteTaskSalesOrderNos
    {
        bool DeleteSalesOrderNos(string prefix, long year, long companyId);
    }
}