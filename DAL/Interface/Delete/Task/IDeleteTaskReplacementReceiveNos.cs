namespace DAL.Interface.Delete.Task
{
    public interface IDeleteTaskReplacementReceiveNos
    {
        bool DeleteReplacementReceiveNos(string prefix, long year, long companyId);
    }
}