using ERPAPP.Helper;
using ERPAPP.Interfaces;
using ERPAPP.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using static ERPAPP.Helper.Enums;

namespace ERPAPP.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DbHelper _db;

        public CustomerRepository(DbHelper db)
        {
            _db = db;
        }

        public async Task<bool> CheckCustomerInMasterAndBrand(string masterCode, int brandId)
        {
            bool exists = false;

            var parameters = new[]
            {
                new SqlParameter("@No_", masterCode),
                new SqlParameter("@BrandRowId", brandId)
            };

            DataTable dt = _db.GetDataTable("Customer_CheckCodeInmasterAndBrand", parameters);

            if (dt != null && dt.Rows.Count > 0)
            {
                string companyCode = dt.Rows[0]["CompanyCode"]?.ToString();
                exists = !string.IsNullOrEmpty(companyCode);
            }

            return await Task.FromResult(exists);
        }

        public async Task<GetCustomerAddModel> GetCustomerAddData()
        {
            var model = new GetCustomerAddModel();
            var dropDown = new CustomerDropDownModel();

            DataSet ds = _db.GetDataSet("GetCustomerAddDropDownData");

            // 0 Division
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                dropDown.Divisions.Add(new DivisionModel
                {
                    Code = row["RowID"].ToString(),
                    Name = row["Division"].ToString()
                });
            }

            // 1 Country
            foreach (DataRow row in ds.Tables[1].Rows)
            {
                dropDown.Countries.Add(new CountryModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 2 SalesPerson
            foreach (DataRow row in ds.Tables[2].Rows)
            {
                dropDown.SalesPersons.Add(new SalesPersonModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 3 HO SalesPerson
            foreach (DataRow row in ds.Tables[3].Rows)
            {
                dropDown.HOSalesPersons.Add(new SalesPersonModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 4 Customer Category
            foreach (DataRow row in ds.Tables[4].Rows)
            {
                dropDown.CustomerCategories.Add(new CustomerCategoryModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 5 Payment Terms
            foreach (DataRow row in ds.Tables[5].Rows)
            {
                dropDown.PaymentTerms.Add(new PaymentTermsModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 6 MRP Group
            foreach (DataRow row in ds.Tables[6].Rows)
            {
                dropDown.MRPGroups.Add(new MRPGroupModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 7 Payment Method
            foreach (DataRow row in ds.Tables[7].Rows)
            {
                dropDown.PaymentMethods.Add(new PaymentMethodModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 8 Vendor
            foreach (DataRow row in ds.Tables[8].Rows)
            {
                dropDown.Vendors.Add(new VendorModel
                {
                    Code = row["No_"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 9 Customer Price Group
            foreach (DataRow row in ds.Tables[9].Rows)
            {
                dropDown.CustomerPriceGroups.Add(new CustomerPriceGroupModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 10 Price List
            foreach (DataRow row in ds.Tables[10].Rows)
            {
                dropDown.PriceLists.Add(new PriceListModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 11 Gen Business Posting Group
            foreach (DataRow row in ds.Tables[11].Rows)
            {
                dropDown.GenBusPostingGroups.Add(new GenBusPostingGroupModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 12 Customer Posting Group
            foreach (DataRow row in ds.Tables[12].Rows)
            {
                dropDown.CustomerPostingGroups.Add(new CustomerPostingGroupModel
                {
                    Code = row["Code"]?.ToString(),
                    Name = row["Name"]?.ToString()
                });
            }

            // 13 Currency
            foreach (DataRow row in ds.Tables[13].Rows)
            {
                dropDown.Currencies.Add(new CurrencyModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 14 Parent Customer
            foreach (DataRow row in ds.Tables[14].Rows)
            {
                dropDown.ParentCustomers.Add(new ParentCustomerModel
                {
                    Code = row["No_"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 15 Access Code
            foreach (DataRow row in ds.Tables[15].Rows)
            {
                dropDown.AccessCodes.Add(new AccessCodeModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 16 NOD/NOC
            foreach (DataRow row in ds.Tables[16].Rows)
            {
                dropDown.NODNOCs.Add(new NODNOCModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 17 Concessional Code
            foreach (DataRow row in ds.Tables[17].Rows)
            {
                dropDown.ConcessionalCodes.Add(new ConcessionalCodeModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 18 Promo Code
            foreach (DataRow row in ds.Tables[18].Rows)
            {
                dropDown.PromoCodes.Add(new PromoCodeModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 19 Charges Group
            foreach (DataRow row in ds.Tables[19].Rows)
            {
                dropDown.ChargesGroups.Add(new ChargesGroupModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            #region Set Data Enum Values

            dropDown.BusinessCategories = Enum.GetValues(typeof(BusinessCategory))
                .Cast<BusinessCategory>()
                .Select(e => new BusinessCategoryModel
                {
                    Code = ((int)e).ToString(),
                    Name = e.ToString()
                }).ToList();

            // Application Method
            dropDown.ApplicationMethods = Enum.GetValues(typeof(ApplicationMethod))
                .Cast<ApplicationMethod>()
                .Select(e => new ApplicationMethodModel
                {
                    Code = ((int)e).ToString(),
                    Name = e.ToString()
                }).ToList();

            // Dealer Classification
            dropDown.DealerClassifications = Enum.GetValues(typeof(DealerClassification))
                .Cast<DealerClassification>()
                .Select(e => new DealerClassificationModel
                {
                    Code = ((int)e).ToString(),
                    Name = e.ToString()
                }).ToList();

            // Customer Type
            dropDown.CustomerTypes = Enum.GetValues(typeof(CustomerType))
                .Cast<CustomerType>()
                .Select(e => new CustomerTypeModel
                {
                    Code = ((int)e).ToString(),
                    Name = e.ToString()
                }).ToList();

            // Commission Type
            dropDown.CommissionTypes = Enum.GetValues(typeof(CommissionType))
                .Cast<CommissionType>()
                .Select(e => new CommissionTypeModel
                {
                    Code = ((int)e).ToString(),
                    Name = e.ToString()
                }).ToList();

            // GST Customer Type
            dropDown.GSTCustomerTypes = Enum.GetValues(typeof(GSTCustomerType))
                .Cast<GSTCustomerType>()
                .Select(e => new GSTCustomerTypeModel
                {
                    Code = ((int)e).ToString(),
                    Name = e.ToString()
                }).ToList();

            // GST Registration Type
            dropDown.GSTRegistrationTypes = Enum.GetValues(typeof(GSTRegistrationType))
                .Cast<GSTRegistrationType>()
                .Select(e => new GSTRegistrationTypeModel
                {
                    Code = ((int)e).ToString(),
                    Name = e.ToString()
                }).ToList();

            #endregion

            model.CustomerDropDownModel = dropDown;

            var displayNo = await GetCustomerTransferNewNo();
            model.DisplayNo = displayNo > 0 ? displayNo : 0;

            return model;
        }

        public List<AddressDropdownModel> GetCustomerCityList(string city)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Type", "City"),
                new SqlParameter("@City", (object?)city ?? DBNull.Value)
            };

            DataTable dt = _db.GetDataTable("GetCustomer_Address", parameters);

            List<AddressDropdownModel> list = new List<AddressDropdownModel>();

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new AddressDropdownModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            return list;
        }

        public async Task<CustomerMasterModel> GetCustomerMaster(string customerNo)
        {
            CustomerMasterModel model = new CustomerMasterModel();

            var parameters = new[]
    {
        new SqlParameter("@No_", customerNo)
    };

            DataTable dt = _db.GetDataTable($"Customer_GetMasterData", parameters);

            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];

                model = new CustomerMasterModel
                {
                    No = row["NO_"]?.ToString(),
                    Name = row["Name"]?.ToString(),
                    Address = row["Address"]?.ToString(),
                    Address2 = row["Address2"]?.ToString(),

                    City = row["City"]?.ToString(),
                    Postcode = row["Postcode"]?.ToString(),
                    StateCode = row["StateCode"]?.ToString(),
                    CountryCode = row["CountryCode"]?.ToString(),
                    Region = row["Region"]?.ToString(),
                    Zone = row["Zone"]?.ToString(),

                    ContactPerson = row["ContactPerson"]?.ToString(),
                    MobileNo = row["MobileNo"]?.ToString(),
                    PhoneNo = row["PhoneNo"]?.ToString(),
                    FaxNo = row["FaxNo"]?.ToString(),
                    EMail = row["EMail"]?.ToString(),
                    E_Inv_E_Mail = row["E_Inv_E-Mail"]?.ToString(),
                    E_Inv_PhoneNo = row["E_Inv_Phone No_"]?.ToString(),

                    Website_Homepage = row["Website_Homepage"]?.ToString(),
                    LSTNo = row["LSTNo"]?.ToString(),
                    CSTNo = row["CSTNo"]?.ToString(),
                    PANNO = row["PANNO"]?.ToString(),

                    BankName = row["Bank Name"]?.ToString(),
                    BankAccountNo = row["Bank Account No_"]?.ToString(),
                    BranchName = row["Branch Name"]?.ToString(),
                    IFSCode = row["IFS Code"]?.ToString(),

                    GSTRegistrationNo = row["GST Registration No_"]?.ToString(),
                    Allocation = row["Allocation"]?.ToString()
                };

                // Nullable Date Handling
                if (row["LSTTINDate"] != DBNull.Value)
                    model.LSTTINDate = Convert.ToDateTime(row["LSTTINDate"]);

                if (row["CSTTINDate"] != DBNull.Value)
                    model.CSTTINDate = Convert.ToDateTime(row["CSTTINDate"]);

                // Integer Handling
                if (!string.IsNullOrEmpty(row["GST Registration Type"]?.ToString()))
                    model.GSTRegistrationType = Convert.ToInt32(row["GST Registration Type"]);

                if (!string.IsNullOrEmpty(row["GST Customer Type"]?.ToString()))
                    model.GSTCustomerType = Convert.ToInt32(row["GST Customer Type"]);

                if (!string.IsNullOrEmpty(row["Customer Type"]?.ToString()))
                    model.CustomerType = Convert.ToInt32(row["Customer Type"]);
            }

            return model;
        }

        public async Task<int> GetCustomerTransferNewNo()
        {
            int newNo = 0;

            DataTable dt = _db.GetDataTable("CustomerTransferEntry_GetNewNo");

            if (dt != null && dt.Rows.Count > 0)
            {
                newNo = Convert.ToInt32(dt.Rows[0]["NewDisplayNo"]);
            }

            return newNo;
        }


        public async Task<List<CustomerSearchModel>> SearchCustomer(string searchText)
        {
            SqlParameter[] param = { new SqlParameter("@SearchText", searchText) };

            DataTable dt = _db.GetDataTable("Customer_SearchByName", param);

            List<CustomerSearchModel> list = new List<CustomerSearchModel>();

            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    list.Add(new CustomerSearchModel
                    {
                        No = row["No_"].ToString(),
                        Name = row["Name"].ToString()
                    });
                }
            }

            return list;
        }


        public async Task<bool> CheckStateGSTMatch(string stateCode, string gstRegistrationNo)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
        new SqlParameter("@StateCode", stateCode),
        new SqlParameter("@GSTRegistrationNo", gstRegistrationNo)
            };

            DataTable dt = _db.GetDataTable("Customer_StateGSTMatchCheck", parameters);

            if (dt.Rows.Count > 0)
            {
                return Convert.ToBoolean(dt.Rows[0]["IsValid"]);
            }

            return false;
        }

        public async Task<ModifyPermissionResult> CheckModifyPermission(int userRowId, int entryRowId)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
        new SqlParameter("@UserRowId", userRowId),
        new SqlParameter("@EntryRowId", entryRowId)
            };

            DataTable dt = _db.GetDataTable("Customer_CheckModifyPermission", parameters);

            if (dt.Rows.Count > 0)
            {
                return new ModifyPermissionResult
                {
                    HasPermission = Convert.ToBoolean(dt.Rows[0]["HasPermission"]),
                    IsSentForApproval = Convert.ToBoolean(dt.Rows[0]["IsSentForApproval"])
                };
            }

            return new ModifyPermissionResult();
        }

        #region Address -> Country, State, City, PostCode
        public List<AddressDropdownModel> GetCityList(string city)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@Type","CitySearch"),
                new SqlParameter("@City",(object?)city ?? DBNull.Value)
            };

            DataTable dt = _db.GetDataTable("GetCustomer_Address", parameters);

            List<AddressDropdownModel> list = new();

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new AddressDropdownModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            return list;
        }
        public AddressCityDetailModel GetCityDetail(string city)
        {
            SqlParameter[] parameters =
            {
                    new SqlParameter("@Type","CityDetails"),
                    new SqlParameter("@City",(object?)city ?? DBNull.Value)
            };

            DataTable dt = _db.GetDataTable("GetCustomer_Address", parameters);

            AddressCityDetailModel model = new();

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];

                model.CountryCode = row["CountryCode"].ToString();
                model.StateCode = row["StateCode"].ToString();
                model.City = row["City"].ToString();
            }

            return model;
        }
        public List<AddressPostCodeModel> GetPostCodeList(string city)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@Type","PostCode"),
                new SqlParameter("@City",(object?)city ?? DBNull.Value)
            };

            DataTable dt = _db.GetDataTable("GetCustomer_Address", parameters);

            List<AddressPostCodeModel> list = new();

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new AddressPostCodeModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            return list;
        }
        public AddressPostCodeDetailModel GetPostCodeDetail(string code)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@Type","PostCodeDetails"),
                new SqlParameter("@Code",(object?)code ?? DBNull.Value)
            };

            DataTable dt = _db.GetDataTable("GetCustomer_Address", parameters);

            AddressPostCodeDetailModel model = new AddressPostCodeDetailModel();

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];

                model.PostCode = row["PostCode"]?.ToString();
                model.Region = row["Region"]?.ToString();
                model.Zone = row["Zone"]?.ToString();
            }

            return model;
        }

        public async Task<CustomerBrandWiseModel> GetCustomerBrandWiseDropdown(int divisionRowId)
        {
            CustomerBrandWiseModel model = new CustomerBrandWiseModel();

            SqlParameter[] param =
                    {
                new SqlParameter("@DivisionRowId", divisionRowId)
            };

            DataSet ds = _db.GetDataSet("GetCustomerBrandWithDetailsDropDown", param);

            // 1 Dimension / Brand
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                model.DimensionList.Add(new DimesionModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 2 Customer Category
            foreach (DataRow row in ds.Tables[1].Rows)
            {
                model.CustomerCategoryList.Add(new CustomerCategoryModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 3 Discount Group
            foreach (DataRow row in ds.Tables[2].Rows)
            {
                model.DiscountGroupList.Add(new CustomerDiscountGroupModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 4 Sales Person
            foreach (DataRow row in ds.Tables[3].Rows)
            {
                model.SalesPersonList.Add(new SalesPersonModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString(),
                    Allocation = row["Allocation"]?.ToString()
                });
            }

            // 5 HO Sales Person
            foreach (DataRow row in ds.Tables[4].Rows)
            {
                model.HOSalesPersonList.Add(new HOSalesPersonModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            return model;
        }

        public async Task<bool> InsertCustomer(CustomerModel model)
        {
            bool result = false;

            if (model.CustomerBrandAddList != null && model.CustomerBrandAddList.Count > 0)
            {
                foreach (var brand in model.CustomerBrandAddList)
                {
                    SqlParameter[] param =
                    {

                                // ================= SYSTEM =================

                                new SqlParameter("@LoginRowId", Convert.ToInt32(model.LoginRowId)),
                                new SqlParameter("@PortalRowId", model.PortalRowId),
                                new SqlParameter("@DisplayNo", model.DisplayNo == 0 ? 0 : model.DisplayNo),


                                // ================= GENERAL DETAILS =================

                                new SqlParameter("@Division", model.Division ?? (object)DBNull.Value),
                                new SqlParameter("@MasterCode", model.MasterCode ?? ""),

                                new SqlParameter("@Name", model.Name ?? ""),
                                new SqlParameter("@Address", model.Address ?? ""),
                                new SqlParameter("@Address2", model.Address2 ?? ""),
                                new SqlParameter("@City", model.CityCode ?? ""),
                                new SqlParameter("@PostCode", model.PostCode ?? ""),
                                new SqlParameter("@StateCode", model.StateCode ?? ""),
                                new SqlParameter("@CountryCode", model.CountryCode ?? ""),
                                new SqlParameter("@Region", model.Region ?? ""),
                                new SqlParameter("@Zone", model.Zone ?? ""),

                                new SqlParameter("@CreditLimit", model.CreditLimit <= 0 ? 0 : model.CreditLimit),
                                new SqlParameter("@PriceListCode", model.PriceListCode ?? ""),
                                new SqlParameter("@PromoCode", model.PromoCode ?? ""),
                                new SqlParameter("@ChargesGroup", model.ChargesGroup ?? ""),


                                // ================= CONTACT =================

                                new SqlParameter("@ContactPerson", model.ContactPerson ?? ""),
                                new SqlParameter("@MobileNo", model.MobileNo ?? ""),
                                new SqlParameter("@PhoneNo", model.PhoneNo ?? ""),
                                new SqlParameter("@Email", model.Email ?? ""),
                                new SqlParameter("@Website", model.Website ?? ""),


                                // ================= MARKETING =================

                                new SqlParameter("@CustomerType", model.CustomerType ?? 0),
                                new SqlParameter("@ParentCustomerCode", model.ParentCustomerCode ?? ""),
                                new SqlParameter("@VendorCode", model.VendorCode ?? ""),

                                new SqlParameter("@CommissionVendorNo", model.CommissionVendorNo ?? 0),
                                new SqlParameter("@CommissionType", model.CommissionType ?? 0),
                                new SqlParameter("@Commission", model.Commission ?? 0),

                                new SqlParameter("@BankName", model.BankName ?? ""),
                                new SqlParameter("@BranchName", model.BranchName ?? ""),
                                new SqlParameter("@BankAccountNo", model.BankAccountNo ?? ""),
                                new SqlParameter("@IFSCCode", model.IFSCCode ?? ""),


                                // ================= INVOICING =================

                                new SqlParameter("@BillToCustomer", model.BillToCustomer ?? ""),
                                new SqlParameter("@LocationCode", model.LocationCode ?? ""),

                                new SqlParameter("@CustomerPostingGroup", model.CustomerPostingGroup ?? ""),
                                new SqlParameter("@GenBusPostingGroup", model.GenBusPostingGroup ?? ""),

                                new SqlParameter("@EInvPhoneNo", model.EInvPhoneNo ?? ""),
                                new SqlParameter("@EInvEmail", model.EInvEmail ?? ""),

                                new SqlParameter("@CurrencyCode", model.CurrencyCode ?? ""),


                                // ================= PAYMENTS =================

                                new SqlParameter("@ApplicationMethod", model.ApplicationMethod ?? 0),
                                new SqlParameter("@PaymentTermsCode", model.PaymentTermsCode ?? ""),
                                new SqlParameter("@PaymentMethodCode", model.PaymentMethodCode ?? ""),


                                // ================= TAX =================

                                new SqlParameter("@PANNo", model.PANNo ?? ""),

                                new SqlParameter("@GSTRegistrationType", model.GSTRegistrationType ?? 0),
                                new SqlParameter("@GSTRegistrationNo", model.GSTRegistrationNo ?? ""),

                                new SqlParameter("@GSTCustomerType", model.GSTCustomerType ?? 0),
                                new SqlParameter("@ARNNo", model.ARNNo ?? ""),

                                new SqlParameter("@BusinessCategory", model.BusinessCategory ?? 0),
                                new SqlParameter("@MSMEUAMNo", model.MSMEUAMNo ?? ""),


                                // ================= NOD / NOC =================

                                new SqlParameter("@IsNodNocCreation", model.IsNodNocCreation),
                                new SqlParameter("@NODAccessCode", model.NODAccessCode ?? ""),
                                new SqlParameter("@NODNOC", model.NODNOC ?? ""),
                                new SqlParameter("@ConcessionalCode", model.ConcessionalCode ?? ""),
                                new SqlParameter("@ThresholdOverlook", model.ThresholdOverlook),
                                new SqlParameter("@SurchargeOverlook", model.SurchargeOverlook),


                                // ================= EXTRA =================

                                new SqlParameter("@Dimension", model.DivisionCode ?? ""),
                                new SqlParameter("@Allocation", brand.Allocation ?? ""),

                                new SqlParameter("@SalespersonCode", brand.SalesPersonCode ?? ""),
                                new SqlParameter("@HOSalesPersonCode", brand.HOSalesPerson ?? ""),

                                new SqlParameter("@DealerAppointmentDate", brand.DLRAppointmentDate ?? (object)DBNull.Value),
                                new SqlParameter("@DealerClassification", Convert.ToInt32(brand.DealerClassification)),
                                new SqlParameter("@CustomerCategoryCode", brand.CustomerCategoryCode ?? ""),

            };

                    int rows = _db.ExecuteNonQuery("Customer_InsertData", param);

                    if (rows > 0)
                        result = true;
                }
            }

            return result;
        }

        public List<LocationModel> GetLocationListByDivisionCode(int divisionCode)
        {
            List<LocationModel> list = new List<LocationModel>();

            SqlParameter[] param =
            {
                new SqlParameter("@DivisionCode", divisionCode)
            };

            DataTable dt = _db.GetDataTable("GetLocationListByDivisionCode", param);

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new LocationModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            return list;
        }

        public async Task<GetCustomerDataWithPortalRowIdModel> GetCustomerDataWithPortalRowId(int portalRowId)
        {
            try
            {
                SqlParameter[] param =
                {
                new SqlParameter("@CustRowId", portalRowId)
            };

                DataTable dt = _db.GetDataTable("GetCustomerDataWithPortalRowId", param);

                GetCustomerDataWithPortalRowIdModel model = new GetCustomerDataWithPortalRowIdModel();

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    model.Name = row["Name"]?.ToString();
                    model.Address = row["Address"]?.ToString();
                    model.Address2 = row["Address2"]?.ToString();
                    model.CityCode = row["CityCode"]?.ToString();
                    model.PostCode = row["PostCode"]?.ToString();
                    model.StateCode = row["StateCode"]?.ToString();
                    model.CountryCode = row["CountryCode"]?.ToString();
                    model.Region = row["Region"]?.ToString();
                    model.Zone = row["Zone"]?.ToString();

                    model.ContactPerson = row["ContactPerson"]?.ToString();
                    model.MobileNo = row["MobileNo"]?.ToString();
                    model.Email = row["Email"]?.ToString();
                }

                return model;
            }
            catch(Exception ex)
            {
                
                return new GetCustomerDataWithPortalRowIdModel();
            }
        }
        #endregion

    }
}
