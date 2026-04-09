using ERPAPP.Models;
using static ERPAPP.Models.VendorsModel;

namespace ERPAPP.Interfaces
{
    public interface IVendorRepository
    {
        Task<int> GetVendorTransferNewNo();
        Task<GetVendorAddData> GetVendorAddData();
        Task<bool> InsertVendor(VendorsModel model);

        Task<List<GetVendorListModel>> GetVendorList(string searchVendor);

        Task<bool> CheckStateGSTMatch(string stateCode, string gstRegistrationNo);

        List<AddressDropdownModel> GetCityList(string city);
        AddressCityDetailModel GetCityDetail(string city);
        List<AddressPostCodeModel> GetPostCodeList(string city);
        AddressPostCodeDetailModel GetPostCodeDetail(string code);

        Task<GetVendorEditData> GetVendorEditData(string companyCode);

        Task<VendorEditDropDownModel> GetVendorEditDropDownData();

        Task<bool> VendorUnblock(string vendorNo);

        Task<bool> CheckVendorGSTRegistrationAlreadyExists(string GstRegistrationNo);

        Task<List<GetVendorSearchModel>> SearchVendor(string searchVendor);

        Task<VendorsEditModel?> GetVendorMasterDataWithMasterCode(string masterCode);
    }
}
