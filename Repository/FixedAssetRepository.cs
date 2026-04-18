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
        private readonly IEmailRepository _emailRepository;

        public FixedAssetRepository(DbHelper db, IEmailRepository emailRepository)
        {
            _db = db;
            _emailRepository = emailRepository;
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

            dropDown.GSTCreditList = Enum.GetValues(typeof(FixedAssetGSTCredit))
               .Cast<FixedAssetGSTCredit>()
               .Select(e => new FixedAssetGSTCreditModel
               {
                   Code = ((int)e).ToString(),
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

                      new SqlParameter("@DepreciationStartingDate",
                        model.DepreciationStartingDate == null || model.DepreciationStartingDate <= new DateTime(1753, 1, 1)
                        ? new DateTime(1753, 1, 1)
                        : model.DepreciationStartingDate
                    ),

                        new SqlParameter("@StraightLinePercent", model.StraightLinePercent ?? 0),
                        new SqlParameter("@DecliningBalancePercent", model.DecliningBalancePercent ?? 0),

                        new SqlParameter("@InstallationDate",
                            model.InstallationDate == null || model.InstallationDate <= new DateTime(1753, 1, 1)
                            ? new DateTime(1753, 1, 1)
                            : model.InstallationDate
                        ),

                        new SqlParameter("@MasterCode", (object?)model.MasterCode ?? DBNull.Value),
                        new SqlParameter("@CompanyCode", (object?)model.CompanyCode ?? DBNull.Value),
                         new SqlParameter("@DivisionStr", model.DivisionStr),
                        new SqlParameter("@Division", model.Division),

                        new SqlParameter("@GSTGroupCode", (object?)model.GSTGroupCode ?? DBNull.Value),
                        new SqlParameter("@HSNSACCode", (object?)model.HSNSACCode ?? DBNull.Value),
                        new SqlParameter("@GSTCredit", model.GSTCredit ?? 0),

                    };

                // ================= EXECUTE =================

                var resultObj = _db.ExecuteScalar("FixedAsset_InsertDataWithCompanyData", param);

                string resultValue = resultObj?.ToString();

                // ================= RESULT CHECK =================

                if (!string.IsNullOrEmpty(resultValue) && (resultValue.StartsWith("AFA") || resultValue.StartsWith("MFA") || resultValue.StartsWith("FA")))
                {

                    var sendEmail = await _emailRepository.SendMailFixedAssetUnBlock(new FixedAssetEmailItem
                    {
                        SrNo = 1,
                        Description = model.Description,
                        FixedAssetNo = resultValue,
                        Division = model.DivisionStr
                    });

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<GetFixedAssetListModel>> GetFixedAssetList(string searchDescription)
        {
            try
            {
                SqlParameter[] param =
                {
                    new SqlParameter("@SearchDescription", searchDescription ?? (object)DBNull.Value)
                };

                DataTable dt = _db.GetDataTable("GetFixedAssetList", param);

                List<GetFixedAssetListModel> list = new List<GetFixedAssetListModel>();

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        GetFixedAssetListModel model = new GetFixedAssetListModel();

                        model.RowID = row["RowID"]?.ToString();
                        model.DisplayNo = row["DisplayNo"] != DBNull.Value ? Convert.ToInt32(row["DisplayNo"]) : 0;
                        model.Description = row["Description"]?.ToString();
                        model.CompanyCode = row["CompanyCode"]?.ToString();

                        model.Division = row["Division"]?.ToString();
                        model.LocationCode = row["LocationCode"]?.ToString();
                        model.FASubClassCode = row["FASubClassCode"]?.ToString();
                        model.FAClassCode = row["FAClassCode"]?.ToString();
                        model.CreatedBy = row["CreatedBy"]?.ToString();

                        list.Add(model);
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                return new List<GetFixedAssetListModel>();
            }
        }

        public async Task<GetFixedAssetEditData> GetFixedAssetEditData(string fixedAssetNo)
        {
            SqlParameter[] param =
     {
                new SqlParameter("@CompanyCode", fixedAssetNo)
            };

            DataSet ds = _db.GetDataSet("GetFixedAsset_GetEditData", param);

            GetFixedAssetEditData model = new GetFixedAssetEditData();

            // ================= MAIN DATA =================
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];

                model.RowID = Convert.ToInt32(row["RowID"]);
                model.DisplayNo = Convert.ToInt32(row["DisplayNo"]);

                // ===== BASIC =====
                model.FAClassCode = row["FAClassCode"]?.ToString();
                model.FASubclassCode = row["FASubclassCode"]?.ToString();
                model.Description = row["Description"]?.ToString();
                model.Description2 = row["Description2"]?.ToString();
                model.SerialNo = row["SerialNo"]?.ToString();

                // ===== ASSET =====
                model.MainAssetComponent = row["MainAssetComponent"] as int?;
                model.ComponentOfMainAsset = row["ComponentOfMainAsset"]?.ToString();

                model.LocationCode = row["LocationCode"]?.ToString();
                model.FALocationCode = row["FALocationCode"]?.ToString();

                // ===== POSTING =====
                model.GenProdPostingGroup = row["GenProdPostingGroup"]?.ToString();
                model.FAPostingGroup = row["FAPostingGroup"]?.ToString();

                model.ExciseAccountingType = row["ExciseAccountingType"] as int?;
                model.TaxGroupCode = row["TaxGroupCode"]?.ToString();
                model.VATProductPostingGroup = row["VATProductPostingGroup"]?.ToString();

                // ===== DEPRECIATION =====
                model.DepreciationBookCode = row["DepreciationBookCode"]?.ToString();
                model.DepreciationMethod = row["DepreciationMethod"] as int?;
                model.DepreciationStartingDate = row["DepreciationStartingDate"] as DateTime?;

                model.StraightLinePercent = row["StraightLinePercent"] as double?;
                model.DecliningBalancePercent = row["DecliningBalancePercent"] as double?;

                model.InstallationDate = row["InstallationDate"] as DateTime?;

                // ===== OTHER =====
                model.MasterCode = row["MasterCode"]?.ToString();
                model.CompanyCode = row["CompanyCode"]?.ToString();
                model.LoginRowID = Convert.ToInt32(row["LoginRowID"]);

                model.GSTGroupCode = row["GSTGroupCode"]?.ToString();
                model.HSNSACCode = row["HSNSACCode"]?.ToString();

                model.Blocked = Convert.ToInt32(row["Blocked"]);

                // ===== DIVISION =====
                model.DivisionStr = row["Division"]?.ToString();

                model.GSTCredit = row["GSTCredit"] != DBNull.Value ? Convert.ToInt32(row["GSTCredit"]) : 0;

                var divisionCode = model.DivisionStr?.Trim().ToUpper();

                if (divisionCode == "MOSAIC")
                    model.Division = 2;
                else if (divisionCode == "TILE")
                    model.Division = 1;
                else
                    model.Division = 3;
            }

            return model;
        }

        public async Task<GetFixedAssetEditData> GetFixedAssetEditDropDownData()
        {
            var model = new GetFixedAssetEditData();
            var dropDown = new FixedAssetEditDropDownModel();

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

            dropDown.GSTCreditList = Enum.GetValues(typeof(FixedAssetGSTCredit))
              .Cast<FixedAssetGSTCredit>()
              .Select(e => new FixedAssetGSTCreditModel
              {
                  Code = ((int)e).ToString(),
                  Name = e.GetDisplayName().ToString()
              }).ToList();
            #endregion


            model.DropDownData = dropDown;


            return await Task.FromResult(model);
        }

        public async Task<bool> FixedAssetUnblock(string fixedAssetNo)
        {
            SqlParameter[] param =
             {
                new SqlParameter("@CompanyCode", fixedAssetNo)
            };

            object result = _db.ExecuteScalar("FixedAsset_ChangeUnBlocked", param);

            int rowsAffected = (result != null) ? Convert.ToInt32(result) : 0;

            return rowsAffected > 0;
        }

        public async Task<bool> UpdateFixedAssetData(FixedAssetEditModel model)
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

                      new SqlParameter("@DepreciationStartingDate",
                        model.DepreciationStartingDate == null || model.DepreciationStartingDate <= new DateTime(1753, 1, 1)
                        ? new DateTime(1753, 1, 1)
                        : model.DepreciationStartingDate
                    ),

                        new SqlParameter("@StraightLinePercent", model.StraightLinePercent ?? 0),
                        new SqlParameter("@DecliningBalancePercent", model.DecliningBalancePercent ?? 0),

                        new SqlParameter("@InstallationDate",
                            model.InstallationDate == null || model.InstallationDate <= new DateTime(1753, 1, 1)
                            ? new DateTime(1753, 1, 1)
                            : model.InstallationDate
                        ),

                        new SqlParameter("@MasterCode", (object?)model.MasterCode ?? DBNull.Value),
                        new SqlParameter("@CompanyCode", (object?)model.CompanyCode ?? DBNull.Value),
                         new SqlParameter("@DivisionStr", model.DivisionStr),
                        new SqlParameter("@Division", model.Division),

                        new SqlParameter("@GSTGroupCode", (object?)model.GSTGroupCode ?? DBNull.Value),
                        new SqlParameter("@HSNSACCode", (object?)model.HSNSACCode ?? DBNull.Value),
                        new SqlParameter("@GSTCredit", model.GSTCredit ?? 0),
                    };

                // ================= EXECUTE =================

                var resultObj = _db.ExecuteScalar("FixedAsset_UpdateData", param);
                //var resultObj = "AFA";

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
