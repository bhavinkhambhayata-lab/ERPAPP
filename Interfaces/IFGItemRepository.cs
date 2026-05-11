using ERPAPP.Models;

namespace ERPAPP.Interfaces
{
    public interface IFGItemRepository
    {
        public Task<GetFGItemAddModel> GetFGItemAddData();

        Task<List<FAHSNModel>> GetFixedAssetHSNDataWithGSTGroupCode(string gstGroupCode);

        Task<int> GetItemsTransferNewNo();

        Task<List<ProductionBOMHeaderModel>> GetItem_ChangeUnitOfMeasure(string baseUnitOfMeasure);
    }
}
