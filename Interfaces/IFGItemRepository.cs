using ERPAPP.Models;

namespace ERPAPP.Interfaces
{
    public interface IFGItemRepository
    {
        Task<GetFGItemAddModel> GetFGItemAddData();

        Task<List<FGItemHSNModel>> GetFGItemHSNDataWithGSTGroupCode(string gstGroupCode,string category);

        Task<int> GetItemsTransferNewNo();

        Task<List<ProductionBOMHeaderModel>> GetItem_ChangeUnitOfMeasure(string baseUnitOfMeasure);

        Task<bool> InsertFGItem(FGItemModel model, string createdBy);

        Task<string> GetFGItemCompanyLastNoUsedCompanyCode();

        Task<List<UnitOfMeasureModel>> GetFGUnitOfMeasureDropDownData();

        Task<List<BrandModel>> GetFGBrandDropDownData();

        Task<string> GetWIPItemCompanyLastNoUsedCompanyCode();
        Task<FGItemDropDownEditModel> GetFGItemEditDropDownData();
    }
}
