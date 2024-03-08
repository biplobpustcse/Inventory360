namespace DAL.Interface.Update.Task
{
    public interface IUpdateTaskTransferOrder
    {
        bool UpdateTransferOrderForApprove(long approvedBy);
        bool UpdateTransferOrderForCancel(string reason, long cancelledBy);
        bool UpdateTransferOrderForIsSettled(bool value);
    }
}