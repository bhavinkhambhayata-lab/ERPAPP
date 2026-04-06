using ERPAPP.Interfaces;
using ERPAPP.Models;
using ERPAPP.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace ERPAPP.Controllers
{
    public class FixedAssetController : Controller
    {

        private readonly IFixedAssetRepository _fixedAssetRepository;

        public FixedAssetController(IFixedAssetRepository fixedAssetRepository)
        {
            _fixedAssetRepository = fixedAssetRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> FixedAssetDetails()
        {
            var model = await _fixedAssetRepository.GetFixedAssetAddData();
            return PartialView("_AddFixedAssetDetails", model);
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

                var data = await _fixedAssetRepository.GetFixedAssetHSNDataWithGSTGroupCode(gstGroupCode);

                return Json(data);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public async Task<IActionResult> SaveFixedAssetMaster(FixedAssetModel model)
        {
            model.LoginRowID = Convert.ToInt16(HttpContext.Session.GetInt32("UserRowId").ToString());

            if (model.FAClassCode == null || model.FAClassCode == "")
                ModelState.AddModelError("FAClassCode", "FA Class Code is required");

            if (model.FASubclassCode == null || model.FASubclassCode == "")
                ModelState.AddModelError("FASubClassCode", "FA Subclass Code is required");

            if (string.IsNullOrWhiteSpace(model.Description))
                ModelState.AddModelError("Description", "Description is required");

            if (string.IsNullOrWhiteSpace(model.GenProdPostingGroup))
                ModelState.AddModelError("GenProdPostingGroup", "Gen Prod Posting Group is required");

            if (string.IsNullOrWhiteSpace(model.FAPostingGroup))
                ModelState.AddModelError("FAPostingGroup", "FA Posting Group is required");

            if (!model.DepreciationMethod.HasValue)
            {
                ModelState.AddModelError("DepreciationMethod", "Depreciation Method is required");
            }


            // 🔢 Numeric + Range Validation
            if (model.DepreciationMethod == 0) // Straight Line
            {
                if (model.StraightLinePercent != null)
                {
                    if (model.StraightLinePercent < 0 || model.StraightLinePercent > 100)
                    {
                        ModelState.AddModelError("StraightLinePercent", "Straight-Line % must be between 0 to 100");
                    }
                }
            }

            if (model.DepreciationMethod == 1 || model.DepreciationMethod == 2)
            {
                if (model.DecliningBalancePercent != null)
                {
                    if (model.DecliningBalancePercent < 0 || model.DecliningBalancePercent > 100)
                    {
                        ModelState.AddModelError("DecliningBalancePercent", "Declining Balance % must be between 0 to 100");
                    }
                }
            }

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
                var insertResult = await _fixedAssetRepository.InsertFixedAssetData(model);

                if (insertResult)
                {
                    return Json(new
                    {
                        success = true,
                        message = "Fixed Asset saved successfully."
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

        [HttpGet]
        public async Task<JsonResult> GetFixedAssetComponetWithDivision(string division)
        {
            try
            {
                if (string.IsNullOrEmpty(division))
                {
                    return Json(new List<FAHSNModel>());
                }

                var data = await _fixedAssetRepository.GetFixedAssetComponentWithDivision(division);

                return Json(data);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
