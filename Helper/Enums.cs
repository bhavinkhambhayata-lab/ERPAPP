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
    }
}
