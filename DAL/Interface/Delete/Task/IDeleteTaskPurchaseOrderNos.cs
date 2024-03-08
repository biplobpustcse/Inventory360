namespace DAL.Interface.Delete.Task
{
    public interface IDeleteTaskPurchaseOrderNos
    {
        bool DeletePurchaseOrderNos(string prefix, long year, long companyId);
    }
}