namespace DAL.Interface.Update.Task
{
    public interface IUpdateTaskPayment
    {
        bool UpdatePaymentForApprove(long approvedBy);
        bool UpdatePaymentForCancel(string reason, long cancelledBy);
    }
}