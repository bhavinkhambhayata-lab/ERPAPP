using ERPAPP.Helper;
using ERPAPP.Interfaces;
using ERPAPP.Models;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Data;
using static ERPAPP.Helper.Enums;

namespace ERPAPP.Repository
{
    public class FixedAssetRepository : IFixedAssetRepository
    {
        private readonly DbHelper _db;

        public FixedAssetRepository(DbHelper db)
        {
            _db = db;
        }

        public async Task<GetFixedAssetAddData> GetFixedAssetAddData()
        {
            var model = new GetFixedAssetAddData();
            var dropDown = new FixedAssetDropDownModel();

            DataSet ds = _db.GetDataSet("FixedAssetAddDropDownData");

            // 0 FA Class
            dropDown.FAClass = ds.Tables[0].AsEnumerable().Select(row => new FAClassModel
            {
                Code = row["Code"]?.ToString(),
                Name = row["Name"]?.ToString()
            }).ToList();

            // 1 FA Subclass
            dropDown.FASubClass = ds.Tables[1].AsEnumerable().Select(row => new FASubClassModel
            {
                Code = row["Code"]?.ToString(),
                Name = row["Name"]?.ToString()
            }).ToList();

            // 2 Location
            dropDown.Location = ds.Tables[2].AsEnumerable().Select(row => new LocationModel
            {
                Code = row["Code"]?.ToString(),
                Name = row["Name"]?.ToString()
            }).ToList();

            // 3 FA Location
            dropDown.FALocation = ds.Tables[3].AsEnumerable().Select(row => new FALocationModel
            {
                Code = row["Code"]?.ToString(),
                Name = row["Name"]?.ToString()
            }).ToList();

            // 4 Gen Product Posting Group
            dropDown.GenProductPostingGroup = ds.Tables[4].AsEnumerable().Select(row => new GenProductPostingGroupModel
            {
                Code = row["Code"]?.ToString(),
                Name = row["Name"]?.ToString()
            }).ToList();

            // 5 FA Posting Group
            dropDown.FAPostingGroup = ds.Tables[5].AsEnumerable().Select(row => new FAPostingGroupModel
            {
                Code = row["Code"]?.ToString(),
                Name = row["Name"]?.ToString()
            }).ToList();

            // 6 GST Group
            dropDown.GSTGroup = ds.Tables[6].AsEnumerable().Select(row => new GSTGroupModel
            {
                Code = row["Code"]?.ToString(),
                Name = row["Name"]?.ToString()
            }).ToList();

            // 7 Tax Group
            dropDown.TaxGroup = ds.Tables[7].AsEnumerable().Select(row => new TaxGroupModel
            {
                Code = row["Code"]?.ToString(),
                Name = row["Name"]?.ToString()
            }).ToList();

            // 8 VAT Product Posting Group
            dropDown.VATProductPostingGroup = ds.Tables[8].AsEnumerable().Select(row => new VATProductPostingGroupModel
            {
                Code = row["Code"]?.ToString(),
                Name = row["Name"]?.ToString()
            }).ToList();

            // 9 Company Division
            dropDown.FixedAssetDivision = ds.Tables[9].AsEnumerable().Select(row => new FixedAssetDivisionModel
            {
                Code = row["Code"]?.ToString(),
                Name = row["Name"]?.ToString()
            }).ToList();

            #region Enums 

            dropDown.MainAssetComponentList = Enum.GetValues(typeof(FixedAssetMainAssetComponet))
                 .Cast<FixedAssetMainAssetComponet>()
                 .Select(e => new MainAssetComponentModel
                 {
                     Code = ((int)e).ToString(),
                     Name = e.GetDisplayName().ToString()
                 }).ToList();

            dropDown.DepreciationMethodList = Enum.GetValues(typeof(FixedAssetDepreciationMethod))
                .Cast<FixedAssetDepreciationMethod>()
                .Select(e => new DepreciationMethodModel
                {
                    Code = ((int)e).ToString(),
                    Name = e.GetDisplayName().ToString()
                }).ToList();

            dropDown.ExciseAccountingTypeList = Enum.GetValues(typeof(FixedAssetExciseAccountingType))
                .Cast<FixedAssetExciseAccountingType>()
                .Select(e => new ExciseAccountingTypeModel
                {
                    Code = ((int)e).ToString(),
                    Name = e.GetDisplayName().ToString()
                }).ToList();

            dropDown.DepreciationBookCode = Enum.GetValues(typeof(FixedAssetDepreciationBookCode))
                .Cast<FixedAssetDepreciationBookCode>()
                .Select(e => new DepreciationBookCodeModel
                {
                    Code = e.GetDisplayName().ToString(),
                    Name = e.GetDisplayName().ToString()
                }).ToList();
            #endregion


            model.DropDownData = dropDown;

            var displayNo = await GetFixedAssetTransferNewNo();
            model.DisplayNo = displayNo > 0 ? displayNo : 0;

            return await Task.FromResult(model);
        }

        public async Task<int> GetFixedAssetTransferNewNo()
        {
            int newNo = 0;

            DataTable dt = _db.GetDataTable("Fixed_Asset_Transfer_Entry_GetNewNo");

            if (dt != null && dt.Rows.Count > 0)
            {
                newNo = Convert.ToInt32(dt.Rows[0]["NewDisplayNo"]);
            }

            return newNo;
        }

