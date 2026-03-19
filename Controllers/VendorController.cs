using Microsoft.AspNetCore.Mvc;

namespace ERPAPP.Controllers
{
    public class VendorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult VendorList()
        {
            return PartialView("_VendorList");
        }

        public IActionResult VendorDetails()
        {
            return PartialView("_VendorDetails");
        }

    }
}
