using ERPAPP.Interfaces;
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

    }
}
