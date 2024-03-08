namespace DAL.Interface.Delete.Task
{
    public interface IDeleteTaskStockAdjustmentNos
    {
        bool DeleteStockAdjustmentNos(string prefix, long year, long companyId);
    }
}