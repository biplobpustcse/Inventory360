namespace DAL.Interface.Update.Task
{
    public interface IUpdateTaskCollection
    {
        bool UpdateCollectionForApprove(long approvedBy);
        bool UpdateCollectionForCancel(string reason, long cancelledBy);
    }
}