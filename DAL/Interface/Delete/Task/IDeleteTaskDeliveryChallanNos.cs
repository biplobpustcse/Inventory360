namespace DAL.Interface.Delete.Task
{
    public interface IDeleteTaskDeliveryChallanNos
    {
        bool DeleteDeliveryChallanNos(string prefix, long year, long companyId);
    }
}