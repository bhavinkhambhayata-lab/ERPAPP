using System.ComponentModel.DataAnnotations;

namespace ERPAPP.Models
{

    public class VendorsModel
    {
        public string? LoginRowId { get; set; }
        public int DisplayNo { get; set; }

        // ================= BASIC =================

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? MasterCode { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(100)]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address 2 is required")]
        [StringLength(50)]
        public string Address2 { get; set; } = string.Empty;

        [Required(ErrorMessage = "City is required")]
        [StringLength(30)]
        public string CityCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Postal Code is required")]
        [StringLength(20)]
        public string PostCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "State is required")]
        [StringLength(10)]
        public string StateCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Country is required")]
        [StringLength(10)]
        public string CountryCode { get; set; } = string.Empty;

        // ================= EXTRA GENERAL =================

        [StringLength(20)]
        public string? Range { get; set; }

        [StringLength(20)]
        public string? Collectorate { get; set; }

        public int? GTA { get; set; }

        [StringLength(20)]
        public string? VendorLocation { get; set; }

        public int? GSTNotToHold { get; set; }

        public int? FixedDueDate { get; set; }

        public int? AggregateTurnover { get; set; }

        [StringLength(30)]
        public string? FaxNo { get; set; }

        [StringLength(30)]
        public string? ECCNo { get; set; }

        [StringLength(30)]
        public string? ServiceTaxRegNo { get; set; }

        // ================= CONTACT =================

        [Required(ErrorMessage = "Contact is required")]
        [StringLength(100)]
        public string ContactPerson { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mobile Phone No is required")]
        [StringLength(30)]
        [RegularExpression(@"^(\+?[\d\-]+)(,\+?[\d\-]+)*$", ErrorMessage = "Invalid mobile phone number")]
        public string MobileNo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone No is required")]
        [StringLength(30)]
        [RegularExpression(@"^(\+?[\d\-]+)(,\+?[\d\-]+)*$", ErrorMessage = "Invalid phone number")]
        public string PhoneNo { get; set; } = string.Empty;

        [StringLength(80)]
        public string? Email { get; set; }

        [StringLength(80)]
        public string? Website { get; set; }

        public bool EmailNotAvailable { get; set; } = false;

        // ================= TAX =================

        [RegularExpression(@"^[A-Z]{5}[0-9]{4}[A-Z]$", ErrorMessage = "Invalid PAN format")]
        [StringLength(10, MinimumLength = 10)]
        public string? PANNo { get; set; }

        [StringLength(10)]
        public string? CurrencyCode { get; set; }

        // ================= GST =================

        [Required(ErrorMessage = "GST Vendor Type is required")]
        public int GSTVendorType { get; set; }

        public int? GSTReturnFrequency { get; set; }

        [RegularExpression(@"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z][1-9A-Z]Z[0-9A-Z]$", ErrorMessage = "Invalid GSTIN format")]
        [StringLength(15, MinimumLength = 15)]
        public string? GSTRegNo { get; set; }

        [RegularExpression(@"^[A-Z]{2}[0-9]{12}[A-Z]{1}$", ErrorMessage = "Invalid ARN format")]
        [StringLength(20)]
        public string? ARN { get; set; }

        // ================= BANK =================

        [StringLength(50)]
        public string? BankName { get; set; }

        [RegularExpression(@"^[0-9]{9,18}$", ErrorMessage = "Invalid bank account number")]
        [StringLength(20)]
        public string? BankAccountNo { get; set; }

        [StringLength(30)]
        public string? BranchName { get; set; }

        [RegularExpression(@"^[A-Z]{4}0[A-Z0-9]{6}$", ErrorMessage = "Invalid IFSC code")]
        [StringLength(20)]
        public string? IFSCCode { get; set; }

        // ================= BUSINESS =================

        public int? VendorType { get; set; }

        [Required(ErrorMessage = "Vendor Category is required")]
        [StringLength(20)]
        public string VendorCategory { get; set; } = string.Empty;

        public int? BusinessCategory { get; set; }

        public bool? RelatedParty { get; set; }
        public bool? Subcontractor { get; set; }

        [Required(ErrorMessage = "Payment Terms is required")]
        [StringLength(10)]
        public string PaymentTerms { get; set; } = string.Empty;

        [StringLength(10)]
        public string? PaymentMethod { get; set; }

        [Required(ErrorMessage = "Purchaser Code is required")]
        [StringLength(20)]
        public string PurchaserCode { get; set; } = string.Empty;

        // ================= POSTING =================

        [StringLength(20)]
        public string? VATBusPostingGroup { get; set; }

        [Required(ErrorMessage = "Gen Bus Posting Group is required")]
        [StringLength(20)]
        public string GenBusPostingGroup { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vendor Posting Group is required")]
        [StringLength(20)]
        public string VendorPostingGroup { get; set; } = string.Empty;

        // ================= OTHER CONFIG =================

        [Required(ErrorMessage = "Application Method is required")]
        //[StringLength(10)]
        public int ApplicationMethod { get; set; }

        [Required(ErrorMessage = "Tax Liable is required")]
        //[StringLength(10)]
        public int TaxLiable { get; set; } = 0;

        [StringLength(10)]
        public string? Location { get; set; }

        // ================= MSME =================

        [StringLength(50)]
        public string? MSMEUAMNo { get; set; }

        public DateTime? MSMEIntimationDate { get; set; }
        public DateTime? MSMEEffectiveDate { get; set; }

        // ================= NOD/NOC =================

        [StringLength(20)]
        public string? AccessCode { get; set; }

        [StringLength(20)]
        public string? NOCNOD { get; set; }

        [StringLength(20)]
        public string? ConcessionalCode { get; set; }

        public bool ThresholdOverlook { get; set; }
        public bool SurchargeOverlook { get; set; }
    }


    public class GetVendorAddData : VendorsModel
    {
        public VendorDropDownModel DropDownData { get; set; } = new();
    }

    public class VendorStructureModel : BaseDropDown { }
    public class VendorGSTVendorTypeModel : BaseDropDown { }
    public class VendorGSTReturnFrequencyModel : BaseDropDown { }
    public class VendorLocationModel : BaseDropDown { }
    public class VendorTypeModel : BaseDropDown { }
    public class VendorPaymentTermsModel : BaseDropDown { }
    public class VendorPaymentMethodModel : BaseDropDown { }
    public class VendorGenBusPostingGroupModel : BaseDropDown { }
    public class VendorPostingGroupModel : BaseDropDown { }
    public class VendorApplicationMethodModel : BaseDropDown { }
    public class VendorCategoryModel : BaseDropDown { }
    public class VendorPurchaserModel : BaseDropDown { }
    public class VendorBussinessCategoryModel : BaseDropDown { }
    public class VendorCurrencyModel : BaseDropDown { }
    public class VendorVATBusPostingGroupModel : BaseDropDown { }

    public class VendorAllLocationModel : BaseDropDown { }
    public class VendorCountryModel : BaseDropDown { }
    public class VendorCurrencyCodeModel : BaseDropDown { }

    public class VendorAggregateTurnoverModel : BaseDropDown { }


    public class VendorDropDownModel
    {
        public List<VendorGSTVendorTypeModel> VendorGSTVendorTypes { get; set; } = new();
        public List<VendorGSTReturnFrequencyModel> VendorGSTReturnFrequencies { get; set; } = new();
        public List<VendorLocationModel> VendorLocations { get; set; } = new(); // Subcontracting
        public List<VendorTypeModel> VendorTypes { get; set; } = new();
        public List<VendorBussinessCategoryModel> VendorBussinessCategories { get; set; } = new();
        public List<VendorApplicationMethodModel> VendorApplicationMethods { get; set; } = new();
        public List<VendorCurrencyModel> VendorCurrencies { get; set; } = new();
        public List<VendorCategoryModel> VendorCategories { get; set; } = new();
        public List<VendorPaymentTermsModel> VendorPaymentTerms { get; set; } = new();
        public List<VendorPaymentMethodModel> VendorPaymentMethods { get; set; } = new();
        public List<VendorPurchaserModel> VendorPurchasers { get; set; } = new();
        public List<VendorGenBusPostingGroupModel> VendorGenBusPostingGroups { get; set; } = new();
        public List<VendorPostingGroupModel> VendorPostingGroups { get; set; } = new();
        public List<VendorVATBusPostingGroupModel> VendorVATBusPostingGroups { get; set; } = new();
        public List<VendorAllLocationModel> VendorAllLocations { get; set; } = new(); // All Locations
        public List<VendorCountryModel> Countries { get; set; } = new();
        public List<VendorCurrencyCodeModel> VendorCurrencyCodes { get; set; } = new();
        public List<VendorAggregateTurnoverModel> VendorAggregateTurnover { get; set; } = new List<VendorAggregateTurnoverModel>();

    }

    public class GetVendorListModel
    {
        public int DisplayNo { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Location { get; set; }
        public string ContactPerson { get; set; }
        public string MobileNo { get; set; }
        public string MasterCode { get; set; }
        public string CompanyCode { get; set; }
        public string CreatedBy { get; set; }
    }

}
