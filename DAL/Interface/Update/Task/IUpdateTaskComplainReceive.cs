namespace DAL.Interface.Update.Task
{
    public interface IUpdateTaskComplainReceive
    {
        bool UpdateComplainReceiveForApprove(long approvedBy);
        bool UpdateComplainReceiveForCancel(string reason, long cancelledBy);
    }
}