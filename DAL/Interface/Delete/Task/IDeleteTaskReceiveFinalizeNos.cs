namespace DAL.Interface.Delete.Task
{
    public interface IDeleteTaskReceiveFinalizeNos
    {
        bool DeleteReceiveFinalizeNos(string prefix, long year, long companyId);
    }
}