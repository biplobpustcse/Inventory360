namespace DAL.Interface.Update.Task
{
    public interface IUpdateTaskRequisitionFinalize
    {
        bool UpdateRequisitionFinalizeForApprove(long approvedBy);
        bool UpdateRequisitionFinalizeForCancel(string reason, long cancelledBy);
        bool UpdateRequisitionFinalizeForIsSettled(bool value);
    }
}