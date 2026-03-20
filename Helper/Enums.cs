using System.ComponentModel.DataAnnotations;

namespace ERPAPP.Helper
{



    public class Enums
    {

        public enum BusinessCategory
        {
            None = 0,
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
            None = 0,
            A = 1,
            B = 2,
            C = 3,
            D = 4
        }

        public enum CustomerType
        {
            None = 0,
            Channel = 1,
            InstitutionalClient = 2,
            EndClient = 3
        }

        public enum CommissionType
        {
            None = 0,
            Differential = 1,
            PercentOfNetRealization = 2,
            QtyPerUOM = 3
        }

        public enum GSTCustomerType
        {
            None = 0,
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
            GSTIN = 1,
            UID = 2,
            GID = 3
        }

        public enum Shipping_To_GST_Customer_Type
        {
            None = 0,
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
            ShippingAddress = 0,

            [Display(Name = "Document Forwarding Address")]
            DocumentForwardingAddress = 1
        }
    }
}
