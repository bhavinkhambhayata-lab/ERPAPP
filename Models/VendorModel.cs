using System.ComponentModel.DataAnnotations;

namespace ERPAPP.Models
{
    public class VendorsModel
    {
        public class VendorModel
        {// ================= BASIC =================

            public int DisplayNo { get; set; }

            public int? CompanyId { get; set; }

            [Required(ErrorMessage = "Name is required")]
            public string? Name { get; set; }

            public string? MasterCode { get; set; }

            public string? VendorCode { get; set; }

            public string? Address { get; set; }
            public string? Address2 { get; set; }

            public string? CityCode { get; set; }
            public string? PostCode { get; set; }
            public string? StateCode { get; set; }
            public string? CountryCode { get; set; }

            // ================= EXTRA GENERAL =================

            public string? Range { get; set; }
            public string? Collectorate { get; set; }
            public string? GTA { get; set; }
            public string? VendorLocation { get; set; }

            public bool GSTNotToHold { get; set; }

            public DateTime? FixedDueDate { get; set; }

            public decimal? AggregateTurnover { get; set; }

            public string? FaxNo { get; set; }
            public string? ECCNo { get; set; }
            public string? ServiceTaxRegNo { get; set; }

            // ================= CONTACT =================

            public string? ContactPerson { get; set; }
            public string? MobileNo { get; set; }
            public string? PhoneNo { get; set; }
            public string? Email { get; set; }
            public string? Website { get; set; }

            public bool EmailNotAvailable { get; set; }

            // ================= TAX =================

            public string? PANNo { get; set; }

            // ================= GST =================

            public string? GSTVendorType { get; set; }
            public string? GSTReturnFrequency { get; set; }
            public string? GSTRegNo { get; set; }
            public string? ARN { get; set; }

            // ================= BANK =================

            public string? BankName { get; set; }
            public string? BankAccountNo { get; set; }
            public string? IFSCCode { get; set; }

            // ================= BUSINESS =================

            public string? VendorType { get; set; }
            public string? VendorCategory { get; set; }
            public string? BusinessCategory { get; set; }
            public string? Structure { get; set; }

            public bool? RelatedParty { get; set; }
            public bool? Subcontractor { get; set; }

            public string? PaymentTerms { get; set; }
            public string? PaymentMethod { get; set; }
            public string? PurchaserCode { get; set; }

            // ================= POSTING =================

            public string? VATBusPostingGroup { get; set; }
            public string? GenBusPostingGroup { get; set; }
            public string? ExciseBusPostingGroup { get; set; }
            public string? VendorPostingGroup { get; set; }

            // ================= OTHER CONFIG =================

            public string? ApplicationMethod { get; set; }
            public string? TaxLiable { get; set; }
            public string? Location { get; set; }

            // ================= MSME =================

            public string? MSMEUAMNo { get; set; }

            public DateTime? MSMEIntimationDate { get; set; }
            public DateTime? MSMEEffectiveDate { get; set; }

            // ================= NOD/NOC =================

            public string? AccessCode { get; set; }
            public string? NOCNOD { get; set; }
            public string? ConcessionalCode { get; set; }

            public bool ThresholdOverlook { get; set; }
            public bool SurchargeOverlook { get; set; }

        }


        public class GetVendorAddData : VendorModel
        {
            public VendorDropDownModel DropDownData { get; set; } = new();
        }

        public class VendorCategoryModel : BaseDropDown { }
      
        public class VendorPurchaserModel : BaseDropDown { }
        public class VendorPostingGroupModel : BaseDropDown { }
        public class VendorVATBusPostingGroupModel : BaseDropDown { }
        public class VendorAssesseeCodeModel : BaseDropDown { }
        public class VendorPaymentTermsModel : BaseDropDown { }
        public class VendorCurrencyModel : BaseDropDown { }
        public class VendorLocationModel : BaseDropDown { }
        public class VendorPaymentMethodModel : BaseDropDown { }
        public class VendorGenBusPostingGroupModel : BaseDropDown { }
        public class VendorCountryModel : BaseDropDown { }

        public class VendorDropDownModel
        {
            public List<VendorCountryModel> Countries { get; set; } = new();
            public List<VendorCurrencyModel> VendorCurrencies { get; set; } = new();
            public List<VendorLocationModel> VendorLocations { get; set; } = new(); // Subcontracting
            public List<VendorCategoryModel> VendorCategories { get; set; } = new();
            public List<VendorPaymentTermsModel> VendorPaymentTerms { get; set; } = new();
            public List<VendorPaymentMethodModel> VendorPaymentMethods { get; set; } = new();
            public List<VendorPurchaserModel> VendorPurchasers { get; set; } = new();
            public List<VendorGenBusPostingGroupModel> VendorGenBusPostingGroups { get; set; } = new();
            public List<VendorPostingGroupModel> VendorPostingGroups { get; set; } = new();
            public List<VendorVATBusPostingGroupModel> VendorVATBusPostingGroups { get; set; } = new();
            public List<LocationModel> VendorAllLocations { get; set; } = new(); // All Locations
            public List<VendorAssesseeCodeModel> VendorAssesseeCodes { get; set; } = new();
            public List<ConcessionalCodeModel> VendorConcessionalCodes { get; set; } = new();
        }
    }
}
