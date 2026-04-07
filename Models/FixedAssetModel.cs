using System.ComponentModel.DataAnnotations;

namespace ERPAPP.Models
{
    public class FixedAssetModel
    {
        public int RowID { get; set; }
        public int Brand { get; set; }
        public int DisplayNo { get; set; }
        [Required(ErrorMessage = "Division is required")]
        public int Division { get; set; }
        public string? DivisionStr { get; set; }
        [Required(ErrorMessage = "FA Class Code is required")]
        public string FAClassCode { get; set; } = string.Empty;
        [Required(ErrorMessage = "FA Subclass Code is required")]
        public string FASubclassCode { get; set; } = string.Empty;
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = string.Empty;
        public string? Description2 { get; set; }
        public string? SerialNo { get; set; }
        public int? MainAssetComponent { get; set; }
        public string? ComponentOfMainAsset { get; set; }
        public string? LocationCode { get; set; }
        public string? FALocationCode { get; set; }
        [Required(ErrorMessage = "Gen Prod Posting Group is required")]
        public string GenProdPostingGroup { get; set; } = string.Empty;
        [Required(ErrorMessage = "FA Posting Group is required")]
        public string FAPostingGroup { get; set; } = string.Empty;
        public int? ExciseAccountingType { get; set; }
        public string? TaxGroupCode { get; set; }
        public string? VATProductPostingGroup { get; set; }
        public string? DepreciationBookCode { get; set; }
        //[Required(ErrorMessage = "Depreciation Method is required")]
        public int? DepreciationMethod { get; set; }
        public DateTime? DepreciationStartingDate { get; set; }
        [Range(0, 100, ErrorMessage = "Straight Line % must be between 0 and 100")]
        public double? StraightLinePercent { get; set; }
        [Range(0, 100, ErrorMessage = "Declining Balance % must be between 0 and 100")]
        public double? DecliningBalancePercent { get; set; }
        public DateTime? InstallationDate { get; set; }
        public string? MasterCode { get; set; }
        public string? CompanyCode { get; set; }

        //public int SendRequestforApproval { get; set; }
        //public DateTime? SendRequestforApprovalLog { get; set; }

        //public int IsPopupDisplayed { get; set; }
        public int LoginRowID { get; set; }
        public string? GSTGroupCode { get; set; }
        public string? HSNSACCode { get; set; }
    }

    public class GetFixedAssetAddData : FixedAssetModel
    {
        public FixedAssetDropDownModel DropDownData { get; set; } = new();
    }

    public class FAClassModel : BaseDropDown { }
    public class FASubClassModel : BaseDropDown { }
    public class FALocationModel : BaseDropDown { }
    public class GenProductPostingGroupModel : BaseDropDown { }
    public class FAPostingGroupModel : BaseDropDown { }
    public class GSTGroupModel : BaseDropDown { }
    public class TaxGroupModel : BaseDropDown { }
    public class VATProductPostingGroupModel : BaseDropDown { }
    public class MainAssetComponentModel : BaseDropDown { }
    public class DepreciationMethodModel : BaseDropDown { }
    public class ExciseAccountingTypeModel : BaseDropDown { }
    public class DepreciationBookCodeModel : BaseDropDown { }
    public class FAHSNModel : BaseDropDown { }
    public class FixedAssetDivisionModel : BaseDropDown { }

    public class FixedAssetComponetOfMainAssetModel : BaseDropDown { }

    public class FixedAssetDropDownModel
    {
        public List<FAClassModel> FAClass { get; set; } = new();
        public List<FASubClassModel> FASubClass { get; set; } = new();
        public List<LocationModel> Location { get; set; } = new();
        public List<FALocationModel> FALocation { get; set; } = new();
        public List<GenProductPostingGroupModel> GenProductPostingGroup { get; set; } = new();
        public List<FAPostingGroupModel> FAPostingGroup { get; set; } = new();
        public List<GSTGroupModel> GSTGroup { get; set; } = new();
        public List<TaxGroupModel> TaxGroup { get; set; } = new();
        public List<VATProductPostingGroupModel> VATProductPostingGroup { get; set; } = new();
        public List<MainAssetComponentModel> MainAssetComponentList { get; set; } = new();
        public List<DepreciationMethodModel> DepreciationMethodList { get; set; } = new();
        public List<ExciseAccountingTypeModel> ExciseAccountingTypeList { get; set; } = new();
        public List<DepreciationBookCodeModel> DepreciationBookCode { get; set; } = new();
        public List<FixedAssetDivisionModel> FixedAssetDivision { get; set; } = new();
    }

