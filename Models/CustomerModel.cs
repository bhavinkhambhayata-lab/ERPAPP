using Microsoft.Extensions.Primitives;
using System.ComponentModel.DataAnnotations;

namespace ERPAPP.Models
{
    public class GetCustomerAddModel : CustomerModel
    {
        public CustomerDropDownModel CustomerDropDownModel { get; set; } = new();
    }

    public class CustomerModel
    {
        public string? LoginRowId { get; set; }
        public int PortalRowId { get; set; }
        public int DisplayNo { get; set; }

        // ================= GENERAL DETAILS =================

        [Required(ErrorMessage = "Division is required")]
        public int? Division { get; set; }
        public string? DivisionCode { get; set; }
        public string? MasterCode { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required")]
        [StringLength(100)]
        public string Address { get; set; } = string.Empty;


        [StringLength(50)]
        public string? Address2 { get; set; } = string.Empty;


        [Required(ErrorMessage = "City is required")]
        [StringLength(30)]
        public string CityCode { get; set; } = string.Empty;


        [Required(ErrorMessage = "Post Code is required")]
        [StringLength(20)]
        public string PostCode { get; set; } = string.Empty;

        [StringLength(10)]
        public string? StateCode { get; set; }


        [Required(ErrorMessage = "Country is required")]
        [StringLength(10)]
        public string CountryCode { get; set; } = string.Empty;


        [Required(ErrorMessage = "Region is required")]
        [StringLength(20)]
        public string Region { get; set; } = string.Empty;


        [Required(ErrorMessage = "Zone is required")]
        [StringLength(20)]
        public string Zone { get; set; } = string.Empty;

        public decimal? CreditLimit { get; set; } = 0;
        public string? PriceListCode { get; set; }
        public string? PromoCode { get; set; }
        public string? ChargesGroup { get; set; }

        [Required(ErrorMessage = "Sales Person Code is required")]
        public string SalesPersonCode { get; set; } = string.Empty;

        // ================= CONTACT =================

        [Required(ErrorMessage = "Contact Person is required")]
        [StringLength(50)]
        public string ContactPerson { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mobile No is required")]
        [RegularExpression(@"^(\+?[\d\-]+)(\/\+?[\d\-]+)*$",
    ErrorMessage = "Invalid mobile number")]
        [StringLength(30)]
        public string MobileNo { get; set; } = string.Empty;

        [StringLength(30)]
        [RegularExpression(@"^(\+?[\d\-]+)(\/\+?[\d\-]+)*$",
    ErrorMessage = "Invalid mobile number")]
        public string? PhoneNo { get; set; }

        [Required(ErrorMessage = "Email is required")]
        //[EmailAddress(ErrorMessage = "Invalid email address")]
        [StringLength(80)]
        public string Email { get; set; } = string.Empty;

        [StringLength(80)]
        public string? Website { get; set; }


        // ================= MARKETING =================

        public int? CustomerType { get; set; }
        public string? ParentCustomerCode { get; set; }
        public string? VendorCode { get; set; }

        public string? CommissionVendorNo { get; set; }
        public int? CommissionType { get; set; }
        public int? Commission { get; set; }

        [StringLength(50)]
        public string? BankName { get; set; }

        [StringLength(100)]
        public string? BranchName { get; set; }

        [RegularExpression(@"^[0-9]{9,18}$", ErrorMessage = "Invalid bank account number")]
        [StringLength(20)]
        public string? BankAccountNo { get; set; }

        //[RegularExpression(@"^[A-Z]{4}0[A-Z0-9]{6}$", ErrorMessage = "Invalid IFSC code")]
        [StringLength(20)]
        public string? IFSCCode { get; set; }


        // ================= INVOICING =================

        public string? BillToCustomer { get; set; }
        public string? LocationCode { get; set; }
        public string? CustomerPostingGroup { get; set; }
        public string? GenBusPostingGroup { get; set; }

        [Required(ErrorMessage = "E-Invoice Phone is required")]
        [StringLength(30)]
        [RegularExpression(@"^[0-9]{1,20}$", ErrorMessage = "Invalid phone number")]
        public string EInvPhoneNo { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-Invoice Email is required")]
        [StringLength(80)]
        [EmailAddress(ErrorMessage = "Invalid email")]
        public string EInvEmail { get; set; } = string.Empty;

        public string? CurrencyCode { get; set; }

        // ================= PAYMENTS =================

        public int? ApplicationMethod { get; set; }
        public string? PaymentTermsCode { get; set; }
        public string? PaymentMethodCode { get; set; }

        // ================= TAX =================

        //[RegularExpression(@"^[A-Z]{5}[0-9]{4}[A-Z]$", ErrorMessage = "Invalid PAN format")]
        [StringLength(10, MinimumLength = 10)]
        public string? PANNo { get; set; }

        public int? GSTRegistrationType { get; set; }

        [RegularExpression(@"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z][1-9A-Z]Z[0-9A-Z]$", ErrorMessage = "Invalid GSTIN format")]
        [StringLength(15, MinimumLength = 15)]
        public string? GSTRegistrationNo { get; set; }

        public int? GSTCustomerType { get; set; }

        [RegularExpression(@"^[A-Z]{2}[0-9]{12}[A-Z]{1}$", ErrorMessage = "Invalid ARN format")]
        [StringLength(15, MinimumLength = 15)]
        public string? ARNNo { get; set; }

        public int? BusinessCategory { get; set; }
        public string? MSMEUAMNo { get; set; }


        // ================= NOD / NOC =================

        public bool IsNodNocCreation { get; set; }
        public string? NODAccessCode { get; set; }
        public string? NODNOC { get; set; }
        public string? ConcessionalCode { get; set; }
        public bool ThresholdOverlook { get; set; }
        public bool SurchargeOverlook { get; set; }

        // ===================Shipping Details=================
        public string? ShippingCode { get; set; }

        [StringLength(100)]
        public string? ShippingName { get; set; }

        [StringLength(100)]
        public string? ShippingAddress { get; set; }

        [StringLength(50)]
        public string? ShippingAddress2 { get; set; }

        [StringLength(30)]
        public string? ShippingCity { get; set; }

        [StringLength(20)]
        public string? ShippingPostalCode { get; set; }

        [StringLength(30)]
        public string? ShippingCountry { get; set; }

        [StringLength(30)]
        public string? ShippingPhoneNo { get; set; }

        [StringLength(100)]
        public string? ShippingContactPerson { get; set; }

        // New Fields
        [StringLength(80)]
        public string? ShippingEmail { get; set; }

        [StringLength(10)]
        public string? ShippingLocationCode { get; set; }

        [StringLength(10)]
        public string? ShippingMethodCode { get; set; }

        [StringLength(10)]
        public string? ShippingAgentCode { get; set; }

        [StringLength(10)]
        public string? ShippingAgentServiceCode { get; set; }
        public string? ShippingState { get; set; }

        [RegularExpression(@"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z][1-9A-Z]Z[0-9A-Z]$", ErrorMessage = "Invalid GSTIN format")]
        [StringLength(15, MinimumLength = 15)]
        public string? ShippingGSTRegistrationNo { get; set; }
        public int? ShippingAddressType { get; set; }
        public int? ShipToGSTCustomerType { get; set; }


        [StringLength(10)]
        public string? AssesseeCode { get; set; }

        public int? IsAlreadyCreatedMaster { get; set; } = 0;

        // ================= BRAND LIST =================

        public List<CustomerBrandWiseAddModel> CustomerBrandAddList { get; set; } = new();
    }

    public class BaseDropDown
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
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

    public class SalesPersonModel : BaseDropDown
    {
        public string? Allocation { get; set; }
    }

    public class HOSalesPersonModel : BaseDropDown { }

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

    public class PromoCodeModel : BaseDropDown { }

    public class ChargesGroupModel : BaseDropDown { }

    public class ShipmentMethodCodeModel : BaseDropDown { }

    public class ShippingAgentCodeModel : BaseDropDown { }

    public class ShippingAgentServiceModel : BaseDropDown { }

    public class ShippingAgentServiceZoneCodeModel : BaseDropDown { }

    public class ShippingGSTCustomerTypeModel : BaseDropDown { }

    public class ShippingAddressTypeModel : BaseDropDown { }

    public class ShipAlternatePriceGroupModel : BaseDropDown { }

    public class ShippingCountryModel : BaseDropDown { }

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
        public List<PromoCodeModel> PromoCodes { get; set; } = new();
        public List<ChargesGroupModel> ChargesGroups { get; set; } = new();
        public List<ShipmentMethodCodeModel> ShipmentMethodCodes { get; set; } = new();
        public List<ShippingAgentCodeModel> ShippingAgentCodes { get; set; } = new();
        public List<ShippingAgentServiceModel> ShippingAgentServices { get; set; } = new();
        public List<ShippingAgentServiceZoneCodeModel> ShippingAgentServiceZoneCodes { get; set; } = new();
        public List<ShippingGSTCustomerTypeModel> ShippingGSTCustomerTypes { get; set; } = new();
        public List<ShippingAddressTypeModel> ShippingAddressTypes { get; set; } = new();
        public List<ShipAlternatePriceGroupModel> ShipAlternatePriceGroups { get; set; } = new();

        public List<ShippingCountryModel> ShippingCountries { get; set; } = new();

    }


    public class CustomerSearchModel
    {
        public string No { get; set; }
        public string Name { get; set; }
    }

    public class CustomerMasterModel
    {
        public string No { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;
        public string Address2 { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;
        public string Postcode { get; set; } = string.Empty;
        public string StateCode { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string Zone { get; set; } = string.Empty;

        public string ContactPerson { get; set; } = string.Empty;
        public string MobileNo { get; set; } = string.Empty;
        public string PhoneNo { get; set; } = string.Empty;

        public string EMail { get; set; } = string.Empty;
        public string E_Inv_E_Mail { get; set; } = string.Empty;
        public string E_Inv_PhoneNo { get; set; } = string.Empty;

        public string? Website_Homepage { get; set; }

        public string PANNO { get; set; } = string.Empty;

        public string? BankName { get; set; }
        public string? BankAccountNo { get; set; }
        public string? BranchName { get; set; }
        public string? IFSCode { get; set; }

        public string? GSTRegistrationNo { get; set; }
        public int? GSTRegistrationType { get; set; }
        public int? GSTCustomerType { get; set; }

        public int? CustomerType { get; set; }

        public string? ParentCustomerCode { get; set; }

        public string? CommissionVendorNo { get; set; }

        public int? CommissionType { get; set; }

        public string? PriceListCode { get; set; }

        public string? PromoCode { get; set; }

        public string? ChargesGroup { get; set; }

        public decimal? CreditLimit { get; set; }

        public int? ApplicationMethod { get; set; }

        public string? PaymentTermsCode { get; set; }

        public string? PaymentMethodCode { get; set; }

        public string? CustomerPostingGroup { get; set; }

        public string? GenBusPostingGroup { get; set; }

        public string? Currency { get; set; }

        public string? BusinessCategory { get; set; }
        public string? MSMEUAMNo { get; set; }

    }

    public class AddressDropdownModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class AddressCityDetailModel
    {
        public string CountryCode { get; set; }
        public string StateCode { get; set; }
        public string City { get; set; }
    }

    public class AddressPostCodeDetailModel
    {
        public string PostCode { get; set; }
        public string Region { get; set; }
        public string Zone { get; set; }
    }

    public class AddressPostCodeModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class ModifyPermissionResult
    {
        public bool HasPermission { get; set; }
        public bool IsSentForApproval { get; set; }
    }


    public class DimesionModel : BaseDropDown { }

    public class CustomerDiscountGroupModel : BaseDropDown { }


    public class CustomerBrandWiseModel
    {
        public List<DimesionModel> DimensionList { get; set; } = new();

        public List<CustomerCategoryModel> CustomerCategoryList { get; set; } = new();

        public List<CustomerDiscountGroupModel> DiscountGroupList { get; set; } = new();

        public List<SalesPersonModel> SalesPersonList { get; set; } = new();

        public List<HOSalesPersonModel> HOSalesPersonList { get; set; } = new();

        public List<DealerClassificationModel> DealerClassficationList { get; set; } = new();
    }

    public class CustomerBrandWiseAddModel
    {
        public string? CustomerNo { get; set; }

        public string BrandCode { get; set; }

        public string CustomerCategoryCode { get; set; }

        public decimal? TradeSecurityAmount { get; set; }

        public string CustomerDiscountGroup { get; set; }

        public string? DealerClassification { get; set; }

        public string SalesPersonCode { get; set; }

        public string? Allocation { get; set; }

        public string HOSalesPerson { get; set; }

        public DateTime? DLRAppointmentDate { get; set; }

        public DateTime? DLRTerminationDate { get; set; }
    }


    public class GetCustomerDataWithPortalRowIdModel
    {
        public int PortalRowId { get; set; }

        public string? MasterCode { get; set; }

        public string? Name { get; set; }

        public string? Address { get; set; }

        public string? Address2 { get; set; }

        public string? CityCode { get; set; }

        public string? PostCode { get; set; }

        public string? StateCode { get; set; }

        public string? CountryCode { get; set; }

        public string? Region { get; set; }

        public string? Zone { get; set; }

        public string? ContactPerson { get; set; }

        public string? MobileNo { get; set; }

        public string? PhoneNo { get; set; }

        public string? Email { get; set; }

        public string? Website { get; set; }

        public int? CustomerType { get; set; }

        public string? PANNo { get; set; }

        public string? GSTRegistrationNo { get; set; }

        public int? GSTRegistrationType { get; set; }

        public string? ARNNo { get; set; }
    }

    public class GetCustomerListModel
    {
        public int RowId { get; set; }
        public int DisplayNo { get; set; }

        public string? Name { get; set; }
        public string? City { get; set; }

        public string? Region { get; set; }
        public string? Zone { get; set; }
        public string? Location { get; set; }
        public string? ContactPerson { get; set; }

        public string? MobileNo { get; set; }

        public string? MasterCode { get; set; }
        public string? CompanyCode { get; set; }
        public string? MasterCodeInCompany { get; set; }

        public string? Division { get; set; }

        public string? CreatedBy { get; set; }

        public int Blocked { get; set; } = 0;
    }

    //Edit Customer Model

    public class CustomerBrandWiseEditModel
    {
        public string? CustomerNo { get; set; }

        public string BrandCode { get; set; }

        public string CustomerCategoryCode { get; set; }

        public decimal? TradeSecurityAmount { get; set; }

        public string CustomerDiscountGroup { get; set; }

        public string? DealerClassification { get; set; }

        public string SalesPersonCode { get; set; }

        public string? Allocation { get; set; }

        public string HOSalesPerson { get; set; }

        public DateTime? DLRAppointmentDate { get; set; }

        public DateTime? DLRTerminationDate { get; set; }

        public bool IsEdited { get; set; } = false;
    }


    public class GetCustomerEditModel : CustomerEditModel
    {
        public CustomerDropDownModel CustomerDropDownModel { get; set; } = new();
    }

    public class CustomerEditModel
    {
        public string? LoginRowId { get; set; }
        public int PortalRowId { get; set; }
        public int DisplayNo { get; set; }

        public string? CustomerCode { get; set; }

        // ================= GENERAL DETAILS =================

        [Required(ErrorMessage = "Division is required")]
        public int? Division { get; set; }
        public string? DivisionCode { get; set; }
        public string? MasterCode { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required")]
        [StringLength(100)]
        public string Address { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Address2 { get; set; } = string.Empty;


        [Required(ErrorMessage = "City is required")]
        [StringLength(30)]
        public string CityCode { get; set; } = string.Empty;


        [Required(ErrorMessage = "Post Code is required")]
        [StringLength(20)]
        public string PostCode { get; set; } = string.Empty;

        public string? PostCodeStr { get; set; } = string.Empty;

        [StringLength(10)]
        public string? StateCode { get; set; }


        [Required(ErrorMessage = "Country is required")]
        [StringLength(10)]
        public string CountryCode { get; set; } = string.Empty;


        [Required(ErrorMessage = "Region is required")]
        [StringLength(20)]
        public string Region { get; set; } = string.Empty;


        [Required(ErrorMessage = "Zone is required")]
        [StringLength(20)]
        public string Zone { get; set; } = string.Empty;

        public decimal? CreditLimit { get; set; } = 0;
        public string? PriceListCode { get; set; }
        public string? PromoCode { get; set; }
        public string? ChargesGroup { get; set; }

        [Required(ErrorMessage = "Sales Person Code is required")]
        public string SalesPersonCode { get; set; } = string.Empty;

        // ================= CONTACT =================

        [Required(ErrorMessage = "Contact Person is required")]
        [StringLength(50)]
        public string ContactPerson { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mobile No is required")]
        [RegularExpression(@"^(\+?[\d\-]+)(\/\+?[\d\-]+)*$",
    ErrorMessage = "Invalid mobile number")]
        [StringLength(30)]
        public string MobileNo { get; set; } = string.Empty;

        [StringLength(30)]
        [RegularExpression(@"^(\+?[\d\-]+)(\/\+?[\d\-]+)*$",
    ErrorMessage = "Invalid phone number")]
        public string? PhoneNo { get; set; }

        [Required(ErrorMessage = "Email is required")]
        //[EmailAddress(ErrorMessage = "Invalid email address")]
        [StringLength(80)]
        public string Email { get; set; } = string.Empty;

        [StringLength(80)]
        public string? Website { get; set; }


        // ================= MARKETING =================

        public int? CustomerType { get; set; }
        public string? ParentCustomerCode { get; set; }
        public string? VendorCode { get; set; }

        public string? CommissionVendorNo { get; set; }
        public int? CommissionType { get; set; }
        public int? Commission { get; set; }

        [StringLength(50)]
        public string? BankName { get; set; }

        [StringLength(100)]
        public string? BranchName { get; set; }

        [RegularExpression(@"^[0-9]{9,18}$", ErrorMessage = "Invalid bank account number")]
        [StringLength(20)]
        public string? BankAccountNo { get; set; }

        //[RegularExpression(@"^[A-Z]{4}0[A-Z0-9]{6}$", ErrorMessage = "Invalid IFSC code")]
        [StringLength(20)]
        public string? IFSCCode { get; set; }


        // ================= INVOICING =================

        public string? BillToCustomer { get; set; }
        public string? LocationCode { get; set; }
        public string? CustomerPostingGroup { get; set; }
        public string? GenBusPostingGroup { get; set; }

        [Required(ErrorMessage = "E-Invoice Phone is required")]
        [StringLength(30)]
        [RegularExpression(@"^[0-9]{1,20}$", ErrorMessage = "Invalid phone number")]
        public string EInvPhoneNo { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-Invoice Email is required")]
        [StringLength(80)]
        [EmailAddress(ErrorMessage = "Invalid email")]
        public string EInvEmail { get; set; } = string.Empty;

        public string? CurrencyCode { get; set; }

        // ================= PAYMENTS =================

        public int? ApplicationMethod { get; set; }
        public string? PaymentTermsCode { get; set; }
        public string? PaymentMethodCode { get; set; }

        // ================= TAX =================

        //[RegularExpression(@"^[A-Z]{5}[0-9]{4}[A-Z]$", ErrorMessage = "Invalid PAN format")]
        [StringLength(10, MinimumLength = 10)]
        public string? PANNo { get; set; }

        public int? GSTRegistrationType { get; set; }

        [RegularExpression(@"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z][1-9A-Z]Z[0-9A-Z]$", ErrorMessage = "Invalid GSTIN format")]
        [StringLength(15, MinimumLength = 15)]
        public string? GSTRegistrationNo { get; set; }

        public int? GSTCustomerType { get; set; }

        [RegularExpression(@"^[A-Z]{2}[0-9]{12}[A-Z]{1}$", ErrorMessage = "Invalid ARN format")]
        [StringLength(15, MinimumLength = 15)]
        public string? ARNNo { get; set; }

        public int? BusinessCategory { get; set; }
        public string? MSMEUAMNo { get; set; }


        // ================= NOD / NOC =================

        public bool IsNodNocCreation { get; set; }
        public string? NODAccessCode { get; set; }
        public string? NODNOC { get; set; }
        public string? ConcessionalCode { get; set; }
        public bool ThresholdOverlook { get; set; }
        public bool SurchargeOverlook { get; set; }

        // ===================Shipping Details=================
        public string? ShippingCode { get; set; }

        [StringLength(100)]
        public string? ShippingName { get; set; }

        [StringLength(100)]
        public string? ShippingAddress { get; set; }

        [StringLength(50)]
        public string? ShippingAddress2 { get; set; }

        [StringLength(30)]
        public string? ShippingCity { get; set; }

        [StringLength(20)]
        public string? ShippingPostalCode { get; set; }

        [StringLength(30)]
        public string? ShippingCountry { get; set; }

        [StringLength(30)]
        public string? ShippingPhoneNo { get; set; }

        [StringLength(100)]
        public string? ShippingContactPerson { get; set; }

        // New Fields
        [StringLength(80)]
        public string? ShippingEmail { get; set; }

        [StringLength(10)]
        public string? ShippingLocationCode { get; set; }

        [StringLength(10)]
        public string? ShippingMethodCode { get; set; }

        [StringLength(10)]
        public string? ShippingAgentCode { get; set; }

        [StringLength(10)]
        public string? ShippingAgentServiceCode { get; set; }
        public string? ShippingState { get; set; }

        [RegularExpression(@"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z][1-9A-Z]Z[0-9A-Z]$", ErrorMessage = "Invalid GSTIN format")]
        [StringLength(15, MinimumLength = 15)]
        public string? ShippingGSTRegistrationNo { get; set; }
        public int? ShippingAddressType { get; set; }
        public int? ShipToGSTCustomerType { get; set; }

        public int Blocked { get; set; } = 0;
        
        [StringLength(10)]
        public string? AssesseeCode { get; set; }

        public int? IsAlreadyCreatedMaster { get; set; } = 0;

        // ================= BRAND LIST =================

        public List<CustomerBrandWiseEditModel> CustomerBrandEditList { get; set; } = new();
    }


    public class GetCustomerDivisionWiseDropDown
    {
        public List<PriceListModel> PriceList = new List<PriceListModel>();

        public List<PromoCodeModel> PromoCodeList = new List<PromoCodeModel>();

        public List<ChargesGroupModel> ChargesGroupList = new List<ChargesGroupModel>();

        public List<ParentCustomerModel> ParentCustomerList = new List<ParentCustomerModel>();
    }

    public class CustomerBrandEditRequest
    {
        public List<CustomerBrandWiseEditModel> list { get; set; } = new();
        public string SalesPersonCode { get; set; } = string.Empty;
    }


    //UnBlock Edit Customer Model

    public class GetCustomerUnBlockEditModel : CustomerUnBlockEditModel
    {
        public CustomerDropDownModel CustomerDropDownModel { get; set; } = new();

        public CustomerBrandWiseModel customerBrandWiseModel { get; set; } = new CustomerBrandWiseModel();
    }

    public class CustomerUnBlockEditModel
    {
        public string? LoginRowId { get; set; }
        public int PortalRowId { get; set; }
        public int DisplayNo { get; set; }

        public string? CustomerCode { get; set; }

        // ================= GENERAL DETAILS =================

        [Required(ErrorMessage = "Division is required")]
        public int? Division { get; set; }
        public string? DivisionCode { get; set; }
        public string? MasterCode { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required")]
        [StringLength(100)]
        public string Address { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Address2 { get; set; } = string.Empty;


        [Required(ErrorMessage = "City is required")]
        [StringLength(30)]
        public string CityCode { get; set; } = string.Empty;


        [Required(ErrorMessage = "Post Code is required")]
        [StringLength(20)]
        public string PostCode { get; set; } = string.Empty;

        public string? PostCodeStr { get; set; } = string.Empty;

        [StringLength(10)]
        public string? StateCode { get; set; }


        [Required(ErrorMessage = "Country is required")]
        [StringLength(10)]
        public string CountryCode { get; set; } = string.Empty;


        [Required(ErrorMessage = "Region is required")]
        [StringLength(20)]
        public string Region { get; set; } = string.Empty;


        [Required(ErrorMessage = "Zone is required")]
        [StringLength(20)]
        public string Zone { get; set; } = string.Empty;

        public decimal? CreditLimit { get; set; } = 0;
        public string? PriceListCode { get; set; }
        public string? PromoCode { get; set; }
        public string? ChargesGroup { get; set; }

        [Required(ErrorMessage = "Sales Person Code is required")]
        public string SalesPersonCode { get; set; } = string.Empty;

        // ================= CONTACT =================

        [Required(ErrorMessage = "Contact Person is required")]
        [StringLength(50)]
        public string ContactPerson { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mobile No is required")]
        [RegularExpression(@"^(\+?[\d\-]+)(\/\+?[\d\-]+)*$",
    ErrorMessage = "Invalid mobile number")]
        [StringLength(30)]
        public string MobileNo { get; set; } = string.Empty;

        [StringLength(30)]
        [RegularExpression(@"^(\+?[\d\-]+)(\/\+?[\d\-]+)*$",
    ErrorMessage = "Invalid phone number")]
        public string? PhoneNo { get; set; }

        [Required(ErrorMessage = "Email is required")]
        //[EmailAddress(ErrorMessage = "Invalid email address")]
        [StringLength(80)]
        public string Email { get; set; } = string.Empty;

        [StringLength(80)]
        public string? Website { get; set; }


        // ================= MARKETING =================

        public int? CustomerType { get; set; }
        public string? ParentCustomerCode { get; set; }
        public string? VendorCode { get; set; }

        public string? CommissionVendorNo { get; set; }
        public int? CommissionType { get; set; }
        public int? Commission { get; set; }

        [StringLength(50)]
        public string? BankName { get; set; }

        [StringLength(100)]
        public string? BranchName { get; set; }

        [RegularExpression(@"^[0-9]{9,18}$", ErrorMessage = "Invalid bank account number")]
        [StringLength(20)]
        public string? BankAccountNo { get; set; }

        //[RegularExpression(@"^[A-Z]{4}0[A-Z0-9]{6}$", ErrorMessage = "Invalid IFSC code")]
        [StringLength(20)]
        public string? IFSCCode { get; set; }


        // ================= INVOICING =================

        public string? BillToCustomer { get; set; }
        public string? LocationCode { get; set; }
        public string? CustomerPostingGroup { get; set; }
        public string? GenBusPostingGroup { get; set; }

        [Required(ErrorMessage = "E-Invoice Phone is required")]
        [StringLength(30)]
        [RegularExpression(@"^[0-9]{1,20}$", ErrorMessage = "Invalid phone number")]
        public string EInvPhoneNo { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-Invoice Email is required")]
        [StringLength(80)]
        [EmailAddress(ErrorMessage = "Invalid email")]
        public string EInvEmail { get; set; } = string.Empty;

        public string? CurrencyCode { get; set; }

        // ================= PAYMENTS =================

        public int? ApplicationMethod { get; set; }
        public string? PaymentTermsCode { get; set; }
        public string? PaymentMethodCode { get; set; }

        // ================= TAX =================

        //[RegularExpression(@"^[A-Z]{5}[0-9]{4}[A-Z]$", ErrorMessage = "Invalid PAN format")]
        [StringLength(10, MinimumLength = 10)]
        public string? PANNo { get; set; }

        public int? GSTRegistrationType { get; set; }

        [RegularExpression(@"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z][1-9A-Z]Z[0-9A-Z]$", ErrorMessage = "Invalid GSTIN format")]
        [StringLength(15, MinimumLength = 15)]
        public string? GSTRegistrationNo { get; set; }

        public int? GSTCustomerType { get; set; }

        [RegularExpression(@"^[A-Z]{2}[0-9]{12}[A-Z]{1}$", ErrorMessage = "Invalid ARN format")]
        [StringLength(15, MinimumLength = 15)]
        public string? ARNNo { get; set; }

        public int? BusinessCategory { get; set; }
        public string? MSMEUAMNo { get; set; }


        // ================= NOD / NOC =================

        public bool IsNodNocCreation { get; set; }
        public string? NODAccessCode { get; set; }
        public string? NODNOC { get; set; }
        public string? ConcessionalCode { get; set; }
        public bool ThresholdOverlook { get; set; }
        public bool SurchargeOverlook { get; set; }

        // ===================Shipping Details=================
        public string? ShippingCode { get; set; }

        [StringLength(100)]
        public string? ShippingName { get; set; }

        [StringLength(100)]
        public string? ShippingAddress { get; set; }

        [StringLength(50)]
        public string? ShippingAddress2 { get; set; }

        [StringLength(30)]
        public string? ShippingCity { get; set; }

        [StringLength(20)]
        public string? ShippingPostalCode { get; set; }

        [StringLength(30)]
        public string? ShippingCountry { get; set; }

        [StringLength(30)]
        public string? ShippingPhoneNo { get; set; }

        [StringLength(100)]
        public string? ShippingContactPerson { get; set; }

        // New Fields
        [StringLength(80)]
        public string? ShippingEmail { get; set; }

        [StringLength(10)]
        public string? ShippingLocationCode { get; set; }

        [StringLength(10)]
        public string? ShippingMethodCode { get; set; }

        [StringLength(10)]
        public string? ShippingAgentCode { get; set; }

        [StringLength(10)]
        public string? ShippingAgentServiceCode { get; set; }
        public string? ShippingState { get; set; }

        [RegularExpression(@"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z][1-9A-Z]Z[0-9A-Z]$", ErrorMessage = "Invalid GSTIN format")]
        [StringLength(15, MinimumLength = 15)]
        public string? ShippingGSTRegistrationNo { get; set; }
        public int? ShippingAddressType { get; set; }
        public int? ShipToGSTCustomerType { get; set; }

        public int Blocked { get; set; } = 0;

        [StringLength(10)]
        public string? AssesseeCode { get; set; }

        public int? IsAlreadyCreatedMaster { get; set; } = 0;

        // ================= BRAND LIST =================

        public List<CustomerBrandWiseEditModel> CustomerBrandEditList { get; set; } = new();
    }


    public class CustomerAlreadyExistModel
    {
        public bool IsExist { get; set; }
        public string? CustomerNo { get; set; }
        public string? Name { get; set; }

        public string? Division { get; set; }
    }
}
