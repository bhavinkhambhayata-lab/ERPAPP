using ERPAPP.Helper;
using ERPAPP.Interfaces;
using ERPAPP.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ERPAPP.Repository
{
    public class LoginRepository : ILoginRepository
    {
        private readonly DbHelper _db;

        public LoginRepository(DbHelper db)
        {
            _db = db;
        }

        #region Login Method
        public LoginResult Login(LoginViewModel model)
        {
            LoginResult result = new LoginResult();

            SqlParameter[] loginParam =
            {
               new SqlParameter("@UserName", model.UserName),
               new SqlParameter("@Pssword", model.Password)
            };

            DataTable loginDt = _db.GetDataTable("HRMS_1202.dbo.CheckUserNameAndPassword", loginParam);

            if (loginDt.Rows.Count == 0)
            {
                result.Status = false;
                result.Message = "Invalid username or password";
                return result;
            }

            int employeeRowId = Convert.ToInt32(loginDt.Rows[0]["UserRowId"]);

            result.UserRowId = employeeRowId;
            result.UserName = Convert.ToString(loginDt.Rows[0]["UserName"]);

            SqlParameter[] menuParam =
            {
              new SqlParameter("@RowId", employeeRowId)
            };

            string menuQuery = @"SELECT
                        Nav2009_CustomerDetails_FormDisplay,
                        Nav2009_VendorDetails_FormDiaplsy,
                        Nav2009_FixedAssetDetails_FormDisplay,
                        Nav2009_Item_FormDisplay
                             FROM HRMS_1202.dbo.Master_UserMaster WHERE RowId = @RowId";

            DataTable menuDt = _db.GetDataTable(menuQuery, menuParam, false);

            result.Status = true;
            result.Message = "Login successful";

            if (menuDt.Rows.Count > 0)
            {
                DataRow row = menuDt.Rows[0];

                result.MenuList = new MenuPermissionModel
                {
                    CustomerDetails = Convert.ToBoolean(row["Nav2009_CustomerDetails_FormDisplay"]),
                    VendorDetails = Convert.ToBoolean(row["Nav2009_VendorDetails_FormDiaplsy"]),
                    FADetails = Convert.ToBoolean(row["Nav2009_FixedAssetDetails_FormDisplay"]),
                    ItemMaster = Convert.ToBoolean(row["Nav2009_Item_FormDisplay"])
                };
            }

            return result;
        }
        #endregion

    }
}
