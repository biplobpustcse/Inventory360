namespace DAL.Interface.Delete.Task
{
    public interface IDeleteTaskSalesInvoiceNos
    {
        bool DeleteSalesInvoiceNos(string prefix, long year, long companyId);
    }
}