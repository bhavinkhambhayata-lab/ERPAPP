using ERPAPP.Helper;
using ERPAPP.Interfaces;
using ERPAPP.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace ERPAPP.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginRepository _loginRepository;

        public LoginController(ILoginRepository loginRepository)
        {
            _loginRepository = loginRepository; 
        }

        #region Login ERP APP
        public IActionResult Index(string token)
        {
            if (HttpContext.Session.GetString("ERPSystemSessionExpired") == "1")
            {
                TempData["ToastMessage"] = "Session expired. Please login again.";
                TempData["ToastType"] = "warning";
                HttpContext.Session.Remove("ERPSystemSessionExpired");
            }

            // Token Login
            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    EncryptDecrypt obj = new EncryptDecrypt();
                    string decryptedText = obj.DecryptNEW(token);
                    string[] loginDetails = decryptedText.Split('|');

                    if (loginDetails.Length != 2)
                    {
                        TempData["ToastMessage"] = "Invalid token.";
                        TempData["ToastType"] = "danger";
                        return View();
                    }

                    LoginViewModel objTS = new LoginViewModel
                    {
                        UserName = loginDetails[0],
                        Password = loginDetails[1]
                    };

                    // Direct login
                    return AutoLoginUser(objTS);
                }
                catch
                {
                    TempData["ToastMessage"] = "Invalid or expired token.";
                    TempData["ToastType"] = "danger";
                    return View();
                }
            }

            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel objTS)
        {
            return AutoLoginUser(objTS);
        }

        [HttpPost]
        public IActionResult AutoLoginUser(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Index");

            var result = _loginRepository.Login(model);

            if (result.Status)
            {
                HttpContext.Session.SetInt32("UserRowId", result.UserRowId);
                HttpContext.Session.SetString("UserName", result.UserName);
                HttpContext.Session.SetString("IsCustomerMasterCompanyPermission", result.CustomerDetailsMasterEntryTransferPermission.ToString().ToLower());
                HttpContext.Session.SetString("IsVendorMasterCompanyPermission", result.VendorDetailsMasterEntryTransferPermission.ToString().ToLower());
                HttpContext.Session.SetString("IsFixedAssetMasterCompanyPermission", result.FADetailsMasterEntryTransferPermission.ToString().ToLower());
                HttpContext.Session.SetString("MenuPermission",JsonSerializer.Serialize(result.MenuList));

                TempData["ToastMessage"] = result.Message;
                TempData["ToastType"] = "success";

                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["ToastMessage"] = result.Message;
                TempData["ToastType"] = "danger";

                return RedirectToAction("Index", "Login");
            }
        }
        #endregion

        #region Logout ERP APP
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            TempData["ToastMessage"] = "You have been logged out successfully";
            TempData["ToastType"] = "info";

            return RedirectToAction("Index", "Login");
        }
        #endregion

    }
}
