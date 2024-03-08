namespace DAL.Interface.Delete.Task
{
    public interface IDeleteTaskCollectionNos
    {
        bool DeleteCollectionNos(string prefix, long year, long companyId);
    }
}