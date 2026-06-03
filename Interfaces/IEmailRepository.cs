using ERPAPP.Models;

namespace ERPAPP.Interfaces
{
    public interface IEmailRepository
    {
        List<MailListDto> GetMailCustomerUnBlockList(string division, int displayNo);
        Task<bool> SendMailCustomerUnBlock(string division, CustomerEmailItemDto approvalData,int displayNo);

        List<MailListDto> GetMailVendorUnBlockList(string division, int displayNo);
        Task<bool> SendMailVendorUnBlock(string division, VendorEmailItemDto approvalData,int displayNo);



        public List<MailListDto> GetMailCustomerBlockList(string division, int displayNo);

        Task<bool> SendMailCustomerBlock(string division, int displayNo, string customerCode, string customerName);


        List<MailListDto> GetMailVendorBlockList(string division, int displayNo);
        Task<bool> SendMailVendorBlock(string division, int displayNo, string vendorCode, string vendorName);

        VendorLocationEmail GetVendorSendEmailDetailByLocationCode(string code);

        Task<bool> SendMailFixedAssetUnBlock(FixedAssetEmailItem model, int displayNo);

        List<MailListDto> GetMailFixedAssetUnBlockList(int displayNo);

        List<MailListDto> GetMailFixedAssetBlockList(int displayNo);

        Task<bool> SendMailFixedAssetBlock(int displayNo, string fixedAssetCode, string fixedDescription);

        //FG Item Creation Mail

        Task<ERPEmailConfigurationModel> GetERPEmailConfiguration(string moduleName, string type);
        Task<List<FGItemEmailDetailModel>> GetFGItemsEmailList(string itemCodes);
        Task<List<ERPEmailConfigurationModel>> GetFGItemMailConfigurationList(string type, string createdUser);
        Task<bool> SendFGItemCreationMail(List<FGItemEmailDetailModel> itemList, string createdUser);
    }
}
