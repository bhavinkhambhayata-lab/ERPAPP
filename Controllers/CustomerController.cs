using ERPAPP.Helper;
using ERPAPP.Interfaces;
using ERPAPP.Models;
using ERPAPP.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using static ERPAPP.Helper.Enums;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ERPAPP.Controllers
{
    public class CustomerController : Controller
    {

        private readonly ICustomerRepository _customerRepository;
        private readonly IEmailRepository _emailRepository;

        public CustomerController(ICustomerRepository customerRepository, IEmailRepository emailRepository)
        {
            _customerRepository = customerRepository;
            _emailRepository = emailRepository;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }



        public IActionResult CustomerList()
        {
            return PartialView("_CustomerList");
        }

        public async Task<IActionResult> GetCustomerList(string searchCustomer)
        {
            var data = await _customerRepository.GetCustomerList(searchCustomer);
            return Json(data);
        }


        public async Task<IActionResult> AddEditCustomer()
        {
            var data = await _customerRepository.GetCustomerAddData();
            return PartialView("_AddEditCustomer", data);
        }


        public async Task<IActionResult> SearchCustomer(string searchText)
        {
            var results = await _customerRepository.SearchCustomer(searchText);
            return Json(results);
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomerMasterData(string masterCode, string division)
        {
            var companyCode = await _customerRepository.GetCustomerAlreadyExistDetails(masterCode);

            if (companyCode != null && companyCode.CustomerNo != null && companyCode.Division != null && companyCode.Division.ToLower() == division.ToLower())
            {
                var customerEditData = await _customerRepository.GetCustomerEditData(companyCode.CustomerNo);
                if (customerEditData == null || string.IsNullOrEmpty(customerEditData.Name))
                {
                    return Json(new
                    {
                        success = false,
                        message = "Customer not found."
                    });
                }

                var dropdownData = await _customerRepository.GetCustomerEditDropDownData();

                customerEditData.CustomerDropDownModel = dropdownData.CustomerDropDownModel;

                customerEditData.CustomerDropDownModel.PostCode = _customerRepository.GetPostCodeList(customerEditData.CityCode).Select(x => new PostCodeModel
                {
                    Code = x.Code,
                    Name = x.Name
                }).ToList();

                if (customerEditData.Division.HasValue)
                {
                    customerEditData.CustomerDropDownModel.Locations = _customerRepository.GetLocationListByDivisionCode(customerEditData.Division.Value).Select(x => new LocationModel
                    {
                        Code = x.Code,
                        Name = x.Name
                    }).ToList();
                }

                //customerEditData.CustomerBrandEditList.ForEach(x =>
                //{
                //    if (!string.IsNullOrWhiteSpace(x.DealerClassification))
                //    {
                //        switch (x.DealerClassification.Trim())
                //        {
                //            case "1":
                //                x.DealerClassification = "A";
                //                break;

                //            case "2":
                //                x.DealerClassification = "B";
                //                break;

                //            case "3":
                //                x.DealerClassification = "C";
                //                break;

                //            case "4":
                //                x.DealerClassification = "D";
                //                break;

                //            default:
                //                x.DealerClassification = null;
                //                break;
                //        }
                //    }
                //    else
                //    {
                //        x.DealerClassification = null;
                //    }
                //});

                var masterData = await _customerRepository.GetCustomerMaster(masterCode);
                if (masterData != null)
                {
                    customerEditData.Name = masterData.Name;
                    customerEditData.Address = masterData.Address;
                    customerEditData.Address2 = masterData.Address2;
                    customerEditData.CityCode = masterData.City;
                    customerEditData.PostCode = masterData.Postcode;
                    customerEditData.StateCode = masterData.StateCode;
                    customerEditData.CountryCode = masterData.CountryCode;
                    customerEditData.Region = masterData.Region;
                    customerEditData.Zone = masterData.Zone;
                    customerEditData.ContactPerson = masterData.ContactPerson;

                    customerEditData.MobileNo = masterData.MobileNo;
                    customerEditData.PhoneNo = masterData.PhoneNo;
                    customerEditData.Email = masterData.EMail;
                    customerEditData.EInvEmail = masterData.E_Inv_E_Mail;
                    customerEditData.EInvPhoneNo = masterData.E_Inv_PhoneNo;
                    customerEditData.Website = masterData.Website_Homepage;
                    customerEditData.PANNo = masterData.PANNO;

                    customerEditData.BankName = masterData.BankName;
                    customerEditData.BankAccountNo = masterData.BankAccountNo;
                    customerEditData.BranchName = masterData.BranchName;
                    customerEditData.IFSCCode = masterData.IFSCode;
                    customerEditData.GSTRegistrationNo = masterData.GSTRegistrationNo;
                    customerEditData.GSTRegistrationType = masterData.GSTRegistrationType;
                    customerEditData.GSTCustomerType = masterData.GSTCustomerType;
                    customerEditData.BusinessCategory = masterData.BusinessCategory != null ? Convert.ToInt32(masterData.BusinessCategory) : 0;
                    customerEditData.MSMEUAMNo = masterData.MSMEUAMNo;
                }

                return PartialView("_EditCustomer", customerEditData);

            }

            var data = await _customerRepository.GetCustomerMaster(masterCode);

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

        public JsonResult GetPostCodeDetail(string postcode, string city)
        {
            var result = _customerRepository.GetPostCodeDetail(postcode, city);
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

            if (!string.IsNullOrWhiteSpace(model.CountryCode) && model.CountryCode != "IN" && string.IsNullOrWhiteSpace(model.CurrencyCode))
            {
                ModelState.AddModelError("CurrencyCode", "Currency is required.");
            }

            if (!string.IsNullOrWhiteSpace(model.Email))
            {
                var emails = model.Email.Split(';');

                if (emails.Any(e =>
                    !Regex.IsMatch(e.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$")))
                {
                    ModelState.AddModelError("Email", "Invalid email format");
                }
            }
            else
            {
                ModelState.AddModelError("Email", "Email is required.");
            }


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

            if (model.GSTCustomerType == 1 || model.GSTCustomerType == 7)  //Registered  // Composite // SEZ
            {
                if (string.IsNullOrWhiteSpace(model.GSTRegistrationNo) && string.IsNullOrWhiteSpace(model.ARNNo))
                {
                    ModelState.AddModelError("GSTRegistrationNo", "Either GST No or ARN is mandatory");
                }

                if (!string.IsNullOrWhiteSpace(model.GSTRegistrationNo) && string.IsNullOrWhiteSpace(model.PANNo))
                {
                    ModelState.AddModelError("PANNo", "PAN Number is mandatory when GST Registration Number is provided.");
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
                //else if (!System.Text.RegularExpressions.Regex
                //    .IsMatch(model.PANNo, @"^[A-Z]{5}[0-9]{4}[A-Z]$"))
                //{
                //    ModelState.AddModelError("PANNo", "Invalid PAN No.");
                //}
            }

            // =========================
            // GSTIN VALIDATION
            // =========================

            if (!string.IsNullOrWhiteSpace(model.GSTRegistrationNo))
            {
                model.GSTRegistrationNo = model.GSTRegistrationNo.Trim().ToUpper();
                model.PANNo = model.PANNo?.Trim().ToUpper();

                string pattern = @"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z][1-9A-Z]Z[0-9A-Z]$";

                if (model.GSTRegistrationNo.Length != 15 ||
                    !Regex.IsMatch(model.GSTRegistrationNo, pattern))
                {
                    ModelState.AddModelError("GSTRegistrationNo", "Invalid GSTIN format.");
                }
                else
                {
                    string gstPanPart = model.GSTRegistrationNo.Substring(2, 10);

                    if (!string.IsNullOrWhiteSpace(model.PANNo) &&
                        gstPanPart != model.PANNo)
                    {
                        ModelState.AddModelError("GSTRegistrationNo", "GSTIN PAN does not match PAN No.");
                    }

                    if (!string.IsNullOrWhiteSpace(model.StateCode))
                    {
                        var stateValid = await _customerRepository.CheckStateGSTMatch(model.StateCode, model.GSTRegistrationNo);

                        if (!stateValid)
                        {
                            ModelState.AddModelError("GSTRegistrationNo", "Invalid GST Regi. No as per State.");
                        }
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

            // =========================
            // Shipping Sections
            // =========================

            if (model.ShipToGSTCustomerType == 1 && string.IsNullOrWhiteSpace(model.ShippingGSTRegistrationNo))
            {
                ModelState.AddModelError("ShippingGSTRegistrationNo", "Shipping GST Registration No is required.");
            }

            // =========================
            // ERP Check City/Post Code / Country
            // =========================

            #region Check ERP City And Post Code And Country Exist 
            var allCityList = await _customerRepository.GetCityList();
            var allPostCodeList = await _customerRepository.GetPostCodeList();
            var allCountryList = await _customerRepository.GetCountryList();

            #region City And Post Code And Country Code

            // =========================
            // CityCode AND PostCode And CountryCode Check Condition In ERP Exist Name or Code 
            // =========================

            if (!string.IsNullOrWhiteSpace(model.CityCode))
            {
                bool isCityExists = allCityList != null && allCityList.Any(x => x.Code != null && x.Code.ToLower() == model.CityCode.ToLower());

                if (!isCityExists)
                {
                    ModelState.AddModelError("CityCode", "Invalid City.");
                }
            }

            if (!string.IsNullOrWhiteSpace(model.PostCode))
            {
                bool isPostCodeExists = allPostCodeList != null && allPostCodeList.Any(x => x.Code != null && x.Code.ToLower() == model.PostCode.ToLower());

                if (!isPostCodeExists)
                {
                    ModelState.AddModelError("PostCode", "Invalid Post Code.");
                }
            }

            if (!string.IsNullOrWhiteSpace(model.CountryCode))
            {
                bool isCountryExists = allCountryList != null && allCountryList.Any(x => x.Code != null && x.Code.ToLower() == model.CountryCode.ToLower());

                if (!isCountryExists)
                {
                    ModelState.AddModelError("CountryCode", "Invalid Country.");
                }
            }

            #endregion

            #region Shipping City And Post Code And Country Code
            // =========================
            // Shipping CityCode AND PostCode And CountryCode Check Condition In ERP Exist Name or Code 
            // =========================

            if (!string.IsNullOrWhiteSpace(model.ShippingCity))
            {
                bool isShippingCityExists = allCityList != null && allCityList.Any(x => x.Code != null && x.Code.ToLower() == model.ShippingCity.ToLower());

                if (!isShippingCityExists)
                {
                    ModelState.AddModelError("ShippingCity", "Invalid City.");
                }
            }

            if (!string.IsNullOrWhiteSpace(model.ShippingPostalCode))
            {
                bool isShippingPostCodeExists = allPostCodeList != null && allPostCodeList.Any(x => x.Code != null && x.Code.ToLower() == model.ShippingPostalCode.ToLower());

                if (!isShippingPostCodeExists)
                {
                    ModelState.AddModelError("ShippingPostalCode", "Invalid Post Code.");
                }
            }

            if (!string.IsNullOrWhiteSpace(model.ShippingCountry))
            {
                bool isShippingCountryExists = allCountryList != null && allCountryList.Any(x => x.Code != null && x.Code.ToLower() == model.ShippingCountry.ToLower());

                if (!isShippingCountryExists)
                {
                    ModelState.AddModelError("ShippingCountry", "Invalid Country.");
                }
            }

            #endregion

            #endregion

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
                var userName = HttpContext.Session.GetString("UserName") ?? "";

                var insertResult = await _customerRepository.InsertCustomer(model, userName);

                if (insertResult)
                {
                    return Json(new
                    {
                        success = true,
                        message = "Customer saved successfully."
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
        public async Task<IActionResult> GetBrandRowDropdown(int divisionRowId)
        {
            CustomerBrandWiseModel model = await _customerRepository.GetCustomerBrandWiseDropdown(divisionRowId);

            model.DealerClassficationList = Enum.GetValues(typeof(DealerClassification))
                .Cast<DealerClassification>()
                .Select(e => new DealerClassificationModel
                {
                    Code = ((int)e).ToString(),
                    Name = e.ToString()
                }).ToList();

            return Json(model);
        }

        public JsonResult GetLocationListByDivisionCode(int DivisionCode)
        {
            var locationList = _customerRepository.GetLocationListByDivisionCode(DivisionCode);
            return Json(locationList);
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomerDataWithPortalRowId(int portalRowId)
        {
            if (portalRowId <= 0)
            {
                return Json(new { success = false, message = "Invalid Portal Row Id." });
            }

            var data = await _customerRepository.GetCustomerDataWithPortalRowId(portalRowId);

            if (data == null || string.IsNullOrEmpty(data.Name))
            {
                return Json(new
                {
                    success = false,
                    message = "Customer not found in Portal."
                });
            }

            return Json(new
            {
                success = true,
                result = data
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomerDataWithMasterCode(string masterCode)
        {
            var data = await _customerRepository.GetCustomerMaster(masterCode);

            if (data == null || string.IsNullOrEmpty(data.Name))
            {
                return Json(new
                {
                    success = false,
                    message = "Customer not found."
                });
            }

            return Json(new
            {
                success = true,
                result = data
            });
        }


        [HttpGet]
        public async Task<IActionResult> GetCustomerEditData(string customerNo)
        {
            var data = await _customerRepository.GetCustomerEditData(customerNo);
            if (data == null || string.IsNullOrEmpty(data.Name))
            {
                return Json(new
                {
                    success = false,
                    message = "Customer not found."
                });
            }

            var dropdownData = await _customerRepository.GetCustomerEditDropDownData();

            data.CustomerDropDownModel = dropdownData.CustomerDropDownModel;

            data.CustomerDropDownModel.PostCode = _customerRepository.GetPostCodeList(data.CityCode).Select(x => new PostCodeModel
            {
                Code = x.Code,
                Name = x.Name
            }).ToList();

            if (data.Division.HasValue)
            {
                data.CustomerDropDownModel.Locations = _customerRepository.GetLocationListByDivisionCode(data.Division.Value).Select(x => new LocationModel
                {
                    Code = x.Code,
                    Name = x.Name
                }).ToList();
            }

            //data.CustomerBrandEditList.ForEach(x =>
            //{
            //    if (!string.IsNullOrWhiteSpace(x.DealerClassification))
            //    {
            //        switch (x.DealerClassification.Trim())
            //        {
            //            case "1":
            //                x.DealerClassification = "A";
            //                break;

            //            case "2":
            //                x.DealerClassification = "B";
            //                break;

            //            case "3":
            //                x.DealerClassification = "C";
            //                break;

            //            case "4":
            //                x.DealerClassification = "D";
            //                break;

            //            default:
            //                x.DealerClassification = null;
            //                break;
            //        }
            //    }
            //    else
            //    {
            //        x.DealerClassification = null;
            //    }
            //});

            if (data.MasterCode != null)
            {
                var masterData = await _customerRepository.GetCustomerMaster(data.MasterCode);
                if (masterData != null)
                {
                    data.Name = masterData.Name;
                    data.Address = masterData.Address;
                    data.Address2 = masterData.Address2;
                    data.CityCode = masterData.City;
                    data.PostCode = masterData.Postcode;
                    data.StateCode = masterData.StateCode;
                    data.CountryCode = masterData.CountryCode;
                    data.Region = masterData.Region;
                    data.Zone = masterData.Zone;
                    data.ContactPerson = masterData.ContactPerson;

                    data.MobileNo = masterData.MobileNo;
                    data.PhoneNo = masterData.PhoneNo;
                    data.Email = masterData.EMail;
                    data.EInvEmail = masterData.E_Inv_E_Mail;
                    data.EInvPhoneNo = masterData.E_Inv_PhoneNo;
                    data.Website = masterData.Website_Homepage;
                    data.PANNo = masterData.PANNO;

                    data.BankName = masterData.BankName;
                    data.BankAccountNo = masterData.BankAccountNo;
                    data.BranchName = masterData.BranchName;
                    data.IFSCCode = masterData.IFSCode;
                    data.GSTRegistrationNo = masterData.GSTRegistrationNo;
                    data.GSTRegistrationType = masterData.GSTRegistrationType;
                    data.GSTCustomerType = masterData.GSTCustomerType;
                    data.BusinessCategory = masterData.BusinessCategory != null ? Convert.ToInt32(masterData.BusinessCategory) : 0;
                    data.MSMEUAMNo = masterData.MSMEUAMNo;
                }
            }

            return PartialView("_EditCustomer", data);
        }


        [HttpGet]
        public JsonResult GetCustomerDivisionWiseDropDown(string division)
        {
            var result = _customerRepository.GetCustomerDivisionWiseDropDown(division);

            return Json(new
            {
                priceList = result.PriceList,
                promoCodeList = result.PromoCodeList,
                chargesGroupList = result.ChargesGroupList,
                parentCustomerList = result.ParentCustomerList
            });
        }

        [HttpPost]
        public async Task<IActionResult> EditCustomerDetailsBrandWiseDataOnly([FromBody] CustomerBrandEditRequest request)
        {
            if (request == null || request.list == null || request.list.Count == 0)
            {
                return Json(new { success = false, message = "No brand data received!" });
            }

            try
            {
                var result = await _customerRepository.EditCustomerBrandWiseOnly(request.list, request.SalesPersonCode);

                if (result)
                {
                    return Json(new { success = true, message = "Brand details saved successfully!" });
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
        public async Task<IActionResult> CheckCustomerGSTRegistrationAlreadyExists(string gstRegistrationNo)
        {
            try
            {
                var isExists = await _customerRepository
                    .CheckCustomerGSTRegistrationAlreadyExists(gstRegistrationNo);

                return Json(new { success = true, data = isExists });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        [HttpPost]
        public async Task<IActionResult> CustomerUnblock(string customerCode, string displayRowId, string customerName, string division)
        {
            try
            {
                int userRowId = HttpContext.Session.GetInt32("UserRowId") ?? 0;

                var customerUnBlock = await _customerRepository.CustomerUnblock(customerCode, displayRowId, userRowId);

                if (customerUnBlock)
                {
                    var sendEmail = await _emailRepository.SendMailCustomerBlock(division, Convert.ToInt32(displayRowId), customerCode, customerName);

                    return Json(new { success = true, message = "Customer Un-blocked Successfully." });
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
        public async Task<JsonResult> GetCountryList(string searchcountry)
        {
            var result = await _customerRepository.GetCustomerCountryList(searchcountry);
            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> GetPostCodeListWithSearch(string city, string search)
        {
            var result = await _customerRepository.GetCustomerPostCodeListWithSearch(city, search);
            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> GetAssessCodeWithPlace(string Place)
        {
            var result = await _customerRepository.GetAssessCodeWithPlace(Place);
            return Json(result);
        }



        [HttpGet]
        public async Task<IActionResult> GetCustomerUnBlockEditData(string customerNo)
        {
            var data = await _customerRepository.GetCustomerUnBlockEditData(customerNo);
            if (data == null || string.IsNullOrEmpty(data.Name))
            {
                return Json(new
                {
                    success = false,
                    message = "Customer not found."
                });
            }

            var dropdownData = await _customerRepository.GetCustomerEditDropDownData();

            data.CustomerDropDownModel = dropdownData.CustomerDropDownModel;

            if (data.Division.HasValue)
            {
                data.CustomerDropDownModel.Locations = _customerRepository.GetLocationListByDivisionCode(data.Division.Value).Select(x => new LocationModel
                {
                    Code = x.Code,
                    Name = x.Name
                }).ToList();

            }

            if (data.DivisionCode != null)
            {
                var divisionWiseDropdown = _customerRepository.GetCustomerDivisionWiseDropDown(data.DivisionCode.ToString());
                data.CustomerDropDownModel.PriceLists = divisionWiseDropdown.PriceList;
                data.CustomerDropDownModel.PromoCodes = divisionWiseDropdown.PromoCodeList;
                data.CustomerDropDownModel.ChargesGroups = divisionWiseDropdown.ChargesGroupList;
                data.CustomerDropDownModel.ParentCustomers = divisionWiseDropdown.ParentCustomerList;
            }

            data.customerBrandWiseModel = await _customerRepository.GetCustomerBrandWiseDropdown(data.Division.Value);

            data.customerBrandWiseModel.DealerClassficationList = Enum.GetValues(typeof(DealerClassification))
                .Cast<DealerClassification>()
                .Select(e => new DealerClassificationModel
                {
                    Code = ((int)e).ToString(),
                    Name = e.ToString()
                }).ToList();

            return PartialView("_EditCustomerUnBlock", data);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateCustomerMaster(CustomerUnBlockEditModel model)
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

            if (!string.IsNullOrWhiteSpace(model.CountryCode) && model.CountryCode != "IN" && string.IsNullOrWhiteSpace(model.CurrencyCode))
            {
                ModelState.AddModelError("CurrencyCode", "Currency is required.");
            }

            if (!string.IsNullOrWhiteSpace(model.Email))
            {
                var emails = model.Email.Split(';');

                if (emails.Any(e =>
                    !Regex.IsMatch(e.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$")))
                {
                    ModelState.AddModelError("Email", "Invalid email format");
                }
            }
            else
            {
                ModelState.AddModelError("Email", "Email is required.");
            }


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

            if (model.GSTCustomerType == 1 || model.GSTCustomerType == 7)  //Registered  // Composite // SEZ
            {
                if (string.IsNullOrWhiteSpace(model.GSTRegistrationNo) && string.IsNullOrWhiteSpace(model.ARNNo))
                {
                    ModelState.AddModelError("GSTRegistrationNo", "Either GST No or ARN is mandatory");
                }

                if (!string.IsNullOrWhiteSpace(model.GSTRegistrationNo) && string.IsNullOrWhiteSpace(model.PANNo))
                {
                    ModelState.AddModelError("PANNo", "PAN Number is mandatory when GST Registration Number is provided.");
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
                //else if (!System.Text.RegularExpressions.Regex
                //    .IsMatch(model.PANNo, @"^[A-Z]{5}[0-9]{4}[A-Z]$"))
                //{
                //    ModelState.AddModelError("PANNo", "Invalid PAN No.");
                //}
            }

            // =========================
            // GSTIN VALIDATION
            // =========================

            if (!string.IsNullOrWhiteSpace(model.GSTRegistrationNo))
            {
                model.GSTRegistrationNo = model.GSTRegistrationNo.Trim().ToUpper();
                model.PANNo = model.PANNo?.Trim().ToUpper();

                string pattern = @"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z][1-9A-Z]Z[0-9A-Z]$";

                if (model.GSTRegistrationNo.Length != 15 ||
                    !Regex.IsMatch(model.GSTRegistrationNo, pattern))
                {
                    ModelState.AddModelError("GSTRegistrationNo", "Invalid GSTIN format.");
                }
                else
                {
                    string gstPanPart = model.GSTRegistrationNo.Substring(2, 10);

                    if (!string.IsNullOrWhiteSpace(model.PANNo) &&
                        gstPanPart != model.PANNo)
                    {
                        ModelState.AddModelError("GSTRegistrationNo", "GSTIN PAN does not match PAN No.");
                    }

                    if (!string.IsNullOrWhiteSpace(model.StateCode))
                    {
                        var stateValid = await _customerRepository.CheckStateGSTMatch(model.StateCode, model.GSTRegistrationNo);

                        if (!stateValid)
                        {
                            ModelState.AddModelError("GSTRegistrationNo", "Invalid GST Regi. No as per State.");
                        }
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

            // =========================
            // Shipping Sections
            // =========================

            if (model.ShipToGSTCustomerType == 1 && string.IsNullOrWhiteSpace(model.ShippingGSTRegistrationNo))
            {
                ModelState.AddModelError("ShippingGSTRegistrationNo", "Shipping GST Registration No is required.");
            }

            // =========================
            // ERP Check City/Post Code / Country
            // =========================

            #region Check ERP City And Post Code And Country Exist 

            var allCityList = await _customerRepository.GetCityList();
            var allPostCodeList = await _customerRepository.GetPostCodeList();
            var allCountryList = await _customerRepository.GetCountryList();

            #region City And Post Code And Country Code

            // =========================
            // CityCode AND PostCode And CountryCode Check Condition In ERP Exist Name or Code 
            // =========================

            if (!string.IsNullOrWhiteSpace(model.CityCode))
            {
                bool isCityExists = allCityList != null && allCityList.Any(x => x.Code != null && x.Code.ToLower() == model.CityCode.ToLower());

                if (!isCityExists)
                {
                    ModelState.AddModelError("CityCode", "Invalid City.");
                }
            }

            if (!string.IsNullOrWhiteSpace(model.PostCode))
            {
                bool isPostCodeExists = allPostCodeList != null && allPostCodeList.Any(x => x.Code != null && x.Code.ToLower() == model.PostCode.ToLower());

                if (!isPostCodeExists)
                {
                    ModelState.AddModelError("PostCode", "Invalid Post Code.");
                }
            }

            if (!string.IsNullOrWhiteSpace(model.CountryCode))
            {
                bool isCountryExists = allCountryList != null && allCountryList.Any(x => x.Code != null && x.Code.ToLower() == model.CountryCode.ToLower());

                if (!isCountryExists)
                {
                    ModelState.AddModelError("CountryCode", "Invalid Country.");
                }
            }

            #endregion

            #region Shipping City And Post Code And Country Code
            // =========================
            // Shipping CityCode AND PostCode And CountryCode Check Condition In ERP Exist Name or Code 
            // =========================

            if (!string.IsNullOrWhiteSpace(model.ShippingCity))
            {
                bool isShippingCityExists = allCityList != null && allCityList.Any(x => x.Code != null && x.Code.ToLower() == model.ShippingCity.ToLower());

                if (!isShippingCityExists)
                {
                    ModelState.AddModelError("ShippingCity", "Invalid City.");
                }
            }

            if (!string.IsNullOrWhiteSpace(model.ShippingPostalCode))
            {
                bool isShippingPostCodeExists = allPostCodeList != null && allPostCodeList.Any(x => x.Code != null && x.Code.ToLower() == model.ShippingPostalCode.ToLower());

                if (!isShippingPostCodeExists)
                {
                    ModelState.AddModelError("ShippingPostalCode", "Invalid Post Code.");
                }
            }

            if (!string.IsNullOrWhiteSpace(model.ShippingCountry))
            {
                bool isShippingCountryExists = allCountryList != null && allCountryList.Any(x => x.Code != null && x.Code.ToLower() == model.ShippingCountry.ToLower());

                if (!isShippingCountryExists)
                {
                    ModelState.AddModelError("ShippingCountry", "Invalid Country.");
                }
            }

            #endregion

            #endregion

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
                var updateResult = await _customerRepository.UpdateCustomer(model);

                if (updateResult)
                {
                    return Json(new
                    {
                        success = true,
                        message = "Customer updated successfully."
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

        [HttpPost]
        public async Task<IActionResult> EditSalesPersonCode(string customerNo, string salesPersonCode)
        {
            try
            {
                bool result = await _customerRepository.EditSalesPersonCode(customerNo, salesPersonCode);

                if (result)
                {
                    return Json(new
                    {
                        success = true,
                        message = "Customer updated successfully."
                    });
                }

                return Json(new
                {
                    success = false,
                    message = "Failed to update Sales Person Code."
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
    }
}
