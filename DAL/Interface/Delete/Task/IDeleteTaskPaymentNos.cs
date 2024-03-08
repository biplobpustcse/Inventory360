namespace DAL.Interface.Delete.Task
{
    public interface IDeleteTaskPaymentNos
    {
        bool DeletePaymentNos(string prefix, long year, long companyId);
    }
}