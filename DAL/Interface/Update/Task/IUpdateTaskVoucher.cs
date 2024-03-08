namespace DAL.Interface.Update.Task
{
    public interface IUpdateTaskVoucher
    {
        bool UpdateVoucherForApprove(long approvedBy);
        bool UpdateVoucherForCancel(string reason, long cancelledBy);
        bool UpdateVoucherForPosting(long postedBy);
    }
}
