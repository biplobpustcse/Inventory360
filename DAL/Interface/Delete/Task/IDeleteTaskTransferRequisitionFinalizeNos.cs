namespace DAL.Interface.Delete.Task
{
    public interface IDeleteTaskTransferRequisitionFinalizeNos
    {
        bool DeleteTransferRequisitionFinalizeNos(string prefix, long year, long companyId);
    }
}