namespace ERPAPP.Models
{
    public class EmailModel
    {
    }

    public class MailListDto
    {
        public string TOMailID { get; set; }
        public string CCMailID { get; set; }
        public string BCCMailID { get; set; }
    }

    public class CustomerEmailItemDto
    {
        public int SrNo { get; set; }
        public string? RequestedBy { get; set; }
        public string? Name { get; set; }

        public string? CustomerCode { get; set; }
        public string? Division { get; set; }
        public string? MailID { get; set; } // From Mail
    }

    public class VendorEmailItemDto
    {
        public int SrNo { get; set; }

        public string? RequestedBy { get; set; }
        public string? VendorName { get; set; }
        public string? VendorCode { get; set; }
        public string? VendorCategory { get; set; }
        public string? MailID { get; set; } // From Mail
        public string? PurchaseCode { get; set; }
        public string? PaymentTerm { get; set; }
        public string? PaymentMethod { get; set; }
    }

    public class VendorLocationEmail
    {
        public string Code { get; set; }
        public string EmpRowID { get; set; }
    }

    public class FixedAssetEmailItem
    {
        public int SrNo { get; set; }

        public string? RequestedBy { get; set; } = null;
        public string? Description { get; set; } = null;
        public string? FixedAssetNo { get; set; } = null;
        public string? Division { get; set; } = null;
    }
}
