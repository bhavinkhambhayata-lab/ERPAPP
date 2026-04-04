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
                var insertResult = true;

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
    }
}
