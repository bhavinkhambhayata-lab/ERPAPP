using ERPAPP.Models;

namespace ERPAPP.Interfaces
{
    public interface IFixedAssetRepository
    {
        Task<int> GetFixedAssetTransferNewNo();
        Task<GetFixedAssetAddData> GetFixedAssetAddData();

        Task<List<FAHSNModel>> GetFixedAssetHSNDataWithGSTGroupCode(string gstGroupCode);


    }
}
