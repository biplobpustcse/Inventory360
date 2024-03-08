namespace DAL.Interface.Update.Task
{
    public interface IUpdateTaskDeliveryChallan
    {
        bool UpdateDeliveryChallanForApprove(long approvedBy);
        bool UpdateDeliveryChallanForCancel(string reason, long cancelledBy);
        bool UpdateDeliveryChallanForIsSettled(bool value);
    }
}