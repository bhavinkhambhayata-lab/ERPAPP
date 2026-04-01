using ERPAPP.Helper;
using ERPAPP.Interfaces;
using ERPAPP.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using static ERPAPP.Helper.Enums;
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

        public async Task<int> GetVendorTransferNewNo()
        {
            int newNo = 0;

            DataTable dt = _db.GetDataTable("VendorTransferEntry_GetNewNo");

            if (dt != null && dt.Rows.Count > 0)
            {
                newNo = Convert.ToInt32(dt.Rows[0]["NewDisplayNo"]);
            }

            return newNo;
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
            dropDown.VendorAllLocations = ds.Tables[9].AsEnumerable().Select(row => new VendorAllLocationModel
            {
                Code = row["Code"]?.ToString(),
                Name = row["Name"]?.ToString()
            }).ToList();

            // 10 Country Region
            dropDown.Countries = ds.Tables[10].AsEnumerable().Select(row => new VendorCountryModel
            {
                Code = row["Code"]?.ToString(),
                Name = row["Name"]?.ToString()
            }).ToList();

            // Enums
            #region Enums List

            dropDown.VendorGSTVendorTypes = Enum.GetValues(typeof(GSTVendorTypeEnum))
                 .Cast<GSTVendorTypeEnum>()
                 .Select(e => new VendorGSTVendorTypeModel
                 {
                     Code = ((int)e).ToString(),
                     Name = e.ToString()
                 }).ToList();

            dropDown.VendorGSTReturnFrequencies = Enum.GetValues(typeof(VendorGSTReturnFrequencyEnum))
                 .Cast<VendorGSTReturnFrequencyEnum>()
                 .Select(e => new VendorGSTReturnFrequencyModel
                 {
                     Code = ((int)e).ToString(),
                     Name = e.ToString()
                 }).ToList();


            dropDown.VendorTypes = Enum.GetValues(typeof(VendorTypeEnum))
                 .Cast<VendorTypeEnum>()
                 .Select(e => new VendorTypeModel
                 {
                     Code = ((int)e).ToString(),
                     Name = e.ToString()
                 }).ToList();

            dropDown.VendorApplicationMethods = Enum.GetValues(typeof(VendorApplicationMethodEnum))
                 .Cast<VendorApplicationMethodEnum>()
                 .Select(e => new VendorApplicationMethodModel
                 {
                     Code = ((int)e).ToString(),
                     Name = e.ToString()
                 }).ToList();

            dropDown.VendorBussinessCategories = Enum.GetValues(typeof(VendorBusinessCategoryEnum))
                .Cast<VendorBusinessCategoryEnum>()
                .Select(e => new VendorBussinessCategoryModel
                {
                    Code = ((int)e).ToString(),
                    Name = e.ToString()
                }).ToList();

            dropDown.VendorAggregateTurnover = Enum.GetValues(typeof(VendorAggTurnOver))
               .Cast<VendorAggTurnOver>()
               .Select(e => new VendorAggregateTurnoverModel
               {
                   Code = ((int)e).ToString(),
                   Name = e.GetDisplayName().ToString()
               }).ToList();

            #endregion

            model.DropDownData = dropDown;

            var displayNo = await GetVendorTransferNewNo();
            model.DisplayNo = displayNo > 0 ? displayNo : 0;


            return await Task.FromResult(model);
        }

        public async Task<List<GetVendorListModel>> GetVendorList(string searchVendor)
        {
            try
            {
                SqlParameter[] param =
                {
                    new SqlParameter("@SearchVendor", searchVendor ?? (object)DBNull.Value)
                };

                DataTable dt = _db.GetDataTable("GetVendorList", param);

                List<GetVendorListModel> list = new List<GetVendorListModel>();

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        GetVendorListModel model = new GetVendorListModel();

                        model.DisplayNo = row["DisplayNo"] != DBNull.Value ? Convert.ToInt32(row["DisplayNo"]) : 0;

                        model.Name = row["Name"]?.ToString();
                        model.City = row["City"]?.ToString();

                        model.Location = row["Location"]?.ToString();
                        model.ContactPerson = row["ContactPerson"]?.ToString();

                        model.MobileNo = row["MobileNo"]?.ToString();

                        model.MasterCode = row["MasterCode"]?.ToString();
                        model.CompanyCode = row["CompanyCode"]?.ToString();
                       
                        model.CreatedBy = row["CreatedBy"]?.ToString();

                        list.Add(model);
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                return new List<GetVendorListModel>();
            }
        }

        public async Task<bool> InsertVendor(VendorsModel model)
        {
            try
            {
                bool result = false;

                SqlParameter[] param =
                {
            // ================= SYSTEM =================

            new SqlParameter("@LoginRowId", Convert.ToInt32(model.LoginRowId ?? "0")),
            new SqlParameter("@DisplayNo", model.DisplayNo),

            // ================= BASIC =================

            new SqlParameter("@Name", model.Name ?? ""),
            new SqlParameter("@MasterCode", model.MasterCode ?? ""),
            new SqlParameter("@VendorCode", model.VendorCode ?? ""),

            new SqlParameter("@Address", model.Address ?? ""),
            new SqlParameter("@Address2", model.Address2 ?? ""),

            new SqlParameter("@CityCode", model.CityCode ?? ""),
            new SqlParameter("@PostCode", model.PostCode ?? ""),
            new SqlParameter("@StateCode", model.StateCode ?? ""),
            new SqlParameter("@CountryCode", model.CountryCode ?? ""),

            // ================= EXTRA =================

            new SqlParameter("@Range", model.Range ?? ""),
            new SqlParameter("@Collectorate", model.Collectorate ?? ""),
            new SqlParameter("@GTA", model.GTA ?? ""),
            new SqlParameter("@VendorLocation", model.VendorLocation ?? ""),

            new SqlParameter("@GSTNotToHold", model.GSTNotToHold),
            new SqlParameter("@FixedDueDate", model.FixedDueDate ?? (object)DBNull.Value),
            new SqlParameter("@AggregateTurnover", model.AggregateTurnover ?? 0),

            new SqlParameter("@FaxNo", model.FaxNo ?? ""),
            new SqlParameter("@ECCNo", model.ECCNo ?? ""),
            new SqlParameter("@ServiceTaxRegNo", model.ServiceTaxRegNo ?? ""),

            // ================= CONTACT =================

            new SqlParameter("@ContactPerson", model.ContactPerson ?? ""),
            new SqlParameter("@MobileNo", model.MobileNo ?? ""),
            new SqlParameter("@PhoneNo", model.PhoneNo ?? ""),
            new SqlParameter("@Email", model.Email ?? ""),
            new SqlParameter("@Website", model.Website ?? ""),
            new SqlParameter("@EmailNotAvailable", model.EmailNotAvailable),

            // ================= TAX =================

            new SqlParameter("@PANNo", model.PANNo ?? ""),
            new SqlParameter("@CurrencyCode", model.CurrencyCode ?? ""),

            // ================= GST =================

            new SqlParameter("@GSTVendorType", model.GSTVendorType ?? "0"),
            new SqlParameter("@GSTReturnFrequency", model.GSTReturnFrequency ?? "0"),
            new SqlParameter("@GSTRegNo", model.GSTRegNo ?? ""),
            new SqlParameter("@ARN", model.ARN ?? ""),

            // ================= BANK =================

            new SqlParameter("@BankName", model.BankName ?? ""),
            new SqlParameter("@BankAccountNo", model.BankAccountNo ?? ""),
            new SqlParameter("@BranchName", model.BranchName ?? ""),
            new SqlParameter("@IFSCCode", model.IFSCCode ?? ""),

            // ================= BUSINESS =================

            new SqlParameter("@VendorType", model.VendorType ?? "0"),
            new SqlParameter("@VendorCategory", model.VendorCategory ?? ""),
            new SqlParameter("@BusinessCategory", model.BusinessCategory ?? "0"),

            new SqlParameter("@RelatedParty", model.RelatedParty ?? false),
            new SqlParameter("@Subcontractor", model.Subcontractor ?? false),

            new SqlParameter("@PaymentTerms", model.PaymentTerms ?? ""),
            new SqlParameter("@PaymentMethod", model.PaymentMethod ?? ""),
            new SqlParameter("@PurchaserCode", model.PurchaserCode ?? ""),

            // ================= POSTING =================

            new SqlParameter("@VATBusPostingGroup", model.VATBusPostingGroup ?? ""),
            new SqlParameter("@GenBusPostingGroup", model.GenBusPostingGroup ?? ""),
            new SqlParameter("@VendorPostingGroup", model.VendorPostingGroup ?? ""),

            // ================= OTHER =================

            new SqlParameter("@ApplicationMethod", model.ApplicationMethod ?? "0"),
            new SqlParameter("@TaxLiable", model.TaxLiable ?? "0"),
            new SqlParameter("@Location", model.Location ?? ""),

            // ================= MSME =================

            new SqlParameter("@MSMEUAMNo", model.MSMEUAMNo ?? ""),
            new SqlParameter("@MSMEIntimationDate", model.MSMEIntimationDate ?? (object)DBNull.Value),
            new SqlParameter("@MSMEEffectiveDate", model.MSMEEffectiveDate ?? (object)DBNull.Value),

            // ================= NOD/NOC =================

            new SqlParameter("@AccessCode", model.AccessCode ?? ""),
            new SqlParameter("@NOCNOD", model.NOCNOD ?? ""),
            new SqlParameter("@ConcessionalCode", model.ConcessionalCode ?? ""),

            new SqlParameter("@ThresholdOverlook", model.ThresholdOverlook),
            new SqlParameter("@SurchargeOverlook", model.SurchargeOverlook)
        };

                var vendorObj = _db.ExecuteScalar("Vendor_InsertDataWithMasterAndCompnayData", param);

                string vendorNo = vendorObj?.ToString();

                if (!string.IsNullOrEmpty(vendorNo) && vendorNo.StartsWith("ICV"))
                {
                    result = true;
                }
                else
                {
                    result = false;
                }

                return result;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