    public class GetFixedAssetListModel
    {
        public string? RowID { get; set; }
        public int DisplayNo { get; set; }
        public string? Division { get; set; }

        public string? Description { get; set; }
        public string? FAClassCode { get; set; }
        public string? FASubClassCode { get; set; }
        public string? LocationCode { get; set; }
        public string? CompanyCode { get; set; }
        public string? CreatedBy { get; set; }
    }


    public class FixedAssetEditDropDownModel
    {
        public List<FAClassModel> FAClass { get; set; } = new();
        public List<FASubClassModel> FASubClass { get; set; } = new();
        public List<LocationModel> Location { get; set; } = new();
        public List<FALocationModel> FALocation { get; set; } = new();
        public List<GenProductPostingGroupModel> GenProductPostingGroup { get; set; } = new();
        public List<FAPostingGroupModel> FAPostingGroup { get; set; } = new();
        public List<GSTGroupModel> GSTGroup { get; set; } = new();
        public List<TaxGroupModel> TaxGroup { get; set; } = new();
        public List<VATProductPostingGroupModel> VATProductPostingGroup { get; set; } = new();
        public List<MainAssetComponentModel> MainAssetComponentList { get; set; } = new();
        public List<DepreciationMethodModel> DepreciationMethodList { get; set; } = new();
        public List<ExciseAccountingTypeModel> ExciseAccountingTypeList { get; set; } = new();
        public List<DepreciationBookCodeModel> DepreciationBookCode { get; set; } = new();
        public List<FixedAssetDivisionModel> FixedAssetDivision { get; set; } = new();
        public List<FixedAssetComponetOfMainAssetModel> ComponentOfMainAssetList { get; set; } = new();
        public List<FAHSNModel> FAHSNList { get; set; } = new();
    }


    public class FixedAssetEditModel
    {
        public int RowID { get; set; }
        public int Brand { get; set; }
        public int DisplayNo { get; set; }
        [Required(ErrorMessage = "Division is required")]
        public int Division { get; set; }
        public string? DivisionStr { get; set; }
        [Required(ErrorMessage = "FA Class Code is required")]
        public string FAClassCode { get; set; } = string.Empty;
        [Required(ErrorMessage = "FA Subclass Code is required")]
        public string FASubclassCode { get; set; } = string.Empty;
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = string.Empty;
        public string? Description2 { get; set; }
        public string? SerialNo { get; set; }
        public int? MainAssetComponent { get; set; }
        public string? ComponentOfMainAsset { get; set; }
        public string? LocationCode { get; set; }
        public string? FALocationCode { get; set; }
        [Required(ErrorMessage = "Gen Prod Posting Group is required")]
        public string GenProdPostingGroup { get; set; } = string.Empty;
        [Required(ErrorMessage = "FA Posting Group is required")]
        public string FAPostingGroup { get; set; } = string.Empty;
        public int? ExciseAccountingType { get; set; }
        public string? TaxGroupCode { get; set; }
        public string? VATProductPostingGroup { get; set; }
        public string? DepreciationBookCode { get; set; }
        //[Required(ErrorMessage = "Depreciation Method is required")]
        public int? DepreciationMethod { get; set; }
        public DateTime? DepreciationStartingDate { get; set; }
        [Range(0, 100, ErrorMessage = "Straight Line % must be between 0 and 100")]
        public double? StraightLinePercent { get; set; }
        [Range(0, 100, ErrorMessage = "Declining Balance % must be between 0 and 100")]
        public double? DecliningBalancePercent { get; set; }
        public DateTime? InstallationDate { get; set; }
        public string? MasterCode { get; set; }
        public string? CompanyCode { get; set; }

        //public int SendRequestforApproval { get; set; }
        //public DateTime? SendRequestforApprovalLog { get; set; }

        //public int IsPopupDisplayed { get; set; }
        public int LoginRowID { get; set; }
        public string? GSTGroupCode { get; set; }
        public string? HSNSACCode { get; set; }

        public int Blocked { get; set; } = 0;
    }

    public class GetFixedAssetEditData : FixedAssetEditModel
    {
        public FixedAssetEditDropDownModel DropDownData { get; set; } = new();
    }
}
