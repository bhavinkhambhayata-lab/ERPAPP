using System.ComponentModel.DataAnnotations;

namespace ERPAPP.Helper
{



    public class Enums
    {

        public enum BusinessCategory
        {
            MSMEMicro = 1,
            MSMESmall = 2,
            MSMEMedium = 3,
            Large = 4
        }

        public enum ApplicationMethod
        {
            Manual = 1,
            ApplyToOldest = 2
        }

        public enum DealerClassification
        {
            A = 1,
            B = 2,
            C = 3,
            D = 4
        }

        public enum CustomerType
        {
            Channel = 1,
            InstitutionalClient = 2,
            EndClient = 3
        }

        public enum CommissionType
        {
            Differential = 1,
            PercentOfNetRealization = 2,
            QtyPerUOM = 3
        }

        public enum GSTCustomerType
        {
            Registered = 1,
            Unregistered = 2,
            Export = 3,
            DeemedExport = 4,
            Exempted = 5,
            SEZDevelopment = 6,
            SEZUnit = 7
        }

        public enum GSTRegistrationType
        {
            GSTIN = 0,
            UID = 1,
            GID = 2
        }

        public enum Shipping_To_GST_Customer_Type
        {
            Registered = 1,
            Unregistered = 2,
            Export = 3,
            DeemedExport = 4,
            Exempted = 5,
            SEZDevelopment = 6,
            SEZUnit = 7
        }

        public enum ShippingAddressType
        {
            [Display(Name = "Shipping Address")]
            ShippingAddress = 1,

            [Display(Name = "Document Forwarding Address")]
            DocumentForwardingAddress = 2
        }

        //Vendor Module

        public enum VendorGSTReturnFrequencyEnum
        {
            Quarterly = 1,
            Monthly = 2
        }

        public enum VendorTypeEnum
        {
            Manufacturer = 1,
            FirstStageDealer = 2,
            SecondStageDealer = 3,
            Importer = 4
        }

        public enum GSTVendorTypeEnum
        {
            [Display(Name = "Registred")]
            Registred = 1,

            [Display(Name = "Composite")]
            Composite = 2,

            [Display(Name = "Unregistred")]
            Unregistred = 3,

            [Display(Name = "Import")]
            Import = 4,

            [Display(Name = "Exempted")]
            Exempted = 5,

            [Display(Name = "SEZ")]
            SEZ = 6
        }

        public enum VendorApplicationMethodEnum
        {
            Manual = 1,
            ApplyToOldest = 2
        }
        public enum VendorBusinessCategoryEnum
        {
            MSMEMicro = 1,
            MSMESmall = 2,
            MSMEMedium = 3,
            Large = 4
        }

        public enum VendorAggTurnOver
        {
            [Display(Name = "More than 20 lakh")]
            MoreThanTwentyLakh = 1,

            [Display(Name = "Less than 20 lakh")]
            LessThanTwentyLakh = 2
        }

    }
}
