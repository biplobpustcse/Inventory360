namespace DAL.Interface.Delete.Task
{
    public interface IDeleteTaskComplainReceiveNos
    {
        bool DeleteComplainReceiveNos(string prefix, long year, long companyId);
    }
}