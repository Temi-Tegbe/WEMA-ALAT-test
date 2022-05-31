using CustomerOnboarding.Domain.Model;
using CustomerOnboarding.Helpers;
using CustomerOnboarding.Services.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NEWCustomerOnboarding.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetBankController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly GetBankService _getBankService;
        public GetBankController(IConfiguration configuration, GetBankService getBankService, AppDbContext context)
        {
            _context = context;
            _getBankService = getBankService;
        }

        [HttpGet]
        [Route("list-all-banks")]
        public List<GetBankServiceResponseModel> GetAllCustomers()
        {
            try
            {
                var allBanks =  _getBankService.GetBanks();
                return allBanks;
            }
            catch (System.Exception)
            {

                throw;
            }
        }
    }
}
