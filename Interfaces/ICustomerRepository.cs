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

        Task<bool> InsertCustomer(CustomerModel model,string userName);

        List<LocationModel> GetLocationListByDivisionCode(int divisionCode);

        Task<GetCustomerDataWithPortalRowIdModel> GetCustomerDataWithPortalRowId(int portalRowId);

        Task<List<GetCustomerListModel>> GetCustomerList(string searchCustomer);

        Task<GetCustomerEditModel> GetCustomerEditData(string customerNo);

        Task<GetCustomerEditModel> GetCustomerEditDropDownData();

        GetCustomerDivisionWiseDropDown GetCustomerDivisionWiseDropDown(string division);

        Task<bool> EditCustomerBrandWiseOnly(List<CustomerBrandWiseEditModel> model,string SalesPersonCode);

        Task<bool> CheckCustomerEntryAlreadyExists(string masterCode, string dimension);

        Task<bool> CheckCustomerGSTRegistrationAlreadyExists(string GstRegistrationNo);

        Task<bool> CustomerUnblock(string customerNo,string displayRowId,int loginRowId);

        Task<List<CountryModel>> GetCustomerCountryList(string country);
        Task<List<PostCodeModel>> GetCustomerPostCodeListWithSearch(string city, string searchpostcode);

        Task<GetAssessCodeWithPlaceModel> GetAssessCodeWithPlace(string Place);

        Task<GetCustomerUnBlockEditModel> GetCustomerUnBlockEditData(string customerNo);

        Task<bool> UpdateCustomer(CustomerUnBlockEditModel model);

        Task<CustomerAlreadyExistModel> GetCustomerAlreadyExistDetails(string masterCode);

    }
}
