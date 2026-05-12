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
            Manual = 0,
            ApplyToOldest = 1
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
            Client = 2,
            Specifier = 3
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
            Monthly = 1,
            Quarterly = 2
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
            Manual = 0,
            ApplyToOldest = 1
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

        #region Fixed Asset Module
        public enum FixedAssetMainAssetComponet
        {
            [Display(Name = "Main Asset")]
            MainAsset = 0,

            [Display(Name = "Component")]
            Component = 1
        }

        public enum FixedAssetDepreciationMethod
        {
            [Display(Name = "Straight-Line")]
            StraightLine = 0,

            [Display(Name = "Declining-Balance 1")]
            DecliningBalance = 1
        }

        public enum FixedAssetExciseAccountingType
        {
            [Display(Name = "With CENTVAT")]
            WithCENTVAT = 1,

            [Display(Name = "Without CENTVAT")]
            WithoutCENTVAT = 2
        }

        public enum FixedAssetDepreciationBookCode
        {
            [Display(Name = "COMPANY")]
            COMPANY = 1,

            [Display(Name = "INCOME TAX")]
            INCOMETAX = 2
        }

        public enum FixedAssetGSTCredit
        {
            [Display(Name = "Availment")]
            Availment = 1,

            [Display(Name = "Non-Availment")]
            NonAvailment = 2
        }
        #endregion

        #region Item Module

        public enum ItemMovementType
        {
            StdSKU = 1,
            NonStdSKU = 2,
            D0 = 3,
            D1 = 4,
            D2 = 5,
            D3 = 6,
            D4 = 7,
            D5 = 8,
            D6 = 9,
            Obsolete = 10
        }

        public enum ItemTypeOfProduct
        {
            Tile = 1,
            Mosaic = 2
        }

        public enum ItemManufacturingPolicy
        {
            [Display(Name = "Make-to-Stock")]
            MakeToStock = 1,

            [Display(Name = "Make-to-Order")]
            MakeToOrder = 2
        }

        public enum ItemCostingMethod
        {
            [Display(Name = "FIFO")]
            FIFO = 0,
            [Display(Name = "LIFO")]
            LIFO = 1,
            [Display(Name = "Specific")]
            Specific = 2,
            [Display(Name = "Average")]
            Average = 3,
            [Display(Name = "Standard")]
            Standard = 4
        }

        public enum ItemGSTCredit
        {
            [Display(Name = "Availment")]
            Availment = 1,

            [Display(Name = "Non-Availment")]
            NonAvailment = 2
        }

        public enum ItemReplenishmentSystem
        {
            [Display(Name = "Purchase")]
            Purchase = 1,

            [Display(Name = "Prod. Order")]
            ProdOrder = 2,

            [Display(Name = "Assembly")]
            Assembly = 3
        }

        public enum ItemReorderingPolicy
        {
            [Display(Name = "Fixed Reorder Qty.")]
            FixedReorderQty = 1,

            [Display(Name = "Maximum Qty.")]
            MaximumQty =2,

            [Display(Name = "Order.")]
            Order =3,

            [Display(Name = "Lot-for-lot.")]
            LotForLot = 4
        }

        #endregion



    }
}
