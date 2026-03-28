using static ERPAPP.Models.VendorsModel;

namespace ERPAPP.Interfaces
{
    public interface IVendorRepository
    {
        Task<GetVendorAddData> GetVendorAddData();
    }
}
