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
        public async Task<JsonResult> GetFixedAssetHSNDataWithGSTGroupCode(string gstGroupCode)
        {
            try
            {
                if (string.IsNullOrEmpty(gstGroupCode))
                {
                    return Json(new List<FAHSNModel>());
                }

                var data = await _fGItemRepository.GetFixedAssetHSNDataWithGSTGroupCode(gstGroupCode);

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
    }
}