        public async Task<List<FAHSNModel>> GetFixedAssetHSNDataWithGSTGroupCode(string gstGroupCode)
        {
            List<FAHSNModel> list = new List<FAHSNModel>();

            SqlParameter[] param = new SqlParameter[]
            {
                 new SqlParameter("@GSTGroupCode", gstGroupCode)
            };

            DataSet ds = _db.GetDataSet("GetFixedAssetHSNDataWithGSTGroupCode", param);

            if (ds != null && ds.Tables.Count > 0)
            {
                list = ds.Tables[0].AsEnumerable().Select(row => new FAHSNModel
                {
                    Code = row["Code"]?.ToString(),
                    Name = row["Name"]?.ToString()
                }).ToList();
            }

            return list;
        }

        public async Task<List<FixedAssetComponetOfMainAssetModel>> GetFixedAssetComponentWithDivision(string division)
        {
            List<FixedAssetComponetOfMainAssetModel> list = new List<FixedAssetComponetOfMainAssetModel>();

            SqlParameter[] param = new SqlParameter[]
            {
                 new SqlParameter("@Division", division)
            };

            DataSet ds = _db.GetDataSet("GetFixedAssetComponetWithDivision", param);

            if (ds != null && ds.Tables.Count > 0)
            {
                list = ds.Tables[0].AsEnumerable().Select(row => new FixedAssetComponetOfMainAssetModel
                {
                    Code = row["Code"]?.ToString(),
                    Name = row["Name"]?.ToString()
                }).ToList();
            }

            return list;
        }

        public async Task<bool> InsertFixedAssetData(FixedAssetModel model)
        {
            try
            {
                // ================= BUSINESS LOGIC =================

                if (model.DepreciationMethod == 0) // Straight-Line
                {
                    model.DecliningBalancePercent = 0;
                }
                else if (model.DepreciationMethod == 1) // Declining-Balance
                {
                    model.StraightLinePercent = 0;
                }

                // ================= PARAMETERS =================

                SqlParameter[] param =
                {
                        new SqlParameter("@LoginRowId", model.LoginRowID),
                        new SqlParameter("@Brand", model.Brand),
                        new SqlParameter("@DisplayNo", model.DisplayNo),

                        new SqlParameter("@FAClassCode", model.FAClassCode),
                        new SqlParameter("@FASubclassCode", model.FASubclassCode),
                        new SqlParameter("@Description", model.Description),
                        new SqlParameter("@Description2", (object?)model.Description2 ?? DBNull.Value),
                        new SqlParameter("@SerialNo", (object?)model.SerialNo ?? DBNull.Value),

                        new SqlParameter("@MainAssetComponent", model.MainAssetComponent ?? 0),
                        new SqlParameter("@ComponentOfMainAsset", (object?)model.ComponentOfMainAsset ?? DBNull.Value),

                        new SqlParameter("@LocationCode", (object?)model.LocationCode ?? DBNull.Value),
                        new SqlParameter("@FALocationCode", (object?)model.FALocationCode ?? DBNull.Value),

                        new SqlParameter("@GenProdPostingGroup", model.GenProdPostingGroup),
                        new SqlParameter("@FAPostingGroup", model.FAPostingGroup),

                        new SqlParameter("@ExciseAccountingType", model.ExciseAccountingType ?? 0),

                        new SqlParameter("@TaxGroupCode", (object?)model.TaxGroupCode ?? DBNull.Value),
                        new SqlParameter("@VATProductPostingGroup", (object?)model.VATProductPostingGroup ?? DBNull.Value),

                        new SqlParameter("@DepreciationBookCode", (object?)model.DepreciationBookCode ?? DBNull.Value),
                        new SqlParameter("@DepreciationMethod", model.DepreciationMethod ?? 0),

                        new SqlParameter("@DepreciationStartingDate", (object?)model.DepreciationStartingDate ?? DBNull.Value),

                        new SqlParameter("@StraightLinePercent", model.StraightLinePercent ?? 0),
                        new SqlParameter("@DecliningBalancePercent", model.DecliningBalancePercent ?? 0),

                        new SqlParameter("@InstallationDate", (object?)model.InstallationDate ?? DBNull.Value),

                        new SqlParameter("@MasterCode", (object?)model.MasterCode ?? DBNull.Value),
                        new SqlParameter("@CompanyCode", (object?)model.CompanyCode ?? DBNull.Value),
                         new SqlParameter("@DivisionStr", model.DivisionStr),
                        new SqlParameter("@Division", model.Division),

                        new SqlParameter("@GSTGroupCode", (object?)model.GSTGroupCode ?? DBNull.Value),
                        new SqlParameter("@HSNSACCode", (object?)model.HSNSACCode ?? DBNull.Value),
                       
                    };

                // ================= EXECUTE =================

                var resultObj = _db.ExecuteScalar("FixedAsset_InsertDataWithCompanyData", param);

                string resultValue = resultObj?.ToString();

                // ================= RESULT CHECK =================

                if (!string.IsNullOrEmpty(resultValue) && (resultValue.StartsWith("AFA") || resultValue.StartsWith("MFA") || resultValue.StartsWith("FA")))
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
