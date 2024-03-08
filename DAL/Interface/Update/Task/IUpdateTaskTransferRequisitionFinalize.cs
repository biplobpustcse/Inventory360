namespace DAL.Interface.Update.Task
{
    public interface IUpdateTaskTransferRequisitionFinalize
    {
        bool UpdateTransferRequisitionFinalizeForApprove(long approvedBy);
        bool UpdateTransferRequisitionFinalizeForCancel(string reason, long cancelledBy);
        bool UpdateTransferRequisitionFinalizeForIsSettled(bool status);
    }
}