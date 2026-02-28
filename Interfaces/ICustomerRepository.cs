using ERPAPP.Models;

namespace ERPAPP.Interfaces
{
    public interface ICustomerRepository
    {
        Task<GetCustomerAddModel> GetCustomerAddData();
        Task<int> GetCustomerTransferNewNo();
        Task<List<CustomerSearchModel>> SearchCustomer(string searchText);
        Task<CustomerMasterModel> GetCustomerMaster(string customerName);
        Task<bool> CheckCustomerInMasterAndBrand(string masterCode, int brandId);

        List<AddressDropdownModel> GetCustomerAddress(string type, string countryCode, string state, string city, string code);

        AddressDetailModel GetPostCodeDetail(string postCode);

        Task<bool> CheckStateGSTMatch(string stateCode, string gstRegistrationNo);

        Task<ModifyPermissionResult> CheckModifyPermission(int userRowId, int entryRowId);
    }
}
