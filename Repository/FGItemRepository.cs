using ERPAPP.Helper;
using ERPAPP.Interfaces;
using ERPAPP.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection;
using static ERPAPP.Helper.Enums;

namespace ERPAPP.Repository
{
    public class FGItemRepository : IFGItemRepository
    {
        private readonly DbHelper _db;
        public FGItemRepository(DbHelper db)
        {
            _db = db;
        }

        public async Task<bool> CheckMultipleItemsExists(string itemNos)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
            new SqlParameter("@No_", itemNos ?? "")
                };

                bool exists = false;

                DataTable dt = _db.GetDataTable("CheckMultipleItemsExists", param);

                if (dt != null && dt.Rows.Count > 0)
                {
                    exists = Convert.ToInt32(dt.Rows[0]["IsExists"]) == 1;
                }

                return exists;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<BrandModel>> GetFGBrandDropDownData()
        {
            DataTable dt = _db.GetDataTable("GetFGBrandDropDownData");

            List<BrandModel> list = dt.AsEnumerable()
                .Select(x => new BrandModel
                {
                    Code = x["Code"].ToString(),
                    Name = x["Name"].ToString()
                }).ToList();

            return list;
        }

        public async Task<GetFGItemAddModel> GetFGItemAddData()
        {
            var model = new GetFGItemAddModel();
            var dropDown = new FGItemDropDownAddModel();

            DataSet ds = _db.GetDataSet("Item_GetAddDropDownData");

            // 0 Unit of Measure
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                dropDown.UnitOfMeasures.Add(new UnitOfMeasureModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 1 Routing No
            foreach (DataRow row in ds.Tables[1].Rows)
            {
                dropDown.RoutingNos.Add(new RoutingModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 2 Category
            foreach (DataRow row in ds.Tables[2].Rows)
            {
                dropDown.Categories.Add(new CategoryModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 3 Size Of Tile
            foreach (DataRow row in ds.Tables[3].Rows)
            {
                dropDown.SizeOfTiles.Add(new SizeOfTileModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 4 Brand
            foreach (DataRow row in ds.Tables[4].Rows)
            {
                dropDown.Brands.Add(new BrandModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 5 Collection
            foreach (DataRow row in ds.Tables[5].Rows)
            {
                dropDown.Collections.Add(new CollectionModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 6 Surface Finish / Glaze
            foreach (DataRow row in ds.Tables[6].Rows)
            {
                dropDown.SurfaceFinishGlazes.Add(new SurfaceFinishGlazeModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 7 Glaze Effect
            foreach (DataRow row in ds.Tables[7].Rows)
            {
                dropDown.GlazeEffects.Add(new GlazeEffectModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 8 Design Color
            foreach (DataRow row in ds.Tables[8].Rows)
            {
                dropDown.DesignColors.Add(new DesignColorModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 9 Colour Family
            foreach (DataRow row in ds.Tables[9].Rows)
            {
                dropDown.ColourFamilies.Add(new ColourFamilyModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 10 Type Of Tile
            foreach (DataRow row in ds.Tables[10].Rows)
            {
                dropDown.TypeOfTiles.Add(new TypeOfTileModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 11 Packaging
            foreach (DataRow row in ds.Tables[11].Rows)
            {
                dropDown.Packagings.Add(new PackagingModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 12 Grade
            foreach (DataRow row in ds.Tables[12].Rows)
            {
                dropDown.Grades.Add(new GradeModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 13 Thickness
            foreach (DataRow row in ds.Tables[13].Rows)
            {
                dropDown.Thicknesses.Add(new ThicknessModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 14 Body
            foreach (DataRow row in ds.Tables[14].Rows)
            {
                dropDown.Bodies.Add(new BodyModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 15 PL Collection
            foreach (DataRow row in ds.Tables[15].Rows)
            {
                dropDown.PLCollections.Add(new PLCollectionModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 16 PL Colours
            foreach (DataRow row in ds.Tables[16].Rows)
            {
                dropDown.PLColours.Add(new PLColoursModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 17 General Posting Group
            foreach (DataRow row in ds.Tables[17].Rows)
            {
                dropDown.GeneralProductPostingGroups.Add(new GeneralProductPostingGroupModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 18 Inventory Posting Group
            foreach (DataRow row in ds.Tables[18].Rows)
            {
                dropDown.InventoryPostingGroups.Add(new InventoryPostingGroupModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 19 GST Groups
            foreach (DataRow row in ds.Tables[19].Rows)
            {
                dropDown.GSTGroups.Add(new GSTGroupsModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 20 Production BOM Header
            foreach (DataRow row in ds.Tables[20].Rows)
            {
                dropDown.ProductionBOMHeaders.Add(new ProductionBOMHeaderModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 21 Item Grade List Point Wise
            foreach (DataRow row in ds.Tables[21].Rows)
            {
                dropDown.ItemGrades.Add(new ItemGradeModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString(),
                    GradeLinkCode = row["GradeLinkCode"].ToString()
                });
            }

            // 22 Item Grade List Point Wise
            foreach (DataRow row in ds.Tables[22].Rows)
            {
                dropDown.ItemCategories.Add(new ItemCategoryModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString(),
                    GenProdPostingGroup = row["GenProdPostingGroup"].ToString(),
                    CostingMethod = row["CostingMethod"].ToString(),
                    InventoryPostingGroup = row["InventoryPostingGroup"].ToString()
                });
            }

            // ---------------- ENUMS ----------------

            // Movement Type
            dropDown.MovementTypes = Enum.GetValues(typeof(ItemMovementType))
                .Cast<ItemMovementType>()
                .Select(e => new ItemMovementTypeEnumModel
                {
                    Code = ((int)e).ToString(),
                    Name = e.ToString()
                }).ToList();

            // Type Of Product
            dropDown.TypeOfProducts = Enum.GetValues(typeof(ItemTypeOfProduct))
                .Cast<ItemTypeOfProduct>()
                .Select(e => new ItemTypeOfProductEnumModel
                {
                    Code = ((int)e).ToString(),
                    Name = e.ToString()
                }).ToList();

            // Manufacturing Policy (Display Name)
            dropDown.ManufacturingPolicies = Enum.GetValues(typeof(ItemManufacturingPolicy))
                .Cast<ItemManufacturingPolicy>()
                .Select(e => new ItemManufacturingPolicyEnumModel
                {
                    Code = ((int)e).ToString(),
                    Name = e.GetDisplayName()
                }).ToList();

            // Costing Method (Display Name)
            dropDown.CostingMethods = Enum.GetValues(typeof(ItemCostingMethod))
                .Cast<ItemCostingMethod>()
                .Select(e => new ItemCostingMethodEnumModel
                {
                    Code = ((int)e).ToString(),
                    Name = e.GetDisplayName()
                }).ToList();

            // GST Credit (Display Name)
            dropDown.GSTCredits = Enum.GetValues(typeof(ItemGSTCredit))
                .Cast<ItemGSTCredit>()
                .Select(e => new ItemGSTCreditEnumModel
                {
                    Code = ((int)e).ToString(),
                    Name = e.GetDisplayName()
                }).ToList();

            // Replenishment System (Display Name)
            dropDown.ReplenishmentSystems = Enum.GetValues(typeof(ItemReplenishmentSystem))
                .Cast<ItemReplenishmentSystem>()
                .Select(e => new ItemReplenishmentSystemEnumModel
                {
                    Code = ((int)e).ToString(),
                    Name = e.GetDisplayName()
                }).ToList();

            // Reordeing Policy (Display Name)
            dropDown.ReorderingPolicy = Enum.GetValues(typeof(ItemReorderingPolicy))
                .Cast<ItemReorderingPolicy>()
                .Select(e => new ItemReorderingPolicyEnumModel
                {
                    Code = ((int)e).ToString(),
                    Name = e.GetDisplayName()
                }).ToList();

            // ---------------- FINAL ----------------
            model.DropDownData = dropDown;

            var displayNo = await GetItemsTransferNewNo();
            model.DisplayNo = displayNo > 0 ? displayNo : 0;

            return model;
        }

        public async Task<string> GetFGItemCompanyLastNoUsedCompanyCode()
        {
            string newNo = "";

            DataTable dt = _db.GetDataTable("GetFGItemCompanyLastNoUsedCompanyCode");

            if (dt != null && dt.Rows.Count > 0)
            {
                newNo = Convert.ToString(dt.Rows[0]["NewFGItemCode"]);
            }

            return newNo ?? "";
        }

        public async Task<GetFGItemEditModel> GetFGItemEditData(string description, string description2, string category, string sizeOfTile, string thickness, string packaging)
        {
            SqlParameter[] param =
                    {
                new SqlParameter("@Description", description),
                new SqlParameter("@Description2", description2),
                new SqlParameter("@Category", category),
                new SqlParameter("@SizeOfTile", sizeOfTile),
                new SqlParameter("@Thickness", thickness),
                new SqlParameter("@Packaging", packaging)
            };

            DataSet ds = _db.GetDataSet("GetFGItem_GetEditData", param);

            GetFGItemEditModel model = new GetFGItemEditModel();

            // ================= MAIN DATA =================
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];

                // ================= BASIC =================
                model.DisplayNo = row["DisplayNo"] != DBNull.Value ? Convert.ToInt32(row["DisplayNo"]) : 0;
                model.Description = row["Description"]?.ToString() ?? "";
                model.Description2 = row["Description2"]?.ToString() ?? "";

                model.BaseUnitOfMeasure = row["BaseUnitOfMeasure"]?.ToString();
                model.ItemCategoryCode = row["ItemCategoryCode"]?.ToString();

                model.GrossWeight = row["GrossWeight"] != DBNull.Value ? Convert.ToDecimal(row["GrossWeight"]) : null;

                model.NetWeight = row["NetWeight"] != DBNull.Value ? Convert.ToDecimal(row["NetWeight"]) : null;

                model.RoundingPrecision = row["RoundingPrecision"] != DBNull.Value ? Convert.ToDecimal(row["RoundingPrecision"]) : null;

                model.PurchUnitOfMeasure = row["PurchUnitOfMeasure"]?.ToString();

                model.ReplenishmentSystem = row["ReplenishmentSystem"]?.ToString();

                model.ItemTrackingCode = row["ItemTrackingCode"]?.ToString();

                model.CostingMethod = row["CostingMethod"]?.ToString();

                model.GenProdPostingGroup = row["GenProdPostingGroup"]?.ToString();

                model.VATProdPostingGroup = row["VATProdPostingGroup"]?.ToString();

                model.InventoryPostingGroup = row["InventoryPostingGroup"]?.ToString();

                model.ProductionBOMNo = row["ProductionBOMNo"]?.ToString();

                model.RoutingNo = row["RoutingNo"]?.ToString();

                model.ManufacturingPolicy = row["ManufacturingPolicy"]?.ToString();

                model.ReorderingPolicy = row["ReorderingPolicy"]?.ToString();


                // ================= TILE ATTRIBUTES =================

                model.Category = row["Category"]?.ToString() ?? "";

                model.SizeOfTile = row["SizeOfTile"]?.ToString() ?? "";

                model.Collection = row["Collection"]?.ToString();

                model.SurfaceFinishOrGlaze = row["SurfaceFinishOrGlaze"]?.ToString();

                model.GlazeEffect = row["GlazeEffect"]?.ToString();

                model.DesignColor = row["DesignColor"]?.ToString();

                model.ColourFamily = row["ColourFamily"]?.ToString();

                model.TypeOfTile = row["TypeOfTile"]?.ToString();

                model.Packaging = row["Packaging"]?.ToString() ?? "";

                model.Thickness = row["Thickness"]?.ToString() ?? "";

                model.Body = row["Body"]?.ToString();

                model.PLCollection = row["PLCollection"]?.ToString();

                model.PLColours = row["PLColours"]?.ToString();

                model.MovementType = row["MovementType"]?.ToString();

                model.TypeOfProduct = row["TypeOfProduct"]?.ToString();


                // ================= GST =================

                model.GSTGroupCode = row["GSTGroupCode"]?.ToString();

                model.GSTCredit = row["GSTCredit"]?.ToString();

                model.HSNSACCode = row["HSNSACCode"]?.ToString();
            }

            // ================= GRADE LIST =================
            if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[1].Rows)
                {
                    model.GradeListDetails.Add(new FGItemGradeListDetails
                    {
                        GradeItemCode = row["GradeItemCode"]?.ToString(),
                        Brand = row["Brand"]?.ToString(),
                        Grade = row["Grade"]?.ToString(),
                        GradeLinkCode = row["GradeLinkCode"]?.ToString(),
                        SalesUnitOfMeasure = row["SalesUnitOfMeasure"]?.ToString()
                    });
                }
            }

            // ================= DROPDOWN DATA =================

            model.DropDownData = await GetFGItemEditDropDownData();

            return model;
        }



        public async Task<FGItemDropDownEditModel> GetFGItemEditDropDownData()
        {
            var dropDown = new FGItemDropDownEditModel();

            DataSet ds = _db.GetDataSet("Item_GetAddDropDownData");

            // 0 Unit of Measure
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                dropDown.UnitOfMeasures.Add(new UnitOfMeasureModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 1 Routing No
            foreach (DataRow row in ds.Tables[1].Rows)
            {
                dropDown.RoutingNos.Add(new RoutingModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 2 Category
            foreach (DataRow row in ds.Tables[2].Rows)
            {
                dropDown.Categories.Add(new CategoryModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 3 Size Of Tile
            foreach (DataRow row in ds.Tables[3].Rows)
            {
                dropDown.SizeOfTiles.Add(new SizeOfTileModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 4 Brand
            foreach (DataRow row in ds.Tables[4].Rows)
            {
                dropDown.Brands.Add(new BrandModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 5 Collection
            foreach (DataRow row in ds.Tables[5].Rows)
            {
                dropDown.Collections.Add(new CollectionModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 6 Surface Finish / Glaze
            foreach (DataRow row in ds.Tables[6].Rows)
            {
                dropDown.SurfaceFinishGlazes.Add(new SurfaceFinishGlazeModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 7 Glaze Effect
            foreach (DataRow row in ds.Tables[7].Rows)
            {
                dropDown.GlazeEffects.Add(new GlazeEffectModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 8 Design Color
            foreach (DataRow row in ds.Tables[8].Rows)
            {
                dropDown.DesignColors.Add(new DesignColorModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 9 Colour Family
            foreach (DataRow row in ds.Tables[9].Rows)
            {
                dropDown.ColourFamilies.Add(new ColourFamilyModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 10 Type Of Tile
            foreach (DataRow row in ds.Tables[10].Rows)
            {
                dropDown.TypeOfTiles.Add(new TypeOfTileModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 11 Packaging
            foreach (DataRow row in ds.Tables[11].Rows)
            {
                dropDown.Packagings.Add(new PackagingModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 12 Grade
            foreach (DataRow row in ds.Tables[12].Rows)
            {
                dropDown.Grades.Add(new GradeModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 13 Thickness
            foreach (DataRow row in ds.Tables[13].Rows)
            {
                dropDown.Thicknesses.Add(new ThicknessModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 14 Body
            foreach (DataRow row in ds.Tables[14].Rows)
            {
                dropDown.Bodies.Add(new BodyModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 15 PL Collection
            foreach (DataRow row in ds.Tables[15].Rows)
            {
                dropDown.PLCollections.Add(new PLCollectionModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 16 PL Colours
            foreach (DataRow row in ds.Tables[16].Rows)
            {
                dropDown.PLColours.Add(new PLColoursModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 17 General Posting Group
            foreach (DataRow row in ds.Tables[17].Rows)
            {
                dropDown.GeneralProductPostingGroups.Add(new GeneralProductPostingGroupModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 18 Inventory Posting Group
            foreach (DataRow row in ds.Tables[18].Rows)
            {
                dropDown.InventoryPostingGroups.Add(new InventoryPostingGroupModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 19 GST Groups
            foreach (DataRow row in ds.Tables[19].Rows)
            {
                dropDown.GSTGroups.Add(new GSTGroupsModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 20 Production BOM Header
            foreach (DataRow row in ds.Tables[20].Rows)
            {
                dropDown.ProductionBOMHeaders.Add(new ProductionBOMHeaderModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 21 Item Grade List Point Wise
            foreach (DataRow row in ds.Tables[21].Rows)
            {
                dropDown.ItemGrades.Add(new ItemGradeModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString(),
                    GradeLinkCode = row["GradeLinkCode"].ToString()
                });
            }

            // 22 Item Grade List Point Wise
            foreach (DataRow row in ds.Tables[22].Rows)
            {
                dropDown.ItemCategories.Add(new ItemCategoryModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString(),
                    GenProdPostingGroup = row["GenProdPostingGroup"].ToString(),
                    CostingMethod = row["CostingMethod"].ToString(),
                    InventoryPostingGroup = row["InventoryPostingGroup"].ToString()
                });
            }

            // ---------------- ENUMS ----------------

            // Movement Type
            dropDown.MovementTypes = Enum.GetValues(typeof(ItemMovementType))
                .Cast<ItemMovementType>()
                .Select(e => new ItemMovementTypeEnumModel
                {
                    Code = ((int)e).ToString(),
                    Name = e.ToString()
                }).ToList();

            // Type Of Product
            dropDown.TypeOfProducts = Enum.GetValues(typeof(ItemTypeOfProduct))
                .Cast<ItemTypeOfProduct>()
                .Select(e => new ItemTypeOfProductEnumModel
                {
                    Code = ((int)e).ToString(),
                    Name = e.ToString()
                }).ToList();

            // Manufacturing Policy (Display Name)
            dropDown.ManufacturingPolicies = Enum.GetValues(typeof(ItemManufacturingPolicy))
                .Cast<ItemManufacturingPolicy>()
                .Select(e => new ItemManufacturingPolicyEnumModel
                {
                    Code = ((int)e).ToString(),
                    Name = e.GetDisplayName()
                }).ToList();

            // Costing Method (Display Name)
            dropDown.CostingMethods = Enum.GetValues(typeof(ItemCostingMethod))
                .Cast<ItemCostingMethod>()
                .Select(e => new ItemCostingMethodEnumModel
                {
                    Code = ((int)e).ToString(),
                    Name = e.GetDisplayName()
                }).ToList();

            // GST Credit (Display Name)
            dropDown.GSTCredits = Enum.GetValues(typeof(ItemGSTCredit))
                .Cast<ItemGSTCredit>()
                .Select(e => new ItemGSTCreditEnumModel
                {
                    Code = ((int)e).ToString(),
                    Name = e.GetDisplayName()
                }).ToList();

            // Replenishment System (Display Name)
            dropDown.ReplenishmentSystems = Enum.GetValues(typeof(ItemReplenishmentSystem))
                .Cast<ItemReplenishmentSystem>()
                .Select(e => new ItemReplenishmentSystemEnumModel
                {
                    Code = ((int)e).ToString(),
                    Name = e.GetDisplayName()
                }).ToList();

            // Reordeing Policy (Display Name)
            dropDown.ReorderingPolicy = Enum.GetValues(typeof(ItemReorderingPolicy))
                .Cast<ItemReorderingPolicy>()
                .Select(e => new ItemReorderingPolicyEnumModel
                {
                    Code = ((int)e).ToString(),
                    Name = e.GetDisplayName()
                }).ToList();

            return dropDown;
        }

        public async Task<List<FGItemHSNModel>> GetFGItemHSNDataWithGSTGroupCode(string gstGroupCode, string category)
        {
            List<FGItemHSNModel> list = new List<FGItemHSNModel>();

            SqlParameter[] param = new SqlParameter[]
            {
                 new SqlParameter("@GSTGroupCode", gstGroupCode),
                 new SqlParameter("@Category", category ?? "")
            };

            DataSet ds = _db.GetDataSet("GetFGItemHSNDataWithGSTGroupCode", param);

            if (ds != null && ds.Tables.Count > 0)
            {
                list = ds.Tables[0].AsEnumerable().Select(row => new FGItemHSNModel
                {
                    Code = row["Code"]?.ToString(),
                    Name = row["Name"]?.ToString()
                }).ToList();

                // Selected HSN Code
                string selectedHSNCode = "";

                if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                {
                    selectedHSNCode = ds.Tables[1].Rows[0]["SelectedHSNCode"]?.ToString();
                }

                // Set Selected
                foreach (var item in list)
                {
                    item.IsSelected = item.Code == selectedHSNCode;
                }
            }

            return list;
        }

        public async Task<List<UnitOfMeasureModel>> GetFGUnitOfMeasureDropDownData()
        {
            DataTable dt = _db.GetDataTable("GetFGUnitOfMeasureDropDownData");

            List<UnitOfMeasureModel> list = dt.AsEnumerable()
                .Select(x => new UnitOfMeasureModel
                {
                    Code = x["Code"].ToString(),
                    Name = x["Name"].ToString()
                }).ToList();

            return list;
        }

        public async Task<int> GetItemsTransferNewNo()
        {
            int newNo = 0;

            DataTable dt = _db.GetDataTable("ItemsGetNewNo");

            if (dt != null && dt.Rows.Count > 0)
            {
                newNo = Convert.ToInt32(dt.Rows[0]["NewDisplayNo"]);
            }

            return newNo;
        }

        public async Task<List<ProductionBOMHeaderModel>> GetItem_ChangeUnitOfMeasure(string baseUnitOfMeasure)
        {
            SqlParameter[] para =
                {
                    new SqlParameter("@BaseUnitOfMeasure", baseUnitOfMeasure)
                };

            DataTable dt = _db.GetDataTable("GetItem_UnitOfMeasureChange", para);

            List<ProductionBOMHeaderModel> list = dt.AsEnumerable()
                .Select(x => new ProductionBOMHeaderModel
                {
                    Code = x["Code"].ToString(),
                    Name = x["Name"].ToString()
                }).ToList();

            return list;
        }

        public async Task<string> GetWIPItemCompanyLastNoUsedCompanyCode()
        {
            string newNo = "";

            DataTable dt = _db.GetDataTable("GetWIPItemCompanyLastNoUsedCompanyCode");

            if (dt != null && dt.Rows.Count > 0)
            {
                newNo = Convert.ToString(dt.Rows[0]["NewWIPItemCode"]);
            }

            return newNo ?? "";
        }

        public async Task<bool> InsertFGItem(FGItemModel model, string createdBy)
        {
            try
            {
                bool result = false;

                // ============================================
                // CREATE DATATABLE FOR GRADE LIST
                // ============================================

                DataTable dtGrade = new DataTable();

                dtGrade.Columns.Add("GradeItemCode");
                dtGrade.Columns.Add("Brand");
                dtGrade.Columns.Add("Grade");
                dtGrade.Columns.Add("GradeLinkCode");
                dtGrade.Columns.Add("SalesUnitOfMeasure");

                if (model.GradeListDetails != null &&
                    model.GradeListDetails.Count > 0)
                {
                    foreach (var item in model.GradeListDetails)
                    {
                        dtGrade.Rows.Add(
                            item.GradeItemCode ?? "",
                            item.Brand ?? "",
                            item.Grade ?? "",
                            item.GradeLinkCode ?? "",
                            item.SalesUnitOfMeasure ?? ""
                        );
                    }
                }

                // ============================================
                // SQL PARAMETERS
                // ============================================

                SqlParameter[] param =
                {
                    // ================= GENERAL DETAILS =================

                    new SqlParameter("@DisplayNo", model.DisplayNo),

                    new SqlParameter("@CompanyCode", model.CompanyCode ?? ""),
                    new SqlParameter("@Description", model.Description ?? ""),
                    new SqlParameter("@Description2", model.Description2 ?? ""),

                    new SqlParameter("@RoundingPrecision", model.RoundingPrecision ?? 0),
                    new SqlParameter("@GrossWeight", model.GrossWeight ?? 0),
                    new SqlParameter("@NetWeight", model.NetWeight ?? 0),

                    new SqlParameter("@BaseUnitOfMeasure", model.BaseUnitOfMeasure ?? ""),
                    new SqlParameter("@ItemCategoryCode", model.ItemCategoryCode ?? ""),
                    new SqlParameter("@ProductGroupCode", model.ProductGroupCode ?? ""),
                    new SqlParameter("@MovementType", model.MovementType ?? ""),
                    new SqlParameter("@TypeOfProduct", model.TypeOfProduct ?? ""),

                    // ================= ITEM SPECIFICATION =================

                    new SqlParameter("@Category", model.Category ?? ""),
                    new SqlParameter("@SizeOfTile", model.SizeOfTile ?? ""),
                 
                    new SqlParameter("@Collection", model.Collection ?? ""),
                    new SqlParameter("@SurfaceFinishOrGlaze", model.SurfaceFinishOrGlaze ?? ""),
                    new SqlParameter("@GlazeEffect", model.GlazeEffect ?? ""),
                    new SqlParameter("@DesignColor", model.DesignColor ?? ""),
                    new SqlParameter("@ColourFamily", model.ColourFamily ?? ""),
                    new SqlParameter("@TypeOfTile", model.TypeOfTile ?? ""),
                    new SqlParameter("@Packaging", model.Packaging ?? ""),

                   

                    new SqlParameter("@Thickness", model.Thickness ?? ""),
                    new SqlParameter("@Body", model.Body ?? ""),
                    new SqlParameter("@PLCollection", model.PLCollection ?? ""),
                    new SqlParameter("@PLColours", model.PLColours ?? ""),

                    // ================= MANUFACTURING =================

                    new SqlParameter("@ManufacturingPolicy", model.ManufacturingPolicy ?? ""),
                    new SqlParameter("@RoutingNo", model.RoutingNo ?? ""),
                    new SqlParameter("@ProductionBOMNo", model.ProductionBOMNo ?? ""),

                    // ================= COST & POSTING =================

                    new SqlParameter("@CostingMethod", model.CostingMethod ?? ""),
                    new SqlParameter("@GenProdPostingGroup", model.GenProdPostingGroup ?? ""),
                    new SqlParameter("@VATProdPostingGroup", model.VATProdPostingGroup ?? ""),
                    new SqlParameter("@InventoryPostingGroup", model.InventoryPostingGroup ?? ""),
                    new SqlParameter("@GSTGroupCode", model.GSTGroupCode ?? ""),
                    new SqlParameter("@GSTCredit", model.GSTCredit ?? ""),
                    new SqlParameter("@HSNSACCode", model.HSNSACCode ?? ""),

                    // ================= UNITS =================

                    new SqlParameter("@ReplenishmentSystem", model.ReplenishmentSystem ?? ""),
                    new SqlParameter("@PurchUnitOfMeasure", model.PurchUnitOfMeasure ?? ""),

                    // ================= TRACKING =================

                    new SqlParameter("@ItemTrackingCode", model.ItemTrackingCode ?? ""),
                    new SqlParameter("@ReorderingPolicy", model.ReorderingPolicy ?? ""),

                    new SqlParameter("@CreatedBy", createdBy ?? ""),

                    // ================= GRADE LIST TABLE TYPE =================

                    new SqlParameter
                    {
                        ParameterName = "@FGItemGrades",
                        SqlDbType = SqlDbType.Structured,
                        TypeName = "dbo.FGItemGradeType",
                        Value = dtGrade
                    }
                };

                // ============================================
                // STORE PROCEDURE CALL
                // ============================================

                var itemNoObj = _db.ExecuteScalar("FGItems_InsertDataWithCompany", param);

                string itemNo = itemNoObj?.ToString();

                if (!string.IsNullOrWhiteSpace(itemNo) && itemNo == "1")
                {
                    result = true;
                }

                return result;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateFGItem(FGItemeEditModel model, string updatedBy)
        {
            try
            {
                bool result = false;

                // ============================================
                // CREATE DATATABLE FOR GRADE LIST
                // ============================================

                DataTable dtGrade = new DataTable();

                dtGrade.Columns.Add("GradeItemCode");
                dtGrade.Columns.Add("Brand");
                dtGrade.Columns.Add("Grade");
                dtGrade.Columns.Add("GradeLinkCode");
                dtGrade.Columns.Add("SalesUnitOfMeasure");

                if (model.GradeListDetails != null &&
                    model.GradeListDetails.Count > 0)
                {
                    foreach (var item in model.GradeListDetails)
                    {
                        dtGrade.Rows.Add(
                            item.GradeItemCode ?? "",
                            item.Brand ?? "",
                            item.Grade ?? "",
                            item.GradeLinkCode ?? "",
                            item.SalesUnitOfMeasure ?? ""
                        );
                    }
                }

                // ============================================
                // SQL PARAMETERS
                // ============================================

                SqlParameter[] param =
                {
                    // ================= GENERAL DETAILS =================

                    new SqlParameter("@DisplayNo", model.DisplayNo),

                    new SqlParameter("@CompanyCode", model.CompanyCode ?? ""),
                    new SqlParameter("@Description", model.Description ?? ""),
                    new SqlParameter("@Description2", model.Description2 ?? ""),

                    new SqlParameter("@RoundingPrecision", model.RoundingPrecision ?? 0),
                    new SqlParameter("@GrossWeight", model.GrossWeight ?? 0),
                    new SqlParameter("@NetWeight", model.NetWeight ?? 0),

                    new SqlParameter("@BaseUnitOfMeasure", model.BaseUnitOfMeasure ?? ""),
                    new SqlParameter("@ItemCategoryCode", model.ItemCategoryCode ?? ""),
                    new SqlParameter("@ProductGroupCode", model.ProductGroupCode ?? ""),
                    new SqlParameter("@MovementType", model.MovementType ?? ""),
                    new SqlParameter("@TypeOfProduct", model.TypeOfProduct ?? ""),

                    // ================= ITEM SPECIFICATION =================

                    new SqlParameter("@Category", model.Category ?? ""),
                    new SqlParameter("@SizeOfTile", model.SizeOfTile ?? ""),
                    
                    new SqlParameter("@Collection", model.Collection ?? ""),
                    new SqlParameter("@SurfaceFinishOrGlaze", model.SurfaceFinishOrGlaze ?? ""),
                    new SqlParameter("@GlazeEffect", model.GlazeEffect ?? ""),
                    new SqlParameter("@DesignColor", model.DesignColor ?? ""),
                    new SqlParameter("@ColourFamily", model.ColourFamily ?? ""),
                    new SqlParameter("@TypeOfTile", model.TypeOfTile ?? ""),
                    new SqlParameter("@Packaging", model.Packaging ?? ""),

                    new SqlParameter("@Thickness", model.Thickness ?? ""),
                    new SqlParameter("@Body", model.Body ?? ""),
                    new SqlParameter("@PLCollection", model.PLCollection ?? ""),
                    new SqlParameter("@PLColours", model.PLColours ?? ""),

                    // ================= MANUFACTURING =================

                    new SqlParameter("@ManufacturingPolicy", model.ManufacturingPolicy ?? ""),
                    new SqlParameter("@RoutingNo", model.RoutingNo ?? ""),
                    new SqlParameter("@ProductionBOMNo", model.ProductionBOMNo ?? ""),

                    // ================= COST & POSTING =================

                    new SqlParameter("@CostingMethod", model.CostingMethod ?? ""),
                    new SqlParameter("@GenProdPostingGroup", model.GenProdPostingGroup ?? ""),
                    new SqlParameter("@VATProdPostingGroup", model.VATProdPostingGroup ?? ""),
                    new SqlParameter("@InventoryPostingGroup", model.InventoryPostingGroup ?? ""),
                    new SqlParameter("@GSTGroupCode", model.GSTGroupCode ?? ""),
                    new SqlParameter("@GSTCredit", model.GSTCredit ?? ""),
                    new SqlParameter("@HSNSACCode", model.HSNSACCode ?? ""),

                    // ================= UNITS =================

                    new SqlParameter("@ReplenishmentSystem", model.ReplenishmentSystem ?? ""),
                    new SqlParameter("@PurchUnitOfMeasure", model.PurchUnitOfMeasure ?? ""),

                    // ================= TRACKING =================

                    new SqlParameter("@ItemTrackingCode", model.ItemTrackingCode ?? ""),
                    new SqlParameter("@ReorderingPolicy", model.ReorderingPolicy ?? ""),

                    new SqlParameter("@UpdatedBy", updatedBy ?? ""),

                    // ================= GRADE LIST TABLE TYPE =================

                    new SqlParameter
                    {
                        ParameterName = "@FGItemGrades",
                        SqlDbType = SqlDbType.Structured,
                        TypeName = "dbo.FGItemGradeType",
                        Value = dtGrade
                    }
                };

                // ============================================
                // STORE PROCEDURE CALL
                // ============================================

                var itemNoObj = _db.ExecuteScalar("FGItems_UpdateDataWithCompany", param);

                string itemNo = itemNoObj?.ToString();

                if (!string.IsNullOrWhiteSpace(itemNo) && itemNo == "1")
                {
                    result = true;
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
