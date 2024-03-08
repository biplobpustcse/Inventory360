namespace DAL.Interface.Update.Task
{
    public interface IUpdateTaskItemRequisition
    {
        bool UpdateItemRequisitionForApprove(long approvedBy);
        bool UpdateItemRequisitionForCancel(string reason, long cancelledBy);
        bool UpdateItemRequisitionForIsSettled(bool value);
    }
}