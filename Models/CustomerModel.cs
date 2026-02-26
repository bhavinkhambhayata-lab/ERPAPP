using System.ComponentModel.DataAnnotations;

namespace ERPAPP.Models
{
    public class GetCustomerAddModel : CustomerModel
    {
        public CustomerDropDownModel CustomerDropDownModel { get; set; } = new();
    }

    public class CustomerModel
    {
        // ===== General =====
        public int PortalRowId { get; set; }              // int NOT NULL
        public int DisplayNo { get; set; }                // int NOT NULL
        public string Name { get; set; }                  // varchar NULL
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public string StateCode { get; set; }
        public string CountryCode { get; set; }
        public string Region { get; set; }
        public string Zone { get; set; }
        public string ContactPerson { get; set; }
        public string MobileNo { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string FaxNo { get; set; }

        public string EInvPhoneNo { get; set; }           // NOT NULL
        public string EInvEmail { get; set; }             // NOT NULL

        // ===== Tax & Bank =====
        public int GSTRegistrationType { get; set; }      // int NOT NULL
        public int GSTCustomerType { get; set; }          // int NOT NULL
        public string GSTRegistrationNo { get; set; }     // NOT NULL
        public string ARNNo { get; set; }                 // NOT NULL
        public string PANNo { get; set; }

        public string BankName { get; set; }
        public string BankAccountNo { get; set; }
        public string BranchName { get; set; }
        public string IFSCCode { get; set; }

        public int CommissionType { get; set; }           // NOT NULL
        public double CommissionValue { get; set; }       // float NOT NULL
        public string CurrencyCode { get; set; }
        public string VendorCode { get; set; }

        public string ParentCustomerCode { get; set; }
        public DateTime? DealerAppointmentDate { get; set; }

        // ===== Business =====
        public int Brand { get; set; }                    // NOT NULL
        public int CustomerType { get; set; }             // NOT NULL
        public int DealerClassification { get; set; }     // NOT NULL
        public string CustomerCategoryCode { get; set; }

        public decimal CreditLimit { get; set; }          // decimal NOT NULL
        public string PaymentTermsCode { get; set; }
        public string PaymentMethodCode { get; set; }
        public string SalespersonCode { get; set; }
        public string HOSalesCode { get; set; }

        public string Allocation { get; set; }
        public string Structure { get; set; }
        public int BusinessCategory { get; set; }         // NOT NULL
        public string LocationCode { get; set; }

        public string PriceListCode { get; set; }         // NOT NULL
        public string CustomerPriceGroup { get; set; }
        public string MRPGroup { get; set; }
        public string CustomerPostingGroup { get; set; }
        public string GenBusPostingGroup { get; set; }
        public string ExciseBusPostingGroup { get; set; }

        public int ApplicationMethod { get; set; }        // NOT NULL
        public bool TaxLiable { get; set; }               // tinyint NOT NULL
        public string Dimension { get; set; }

        public string MSMEUAMNo { get; set; }             // NOT NULL

        // ===== NOD / NOC =====
        public bool IsNodNocCreation { get; set; }        // int NOT NULL
        public string NODAccessCode { get; set; }
        public string NODNOC { get; set; }
        public string ConcessionalCode { get; set; }
        public bool ThresholdOverlook { get; set; }       // int NOT NULL
        public bool SurchargeOverlook { get; set; }       // int NOT NULL


    }

    public class BaseDropDown
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class BrandModel : BaseDropDown
    {
        public int RowID { get; set; }
    }

    public class CityModel : BaseDropDown { }

    public class CountryModel : BaseDropDown { }

    public class RegionModel : BaseDropDown { }

    public class ZoneModel : BaseDropDown { }

    public class SalesPersonModel : BaseDropDown { }

    public class CustomerCategoryModel : BaseDropDown { }

    public class PaymentTermsModel : BaseDropDown { }

    public class PaymentMethodModel : BaseDropDown { }

    public class VendorModel : BaseDropDown { }

    public class CustomerPriceGroupModel : BaseDropDown { }

    public class PriceListModel : BaseDropDown { }

    public class GenBusPostingGroupModel : BaseDropDown { }

    public class ExciseBusPostingGroupModel : BaseDropDown { }

    public class CustomerPostingGroupModel : BaseDropDown { }

    public class StructureModel : BaseDropDown { }

    public class CurrencyModel : BaseDropDown { }

    public class ParentCustomerModel : BaseDropDown { }

    public class LocationModel : BaseDropDown { }

    public class AccessCodeModel : BaseDropDown { }

    public class NODNOCModel : BaseDropDown { }

    public class ConcessionalCodeModel : BaseDropDown { }

    public class CustomerDropDownModel
    {
        public List<BrandModel> Brands { get; set; } = new();
        public List<CityModel> Cities { get; set; } = new();
        public List<CountryModel> Countries { get; set; } = new();
        public List<RegionModel> Regions { get; set; } = new();
        public List<ZoneModel> Zones { get; set; } = new();
        public List<SalesPersonModel> SalesPersons { get; set; } = new();
        public List<SalesPersonModel> HOSalesPersons { get; set; } = new();
        public List<CustomerCategoryModel> CustomerCategories { get; set; } = new();
        public List<PaymentTermsModel> PaymentTerms { get; set; } = new();
        public List<PaymentMethodModel> PaymentMethods { get; set; } = new();
        public List<VendorModel> Vendors { get; set; } = new();
        public List<CustomerPriceGroupModel> CustomerPriceGroups { get; set; } = new();
        public List<PriceListModel> PriceLists { get; set; } = new();
        public List<GenBusPostingGroupModel> GenBusPostingGroups { get; set; } = new();
        public List<ExciseBusPostingGroupModel> ExciseBusPostingGroups { get; set; } = new();
        public List<CustomerPostingGroupModel> CustomerPostingGroups { get; set; } = new();
        public List<StructureModel> Structures { get; set; } = new();
        public List<CurrencyModel> Currencies { get; set; } = new();
        public List<ParentCustomerModel> ParentCustomers { get; set; } = new();
        public List<LocationModel> Locations { get; set; } = new();
        public List<AccessCodeModel> AccessCodes { get; set; } = new();
        public List<NODNOCModel> NODNOCs { get; set; } = new();
        public List<ConcessionalCodeModel> ConcessionalCodes { get; set; } = new();
    }


    public class CustomerSearchModel
    {
        public string No { get; set; }
        public string Name { get; set; }
    }
}
