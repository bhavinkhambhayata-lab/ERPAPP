using System.ComponentModel.DataAnnotations;

namespace ERPAPP.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; }
    }

    public class LoginResult
    {
        public bool Status { get; set; }
        public MenuPermissionModel? MenuList { get; set; }
        public string Message { get; set; } = string.Empty;

        public int UserRowId { get; set; }
        public string UserName { get; set; } = string.Empty;
    }

    public class MenuPermissionModel
    {
        public bool CustomerDetails { get; set; }
        public bool VendorDetails { get; set; }
        public bool FADetails { get; set; }
        public bool ItemMaster { get; set; }
    }


}
