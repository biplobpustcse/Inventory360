namespace DAL.Interface.Delete.Task
{
    public interface IDeleteTaskItemRequisitionNos
    {
        bool DeleteItemRequisitionNos(string prefix, long year, long companyId);
    }
}