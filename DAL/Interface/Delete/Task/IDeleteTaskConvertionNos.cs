namespace DAL.Interface.Delete.Task
{
    public interface IDeleteTaskConvertionNos
    {
        bool DeleteConvertionNos(string prefix, long year, long companyId);
    }
}