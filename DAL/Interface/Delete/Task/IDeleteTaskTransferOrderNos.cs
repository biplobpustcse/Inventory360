namespace DAL.Interface.Delete.Task
{
    public interface IDeleteTaskTransferOrderNos
    {
        bool DeleteTaskTransferOrderNos(string prefix, long year, long companyId);
    }
}