using ERPAPP.Helper;
using ERPAPP.Interfaces;
using ERPAPP.Models;
using System.Data;
using static ERPAPP.Models.VendorsModel;

namespace ERPAPP.Repository
{
    public class VendorRepository : IVendorRepository
    {
        private readonly DbHelper _db;

        public VendorRepository(DbHelper db)
        {
            _db = db;
        }

        public async Task<GetVendorAddData> GetVendorAddData()
        {
            var model = new GetVendorAddData();
            var dropDown = new VendorDropDownModel();

            DataSet ds = _db.GetDataSet("GetVendorAddDropDownData");

            // 0 Currency
            dropDown.VendorCurrencies = ds.Tables[0].AsEnumerable().Select(row => new VendorCurrencyModel
            {
                Code = row["Code"]?.ToString(),
                Name = row["Name"]?.ToString()
            }).ToList();

            // 1 Vendor Location (Subcontracting)
            dropDown.VendorLocations = ds.Tables[1].AsEnumerable().Select(row => new VendorLocationModel
            {
                Code = row["Code"]?.ToString(),
                Name = row["Name"]?.ToString()
            }).ToList();

            // 2 Category
            dropDown.VendorCategories = ds.Tables[2].AsEnumerable().Select(row => new VendorCategoryModel
            {
                Code = row["Code"]?.ToString(),
                Name = row["Name"]?.ToString()
            }).ToList();

            // 3 Payment Terms
            dropDown.VendorPaymentTerms = ds.Tables[3].AsEnumerable().Select(row => new VendorPaymentTermsModel
            {
                Code = row["Code"]?.ToString(),
                Name = row["Name"]?.ToString()
            }).ToList();

            // 4 Payment Method
            dropDown.VendorPaymentMethods = ds.Tables[4].AsEnumerable().Select(row => new VendorPaymentMethodModel
            {
                Code = row["Code"]?.ToString(),
                Name = row["Name"]?.ToString()
            }).ToList();

            // 5 Purchaser
            dropDown.VendorPurchasers = ds.Tables[5].AsEnumerable().Select(row => new VendorPurchaserModel
            {
                Code = row["Code"]?.ToString(),
                Name = row["Name"]?.ToString()
            }).ToList();

            // 6 Gen Business Posting Group
            dropDown.VendorGenBusPostingGroups = ds.Tables[6].AsEnumerable().Select(row => new VendorGenBusPostingGroupModel
            {
                Code = row["Code"]?.ToString(),
                Name = row["Name"]?.ToString()
            }).ToList();

            // 7 Vendor Posting Group
            dropDown.VendorPostingGroups = ds.Tables[7].AsEnumerable().Select(row => new VendorPostingGroupModel
            {
                Code = row["Code"]?.ToString(),
                Name = row["Name"]?.ToString()
            }).ToList();

            // 8 VAT Business Posting Group
            dropDown.VendorVATBusPostingGroups = ds.Tables[8].AsEnumerable().Select(row => new VendorVATBusPostingGroupModel
            {
                Code = row["Code"]?.ToString(),
                Name = row["Name"]?.ToString()
            }).ToList();

            // 9 Location (All)
            dropDown.VendorAllLocations = ds.Tables[9].AsEnumerable().Select(row => new LocationModel
            {
                Code = row["Code"]?.ToString(),
                Name = row["Name"]?.ToString()
            }).ToList();

            // 10 Assessee Code
            dropDown.VendorAssesseeCodes = ds.Tables[10].AsEnumerable().Select(row => new VendorAssesseeCodeModel
            {
                Code = row["Code"]?.ToString(),
                Name = row["Name"]?.ToString()
            }).ToList();

            // 11 Concessional Codes
            dropDown.VendorConcessionalCodes = ds.Tables[11].AsEnumerable().Select(row => new ConcessionalCodeModel
            {
                Code = row["Code"]?.ToString(),
                Name = row["Name"]?.ToString()
            }).ToList();

            model.DropDownData = dropDown;

            return await Task.FromResult(model);
        }
    }
}
