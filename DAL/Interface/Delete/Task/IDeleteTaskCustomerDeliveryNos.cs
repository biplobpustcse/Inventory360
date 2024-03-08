namespace DAL.Interface.Delete.Task
{
    public interface IDeleteTaskCustomerDeliveryNos
    {
        bool DeleteCustomerDeliveryNos(string prefix, long year, long companyId);
    }
}