namespace DAL.Interface.Delete.Task
{
    public interface IDeleteTaskReplacementClaimNos
    {
        bool DeleteReplacementClaimNos(string prefix, long year, long companyId);
    }
}