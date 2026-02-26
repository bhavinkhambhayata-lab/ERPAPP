using ERPAPP.Interfaces;
using Microsoft.AspNetCore.Mvc;

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


    }
}
