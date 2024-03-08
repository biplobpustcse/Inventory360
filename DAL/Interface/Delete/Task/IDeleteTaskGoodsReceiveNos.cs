namespace DAL.Interface.Delete.Task
{
    public interface IDeleteTaskGoodsReceiveNos
    {
        bool DeleteGoodsReceiveNos(string prefix, long year, long companyId);
    }
}