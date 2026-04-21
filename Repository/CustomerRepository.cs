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
        private readonly IEmailRepository _emailRepository;

        public CustomerRepository(DbHelper db, IEmailRepository emailRepository)
        {
            _db = db;
            _emailRepository = emailRepository;
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

            // 20 Shipment Method
            foreach (DataRow row in ds.Tables[20].Rows)
            {
                dropDown.ShipmentMethodCodes.Add(new ShipmentMethodCodeModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 21 Shipping Agent
            foreach (DataRow row in ds.Tables[21].Rows)
            {
                dropDown.ShippingAgentCodes.Add(new ShippingAgentCodeModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 22 Shipping Agent Service
            foreach (DataRow row in ds.Tables[22].Rows)
            {
                dropDown.ShippingAgentServices.Add(new ShippingAgentServiceModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 23 Service Zone
            foreach (DataRow row in ds.Tables[23].Rows)
            {
                dropDown.ShippingAgentServiceZoneCodes.Add(new ShippingAgentServiceZoneCodeModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 24 Alternate Customer Group
            foreach (DataRow row in ds.Tables[24].Rows)
            {
                dropDown.ShipAlternatePriceGroups.Add(new ShipAlternatePriceGroupModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 25 Shiiping Country
            foreach (DataRow row in ds.Tables[1].Rows)
            {
                dropDown.ShippingCountries.Add(new ShippingCountryModel
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

            // Shipping Address Type
            dropDown.ShippingAddressTypes = Enum.GetValues(typeof(ShippingAddressType))
                .Cast<ShippingAddressType>()
                .Select(e => new ShippingAddressTypeModel
                {
                    Code = ((int)e).ToString(),
                    Name = e.GetDisplayName().ToString()
                }).ToList();

            //Shipping GST Customer Type
            dropDown.ShippingGSTCustomerTypes = Enum.GetValues(typeof(Shipping_To_GST_Customer_Type))
                .Cast<Shipping_To_GST_Customer_Type>()
                .Select(e => new ShippingGSTCustomerTypeModel
                {
                    Code = ((int)e).ToString(),
                    Name = e.GetDisplayName().ToString()
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
            try
            {
                CustomerMasterModel model = new CustomerMasterModel();

                var parameters = new[]
                {
            new SqlParameter("@MasterCode", customerNo)
        };

                DataTable dt = _db.GetDataTable("Customer_GetMasterDataWithMasterCode", parameters);

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    model = new CustomerMasterModel
                    {
                        No = row["No"]?.ToString(),
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
                        EMail = row["EMail"]?.ToString(),
                        E_Inv_E_Mail = row["E_Inv_E_Mail"]?.ToString(),
                        E_Inv_PhoneNo = row["E_Inv_PhoneNo"]?.ToString(),

                        Website_Homepage = row["WebsiteHomepage"]?.ToString(),

                        BankName = row["BankName"]?.ToString(),
                        BankAccountNo = row["BankAccountNo"]?.ToString(),
                        BranchName = row["BranchName"]?.ToString(),
                        IFSCode = row["IFSCode"]?.ToString(),

                        ParentCustomerCode = row["ParentCustomerCode"]?.ToString(),
                        CommissionVendorNo = row["CommisionVendorNo"]?.ToString(), // check spelling
                        PriceListCode = row["PriceListCode"]?.ToString(),
                        PromoCode = row["PromoCode"]?.ToString(),
                        ChargesGroup = row["ChargesGroup"]?.ToString(),
                        PaymentTermsCode = row["PaymentTermsCode"]?.ToString(),
                        PaymentMethodCode = row["PaymentMethodCode"]?.ToString(),
                        CustomerPostingGroup = row["CustomerPostingGroup"]?.ToString(),
                        GenBusPostingGroup = row["GenBusPostingGroup"]?.ToString(),
                        Currency = row["Currency"]?.ToString(),
                        PANNO = row["PANNO"]?.ToString(),
                        BusinessCategory = row["BusinessCategory"]?.ToString(),
                        MSMEUAMNo = row["MSMEUAMNo"]?.ToString(),
                        CreditLimit = row["CreditLimit"] != DBNull.Value ? Convert.ToDecimal(row["CreditLimit"]) : (decimal?)null
                    };


                    if (row["GSTRegistrationType"] != DBNull.Value)
                        model.GSTRegistrationType = Convert.ToInt32(row["GSTRegistrationType"]);

                    if (row["GSTCustomerType"] != DBNull.Value)
                        model.GSTCustomerType = Convert.ToInt32(row["GSTCustomerType"]);

                    if (row["GSTRegistrationNo"] != DBNull.Value)
                        model.GSTRegistrationNo = row["GSTRegistrationNo"].ToString();

                    if (row["CustomerType"] != DBNull.Value)
                        model.CustomerType = Convert.ToInt32(row["CustomerType"]);

                    if (row["CommissionType"] != DBNull.Value)
                        model.CommissionType = Convert.ToInt32(row["CommissionType"]);

                    if (row["ApplicationMethod"] != DBNull.Value)
                        model.ApplicationMethod = Convert.ToInt32(row["ApplicationMethod"]);
                }

                return model;
            }
            catch (Exception)
            {
                return new CustomerMasterModel();
            }
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

        public async Task<bool> InsertCustomer(CustomerModel model, string userName)
        {
            try
            {
                bool result = false;

                // Create DataTable for Brand List
                DataTable dtBrand = new DataTable();

                dtBrand.Columns.Add("BrandCode");
                dtBrand.Columns.Add("CustomerCategoryCode");
                dtBrand.Columns.Add("TradeSecurityAmount", typeof(decimal));
                dtBrand.Columns.Add("CustomerDiscountGroup");
                dtBrand.Columns.Add("DealerClassification");
                dtBrand.Columns.Add("SalesPersonCode");
                dtBrand.Columns.Add("Allocation");
                dtBrand.Columns.Add("HOSalesPerson");
                dtBrand.Columns.Add("DLRAppointmentDate", typeof(DateTime));
                dtBrand.Columns.Add("DLRTerminationDate", typeof(DateTime));

                if (model.CustomerBrandAddList != null && model.CustomerBrandAddList.Count > 0)
                {
                    foreach (var brand in model.CustomerBrandAddList)
                    {
                        dtBrand.Rows.Add(
                            brand.BrandCode ?? "",
                            brand.CustomerCategoryCode ?? "",
                            brand.TradeSecurityAmount ?? (object)DBNull.Value,
                            brand.CustomerDiscountGroup ?? "",
                            brand.DealerClassification ?? "",
                            brand.SalesPersonCode ?? "",
                            brand.Allocation ?? "",
                            brand.HOSalesPerson ?? "",
                            brand.DLRAppointmentDate ?? new DateTime(1753, 1, 1),
                            brand.DLRTerminationDate ?? new DateTime(1753, 1, 1)
                        );
                    }
                }

                SqlParameter[] param =
                {
                        // ================= SYSTEM =================

                        new SqlParameter("@LoginRowId", Convert.ToInt32(model.LoginRowId)),
                        new SqlParameter("@PortalRowId", model.PortalRowId),
                        new SqlParameter("@DisplayNo", model.DisplayNo == 0 ? 0 : model.DisplayNo),

                        // ================= GENERAL =================

                        new SqlParameter("@Division", model.Division ?? (object)DBNull.Value),
                        new SqlParameter("@DivisionStr", model.DivisionCode ?? (object)DBNull.Value),
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

                        new SqlParameter("@CommissionVendorNo", model.CommissionVendorNo ?? ""),
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

                       // ================= ShipTo =================

                        new SqlParameter("@ShipToCode", model.ShippingCode ?? ""),
                        new SqlParameter("@ShipToName", model.ShippingName ?? ""),
                        new SqlParameter("@ShipToAddress", model.ShippingAddress ?? ""),
                        new SqlParameter("@ShipToAddress2", model.ShippingAddress2 ?? ""),
                        new SqlParameter("@ShipToCity", model.ShippingCity ?? ""),
                        new SqlParameter("@ShipToPostCode", model.ShippingPostalCode ?? ""),
                        new SqlParameter("@ShipToCountryCode", model.ShippingCountry ?? ""),
                        new SqlParameter("@ShipToPhoneNo", model.ShippingPhoneNo ?? ""),
                        new SqlParameter("@ShipToContact", model.ShippingContactPerson ?? ""),

                        new SqlParameter("@ShipToEmail", model.ShippingEmail ?? ""),
                        new SqlParameter("@ShipToLocationCode", model.ShippingLocationCode ?? ""),
                        new SqlParameter("@ShipToShippingMethodCode", model.ShippingMethodCode ?? ""),
                        new SqlParameter("@ShipToShippingAgentCode", model.ShippingAgentCode ?? ""),
                        new SqlParameter("@ShipToShippingAgentServiceCode", model.ShippingAgentServiceCode ?? ""),
                        new SqlParameter("@ShipToStateCode", model.ShippingState ?? ""),
                        new SqlParameter("@ShipToGSTNo", model.ShippingGSTRegistrationNo ?? ""),

                        new SqlParameter("@ShipToAddressType", model.ShippingAddressType ?? 0),
                        new SqlParameter("@ShipToGSTCustomerType", model.ShipToGSTCustomerType ?? 0),

                        new SqlParameter("@AssesseeCode", model.AssesseeCode ?? ""),

                        new SqlParameter("@IsAlreadyCreatedMaster", model.IsAlreadyCreatedMaster),

                        // ⭐ TABLE VALUED PARAMETER
                        new SqlParameter
                        {
                            ParameterName = "@CustomerBrands",
                            SqlDbType = SqlDbType.Structured,
                            TypeName = "dbo.CustomerBrandType",
                            Value = dtBrand
                        }
    };

                var customerNoObj = _db.ExecuteScalar("Customer_InsertDataWithTranferMasterAndCompnayData", param);

                string customerNo = customerNoObj?.ToString();

                if (!string.IsNullOrEmpty(customerNo) &&
                    (customerNo.StartsWith("TD") || customerNo.StartsWith("MD")))
                {
                    if (model.DivisionCode != null)
                    {
                        var emailSend = await _emailRepository.SendMailCustomerUnBlock(model.DivisionCode, new CustomerEmailItemDto
                        {
                            SrNo = 1,
                            Name = model.Name ?? "",
                            RequestedBy = userName,
                            Division = model.DivisionCode,
                            MailID = "softwarecare@italiagroup.in",
                            CustomerCode = customerNo
                        }, model.DisplayNo);
                    }

                    result = true;
                }
                else
                {
                    result = false;
                }

                return result;
            }
            catch (Exception e)
            {
                return false;
            }
        }


        #region comment code insert data in customer and customer brand wise table separately, now merged in single SP with transaction
        //public async Task<bool> InsertCustomer(CustomerModel model)
        //{
        //    bool result = false;

        //    //Add Customer Entry Data Tables
        //    if (model.CustomerBrandAddList != null && model.CustomerBrandAddList.Count > 0)
        //    {
        //        foreach (var brand in model.CustomerBrandAddList)
        //        {
        //            SqlParameter[] param =
        //            {

        //                        // ================= SYSTEM =================

        //                        new SqlParameter("@LoginRowId", Convert.ToInt32(model.LoginRowId)),
        //                        new SqlParameter("@PortalRowId", model.PortalRowId),
        //                        new SqlParameter("@DisplayNo", model.DisplayNo == 0 ? 0 : model.DisplayNo),


        //                        // ================= GENERAL DETAILS =================

        //                        new SqlParameter("@Division", model.Division ?? (object)DBNull.Value),
        //                        new SqlParameter("@MasterCode", model.MasterCode ?? ""),

        //                        new SqlParameter("@Name", model.Name ?? ""),
        //                        new SqlParameter("@Address", model.Address ?? ""),
        //                        new SqlParameter("@Address2", model.Address2 ?? ""),
        //                        new SqlParameter("@City", model.CityCode ?? ""),
        //                        new SqlParameter("@PostCode", model.PostCode ?? ""),
        //                        new SqlParameter("@StateCode", model.StateCode ?? ""),
        //                        new SqlParameter("@CountryCode", model.CountryCode ?? ""),
        //                        new SqlParameter("@Region", model.Region ?? ""),
        //                        new SqlParameter("@Zone", model.Zone ?? ""),

        //                        new SqlParameter("@CreditLimit", model.CreditLimit <= 0 ? 0 : model.CreditLimit),
        //                        new SqlParameter("@PriceListCode", model.PriceListCode ?? ""),
        //                        new SqlParameter("@PromoCode", model.PromoCode ?? ""),
        //                        new SqlParameter("@ChargesGroup", model.ChargesGroup ?? ""),


        //                        // ================= CONTACT =================

        //                        new SqlParameter("@ContactPerson", model.ContactPerson ?? ""),
        //                        new SqlParameter("@MobileNo", model.MobileNo ?? ""),
        //                        new SqlParameter("@PhoneNo", model.PhoneNo ?? ""),
        //                        new SqlParameter("@Email", model.Email ?? ""),
        //                        new SqlParameter("@Website", model.Website ?? ""),


        //                        // ================= MARKETING =================

        //                        new SqlParameter("@CustomerType", model.CustomerType ?? 0),
        //                        new SqlParameter("@ParentCustomerCode", model.ParentCustomerCode ?? ""),
        //                        new SqlParameter("@VendorCode", model.VendorCode ?? ""),

        //                        new SqlParameter("@CommissionVendorNo", model.CommissionVendorNo ?? 0),
        //                        new SqlParameter("@CommissionType", model.CommissionType ?? 0),
        //                        new SqlParameter("@Commission", model.Commission ?? 0),

        //                        new SqlParameter("@BankName", model.BankName ?? ""),
        //                        new SqlParameter("@BranchName", model.BranchName ?? ""),
        //                        new SqlParameter("@BankAccountNo", model.BankAccountNo ?? ""),
        //                        new SqlParameter("@IFSCCode", model.IFSCCode ?? ""),


        //                        // ================= INVOICING =================

        //                        new SqlParameter("@BillToCustomer", model.BillToCustomer ?? ""),
        //                        new SqlParameter("@LocationCode", model.LocationCode ?? ""),

        //                        new SqlParameter("@CustomerPostingGroup", model.CustomerPostingGroup ?? ""),
        //                        new SqlParameter("@GenBusPostingGroup", model.GenBusPostingGroup ?? ""),

        //                        new SqlParameter("@EInvPhoneNo", model.EInvPhoneNo ?? ""),
        //                        new SqlParameter("@EInvEmail", model.EInvEmail ?? ""),

        //                        new SqlParameter("@CurrencyCode", model.CurrencyCode ?? ""),


        //                        // ================= PAYMENTS =================

        //                        new SqlParameter("@ApplicationMethod", model.ApplicationMethod ?? 0),
        //                        new SqlParameter("@PaymentTermsCode", model.PaymentTermsCode ?? ""),
        //                        new SqlParameter("@PaymentMethodCode", model.PaymentMethodCode ?? ""),


        //                        // ================= TAX =================

        //                        new SqlParameter("@PANNo", model.PANNo ?? ""),

        //                        new SqlParameter("@GSTRegistrationType", model.GSTRegistrationType ?? 0),
        //                        new SqlParameter("@GSTRegistrationNo", model.GSTRegistrationNo ?? ""),

        //                        new SqlParameter("@GSTCustomerType", model.GSTCustomerType ?? 0),
        //                        new SqlParameter("@ARNNo", model.ARNNo ?? ""),

        //                        new SqlParameter("@BusinessCategory", model.BusinessCategory ?? 0),
        //                        new SqlParameter("@MSMEUAMNo", model.MSMEUAMNo ?? ""),


        //                        // ================= NOD / NOC =================

        //                        new SqlParameter("@IsNodNocCreation", model.IsNodNocCreation),
        //                        new SqlParameter("@NODAccessCode", model.NODAccessCode ?? ""),
        //                        new SqlParameter("@NODNOC", model.NODNOC ?? ""),
        //                        new SqlParameter("@ConcessionalCode", model.ConcessionalCode ?? ""),
        //                        new SqlParameter("@ThresholdOverlook", model.ThresholdOverlook),
        //                        new SqlParameter("@SurchargeOverlook", model.SurchargeOverlook),


        //                        // ================= EXTRA =================

        //                        new SqlParameter("@Dimension", model.DivisionCode ?? ""),
        //                        new SqlParameter("@Allocation", brand.Allocation ?? ""),

        //                        new SqlParameter("@SalespersonCode", brand.SalesPersonCode ?? ""),
        //                        new SqlParameter("@HOSalesPersonCode", brand.HOSalesPerson ?? ""),

        //                        new SqlParameter("@DealerAppointmentDate", brand.DLRAppointmentDate ?? (object)DBNull.Value),
        //                        new SqlParameter("@DealerClassification", Convert.ToInt32(brand.DealerClassification)),
        //                        new SqlParameter("@CustomerCategoryCode", brand.CustomerCategoryCode ?? ""),

        //    };

        //            int rows = _db.ExecuteNonQuery("Customer_InsertData", param);

        //            if (rows > 0)
        //                result = true;
        //        }
        //    }


        //    //Master Data Entry Data Tables
        //    SqlParameter[] masterDataCustomer =
        //         {
        //            new SqlParameter("@DisplayNo", model.DisplayNo == 0 ? 0 : model.DisplayNo),
        //            new SqlParameter("@MasterCode", model.MasterCode ?? ""),

        //            new SqlParameter("@Name", model.Name ?? ""),
        //            new SqlParameter("@Address", model.Address ?? ""),
        //            new SqlParameter("@Address2", model.Address2 ?? ""),
        //            new SqlParameter("@CityCode", model.CityCode ?? ""),
        //            new SqlParameter("@PostCode", model.PostCode ?? ""),
        //            new SqlParameter("@StateCode", model.StateCode ?? ""),
        //            new SqlParameter("@CountryCode", model.CountryCode ?? ""),
        //            new SqlParameter("@Region", model.Region ?? ""),
        //            new SqlParameter("@Zone", model.Zone ?? ""),

        //            new SqlParameter("@CreditLimit", model.CreditLimit <= 0 ? 0 : model.CreditLimit),

        //            new SqlParameter("@ContactPerson", model.ContactPerson ?? ""),
        //            new SqlParameter("@MobileNo", model.MobileNo ?? ""),
        //            new SqlParameter("@PhoneNo", model.PhoneNo ?? ""),
        //            new SqlParameter("@Email", model.Email ?? ""),
        //            new SqlParameter("@Website", model.Website ?? ""),

        //            new SqlParameter("@CustomerType", model.CustomerType ?? 0),
        //            new SqlParameter("@ParentCustomerCode", model.ParentCustomerCode ?? ""),
        //            new SqlParameter("@VendorCode", model.VendorCode ?? ""),

        //            new SqlParameter("@CommissionVendorNo", model.CommissionVendorNo ?? 0),
        //            new SqlParameter("@CommissionType", model.CommissionType ?? 0),
        //            new SqlParameter("@Commission", model.Commission ?? 0),

        //            new SqlParameter("@BankName", model.BankName ?? ""),
        //            new SqlParameter("@BranchName", model.BranchName ?? ""),
        //            new SqlParameter("@BankAccountNo", model.BankAccountNo ?? ""),
        //            new SqlParameter("@IFSCCode", model.IFSCCode ?? ""),

        //            new SqlParameter("@BillToCustomer", model.BillToCustomer ?? ""),
        //            new SqlParameter("@LocationCode", model.LocationCode ?? ""),

        //            new SqlParameter("@CustomerPostingGroup", model.CustomerPostingGroup ?? ""),
        //            new SqlParameter("@GenBusPostingGroup", model.GenBusPostingGroup ?? ""),

        //            new SqlParameter("@EInvPhoneNo", model.EInvPhoneNo ?? ""),
        //            new SqlParameter("@EInvEmail", model.EInvEmail ?? ""),

        //            new SqlParameter("@CurrencyCode", model.CurrencyCode ?? ""),

        //            new SqlParameter("@ApplicationMethod", model.ApplicationMethod ?? 0),
        //            new SqlParameter("@PaymentTermsCode", model.PaymentTermsCode ?? ""),
        //            new SqlParameter("@PaymentMethodCode", model.PaymentMethodCode ?? ""),

        //            new SqlParameter("@PANNo", model.PANNo ?? ""),

        //            new SqlParameter("@GSTRegistrationType", model.GSTRegistrationType ?? 0),
        //            new SqlParameter("@GSTRegistrationNo", model.GSTRegistrationNo ?? ""),
        //            new SqlParameter("@GSTCustomerType", model.GSTCustomerType ?? 0),
        //            new SqlParameter("@ARNNo", model.ARNNo ?? ""),

        //            new SqlParameter("@BusinessCategory", model.BusinessCategory ?? 0),
        //            new SqlParameter("@MSMEUAMNo", model.MSMEUAMNo ?? ""),

        //            new SqlParameter("@ChargesGroup", model.ChargesGroup ?? ""),
        //            new SqlParameter("@PromoCode", model.PromoCode ?? "")
        //        };

        //    var customerNoObj = _db.ExecuteScalar("Customer_InsertDataWithTranferMasterData", masterDataCustomer);

        //    string customerNo = customerNoObj?.ToString();

        //    if (!string.IsNullOrEmpty(customerNo))
        //    {
        //        result = true;
        //    }

        //    return result;
        //}
        #endregion




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

                    model.PortalRowId = row["PortalRowId"] != DBNull.Value ? Convert.ToInt32(row["PortalRowId"]) : 0;
                    model.MasterCode = row["MasterCode"]?.ToString();

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

                    model.PhoneNo = row["PhoneNo"]?.ToString();
                    model.Email = row["Email"]?.ToString();
                    model.Website = row["Website"]?.ToString();


                    model.CustomerType = row["CustomerType"] != DBNull.Value ? Convert.ToInt32(row["CustomerType"]) : 0;

                    model.PANNo = row["PANNo"]?.ToString();

                    model.GSTRegistrationNo = row["GSTRegistrationNo"]?.ToString();
                    model.GSTRegistrationType = row["GSTRegistrationType"] != DBNull.Value ? Convert.ToInt32(row["GSTRegistrationType"]) : 0;

                    model.ARNNo = row["ARNNo"]?.ToString();

                }

                return model;
            }
            catch (Exception ex)
            {

                return new GetCustomerDataWithPortalRowIdModel();
            }
        }

        public async Task<List<GetCustomerListModel>> GetCustomerList(string searchCustomer)
        {
            try
            {
                SqlParameter[] param =
                {
                    new SqlParameter("@SearchCustomer", searchCustomer ?? (object)DBNull.Value)
                };

                DataTable dt = _db.GetDataTable("GetCustomerList", param);

                List<GetCustomerListModel> list = new List<GetCustomerListModel>();

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        GetCustomerListModel model = new GetCustomerListModel();

                        model.DisplayNo = row["DisplayNo"] != DBNull.Value ? Convert.ToInt32(row["DisplayNo"]) : 0;

                        model.Name = row["Name"]?.ToString();
                        model.City = row["City"]?.ToString();

                        model.Region = row["Region"]?.ToString();
                        model.Zone = row["Zone"]?.ToString();

                        model.Location = row["Location"]?.ToString();
                        model.ContactPerson = row["ContactPerson"]?.ToString();

                        model.MobileNo = row["MobileNo"]?.ToString();

                        model.MasterCode = row["MasterCode"]?.ToString();
                        model.CompanyCode = row["CompanyCode"]?.ToString();
                        model.MasterCodeInCompany = row["MasterCodeInCompany"]?.ToString();

                        model.Division = row["Division"]?.ToString();
                        model.CreatedBy = row["CreatedBy"]?.ToString();

                        model.Blocked = row["Blocked"] != DBNull.Value ? Convert.ToInt32(row["Blocked"]) : 0;

                        list.Add(model);
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                return new List<GetCustomerListModel>();
            }
        }

        #endregion




        public async Task<GetCustomerEditModel> GetCustomerEditData(string customerNo)
        {
            SqlParameter[] param =
                        {
                        new SqlParameter("@CustomerNo", customerNo)
                    };

            DataSet ds = _db.GetDataSet("Customer_GetDataWithCustomerNo", param);

            GetCustomerEditModel model = new GetCustomerEditModel();
            CustomerEditModel customer = new CustomerEditModel();

            // ================= MAIN DATA =================
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];

                model.DisplayNo = Convert.ToInt32(row["DisplayNo"]);

                // ===== GENERAL =====
                model.Name = row["Name"]?.ToString();
                model.MasterCode = row["MasterCode"]?.ToString();
                model.CustomerCode = row["BillToCustomer"]?.ToString();
                model.Address = row["Address"]?.ToString();
                model.Address2 = row["Address2"]?.ToString();
                model.CityCode = row["City"]?.ToString();
                model.PostCode = row["Postcode"]?.ToString();
                model.StateCode = row["StateCode"]?.ToString();
                model.CountryCode = row["CountryCode"]?.ToString();
                model.Region = row["Region"]?.ToString();
                model.Zone = row["Zone"]?.ToString();
                model.DivisionCode = row["DivisionCode"]?.ToString();
                model.PromoCode = row["PromoCode"]?.ToString();
                model.ChargesGroup = row["ChargesGroup"]?.ToString();
                model.PriceListCode = row["PriceListCode"]?.ToString();
                model.CreditLimit = Convert.ToDecimal(row["CreditLimit"]?.ToString());

                // ===== CONTACT =====
                model.ContactPerson = row["ContactPerson"]?.ToString();
                model.MobileNo = row["MobileNo"]?.ToString();
                model.PhoneNo = row["PhoneNo"]?.ToString();
                model.Email = row["EMail"]?.ToString();
                model.Website = row["Website_Homepage"]?.ToString();

                // ===== MARKETING =====
                model.CustomerType = row["CustomerType"] as int?;
                model.CommissionVendorNo = row["CommissionVendorNo"]?.ToString();
                model.ParentCustomerCode = row["ParentCustomerCode"]?.ToString();
                model.CommissionType = row["CommissionType"] as int?;
                model.BankName = row["BankName"]?.ToString();
                model.BranchName = row["BranchName"]?.ToString();
                model.BankAccountNo = row["BankAccountNo"]?.ToString();
                model.IFSCCode = row["IFSCCode"]?.ToString();

                // ===== INVOICING =====
                model.BillToCustomer = row["BillToCustomer"]?.ToString();
                model.LocationCode = row["LocationCode"]?.ToString();
                model.CustomerPostingGroup = row["CustomerPostingGroup"]?.ToString();
                model.GenBusPostingGroup = row["GenBusPostingGroup"]?.ToString();
                model.EInvEmail = row["EInvEmail"]?.ToString();
                model.EInvPhoneNo = row["EInvPhoneNo"]?.ToString();
                model.CurrencyCode = row["CurrencyCode"]?.ToString();

                // ===== PAYMENTS =====
                model.ApplicationMethod = row["ApplicationMethod"] as int?;
                model.PaymentTermsCode = row["PaymentTermsCode"]?.ToString();
                model.PaymentMethodCode = row["PaymentTermsMethod"]?.ToString();

                // ===== TAX =====
                model.PANNo = row["PANNo"]?.ToString();
                model.GSTRegistrationType = row["GSTRegistrationType"] as int?;
                model.GSTRegistrationNo = row["GSTRegistrationNo"]?.ToString();
                model.GSTCustomerType = row["GSTCustomerType"] as int?;
                model.ARNNo = row["ARNNo"]?.ToString();
                model.BusinessCategory = row["BusinessCategory"] as int?;
                model.MSMEUAMNo = row["MSMEUAMNo"]?.ToString();

                // ===== NOD/NOC =====
                model.IsNodNocCreation = row["IsNODNOC_Creation"] != DBNull.Value && Convert.ToBoolean(row["IsNODNOC_Creation"]);
                model.NODAccessCode = row["NOD_AccessCode"]?.ToString();
                model.NODNOC = row["NOD_NODNOC"]?.ToString();
                model.ConcessionalCode = row["NOD_ConcessionalCode"]?.ToString();
                model.ThresholdOverlook = row["NOD_ThresholdOverlook"] != DBNull.Value && Convert.ToBoolean(row["NOD_ThresholdOverlook"]);
                model.SurchargeOverlook = row["NOD_SurchargeOverlook"] != DBNull.Value && Convert.ToBoolean(row["NOD_SurchargeOverlook"]);

                // ===== SHIPPING =====
                model.ShippingCode = row["ShipToCode"]?.ToString();
                model.ShippingName = row["ShipToName"]?.ToString();
                model.ShippingAddress = row["ShipToAddress"]?.ToString();
                model.ShippingAddress2 = row["ShipToAddress2"]?.ToString();
                model.ShippingCity = row["ShipToCity"]?.ToString();
                model.ShippingPostalCode = row["ShipToPostCode"]?.ToString();
                model.ShippingCountry = row["ShipToCountryCode"]?.ToString();
                model.ShippingPhoneNo = row["ShipToPhoneNo"]?.ToString();
                model.ShippingContactPerson = row["ShipToContact"]?.ToString();
                model.ShippingEmail = row["ShipToEmail"]?.ToString();
                model.ShippingLocationCode = row["ShipToLocationCode"]?.ToString();
                model.ShippingMethodCode = row["ShipToMethodCode"]?.ToString();
                model.ShippingAgentCode = row["ShipToAgentCode"]?.ToString();
                model.ShippingAgentServiceCode = row["ShipToAgentServiceCode"]?.ToString();
                model.ShippingState = row["ShipToStateCode"]?.ToString();
                model.ShippingGSTRegistrationNo = row["ShipToGSTNo"]?.ToString();
                model.ShippingAddressType = row["ShipToAddressType"] as int?;
                model.ShipToGSTCustomerType = row["ShipToGSTCustomerType"] as int?;

                model.Blocked = Convert.ToInt32(row["Blocked"]);

                model.AssesseeCode = row["AssesseeCode"]?.ToString();

                model.IsAlreadyCreatedMaster = row["IsAlreadyCreatedMaster"] != DBNull.Value ? Convert.ToInt32(row["IsAlreadyCreatedMaster"]) : 0;

            }

            // ================= BRAND LIST =================
            if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[1].Rows)
                {
                    DealerClassification dealerEnum = new DealerClassification();

                    DateTime? appointmentDate = row["DealerAppointmnetDate"] as DateTime?;
                    DateTime? terminationDate = row["DLRTerminationDate"] as DateTime?;

                    var dealerClassification = row["DealerClassification"]?.ToString();

                    if (dealerClassification != null)
                    {
                        dealerEnum = (DealerClassification)Enum.Parse(typeof(DealerClassification), dealerClassification);
                    }

                    model.CustomerBrandEditList.Add(new CustomerBrandWiseEditModel
                    {
                        CustomerNo = row["CustomerNo"]?.ToString(),
                        BrandCode = row["BrandCode"]?.ToString(),
                        CustomerCategoryCode = row["CustomerCategoryCode"]?.ToString(),
                        TradeSecurityAmount = row["TradeSecurityAmount"] as decimal?,
                        CustomerDiscountGroup = row["CustomerDiscountGroup"]?.ToString(),
                        DealerClassification = dealerEnum.ToString() ?? "",
                        SalesPersonCode = row["SalespersonCode"]?.ToString(),
                        Allocation = row["Allocation"]?.ToString(),
                        HOSalesPerson = row["HOSalesPersonCode"]?.ToString(),
                        // ✅ fix here
                        DLRAppointmentDate = (appointmentDate.HasValue && appointmentDate.Value > new DateTime(1753, 1, 1))
                                    ? appointmentDate
                                    : null,

                        DLRTerminationDate = (terminationDate.HasValue && terminationDate.Value > new DateTime(1753, 1, 1))
                                    ? terminationDate
                                    : null


                    });
                }
            }

            var divisionCode = model.DivisionCode?.Trim().ToUpper();

            if (divisionCode == "MOSAIC")
            {
                model.Division = 2;
            }
            else if (divisionCode == "TILE")
            {
                model.Division = 1;
            }
            else
            {
                model.Division = 3;
            }

            model.PostCodeStr = model.PostCode;

            if (model.Division > 0)
            {
                model.CustomerDropDownModel.Locations = GetLocationListByDivisionCode(model.Division.Value);
            }

            if (model.CityCode != null)
            {
                model.CustomerDropDownModel.PostCode = GetPostCodeList(model.CityCode).Select(x => new PostCodeModel
                {
                    Code = x.Code,
                    Name = x.Name
                }).ToList();
            }

            return model;
        }

        public async Task<GetCustomerEditModel> GetCustomerEditDropDownData()
        {
            var model = new GetCustomerEditModel();
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

            // 20 Shipment Method
            foreach (DataRow row in ds.Tables[20].Rows)
            {
                dropDown.ShipmentMethodCodes.Add(new ShipmentMethodCodeModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 21 Shipping Agent
            foreach (DataRow row in ds.Tables[21].Rows)
            {
                dropDown.ShippingAgentCodes.Add(new ShippingAgentCodeModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 22 Shipping Agent Service
            foreach (DataRow row in ds.Tables[22].Rows)
            {
                dropDown.ShippingAgentServices.Add(new ShippingAgentServiceModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 23 Service Zone
            foreach (DataRow row in ds.Tables[23].Rows)
            {
                dropDown.ShippingAgentServiceZoneCodes.Add(new ShippingAgentServiceZoneCodeModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 24 Alternate Customer Group
            foreach (DataRow row in ds.Tables[24].Rows)
            {
                dropDown.ShipAlternatePriceGroups.Add(new ShipAlternatePriceGroupModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 25 Shiiping Country
            foreach (DataRow row in ds.Tables[1].Rows)
            {
                dropDown.ShippingCountries.Add(new ShippingCountryModel
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

            // Shipping Address Type
            dropDown.ShippingAddressTypes = Enum.GetValues(typeof(ShippingAddressType))
                .Cast<ShippingAddressType>()
                .Select(e => new ShippingAddressTypeModel
                {
                    Code = ((int)e).ToString(),
                    Name = e.GetDisplayName().ToString()
                }).ToList();

            //Shipping GST Customer Type
            dropDown.ShippingGSTCustomerTypes = Enum.GetValues(typeof(Shipping_To_GST_Customer_Type))
                .Cast<Shipping_To_GST_Customer_Type>()
                .Select(e => new ShippingGSTCustomerTypeModel
                {
                    Code = ((int)e).ToString(),
                    Name = e.GetDisplayName().ToString()
                }).ToList();

            #endregion


            model.CustomerDropDownModel = dropDown;

            return model;
        }

        public GetCustomerDivisionWiseDropDown GetCustomerDivisionWiseDropDown(string division)
        {
            var dropDown = new GetCustomerDivisionWiseDropDown();

            SqlParameter[] parameters = new SqlParameter[]
                         {
                            new SqlParameter("@Division", string.IsNullOrEmpty(division) ? (object)DBNull.Value : division)
                         };

            DataSet ds = _db.GetDataSet("Customer_GetDivisionWiseDropDownData", parameters);

            // 0 Price List
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                dropDown.PriceList.Add(new PriceListModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 1 Promo Code List
            foreach (DataRow row in ds.Tables[1].Rows)
            {
                dropDown.PromoCodeList.Add(new PromoCodeModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 2 Charges List
            foreach (DataRow row in ds.Tables[2].Rows)
            {
                dropDown.ChargesGroupList.Add(new ChargesGroupModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 3 Parent Customer List
            foreach (DataRow row in ds.Tables[3].Rows)
            {
                dropDown.ParentCustomerList.Add(new ParentCustomerModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            return dropDown;
        }

        public async Task<bool> EditCustomerBrandWiseOnly(List<CustomerBrandWiseEditModel> model)
        {
            try
            {
                if (model == null || model.Count == 0)
                    return false;

                var custNo = "";

                if (model.Count > 0)
                {
                    custNo = model?.FirstOrDefault(x => !string.IsNullOrEmpty(x.CustomerNo))?.CustomerNo ?? "";
                }

                // 🔹 Create DataTable
                DataTable dtBrand = new DataTable();

                dtBrand.Columns.Add("BrandCode");
                dtBrand.Columns.Add("CustomerCategoryCode");
                dtBrand.Columns.Add("TradeSecurityAmount", typeof(decimal));
                dtBrand.Columns.Add("CustomerDiscountGroup");
                dtBrand.Columns.Add("DealerClassification");
                dtBrand.Columns.Add("SalesPersonCode");
                dtBrand.Columns.Add("Allocation");
                dtBrand.Columns.Add("HOSalesPerson");
                dtBrand.Columns.Add("DLRAppointmentDate", typeof(DateTime));
                dtBrand.Columns.Add("DLRTerminationDate", typeof(DateTime));

                // 🔹 Fill DataTable
                foreach (var brand in model)
                {
                    dtBrand.Rows.Add(
                        brand.BrandCode ?? "",
                        brand.CustomerCategoryCode ?? "",
                        brand.TradeSecurityAmount ?? (object)DBNull.Value,
                        brand.CustomerDiscountGroup ?? "",
                        brand.DealerClassification ?? "",
                        brand.SalesPersonCode ?? "",
                        brand.Allocation ?? "",
                        brand.HOSalesPerson ?? "",
                        brand.DLRAppointmentDate ?? new DateTime(1753, 1, 1),
                        brand.DLRTerminationDate ?? new DateTime(1753, 1, 1)
                    );
                }

                var param = new SqlParameter[]
                            {
                            new SqlParameter("@CustomerNo", custNo),
                            new SqlParameter
                            {
                                ParameterName = "@CustomerBrands",
                                SqlDbType = SqlDbType.Structured,
                                TypeName = "dbo.CustomerBrandType",
                                Value = dtBrand
                            }
                            };

                var result = _db.ExecuteScalar("Customer_EditBrandWiseDataWithCustomerCode", param);

                return result != null;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> CheckCustomerEntryAlreadyExists(string masterCode, string dimension)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                new SqlParameter("@MasterCode", masterCode),
                new SqlParameter("@Dimension", dimension)
                };

                var result = _db.ExecuteScalar(
                    "Customer_Transfer_Entry_AlreadyExists",
                    param
                );

                return Convert.ToInt32(result) == 1;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public Task<bool> CheckCustomerGSTRegistrationAlreadyExists(string GstRegistrationNo)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@GSTRegistrationNo", GstRegistrationNo)
                };

                var result = _db.ExecuteScalar(
                    "Customer_CheckGSTRegistrationNoAlreadyExist",
                    param
                );

                return Task.FromResult(Convert.ToInt32(result) == 1);
            }
            catch (Exception)
            {
                return Task.FromResult(false);
            }
        }

        public async Task<bool> CustomerUnblock(string customerNo, string displayRowId, int loginRowId)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@CompanyCode", customerNo),
                new SqlParameter("@LoginRowId", loginRowId),
                new SqlParameter("@DisplayRowID", displayRowId)
            };

            object result = _db.ExecuteScalar("Customer_ChangeUnBlocked", param);

            int rowsAffected = (result != null) ? Convert.ToInt32(result) : 0;

            return rowsAffected > 0;
        }

        public async Task<List<CountryModel>> GetCustomerCountryList(string country)
        {
            SqlParameter[] parameters =
           {
                new SqlParameter("@Type","Country"),
                new SqlParameter("@Search",(object?)country ?? DBNull.Value)
            };

            DataTable dt = _db.GetDataTable("GetCustomer_Address", parameters);

            List<CountryModel> list = new();

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new CountryModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            return list;
        }

        public async Task<List<PostCodeModel>> GetCustomerPostCodeListWithSearch(string city, string searchpostcode)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@Type","PostCodeWithSearch"),
                new SqlParameter("@City",(object?)city ?? DBNull.Value),
                new SqlParameter("@Search",(object?)searchpostcode ?? DBNull.Value)
            };

            DataTable dt = _db.GetDataTable("GetCustomer_Address", parameters);

            List<PostCodeModel> list = new();

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new PostCodeModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            return list;
        }

        public async Task<GetAssessCodeWithPlaceModel> GetAssessCodeWithPlace(string Place)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter("@Place", (object?)Place ?? DBNull.Value)
                };

            DataTable dt = _db.GetDataTable("GetAssessCodeWithPlace", parameters);

            List<GetAssessCodeWithPlaceModel> list = new();

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new GetAssessCodeWithPlaceModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            return list.First();
        }

        public async Task<GetCustomerUnBlockEditModel> GetCustomerUnBlockEditData(string customerNo)
        {
            SqlParameter[] param =
                         {
                        new SqlParameter("@CustomerNo", customerNo)
                    };

            DataSet ds = _db.GetDataSet("Customer_GetDataWithCustomerNo", param);

            GetCustomerUnBlockEditModel model = new GetCustomerUnBlockEditModel();
            CustomerUnBlockEditModel customer = new CustomerUnBlockEditModel();

            // ================= MAIN DATA =================
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];

                model.DisplayNo = Convert.ToInt32(row["DisplayNo"]);

                // ===== GENERAL =====
                model.Name = row["Name"]?.ToString();
                model.MasterCode = row["MasterCode"]?.ToString();
                model.CustomerCode = row["BillToCustomer"]?.ToString();
                model.Address = row["Address"]?.ToString();
                model.Address2 = row["Address2"]?.ToString();
                model.CityCode = row["City"]?.ToString();
                model.PostCode = row["Postcode"]?.ToString();
                model.StateCode = row["StateCode"]?.ToString();
                model.CountryCode = row["CountryCode"]?.ToString();
                model.Region = row["Region"]?.ToString();
                model.Zone = row["Zone"]?.ToString();
                model.DivisionCode = row["DivisionCode"]?.ToString();
                model.PromoCode = row["PromoCode"]?.ToString();
                model.ChargesGroup = row["ChargesGroup"]?.ToString();
                model.PriceListCode = row["PriceListCode"]?.ToString();
                model.CreditLimit = Convert.ToDecimal(row["CreditLimit"]?.ToString());

                // ===== CONTACT =====
                model.ContactPerson = row["ContactPerson"]?.ToString();
                model.MobileNo = row["MobileNo"]?.ToString();
                model.PhoneNo = row["PhoneNo"]?.ToString();
                model.Email = row["EMail"]?.ToString();
                model.Website = row["Website_Homepage"]?.ToString();

                // ===== MARKETING =====
                model.CustomerType = row["CustomerType"] as int?;
                model.CommissionVendorNo = row["CommissionVendorNo"]?.ToString();
                model.ParentCustomerCode = row["ParentCustomerCode"]?.ToString();
                model.CommissionType = row["CommissionType"] as int?;
                model.BankName = row["BankName"]?.ToString();
                model.BranchName = row["BranchName"]?.ToString();
                model.BankAccountNo = row["BankAccountNo"]?.ToString();
                model.IFSCCode = row["IFSCCode"]?.ToString();

                // ===== INVOICING =====
                model.BillToCustomer = row["BillToCustomer"]?.ToString();
                model.LocationCode = row["LocationCode"]?.ToString();
                model.CustomerPostingGroup = row["CustomerPostingGroup"]?.ToString();
                model.GenBusPostingGroup = row["GenBusPostingGroup"]?.ToString();
                model.EInvEmail = row["EInvEmail"]?.ToString();
                model.EInvPhoneNo = row["EInvPhoneNo"]?.ToString();
                model.CurrencyCode = row["CurrencyCode"]?.ToString();

                // ===== PAYMENTS =====
                model.ApplicationMethod = row["ApplicationMethod"] as int?;
                model.PaymentTermsCode = row["PaymentTermsCode"]?.ToString();
                model.PaymentMethodCode = row["PaymentTermsMethod"]?.ToString();

                // ===== TAX =====
                model.PANNo = row["PANNo"]?.ToString();
                model.GSTRegistrationType = row["GSTRegistrationType"] as int?;
                model.GSTRegistrationNo = row["GSTRegistrationNo"]?.ToString();
                model.GSTCustomerType = row["GSTCustomerType"] as int?;
                model.ARNNo = row["ARNNo"]?.ToString();
                model.BusinessCategory = row["BusinessCategory"] as int?;
                model.MSMEUAMNo = row["MSMEUAMNo"]?.ToString();

                // ===== NOD/NOC =====
                model.IsNodNocCreation = row["IsNODNOC_Creation"] != DBNull.Value && Convert.ToBoolean(row["IsNODNOC_Creation"]);
                model.NODAccessCode = row["NOD_AccessCode"]?.ToString();
                model.NODNOC = row["NOD_NODNOC"]?.ToString();
                model.ConcessionalCode = row["NOD_ConcessionalCode"]?.ToString();
                model.ThresholdOverlook = row["NOD_ThresholdOverlook"] != DBNull.Value && Convert.ToBoolean(row["NOD_ThresholdOverlook"]);
                model.SurchargeOverlook = row["NOD_SurchargeOverlook"] != DBNull.Value && Convert.ToBoolean(row["NOD_SurchargeOverlook"]);

                // ===== SHIPPING =====
                model.ShippingCode = row["ShipToCode"]?.ToString();
                model.ShippingName = row["ShipToName"]?.ToString();
                model.ShippingAddress = row["ShipToAddress"]?.ToString();
                model.ShippingAddress2 = row["ShipToAddress2"]?.ToString();
                model.ShippingCity = row["ShipToCity"]?.ToString();
                model.ShippingPostalCode = row["ShipToPostCode"]?.ToString();
                model.ShippingCountry = row["ShipToCountryCode"]?.ToString();
                model.ShippingPhoneNo = row["ShipToPhoneNo"]?.ToString();
                model.ShippingContactPerson = row["ShipToContact"]?.ToString();
                model.ShippingEmail = row["ShipToEmail"]?.ToString();
                model.ShippingLocationCode = row["ShipToLocationCode"]?.ToString();
                model.ShippingMethodCode = row["ShipToMethodCode"]?.ToString();
                model.ShippingAgentCode = row["ShipToAgentCode"]?.ToString();
                model.ShippingAgentServiceCode = row["ShipToAgentServiceCode"]?.ToString();
                model.ShippingState = row["ShipToStateCode"]?.ToString();
                model.ShippingGSTRegistrationNo = row["ShipToGSTNo"]?.ToString();
                model.ShippingAddressType = row["ShipToAddressType"] as int?;
                model.ShipToGSTCustomerType = row["ShipToGSTCustomerType"] as int?;

                model.Blocked = Convert.ToInt32(row["Blocked"]);

                model.AssesseeCode = row["AssesseeCode"]?.ToString();

                model.IsAlreadyCreatedMaster = row["IsAlreadyCreatedMaster"] != DBNull.Value ? Convert.ToInt32(row["IsAlreadyCreatedMaster"]) : 0;

            }

            // ================= BRAND LIST =================
            if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[1].Rows)
                {
                    DealerClassification dealerEnum = new DealerClassification();

                    DateTime? appointmentDate = row["DealerAppointmnetDate"] as DateTime?;
                    DateTime? terminationDate = row["DLRTerminationDate"] as DateTime?;

                    var dealerClassification = row["DealerClassification"]?.ToString();

                    if (dealerClassification != null)
                    {
                        dealerEnum = (DealerClassification)Enum.Parse(typeof(DealerClassification), dealerClassification);
                    }

                    model.CustomerBrandEditList.Add(new CustomerBrandWiseEditModel
                    {
                        CustomerNo = row["CustomerNo"]?.ToString(),
                        BrandCode = row["BrandCode"]?.ToString(),
                        CustomerCategoryCode = row["CustomerCategoryCode"]?.ToString(),
                        TradeSecurityAmount = row["TradeSecurityAmount"] as decimal?,
                        CustomerDiscountGroup = row["CustomerDiscountGroup"]?.ToString(),
                        DealerClassification = dealerEnum.ToString() ?? "",
                        SalesPersonCode = row["SalespersonCode"]?.ToString(),
                        Allocation = row["Allocation"]?.ToString(),
                        HOSalesPerson = row["HOSalesPersonCode"]?.ToString(),
                        // ✅ fix here
                        DLRAppointmentDate = (appointmentDate.HasValue && appointmentDate.Value > new DateTime(1753, 1, 1))
                                    ? appointmentDate
                                    : null,

                        DLRTerminationDate = (terminationDate.HasValue && terminationDate.Value > new DateTime(1753, 1, 1))
                                    ? terminationDate
                                    : null


                    });
                }
            }

            var divisionCode = model.DivisionCode?.Trim().ToUpper();

            if (divisionCode == "MOSAIC")
            {
                model.Division = 2;
            }
            else if (divisionCode == "TILE")
            {
                model.Division = 1;
            }
            else
            {
                model.Division = 3;
            }

            model.PostCodeStr = model.PostCode;

            if (model.Division > 0)
            {
                model.CustomerDropDownModel.Locations = GetLocationListByDivisionCode(model.Division.Value);
            }

            //if (model.CityCode != null)
            //{
            //    model.CustomerDropDownModel.PostCode = GetPostCodeList(model.CityCode).Select(x => new PostCodeModel
            //    {
            //        Code = x.Code,
            //        Name = x.Name
            //    }).ToList();
            //}

            return model;
        }

        public async Task<bool> UpdateCustomer(CustomerUnBlockEditModel model)
        {
            try
            {
                bool result = false;

                // Create DataTable for Brand List
                DataTable dtBrand = new DataTable();

                dtBrand.Columns.Add("BrandCode");
                dtBrand.Columns.Add("CustomerCategoryCode");
                dtBrand.Columns.Add("TradeSecurityAmount", typeof(decimal));
                dtBrand.Columns.Add("CustomerDiscountGroup");
                dtBrand.Columns.Add("DealerClassification");
                dtBrand.Columns.Add("SalesPersonCode");
                dtBrand.Columns.Add("Allocation");
                dtBrand.Columns.Add("HOSalesPerson");
                dtBrand.Columns.Add("DLRAppointmentDate", typeof(DateTime));
                dtBrand.Columns.Add("DLRTerminationDate", typeof(DateTime));

                if (model.CustomerBrandEditList != null && model.CustomerBrandEditList.Count > 0)
                {
                    foreach (var brand in model.CustomerBrandEditList)
                    {
                        dtBrand.Rows.Add(
                            brand.BrandCode ?? "",
                            brand.CustomerCategoryCode ?? "",
                            brand.TradeSecurityAmount ?? (object)DBNull.Value,
                            brand.CustomerDiscountGroup ?? "",
                            brand.DealerClassification ?? "",
                            brand.SalesPersonCode ?? "",
                            brand.Allocation ?? "",
                            brand.HOSalesPerson ?? "",
                            brand.DLRAppointmentDate ?? new DateTime(1753, 1, 1),
                            brand.DLRTerminationDate ?? new DateTime(1753, 1, 1)
                        );
                    }
                }

                SqlParameter[] param =
                {
                        // ================= SYSTEM =================

                        new SqlParameter("@LoginRowId", Convert.ToInt32(model.LoginRowId)),
                        new SqlParameter("@PortalRowId", model.PortalRowId),
                        new SqlParameter("@DisplayNo", model.DisplayNo == 0 ? 0 : model.DisplayNo),

                        // ================= GENERAL =================

                        new SqlParameter("@Division", model.Division ?? (object)DBNull.Value),
                        new SqlParameter("@DivisionStr", model.DivisionCode ?? (object)DBNull.Value),
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

                        new SqlParameter("@CommissionVendorNo", model.CommissionVendorNo ?? ""),
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

                       // ================= ShipTo =================

                        new SqlParameter("@ShipToCode", model.ShippingCode ?? ""),
                        new SqlParameter("@ShipToName", model.ShippingName ?? ""),
                        new SqlParameter("@ShipToAddress", model.ShippingAddress ?? ""),
                        new SqlParameter("@ShipToAddress2", model.ShippingAddress2 ?? ""),
                        new SqlParameter("@ShipToCity", model.ShippingCity ?? ""),
                        new SqlParameter("@ShipToPostCode", model.ShippingPostalCode ?? ""),
                        new SqlParameter("@ShipToCountryCode", model.ShippingCountry ?? ""),
                        new SqlParameter("@ShipToPhoneNo", model.ShippingPhoneNo ?? ""),
                        new SqlParameter("@ShipToContact", model.ShippingContactPerson ?? ""),

                        new SqlParameter("@ShipToEmail", model.ShippingEmail ?? ""),
                        new SqlParameter("@ShipToLocationCode", model.ShippingLocationCode ?? ""),
                        new SqlParameter("@ShipToShippingMethodCode", model.ShippingMethodCode ?? ""),
                        new SqlParameter("@ShipToShippingAgentCode", model.ShippingAgentCode ?? ""),
                        new SqlParameter("@ShipToShippingAgentServiceCode", model.ShippingAgentServiceCode ?? ""),
                        new SqlParameter("@ShipToStateCode", model.ShippingState ?? ""),
                        new SqlParameter("@ShipToGSTNo", model.ShippingGSTRegistrationNo ?? ""),

                        new SqlParameter("@ShipToAddressType", model.ShippingAddressType ?? 0),
                        new SqlParameter("@ShipToGSTCustomerType", model.ShipToGSTCustomerType ?? 0),

                        new SqlParameter("@AssesseeCode", model.AssesseeCode ?? ""),
                        new SqlParameter("@CompanyCode", model.CustomerCode ?? ""),
                        new SqlParameter("@IsAlreadyCreatedMaster", model.IsAlreadyCreatedMaster ?? 0),

                        // ⭐ TABLE VALUED PARAMETER
                        new SqlParameter
                        {
                            ParameterName = "@CustomerBrands",
                            SqlDbType = SqlDbType.Structured,
                            TypeName = "dbo.CustomerBrandType",
                            Value = dtBrand
                        }
                        };

                var customerNoObj = _db.ExecuteScalar("Customer_UpdateData", param);
                //var customerNoObj = "";

                string customerNo = customerNoObj?.ToString();

                if (!string.IsNullOrEmpty(customerNo) &&
                    (customerNo.StartsWith("TD") || customerNo.StartsWith("MD")))
                {
                    result = true;
                }
                else
                {
                    result = false;
                }

                return result;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<CustomerAlreadyExistModel> GetCustomerAlreadyExistDetails(string masterCode)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@MasterCode", masterCode),
                };

                DataTable dt = _db.GetDataTable("GetCustomerAlreadyExistDetails", param);

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    return new CustomerAlreadyExistModel
                    {
                        CustomerNo = row["CustomerCode"]?.ToString(),
                        Name = row["Name"]?.ToString(),
                        Division = row["Division"]?.ToString()
                    };
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
