using ERPAPP.Models;

namespace ERPAPP.Interfaces
{
    public interface IEmailRepository
    {
        List<MailListDto> GetMailCustomerUnBlockList(string division);
        Task<bool> SendMailCustomerUnBlock(string division, CustomerEmailItemDto approvalData);

        List<MailListDto> GetMailVendorUnBlockList(string division);
        Task<bool> SendMailVendorUnBlock(string division, VendorEmailItemDto approvalData);



        public List<MailListDto> GetMailCustomerBlockList(string division, int displayNo);

        Task<bool> SendMailCustomerBlock(string division, int displayNo, string customerCode, string customerName);


        List<MailListDto> GetMailVendorBlockList(string division, int displayNo);
        Task<bool> SendMailVendorBlock(string division, int displayNo, string vendorCode, string vendorName);

        VendorLocationEmail GetVendorSendEmailDetailByLocationCode(string code);

        Task<bool> SendMailFixedAssetUnBlock(FixedAssetEmailItem model);

        List<MailListDto> GetMailFixedAssetUnBlockList();

        List<MailListDto> GetMailFixedAssetBlockList(int displayNo);

        Task<bool> SendMailFixedAssetBlock(int displayNo, string fixedAssetCode, string fixedDescription);

    }
}
