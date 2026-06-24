using System.ComponentModel.DataAnnotations;

namespace ERPAPP.Models
{
    public class FGItemModel
    {
        public int DisplayNo { get; set; }

        // ---------------- TextBox Fields ----------------
        public string? CompanyCode { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(100)]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description 2 is required")]
        [StringLength(50)]
        public string Description2 { get; set; } = string.Empty;

        [Required(ErrorMessage = "Rounding Precision is required")]
        public decimal RoundingPrecision { get; set; }

        public decimal? GrossWeight { get; set; }
        public decimal? NetWeight { get; set; }

        // ---------------- DropDown Fields ----------------
        [Required(ErrorMessage = "Base Unit Of Measure is required")]
        public string BaseUnitOfMeasure { get; set; } = string.Empty;

        public string? ItemCategoryCode { get; set; }
        public string? ProductGroupCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Movement Type is required")]
        public string MovementType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Type Of Product is required")]
        public string TypeOfProduct { get; set; } = string.Empty;

        // ---------------- Tile Attributes ----------------
        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; } = string.Empty;

        [Required(ErrorMessage = "Size Of Tile is required")]
        public string SizeOfTile { get; set; } = string.Empty;

        [Required(ErrorMessage = "Collection is required")]
        public string Collection { get; set; } = string.Empty;

        [Required(ErrorMessage = "Surface Finish Or Glaze is required")]
        public string SurfaceFinishOrGlaze { get; set; } = string.Empty;

        [Required(ErrorMessage = "Glaze Effect is required")]
        public string GlazeEffect { get; set; } = string.Empty;

        [Required(ErrorMessage = "Design Color is required")]
        [StringLength(30)]
        public string DesignColor { get; set; } = string.Empty;

        public string? ColourFamily { get; set; } = string.Empty;

        [Required(ErrorMessage = "Type Of Tile is required")]
        public string TypeOfTile { get; set; } = string.Empty;

        [Required(ErrorMessage = "Packaging is required")]
        public string Packaging { get; set; } = string.Empty;

        [Required(ErrorMessage = "Thickness is required")]
        public string Thickness { get; set; } = string.Empty;

        [Required(ErrorMessage = "Body is required")]
        public string Body { get; set; } = string.Empty;

        [Required(ErrorMessage = "PL Collection is required")]
        public string PLCollection { get; set; } = string.Empty;

        [Required(ErrorMessage = "PL Colours is required")]
        public string PLColours { get; set; } = string.Empty;
        // ---------------- Manufacturing ----------------
        [Required(ErrorMessage = "Manufacturing Policy is required")]
        public string ManufacturingPolicy { get; set; } = string.Empty;
        public string? RoutingNo { get; set; }
        public string? ProductionBOMNo { get; set; }

        // ---------------- Posting / Costing ----------------
        [Required(ErrorMessage = "Costing Method is required")]
        public string CostingMethod { get; set; } = string.Empty;

        [Required(ErrorMessage = "General Product Posting Group is required")]
        public string GenProdPostingGroup { get; set; } = string.Empty;
        public string? VATProdPostingGroup { get; set; }

        [Required(ErrorMessage = "Inventory Posting Group is required")]
        public string InventoryPostingGroup { get; set; } = string.Empty;

