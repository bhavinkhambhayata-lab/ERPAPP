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
    }
}
