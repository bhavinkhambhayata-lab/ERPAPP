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
        public int PortalRowId { get; set; }

        public string MasterCode { get; set; }
        public int DisplayNo { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Address2 is required")]
        public string Address2 { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string CityCode { get; set; }

        [Required(ErrorMessage = "Post Code is required")]
        public string PostCode { get; set; }

        [Required(ErrorMessage = "State is required")]
        public string StateCode { get; set; }

        [Required(ErrorMessage = "Country is required")]
        public string CountryCode { get; set; }

        [Required(ErrorMessage = "Region is required")]
        public string Region { get; set; }

        [Required(ErrorMessage = "Zone is required")]
        public string Zone { get; set; }

        [Required(ErrorMessage = "Contact Pereson is required")]
        public string ContactPerson { get; set; }

        [Required(ErrorMessage = "Mobile No is required")]
        [RegularExpression(@"^[6-9][0-9]{9}$",ErrorMessage = "Invalid mobile number")]
        public string MobileNo { get; set; }

        public string PhoneNo { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        public string Website { get; set; }
        //public string FaxNo { get; set; }

        // ===== E-Invoice =====
        [Required(ErrorMessage = "E-Invoice Phone is required")]
        [RegularExpression(@"^[6-9][0-9]{9}$", ErrorMessage = "Invalid E-Invoice phone number")]
        public string EInvPhoneNo { get; set; }

        [Required(ErrorMessage = "E-Invoice Email is required")]
        [EmailAddress(ErrorMessage = "Invalid E-Invoice email")]
        public string EInvEmail { get; set; }

        // ===== Tax & Bank =====
        [Required(ErrorMessage = "GST Registration Type is required")]
        public int GSTRegistrationType { get; set; }

        [Required(ErrorMessage = "GST Customer Type is required")]
        public int GSTCustomerType { get; set; }

        [Required(ErrorMessage = "GSTIN is required")]
        [RegularExpression(@"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}$", ErrorMessage = "Invalid GSTIN format")]
        public string GSTRegistrationNo { get; set; }

        [RegularExpression(@"^[A-Z]{2}[0-9]{13}$", ErrorMessage = "Invalid ARN format")]
        public string ARNNo { get; set; }

        // VB checkPANValid fully covered here
        [Required(ErrorMessage = "PAN is required")]
        [RegularExpression(@"^[A-Z]{5}[0-9]{4}[A-Z]{1}$", ErrorMessage = "Invalid PAN format")]
        public string PANNo { get; set; }

        public string BankName { get; set; }

        [RegularExpression(@"^[0-9]{9,18}$", ErrorMessage = "Invalid bank account number")]
        public string BankAccountNo { get; set; }

        public string BranchName { get; set; }

        [RegularExpression(@"^[A-Z]{4}0[A-Z0-9]{6}$", ErrorMessage = "Invalid IFSC code")]
        public string IFSCCode { get; set; }

        // ===== Commission =====
        [Required(ErrorMessage = "Commission Type is required")]
        public int CommissionType { get; set; }

        [Required(ErrorMessage = "Commission Value is required")]
        public double CommissionValue { get; set; }

        public string CurrencyCode { get; set; }
        public string VendorCode { get; set; }

        public string ParentCustomerCode { get; set; }
        public DateTime? DealerAppointmentDate { get; set; }

        // ===== Business =====
        [Required(ErrorMessage = "Division is required")]
        public int Division { get; set; }

        [Required(ErrorMessage = "Customer Type is required")]
        public int CustomerType { get; set; }

        [Required(ErrorMessage = "Dealer Classification is required")]
        public int DealerClassification { get; set; }

        public string CustomerCategoryCode { get; set; }

        [Required(ErrorMessage = "Credit Limit is required")]
        public decimal CreditLimit { get; set; }

        public string PaymentTermsCode { get; set; }
        public string PaymentMethodCode { get; set; }
        public string SalespersonCode { get; set; }
        public string HOSalesCode { get; set; }

        public string Allocation { get; set; }
        public string Structure { get; set; }

        [Required(ErrorMessage = "Business Category is required")]
        public int BusinessCategory { get; set; }

        public string LocationCode { get; set; }

        [Required(ErrorMessage = "Price List Code is required")]
        public string PriceListCode { get; set; }

        public string CustomerPriceGroup { get; set; }
        public string MRPGroup { get; set; }
        public string CustomerPostingGroup { get; set; }
        public string GenBusPostingGroup { get; set; }
        public string ExciseBusPostingGroup { get; set; }

        [Required(ErrorMessage = "Application Method is required")]
        public int ApplicationMethod { get; set; }

        public bool TaxLiable { get; set; }
        public string Dimension { get; set; }

        [Required(ErrorMessage = "MSME UAM No is required")]
        public string MSMEUAMNo { get; set; }

        // ===== NOD / NOC =====
        public bool IsNodNocCreation { get; set; }

        [Required(ErrorMessage = "NOD Access Code is required")]
        public string NODAccessCode { get; set; }

        public string NODNOC { get; set; }
        public string ConcessionalCode { get; set; }

        public bool ThresholdOverlook { get; set; }
        public bool SurchargeOverlook { get; set; }
    }

    public class BaseDropDown
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class DivisionModel : BaseDropDown
    {
        //public int RowID { get; set; }
    }

    public class CityModel : BaseDropDown { }

    public class StateModel : BaseDropDown { }

    public class PostCodeModel : BaseDropDown { }

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

    public class MRPGroupModel : BaseDropDown { }

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

    public class BusinessCategoryModel : BaseDropDown { }

    public class ApplicationMethodModel : BaseDropDown { }

    public class DealerClassificationModel : BaseDropDown { }

    public class CustomerTypeModel : BaseDropDown { }

    public class CommissionTypeModel : BaseDropDown { }

    public class GSTCustomerTypeModel : BaseDropDown { }

    public class GSTRegistrationTypeModel : BaseDropDown { }

    public class CustomerDropDownModel
    {
        public List<DivisionModel> Divisions { get; set; } = new();
        public List<CityModel> Cities { get; set; } = new();
        public List<CountryModel> Countries { get; set; } = new();

        public List<StateModel> States { get; set; } = new();
        public List<PostCodeModel> PostCode { get; set; } = new();
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

        public List<MRPGroupModel> MRPGroups { get; set; } = new();
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
        public List<BusinessCategoryModel> BusinessCategories { get; set; } = new();
        public List<ApplicationMethodModel> ApplicationMethods { get; set; } = new();
        public List<DealerClassificationModel> DealerClassifications { get; set; } = new();
        public List<CustomerTypeModel> CustomerTypes { get; set; } = new();
        public List<CommissionTypeModel> CommissionTypes { get; set; } = new();
        public List<GSTCustomerTypeModel> GSTCustomerTypes { get; set; } = new();
        public List<GSTRegistrationTypeModel> GSTRegistrationTypes { get; set; } = new();
    }


    public class CustomerSearchModel
    {
        public string No { get; set; }
        public string Name { get; set; }
    }

    public class CustomerMasterModel
    {
        public string No { get; set; }
        public string Name { get; set; }

        public string Address { get; set; }
        public string Address2 { get; set; }

        public string City { get; set; }
        public string Postcode { get; set; }
        public string StateCode { get; set; }
        public string CountryCode { get; set; }
        public string Region { get; set; }
        public string Zone { get; set; }

        public string ContactPerson { get; set; }
        public string MobileNo { get; set; }
        public string PhoneNo { get; set; }
        public string FaxNo { get; set; }

        public string EMail { get; set; }
        public string E_Inv_E_Mail { get; set; }
        public string E_Inv_PhoneNo { get; set; }

        public string Website_Homepage { get; set; }

        public string LSTNo { get; set; }
        public DateTime? LSTTINDate { get; set; }

        public string CSTNo { get; set; }
        public DateTime? CSTTINDate { get; set; }

        public string PANNO { get; set; }

        public string BankName { get; set; }
        public string BankAccountNo { get; set; }
        public string BranchName { get; set; }
        public string IFSCode { get; set; }

        public string GSTRegistrationNo { get; set; }
        public int? GSTRegistrationType { get; set; }
        public int? GSTCustomerType { get; set; }

        public int? CustomerType { get; set; }

        public string Allocation { get; set; }
    }

    public class AddressDropdownModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string DataType { get; set; }  // Important
    }

    public class AddressDetailModel
    {
        public string City { get; set; }
        public string PostCode { get; set; }
        public string Region { get; set; }
        public string Zone { get; set; }
    }

    public class ModifyPermissionResult
    {
        public bool HasPermission { get; set; }
        public bool IsSentForApproval { get; set; }
    }
}
