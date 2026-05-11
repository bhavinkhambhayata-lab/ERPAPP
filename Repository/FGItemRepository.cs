using ERPAPP.Helper;
using ERPAPP.Interfaces;
using ERPAPP.Models;
using Microsoft.Data.SqlClient;
using System.Data;
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

            // ---------------- FINAL ----------------
            model.DropDownData = dropDown;

            var displayNo = await GetItemsTransferNewNo();
            model.DisplayNo = displayNo > 0 ? displayNo : 0;

            return model;
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

        public async Task<int> GetItemsTransferNewNo()
        {
            int newNo = 0;

            DataTable dt = _db.GetDataTable("Items_Transfer_Entry_GetNewNo");

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
    }
}
