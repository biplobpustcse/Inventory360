namespace DAL.Interface.Delete.Setup
{
    public interface IDeleteSetupPriceDetail
    {
        bool DeletePriceDetail(long priceId, long companyId);
    }
}