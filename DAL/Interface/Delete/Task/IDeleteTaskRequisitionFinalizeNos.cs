namespace DAL.Interface.Delete.Task
{
    public interface IDeleteTaskRequisitionFinalizeNos
    {
        bool DeleteRequisitionFinalizeNos(string prefix, long year, long companyId);
    }
}