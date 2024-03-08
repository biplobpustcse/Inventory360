namespace DAL.Interface.Update.Task
{
    public interface IUpdateTaskConvertion
    {
        bool UpdateConvertionForApprove(long approvedBy);
        bool UpdateConvertionForCancel(string reason, long cancelledBy);
    }
}