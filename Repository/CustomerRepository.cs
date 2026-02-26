using ERPAPP.Helper;
using ERPAPP.Interfaces;
using ERPAPP.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ERPAPP.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DbHelper _db;

        public CustomerRepository(DbHelper db)
        {
            _db = db;
        }
        public async Task<GetCustomerAddModel> GetCustomerAddData()
        {
            var model = new GetCustomerAddModel();
            var dropDown = new CustomerDropDownModel();

            DataSet ds = _db.GetDataSet("GetCustomerAddDropDownData");

            // 1️⃣ Brand
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                dropDown.Brands.Add(new BrandModel
                {
                    RowID = Convert.ToInt32(row["RowID"]),
                    Code = row["Brand"].ToString(),
                    Name = row["Brand"].ToString()
                });
            }

            // 2️⃣ Country
            foreach (DataRow row in ds.Tables[1].Rows)
            {
                dropDown.Countries.Add(new CountryModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 3️⃣ SalesPerson Name
            foreach (DataRow row in ds.Tables[2].Rows)
            {
                dropDown.SalesPersons.Add(new SalesPersonModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 4️⃣ HO SalesPerson (using Code list if needed)
            foreach (DataRow row in ds.Tables[5].Rows)
            {
                dropDown.HOSalesPersons.Add(new SalesPersonModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 7️⃣ Customer Category
            foreach (DataRow row in ds.Tables[6].Rows)
            {
                dropDown.CustomerCategories.Add(new CustomerCategoryModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 8️⃣ Payment Terms
            foreach (DataRow row in ds.Tables[7].Rows)
            {
                dropDown.PaymentTerms.Add(new PaymentTermsModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 10️⃣ Payment Method
            foreach (DataRow row in ds.Tables[9].Rows)
            {
                dropDown.PaymentMethods.Add(new PaymentMethodModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 11️⃣ Vendor
            foreach (DataRow row in ds.Tables[10].Rows)
            {
                dropDown.Vendors.Add(new VendorModel
                {
                    Code = row["No_"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 12️⃣ Customer Price Group
            foreach (DataRow row in ds.Tables[11].Rows)
            {
                dropDown.CustomerPriceGroups.Add(new CustomerPriceGroupModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 13️⃣ Price List
            foreach (DataRow row in ds.Tables[12].Rows)
            {
                dropDown.PriceLists.Add(new PriceListModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 18️⃣ Currency
            foreach (DataRow row in ds.Tables[17].Rows)
            {
                dropDown.Currencies.Add(new CurrencyModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 19️⃣ Parent Customer
            foreach (DataRow row in ds.Tables[18].Rows)
            {
                dropDown.ParentCustomers.Add(new ParentCustomerModel
                {
                    Code = row["No_"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 20️⃣ Access Code
            foreach (DataRow row in ds.Tables[19].Rows)
            {
                dropDown.AccessCodes.Add(new AccessCodeModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 21️⃣ NOD/NOC
            foreach (DataRow row in ds.Tables[20].Rows)
            {
                dropDown.NODNOCs.Add(new NODNOCModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            // 22️⃣ Concessional Code
            foreach (DataRow row in ds.Tables[21].Rows)
            {
                dropDown.ConcessionalCodes.Add(new ConcessionalCodeModel
                {
                    Code = row["Code"].ToString(),
                    Name = row["Name"].ToString()
                });
            }

            model.CustomerDropDownModel = dropDown;

            var displayNo = await GetCustomerTransferNewNo();

            model.DisplayNo = displayNo > 0 ? displayNo : 0;

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
    }
}
