using ERPAPP.Interfaces;
using ERPAPP.Models;
using ERPAPP.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ERPAPP.Controllers
{
    public class FGItemController : Controller
    {

        private readonly IFGItemRepository _fGItemRepository;

        public FGItemController(IFGItemRepository fGItemRepository)
        {
            _fGItemRepository = fGItemRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddFGItem()
        {
            var data = await _fGItemRepository.GetFGItemAddData();

            data.TypeOfProduct = "1";

            //        string itemCode = "FG1213";
            //        data.CompanyCode = "FG1213";


            //        string prefix = new string(itemCode.TakeWhile(c => !char.IsDigit(c)).ToArray());
            //        string numberPart = new string(itemCode.SkipWhile(c => !char.IsDigit(c)).ToArray());

            //        int number = int.Parse(numberPart);

            //        // First new grade item
            //        string firstGradeItemCode = prefix + (number + 1);

            //        data.GradeListDetails = new List<FGItemGradeListDetails>
            //{
            //    new FGItemGradeListDetails
            //    {
            //        Grade = "1ST CHOICE",
            //        GradeLinkCode = firstGradeItemCode + ".1",
            //        GradeItemCode = itemCode
            //    },
            //    new FGItemGradeListDetails
            //    {
            //        Grade = "SPRM",
            //        GradeLinkCode = firstGradeItemCode + ".1",
            //        GradeItemCode = firstGradeItemCode
            //    },
            //    new FGItemGradeListDetails
            //    {
            //        Grade = "STD",
            //        GradeLinkCode = firstGradeItemCode + ".2",
            //        GradeItemCode = prefix + (number + 2)
            //    },
            //    new FGItemGradeListDetails
            //    {
            //        Grade = "ECO",
            //        GradeLinkCode = firstGradeItemCode + ".3",
            //        GradeItemCode = prefix + (number + 3)
            //    },
            //    new FGItemGradeListDetails
            //    {
            //        Grade = "SMPL",
            //        GradeLinkCode = firstGradeItemCode + ".4",
            //        GradeItemCode = prefix + (number + 4)
            //    }
            //};

            return PartialView("_AddFGItem", data);
        }

        [HttpGet]
        public async Task<JsonResult> GetFGItemHSNDataWithGSTGroupCode(string gstGroupCode, string category)
        {
            try
            {
                if (string.IsNullOrEmpty(gstGroupCode))
                {
                    return Json(new List<FGItemHSNModel>());
                }

                var data = await _fGItemRepository.GetFGItemHSNDataWithGSTGroupCode(gstGroupCode, category);

                return Json(data);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        public async Task<JsonResult> GetItem_UnitOfMeasureChange(string baseUnitOfMeasure)
        {
            var data = await _fGItemRepository.GetItem_ChangeUnitOfMeasure(baseUnitOfMeasure);

            return Json(data);
        }


        public async Task<string> GetFGItemCompanyLastNoUsedCompanyCode()
        {
            var data = await _fGItemRepository.GetFGItemCompanyLastNoUsedCompanyCode();
            return data;
        }

        public async Task<IActionResult> SaveFGItemMaster(FGItemModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    success = false,
                    message = "Validation failed",
                    errors = ModelState
                                .Where(x => x.Value.Errors.Count > 0)
                                .ToDictionary(
                                    k => k.Key,
                                    v => v.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                                )
                });
            }

            try
            {
                var userName = HttpContext.Session.GetString("UserName");

                var insertResult = await _fGItemRepository.InsertFGItem(model, userName ?? "");

                if (insertResult)
                {
                    return Json(new
                    {
                        success = true,
                        message = "FG item saved successfully."
                    });
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        message = "something went wrong!."
                    });
                }
            }
            catch
            {
                return Json(new
                {
                    success = false,
                    message = "Something went wrong while saving."
                });
            }
        }


        public async Task<JsonResult> GetItem_UnitOfMeasureDropDownData()
        {
            var data = await _fGItemRepository.GetFGUnitOfMeasureDropDownData();

            return Json(data);
        }

        public async Task<JsonResult> GetItem_BrandDropDownData()
        {
            var data = await _fGItemRepository.GetFGBrandDropDownData();

            return Json(data);
        }

        public async Task<string> GetWIPItemCompanyLastNoUsedCompanyCode()
        {
            var data = await _fGItemRepository.GetWIPItemCompanyLastNoUsedCompanyCode();
            return data;
        }


        [HttpPost]
        public async Task<JsonResult> CheckItemInERP(string description, string description2, string category, string sizeOfTile, string thickness, string packaging)
        {
            var data = await _fGItemRepository.GetFGItemEditData(description, description2, category, sizeOfTile, thickness, packaging);

            bool isExist = !string.IsNullOrWhiteSpace(data.Description);

            return Json(new
            {
                success = isExist,
                data = data
            });
        }

        public async Task<IActionResult> EditFGItem(string description, string description2, string category, string sizeOfTile, string thickness, string packaging)
        {
            var data = await _fGItemRepository.GetFGItemEditData(description, description2, category, sizeOfTile, thickness, packaging);
            if (string.IsNullOrWhiteSpace(data.Description))
            {
                return Json(new
                {
                    success = false,
                    message = "Item not found."
                });
            }
            return PartialView("_EditFGItem", data);
        }
    }
}
