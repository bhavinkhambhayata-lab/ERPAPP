using ERPAPP.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace ERPAPP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            bool noPermission = false;

            var menuPermissionJson = HttpContext.Session.GetString("MenuPermission");

            if (!string.IsNullOrEmpty(menuPermissionJson))
            {
                var permission = JsonConvert.DeserializeObject<MenuPermissionModel>(menuPermissionJson);

                if (permission != null && !permission.CustomerDetails && !permission.VendorDetails && !permission.FADetails && !permission.ItemMaster)
                {
                    noPermission = true;
                }
            }
            else
            {
                noPermission = true;
            }

            ViewBag.NoPermission = noPermission;
            return View();
        }
    }
}
