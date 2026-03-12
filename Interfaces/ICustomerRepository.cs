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
        List<AddressDropdownModel> GetCustomerCityList(string city);
        AddressCityDetailModel GetCityDetail(string city);
        List<AddressPostCodeModel> GetPostCodeList(string city);
        AddressPostCodeDetailModel GetPostCodeDetail(string code);
        Task<bool> CheckStateGSTMatch(string stateCode, string gstRegistrationNo);
        Task<ModifyPermissionResult> CheckModifyPermission(int userRowId, int entryRowId);

        Task<CustomerBrandWiseModel> GetCustomerBrandWiseDropdown(int divisionRowId);

        Task<bool> InsertCustomer(CustomerModel model);

        List<LocationModel> GetLocationListByDivisionCode(int divisionCode);

        Task<GetCustomerDataWithPortalRowIdModel> GetCustomerDataWithPortalRowId(int portalRowId);
    }
}
