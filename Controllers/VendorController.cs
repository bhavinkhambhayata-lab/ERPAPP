using ERPAPP.Interfaces;
using ERPAPP.Models;
using ERPAPP.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ERPAPP.Controllers
{
    public class VendorController : Controller
    {

        private readonly IVendorRepository _vendorRepository;

        public VendorController(IVendorRepository vendorRepository)
        {
            _vendorRepository = vendorRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult VendorList()
        {
            return PartialView("_VendorList");
        }

        public async Task<IActionResult> VendorDetails()
        {
            var model = await _vendorRepository.GetVendorAddData();
            return PartialView("_VendorDetails", model);
        }

        public async Task<IActionResult> SaveVendorMaster(VendorsModel model)
        {
            model.LoginRowId = HttpContext.Session.GetInt32("UserRowId").ToString();

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
                //var insertResult = await _vendorRepository.InsertVendor(model);

                return Json(new
                {
                    success = true,
                    message = "Vendor saved successfully."
                });
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

        public async Task<IActionResult> GetVendorList(string searchVendor)
        {
            var data = await _vendorRepository.GetVendorList(searchVendor);
            return Json(data);
        }

    }
}
