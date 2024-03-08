namespace DAL.Interface.Delete.Setup
{
    public interface IDeleteSetupRelatedProduct
    {
        bool DeleteRelatedProduct(long productId, long relatedProductId, long companyId);
    }
}