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
        private readonly IEmailRepository _emailRepository;

        public FixedAssetController(IFixedAssetRepository fixedAssetRepository, IEmailRepository emailRepository)
        {
            _fixedAssetRepository = fixedAssetRepository;
            _emailRepository = emailRepository;
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
                var userName = HttpContext.Session.GetString("UserName");

                var insertResult = await _fixedAssetRepository.InsertFixedAssetData(model, userName);

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

        public IActionResult FixedAssetList()
        {
            return PartialView("_FixedAssetList");
        }

        public async Task<IActionResult> GetFixedAssetList(string searchDescription)
        {
            var data = await _fixedAssetRepository.GetFixedAssetList(searchDescription);
            return Json(data);
        }


        [HttpGet]
        public async Task<IActionResult> GetFixedAssetEditData(string fixedAssetNo)
        {
            var data = await _fixedAssetRepository.GetFixedAssetEditData(fixedAssetNo);
            if (data == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Fixed Asset not found."
                });
            }

            var dropdownData = await _fixedAssetRepository.GetFixedAssetEditDropDownData();

            data.DropDownData = dropdownData.DropDownData;

            if (data.DivisionStr != null)
            {
                data.DropDownData.ComponentOfMainAssetList = await _fixedAssetRepository.GetFixedAssetComponentWithDivision(data.DivisionStr);
            }

            if (data.GSTGroupCode != null)
            {
                data.DropDownData.FAHSNList = await _fixedAssetRepository.GetFixedAssetHSNDataWithGSTGroupCode(data.GSTGroupCode);
            }

            return PartialView("_EditFixedAsset", data);
        }


        [HttpPost]
        public async Task<IActionResult> FixedAssetUnblock(string fixedAssetCode, int displayNo, string fixedAssetDescription)
        {
            try
            {
                var fixedAssetUnBlock = await _fixedAssetRepository.FixedAssetUnblock(fixedAssetCode);

                if (fixedAssetUnBlock)
                {
                    var sendEmail = await _emailRepository.SendMailFixedAssetBlock(displayNo, fixedAssetCode, fixedAssetDescription);
                    return Json(new { success = true, message = "Fixed Asset Un-blocked Successfully." });
                }
                else
                {
                    return Json(new { success = false, message = "something went wrong!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        public async Task<IActionResult> UpdateFixedAssetMaster(FixedAssetEditModel model)
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
                var updateResult = await _fixedAssetRepository.UpdateFixedAssetData(model);

                if (updateResult)
                {
                    return Json(new
                    {
                        success = true,
                        message = "Fixed Asset updated successfully."
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
    }
}
