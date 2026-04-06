using ERPAPP.Models;

namespace ERPAPP.Interfaces
{
    public interface IFixedAssetRepository
    {
        Task<int> GetFixedAssetTransferNewNo();
        Task<GetFixedAssetAddData> GetFixedAssetAddData();

        Task<List<FAHSNModel>> GetFixedAssetHSNDataWithGSTGroupCode(string gstGroupCode);

        Task<List<FixedAssetComponetOfMainAssetModel>> GetFixedAssetComponentWithDivision(string division);

        Task<bool> InsertFixedAssetData(FixedAssetModel model);

        Task<List<GetFixedAssetListModel>> GetFixedAssetList(string searchDescription);

        Task<GetFixedAssetEditData> GetFixedAssetEditData(string fixedAssetNo);

        Task<GetFixedAssetEditData> GetFixedAssetEditDropDownData();
    }
}
