using ERPAPP.Helper;
using ERPAPP.Interfaces;
using ERPAPP.Models;
using Microsoft.AspNetCore.Mvc;
using static ERPAPP.Helper.Enums;

namespace ERPAPP.Controllers
{
    public class CustomerController : Controller
    {

        private readonly ICustomerRepository _customerRepository;

        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _customerRepository.GetCustomerAddData();


            return View(data);
        }

        public async Task<IActionResult> SearchCustomer(string searchText)
        {
            var results = await _customerRepository.SearchCustomer(searchText);
            return Json(results);
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomerMaster(string customerNo, int brandId)
        {
            bool exists = await _customerRepository.CheckCustomerInMasterAndBrand(customerNo, brandId);

            if (exists)
            {
                return Json(new
                {
                    success = false,
                    message = "Customer already exists in this brand."
                });
            }

            var data = await _customerRepository.GetCustomerMaster(customerNo);

            if (data == null)
                return NotFound();

            return Json(new
            {
                success = true,
                data = data
            });
        }

        #region Address  - Search City -> Get City Detail -> Search Postcode -> Get Postcode Detail


        [HttpGet]
        public JsonResult GetCityList(string city)
        {
            var result = _customerRepository.GetCustomerCityList(city);
            return Json(result);
        }

        public JsonResult GetCityDetail(string city)
        {
            var result = _customerRepository.GetCityDetail(city);
            return Json(result);
        }

        [HttpGet]
        public JsonResult GetPostCodeList(string city)
        {
            var result = _customerRepository.GetPostCodeList(city);
            return Json(result);
        }

        public JsonResult GetPostCodeDetail(string postcode)
        {
            var result = _customerRepository.GetPostCodeDetail(postcode);
            return Json(result);
        }

        #endregion

        [HttpPost]
        public async Task<IActionResult> SaveCustomerMaster(CustomerModel model)
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

            if (string.IsNullOrWhiteSpace(model.Region))
                ModelState.AddModelError("Region", "Region is required.");

            if (string.IsNullOrWhiteSpace(model.Zone))
                ModelState.AddModelError("Zone", "Zone is required.");

            //if (string.IsNullOrWhiteSpace(model.PaymentTermsCode))
            //    ModelState.AddModelError("PaymentTermsCode", "Payment Term is required.");

            //if (string.IsNullOrWhiteSpace(model.SalespersonCode))
            //    ModelState.AddModelError("SalespersonCode", "Salesperson Name is required.");

            //if (string.IsNullOrWhiteSpace(model.GenBusPostingGroup))
            //    ModelState.AddModelError("GenBusPostingGroup", "Gen. Bus. Posting Group Should Not Be Blank");

            //if (string.IsNullOrWhiteSpace(model.LocationCode))
            //    ModelState.AddModelError("LocationCode", "Location is required.");

            //if (string.IsNullOrWhiteSpace(model.ExciseBusPostingGroup))
            //    ModelState.AddModelError("ExciseBusPostingGroup", "Excise Bus Posting Group is required.");

            //if (string.IsNullOrWhiteSpace(model.CustomerPostingGroup))
            //    ModelState.AddModelError("CustomerPostingGroup", "Customer Posting Group is required.");

            //if (model.ApplicationMethod == null)
            //    ModelState.AddModelError("ApplicationMethod", "Application Method is required.");

            //if (model.TaxLiable == null)
            //    ModelState.AddModelError("TaxLiable", "TaxLiable is required.");

            //if (model.GSTCustomerType == null)
            //    ModelState.AddModelError("GSTCustomerType", "GST Cust. Type Should Not Be Blank");

            // =========================
            // GST TYPE BASED VALIDATION
            // =========================

            if (model.GSTCustomerType == 1 || model.GSTCustomerType == 2 || model.GSTCustomerType == 3)  //Registered  // Composite // SEZ
            {
                if (string.IsNullOrWhiteSpace(model.GSTRegistrationNo) && string.IsNullOrWhiteSpace(model.ARNNo))
                {
                    ModelState.AddModelError("GSTRegistrationNo", "Either GST No or ARN is mandatory");
                }
            }

            // Registered specific rule
            //if (model.GSTCustomerType == 1)
            //{
            //    if (string.IsNullOrWhiteSpace(model.EInvEmail))
            //        ModelState.AddModelError("EInvEmail", "E-Invoice Email is required.");

            //    if (string.IsNullOrWhiteSpace(model.EInvPhoneNo))
            //        ModelState.AddModelError("EInvPhoneNo", "E-Invoice Phone Number is required.");
            //}

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
                    .IsMatch(model.PANNo, @"^[A-Z]{5}[0-9]{4}[A-Z]{1}$"))
                {
                    ModelState.AddModelError("PANNo", "Invalid PAN No.");
                }
            }

            // =========================
            // GSTIN VALIDATION
            // =========================

            if (!string.IsNullOrWhiteSpace(model.GSTRegistrationNo))
            {
                if (model.GSTRegistrationNo.Length != 15)
                {
                    ModelState.AddModelError("GSTRegistrationNo", "Length of GSTIN No. Must be 15");
                }
                else
                {
                    string gstStateCode = model.GSTRegistrationNo.Substring(0, 2);
                    string gstPanPart = model.GSTRegistrationNo.Substring(2, 10);

                    if (!string.IsNullOrWhiteSpace(model.PANNo) &&
                        gstPanPart != model.PANNo)
                    {
                        ModelState.AddModelError("GSTRegistrationNo", "GSTIN PAN does not match PAN No.");
                    }

                    var stateValid = await _customerRepository.CheckStateGSTMatch(model.StateCode, model.GSTRegistrationNo);

                    if (!stateValid)
                    {
                        ModelState.AddModelError("GSTRegistrationNo", "Invalid GST Regi. No as per State.");
                    }

                    if (!System.Text.RegularExpressions.Regex
                        .IsMatch(gstPanPart, @"^[A-Z]{5}[0-9]{4}[A-Z]{1}$"))
                    {
                        ModelState.AddModelError("GSTRegistrationNo", "Invalid GSTIN format.");
                    }
                }
            }

            // =========================
            // APPROVAL CHECK (Permission Based)
            // =========================

            var approvalResult = await _customerRepository.CheckModifyPermission(0, model.PortalRowId);

            var hasPermission = approvalResult.HasPermission;

            if (!hasPermission)
            {
                var alreadySent = approvalResult.IsSentForApproval;

                if (alreadySent)
                {
                    ModelState.AddModelError("", "Can not modify, Entry already sent for approval.");
                }
            }

            // =========================
            // NOD / NOC VALIDATION
            // =========================

            if (model.IsNodNocCreation)
            {
                if (string.IsNullOrWhiteSpace(model.NODAccessCode))
                    ModelState.AddModelError("NODAccessCode", "AccessCode is required.");

                if (string.IsNullOrWhiteSpace(model.NODNOC))
                    ModelState.AddModelError("NODNOC", "NODNOC is required.");
            }

            // =========================

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
                var insertResult = await _customerRepository.InsertCustomer(model);

                return Json(new
                {
                    success = true,
                    message = "Customer saved successfully."
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


        [HttpGet]
        public async Task<IActionResult> GetBrandRowDropdown(int divisionRowId)
        {
            CustomerBrandWiseModel model = await _customerRepository.GetCustomerBrandWiseDropdown(divisionRowId);

            return Json(model);
        }

        public JsonResult GetLocationListByDivisionCode(int DivisionCode)
        {
            var locationList = _customerRepository.GetLocationListByDivisionCode(DivisionCode);
            return Json(locationList);
        }
    }
}