        [Required(ErrorMessage = "GST Group Code is required")]
        public string GSTGroupCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "GST Credit is required")]
        public string GSTCredit { get; set; } = string.Empty;

        [Required(ErrorMessage = "HSN/SAC Code is required")]
        public string HSNSACCode { get; set; } = string.Empty;
        // ---------------- Units ----------------
        [Required(ErrorMessage = "Replenishment System is required")]
        public string ReplenishmentSystem { get; set; } = string.Empty;

        public string? PurchUnitOfMeasure { get; set; }
        // ---------------- Tracking ----------------
        public string? ItemTrackingCode { get; set; }

        //[Required(ErrorMessage = "Reordering Policy is required")]
        public string? ReorderingPolicy { get; set; } = string.Empty;

        public List<FGItemGradeListDetails> GradeListDetails { get; set; } = new();

    }

    public class UnitOfMeasureModel : BaseDropDown { }
    public class RoutingModel : BaseDropDown { }
    public class GeneralProductPostingGroupModel : BaseDropDown { }
    public class InventoryPostingGroupModel : BaseDropDown { }
    public class GSTGroupsModel : BaseDropDown { }
    public class ProductionBOMHeaderModel : BaseDropDown { }
    public class CategoryModel : BaseDropDown { }
    public class SizeOfTileModel : BaseDropDown { }
    public class BrandModel : BaseDropDown { }
    public class CollectionModel : BaseDropDown { }
    public class SurfaceFinishGlazeModel : BaseDropDown { }
    public class GlazeEffectModel : BaseDropDown { }
    public class DesignColorModel : BaseDropDown { }
    public class ColourFamilyModel : BaseDropDown { }
    public class TypeOfTileModel : BaseDropDown { }
    public class PackagingModel : BaseDropDown { }
    public class GradeModel : BaseDropDown { }
    public class ThicknessModel : BaseDropDown { }
    public class BodyModel : BaseDropDown { }
    public class PLCollectionModel : BaseDropDown { }
    public class PLColoursModel : BaseDropDown { }

    public class ItemMovementTypeEnumModel : BaseDropDown { }
    public class ItemTypeOfProductEnumModel : BaseDropDown { }
    public class ItemManufacturingPolicyEnumModel : BaseDropDown { }
    public class ItemCostingMethodEnumModel : BaseDropDown { }
    public class ItemGSTCreditEnumModel : BaseDropDown { }
    public class ItemReplenishmentSystemEnumModel : BaseDropDown { }

    public class ItemReorderingPolicyEnumModel : BaseDropDown { }

    public class FGItemHSNModel : BaseDropDown
    {
        public bool IsSelected { get; set; }
        public int? SelectedHSNCode { get; set; }
    }

    public class ItemCategoryModel : BaseDropDown
    {
        public string? GenProdPostingGroup { get; set; }
        public string? InventoryPostingGroup { get; set; }
        public string? CostingMethod { get; set; }
    }
    public class ItemGradeModel : BaseDropDown
    {

        public string? GradeLinkCode { get; set; }

    }

    public class FGItemDropDownAddModel
    {
        public List<UnitOfMeasureModel> UnitOfMeasures { get; set; } = new();
        public List<RoutingModel> RoutingNos { get; set; } = new();
        public List<GeneralProductPostingGroupModel> GeneralProductPostingGroups { get; set; } = new();
        public List<InventoryPostingGroupModel> InventoryPostingGroups { get; set; } = new();
        public List<GSTGroupsModel> GSTGroups { get; set; } = new();

        public List<ProductionBOMHeaderModel> ProductionBOMHeaders { get; set; } = new();

        public List<CategoryModel> Categories { get; set; } = new();
        public List<SizeOfTileModel> SizeOfTiles { get; set; } = new();
        public List<BrandModel> Brands { get; set; } = new();
        public List<CollectionModel> Collections { get; set; } = new();
        public List<SurfaceFinishGlazeModel> SurfaceFinishGlazes { get; set; } = new();
        public List<GlazeEffectModel> GlazeEffects { get; set; } = new();
        public List<DesignColorModel> DesignColors { get; set; } = new();
        public List<ColourFamilyModel> ColourFamilies { get; set; } = new();
        public List<TypeOfTileModel> TypeOfTiles { get; set; } = new();
        public List<PackagingModel> Packagings { get; set; } = new();
        public List<GradeModel> Grades { get; set; } = new();
        public List<ThicknessModel> Thicknesses { get; set; } = new();
        public List<BodyModel> Bodies { get; set; } = new();
        public List<PLCollectionModel> PLCollections { get; set; } = new();
        public List<PLColoursModel> PLColours { get; set; } = new();

        public List<ItemMovementTypeEnumModel> MovementTypes { get; set; } = new();
        public List<ItemTypeOfProductEnumModel> TypeOfProducts { get; set; } = new();
        public List<ItemManufacturingPolicyEnumModel> ManufacturingPolicies { get; set; } = new();
        public List<ItemCostingMethodEnumModel> CostingMethods { get; set; } = new();
        public List<ItemGSTCreditEnumModel> GSTCredits { get; set; } = new();
        public List<ItemReplenishmentSystemEnumModel> ReplenishmentSystems { get; set; } = new();

        public List<ItemReorderingPolicyEnumModel> ReorderingPolicy { get; set; } = new();

        public List<ItemGradeModel> ItemGrades { get; set; } = new();
        public List<ItemCategoryModel> ItemCategories { get; set; } = new();
    }

    public class GetFGItemAddModel : FGItemModel
    {
        public FGItemDropDownAddModel DropDownData { get; set; } = new();
    }

    public class FGItemGradeListDetails
    {
        public string? GradeItemCode { get; set; }
        public string? Brand { get; set; }
        public string? Grade { get; set; }
        public string? GradeLinkCode { get; set; }
        public string? SalesUnitOfMeasure { get; set; }
        public string? Description { get; set; }
    }

    public class FGItemDropDownEditModel
    {
        public List<UnitOfMeasureModel> UnitOfMeasures { get; set; } = new();
        public List<RoutingModel> RoutingNos { get; set; } = new();
        public List<GeneralProductPostingGroupModel> GeneralProductPostingGroups { get; set; } = new();
        public List<InventoryPostingGroupModel> InventoryPostingGroups { get; set; } = new();
        public List<GSTGroupsModel> GSTGroups { get; set; } = new();

        public List<ProductionBOMHeaderModel> ProductionBOMHeaders { get; set; } = new();

        public List<CategoryModel> Categories { get; set; } = new();
        public List<SizeOfTileModel> SizeOfTiles { get; set; } = new();
        public List<BrandModel> Brands { get; set; } = new();
        public List<CollectionModel> Collections { get; set; } = new();
        public List<SurfaceFinishGlazeModel> SurfaceFinishGlazes { get; set; } = new();
        public List<GlazeEffectModel> GlazeEffects { get; set; } = new();
        public List<DesignColorModel> DesignColors { get; set; } = new();
        public List<ColourFamilyModel> ColourFamilies { get; set; } = new();
        public List<TypeOfTileModel> TypeOfTiles { get; set; } = new();
        public List<PackagingModel> Packagings { get; set; } = new();
        public List<GradeModel> Grades { get; set; } = new();
        public List<ThicknessModel> Thicknesses { get; set; } = new();
        public List<BodyModel> Bodies { get; set; } = new();
        public List<PLCollectionModel> PLCollections { get; set; } = new();
        public List<PLColoursModel> PLColours { get; set; } = new();

        public List<ItemMovementTypeEnumModel> MovementTypes { get; set; } = new();
        public List<ItemTypeOfProductEnumModel> TypeOfProducts { get; set; } = new();
        public List<ItemManufacturingPolicyEnumModel> ManufacturingPolicies { get; set; } = new();
        public List<ItemCostingMethodEnumModel> CostingMethods { get; set; } = new();
        public List<ItemGSTCreditEnumModel> GSTCredits { get; set; } = new();
        public List<ItemReplenishmentSystemEnumModel> ReplenishmentSystems { get; set; } = new();

        public List<ItemReorderingPolicyEnumModel> ReorderingPolicy { get; set; } = new();

        public List<ItemGradeModel> ItemGrades { get; set; } = new();
        public List<ItemCategoryModel> ItemCategories { get; set; } = new();
    }


    public class FGItemeEditModel
    {
        public int DisplayNo { get; set; }

        // ---------------- TextBox Fields ----------------
        public string? CompanyCode { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(100)]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description 2 is required")]
        [StringLength(50)]
        public string Description2 { get; set; } = string.Empty;

        [Required(ErrorMessage = "Rounding Precision is required")]
        public decimal RoundingPrecision { get; set; }
        public decimal? GrossWeight { get; set; }
        public decimal? NetWeight { get; set; }

        // ---------------- DropDown Fields ----------------
        [Required(ErrorMessage = "Base Unit Of Measure is required")]
        public string BaseUnitOfMeasure { get; set; } = string.Empty;
        public string? ItemCategoryCode { get; set; }
        public string? ProductGroupCode { get; set; }

        [Required(ErrorMessage = "Movement Type is required.")]
        public string MovementType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Type Of Product is required")]
        public string TypeOfProduct { get; set; } = string.Empty;
        // ---------------- Tile Attributes ----------------
        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; } = string.Empty;

        [Required(ErrorMessage = "Size Of Tile is required")]
        public string SizeOfTile { get; set; } = string.Empty;

        [Required(ErrorMessage = "Collection is required")]
        public string Collection { get; set; } = string.Empty;

        [Required(ErrorMessage = "Surface Finish Or Glaze is required")]
        public string SurfaceFinishOrGlaze { get; set; } = string.Empty;

        [Required(ErrorMessage = "Glaze Effect is required")]
        public string GlazeEffect { get; set; } = string.Empty;

        [Required(ErrorMessage = "Design Color is required")]
        [StringLength(30)]
        public string DesignColor { get; set; } = string.Empty;

        public string? ColourFamily { get; set; } = string.Empty;

        [Required(ErrorMessage = "Type Of Tile is required")]
        public string TypeOfTile { get; set; } = string.Empty;

        [Required(ErrorMessage = "Packaging is required")]
        public string Packaging { get; set; } = string.Empty;

        [Required(ErrorMessage = "Thickness is required")]
        public string Thickness { get; set; } = string.Empty;

        [Required(ErrorMessage = "Body is required")]
        public string Body { get; set; } = string.Empty;

        [Required(ErrorMessage = "PL Collection is required")]
        public string PLCollection { get; set; } = string.Empty;

        [Required(ErrorMessage = "PL Colours is required")]
        public string PLColours { get; set; } = string.Empty;
        // ---------------- Manufacturing ----------------

        [Required(ErrorMessage = "Manufacturing Policy is required")]
        public string ManufacturingPolicy { get; set; } = string.Empty;
        public string? RoutingNo { get; set; }
        public string? ProductionBOMNo { get; set; }

        // ---------------- Posting / Costing ----------------
        [Required(ErrorMessage = "Costing Method is required")]
        public string CostingMethod { get; set; } = string.Empty;

        [Required(ErrorMessage = "GenProdPostingGroup is required")]
        public string GenProdPostingGroup { get; set; } = string.Empty;
        public string? VATProdPostingGroup { get; set; }

        [Required(ErrorMessage = "InventoryPostingGroup is required")]
        public string InventoryPostingGroup { get; set; } = string.Empty;

        [Required(ErrorMessage = "GSTGroupCode is required")]
        public string GSTGroupCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "GSTCredit is required")]
        public string GSTCredit { get; set; } = string.Empty;

        [Required(ErrorMessage = "HSNSACCode is required")]
        public string HSNSACCode { get; set; } = string.Empty;
        // ---------------- Units ----------------
        [Required(ErrorMessage = "ReplenishmentSystem is required")]
        public string ReplenishmentSystem { get; set; } = string.Empty;

        public string? PurchUnitOfMeasure { get; set; }
        // ---------------- Tracking ----------------
        public string? ItemTrackingCode { get; set; }

        //[Required(ErrorMessage = "ReorderingPolicy is required")]
        public string? ReorderingPolicy { get; set; } = string.Empty;

        public List<FGItemGradeListDetails> GradeListDetails { get; set; } = new();

    }

    public class GetFGItemEditModel : FGItemeEditModel
    {
        public FGItemDropDownEditModel DropDownData { get; set; } = new();
    }

    public class FGItemEmailDetailModel
    {
        public int DisplayNo { get; set; }

        // Basic Information
        public string? CompanyCode { get; set; }
        public string? Description { get; set; }
        public string? Description2 { get; set; }
        public decimal RoundingPrecision { get; set; }
        public decimal? GrossWeight { get; set; }
        public decimal? NetWeight { get; set; }

        // Item Details
        public string? BaseUnitOfMeasure { get; set; }
        public string? ItemCategoryCode { get; set; }
        public string? ProductGroupCode { get; set; }
        public string? MovementType { get; set; }
        public string? TypeOfProduct { get; set; }

        // Tile Attributes
        public string? Category { get; set; }
        public string? SizeOfTile { get; set; }
        public string? Collection { get; set; }
        public string? SurfaceFinishOrGlaze { get; set; }
        public string? GlazeEffect { get; set; }
        public string? DesignColor { get; set; }
        public string? ColourFamily { get; set; }
        public string? TypeOfTile { get; set; }
        public string? Packaging { get; set; }
        public string? Thickness { get; set; }
        public string? Body { get; set; }
        public string? PLCollection { get; set; }
        public string? PLColours { get; set; }

        // Manufacturing
        public string? ManufacturingPolicy { get; set; }
        public string? RoutingNo { get; set; }
        public string? ProductionBOMNo { get; set; }

        // Posting / Costing
        public string? CostingMethod { get; set; }
        public string? GenProdPostingGroup { get; set; }
        public string? VATProdPostingGroup { get; set; }
        public string? InventoryPostingGroup { get; set; }
        public string? GSTGroupCode { get; set; }
        public string? GSTCredit { get; set; }
        public string? HSNSACCode { get; set; }

        // Units
        public string? ReplenishmentSystem { get; set; }
        public string? PurchUnitOfMeasure { get; set; }

        // Tracking
        public string? ItemTrackingCode { get; set; }
        public string? ReorderingPolicy { get; set; }

        // Grade / Brand Details
        public string? GradeItemCode { get; set; }
        public string? Brand { get; set; }
        public string? Grade { get; set; }
        public string? GradeLinkCode { get; set; }
        public string? SalesUnitOfMeasure { get; set; }

        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }

    public class ERPEmailConfigurationModel
    {
        public int Id { get; set; }

        public string? ModuleName { get; set; }

        public string? Type { get; set; }

        public string? FromEmail { get; set; }

        public string? ToEmails { get; set; }

        public string? CCEmails { get; set; }

        public string? BCCEmails { get; set; }

        public bool IsActive { get; set; }
    }
}
