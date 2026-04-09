using ERPAPP.Interfaces;
using ERPAPP.Models;
using ERPAPP.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

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

            // =========================
            // BASIC REQUIRED VALIDATION
            // =========================

            if (string.IsNullOrWhiteSpace(model.Name))
                ModelState.AddModelError("Name", "Name is required.");

            if (string.IsNullOrWhiteSpace(model.CityCode))
                ModelState.AddModelError("CityCode", "City is required.");

            if (string.IsNullOrWhiteSpace(model.PostCode))
                ModelState.AddModelError("PostCode", "Post Code is required.");

            if (string.IsNullOrWhiteSpace(model.CountryCode))
                ModelState.AddModelError("CountryCode", "Country Code is required.");

            if (!string.IsNullOrWhiteSpace(model.CountryCode) && model.CountryCode != "IN" && string.IsNullOrWhiteSpace(model.CurrencyCode))
            {
                ModelState.AddModelError("CurrencyCode", "Currency is required.");
            }

            // ================= EMAIL =================
            if (!model.EmailNotAvailable) // ✅ Only validate when NOT checked
            {
                if (string.IsNullOrWhiteSpace(model.Email))
                {
                    ModelState.AddModelError("Email", "Email is required");
                }
                else
                {
                    if (model.Email.ToLower().Contains("italiagroup.in"))
                        ModelState.AddModelError("Email", "italiagroup.in emails are not allowed");

                    var emails = model.Email.Split(',');

                    if (emails.Any(e =>
                        !Regex.IsMatch(e.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$")))
                    {
                        ModelState.AddModelError("Email", "Invalid email format");
                    }
                }
            }


            if (model.GSTVendorType == 1 || model.GSTVendorType == 2 || model.GSTVendorType == 6)  //Registered  // Composite // SEZ
            {
                if (string.IsNullOrWhiteSpace(model.GSTRegNo) && string.IsNullOrWhiteSpace(model.ARN))
                {
                    ModelState.AddModelError("GSTRegNo", "Either GST No or ARN is mandatory");
                }

                if (!string.IsNullOrWhiteSpace(model.GSTRegNo) && string.IsNullOrWhiteSpace(model.PANNo))
                {
                    ModelState.AddModelError("PANNo", "PAN Number is mandatory when GST Registration Number is provided.");
                }
                if (!model.GSTReturnFrequency.HasValue || model.GSTReturnFrequency == 0)
                {
                    ModelState.AddModelError("GSTReturnFrequency", "GST Return Frequency is required.");
                }
            }


            // =========================
            // PAN VALIDATION
            // =========================

            if (!string.IsNullOrWhiteSpace(model.PANNo))
            {
                if (model.PANNo.Length != 10)
                {
                    ModelState.AddModelError("PANNo", "Length of PAN No. Must be 10");
                }
                else if (!System.Text.RegularExpressions.Regex
                    .IsMatch(model.PANNo, @"^[A-Z]{5}[0-9]{4}[A-Z]$"))
                {
                    ModelState.AddModelError("PANNo", "Invalid PAN No.");
                }
            }

            // =========================
            // GSTIN VALIDATION
            // =========================

            if (!string.IsNullOrWhiteSpace(model.GSTRegNo))
            {
                model.GSTRegNo = model.GSTRegNo.Trim().ToUpper();
                model.PANNo = model.PANNo?.Trim().ToUpper();

                string pattern = @"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z][1-9A-Z]Z[0-9A-Z]$";

                if (model.GSTRegNo.Length != 15 ||
                    !Regex.IsMatch(model.GSTRegNo, pattern))
                {
                    ModelState.AddModelError("GSTRegNo", "Invalid GSTIN format.");
                }
                else
                {
                    string gstPanPart = model.GSTRegNo.Substring(2, 10);

                    if (!string.IsNullOrWhiteSpace(model.PANNo) &&
                        gstPanPart != model.PANNo)
                    {
                        ModelState.AddModelError("GSTRegNo", "GSTIN PAN does not match PAN No.");
                    }

                    if (!string.IsNullOrWhiteSpace(model.StateCode))
                    {
                        var stateValid = await _vendorRepository.CheckStateGSTMatch(model.StateCode, model.GSTRegNo);

                        if (!stateValid)
                        {
                            ModelState.AddModelError("GSTRegNo", "Invalid GST Regi. No as per State.");
                        }
                    }
                }
            }

            // ================= REQUIRED DROPDOWNS =================

            if (string.IsNullOrWhiteSpace(model.PaymentTerms))
                ModelState.AddModelError("PaymentTerms", "Payment Terms is required");

            if (string.IsNullOrWhiteSpace(model.PurchaserCode))
                ModelState.AddModelError("PurchaserCode", "Purchaser is required");

            if (string.IsNullOrWhiteSpace(model.VendorCategory))
                ModelState.AddModelError("VendorCategory", "Vendor Category is required");

            if (string.IsNullOrWhiteSpace(model.GenBusPostingGroup))
                ModelState.AddModelError("GenBusPostingGroup", "Gen Bus Posting Group is required");

            if (string.IsNullOrWhiteSpace(model.VendorPostingGroup))
                ModelState.AddModelError("VendorPostingGroup", "Vendor Posting Group is required");

            //if (model.ApplicationMethod == 0)
            //    ModelState.AddModelError("ApplicationMethod", "Application Method is required");

            //if (model.TaxLiable == 0)
            //    ModelState.AddModelError("TaxLiable", "Tax Liable required");

            // ================= MSME =================

            if (model.BusinessCategory.HasValue && model.BusinessCategory != 0)
            {
                if (string.IsNullOrWhiteSpace(model.MSMEUAMNo))
                    ModelState.AddModelError("MSMEUAMNo", "MSME UAM No required");

                if (!model.MSMEIntimationDate.HasValue)
                    ModelState.AddModelError("MSMEIntimationDate", "MSME Intimation Date is required");

                if (!model.MSMEEffectiveDate.HasValue)
                    ModelState.AddModelError("MSMEEffectiveDate", "MSME Effective Date is required");
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
                var insertResult = await _vendorRepository.InsertVendor(model);

                if (insertResult)
                {
                    return Json(new
                    {
                        success = true,
                        message = "Vendor saved successfully."
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

        public async Task<IActionResult> GetVendorList(string searchVendor)
        {
            var data = await _vendorRepository.GetVendorList(searchVendor);
            return Json(data);
        }


        #region Address  - Search City -> Get City Detail -> Search Postcode -> Get Postcode Detail


        [HttpGet]
        public JsonResult GetCityList(string city)
        {
            var result = _vendorRepository.GetCityList(city);
            return Json(result);
        }

        public JsonResult GetCityDetail(string city)
        {
            var result = _vendorRepository.GetCityDetail(city);
            return Json(result);
        }

        [HttpGet]
        public JsonResult GetPostCodeList(string city)
        {
            var result = _vendorRepository.GetPostCodeList(city);
            return Json(result);
        }

        public JsonResult GetPostCodeDetail(string postcode)
        {
            var result = _vendorRepository.GetPostCodeDetail(postcode);
            return Json(result);
        }

        #endregion


        public async Task<IActionResult> GetVendorEditData(string vendorCode)
        {
            var model = await _vendorRepository.GetVendorEditData(vendorCode);
            return PartialView("_EditVendor", model);
        }

        [HttpPost]
        public async Task<IActionResult> VendorUnblock(string vendorCode)
        {
            try
            {
                var vendorUnBlock = await _vendorRepository.VendorUnblock(vendorCode);

                if (vendorUnBlock)
                {
                    return Json(new { success = true, message = "Vendor Un-blocked Successfully." });
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


        [HttpGet]
        public async Task<IActionResult> CheckVendorGSTRegistrationAlreadyExists(string gstRegistrationNo)
        {
            try
            {
                var isExists = await _vendorRepository
                    .CheckVendorGSTRegistrationAlreadyExists(gstRegistrationNo);

                return Json(new { success = true, data = isExists });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        public async Task<IActionResult> SearchVendor(string searchVendor)
        {
            var results = await _vendorRepository.SearchVendor(searchVendor);
            return Json(results);
        }

        [HttpGet]
        public async Task<IActionResult> GetVendorMasterDataWithMasterCode(string masterCode)
        {
            //bool exists = await .CheckCustomerEntryAlreadyExists(masterCode, division);

            //if (exists)
            //{
            //    return Json(new
            //    {
            //        success = false,
            //        message = "Customer already exists in this division."
            //    });
            //}

            var data = await _vendorRepository.GetVendorMasterDataWithMasterCode(masterCode);

            if (data == null)
                return NotFound();

            return Json(new
            {
                success = true,
                data = data
            });
        }
    }
}
