using ERPAPP.Models;

namespace ERPAPP.Interfaces
{
    public interface IFGItemRepository
    {
        Task<GetFGItemAddModel> GetFGItemAddData();

        Task<List<FGItemHSNModel>> GetFGItemHSNDataWithGSTGroupCode(string gstGroupCode);

        Task<int> GetItemsTransferNewNo();

        Task<List<ProductionBOMHeaderModel>> GetItem_ChangeUnitOfMeasure(string baseUnitOfMeasure);

        Task<bool> InsertFGItem(FGItemModel model);

        Task<string> GetFGItemCompanyLastNoUsedCompanyCode();
    }
}
