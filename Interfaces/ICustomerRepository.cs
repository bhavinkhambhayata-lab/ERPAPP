using ERPAPP.Models;

namespace ERPAPP.Interfaces
{
    public interface ICustomerRepository
    {
        Task<GetCustomerAddModel> GetCustomerAddData();
        Task<int> GetCustomerTransferNewNo();
        Task<List<CustomerSearchModel>> SearchCustomer(string searchText);
    }
}
