using CustomerOnboarding.Domain.Model;
using CustomerOnboarding.Domain.Model.DTO;
using CustomerOnboarding.Services.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace NEWCustomerOnboarding.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SMSController : ControllerBase
    {
        public CustomerService _customerRepository;
        private readonly OTPModel _otpModel;
        private readonly SMSService _smsService;
        private readonly AppDbContext _context;

        public SMSController(CustomerService customerService, OTPModel otpModel, SMSService smsService, AppDbContext context)
        {
            _customerRepository = customerService;
            _otpModel = otpModel;
            _smsService = smsService;
            _context = context;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SendSMS([FromBody] SMSNotificationDTO smsnotification)
        {
            await _smsService.SendSMS(smsnotification.Recepient, smsnotification.Message);
            return Ok("SMS was sent successfully");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> GenerateOTP([FromBody] ValidateOtpDTO newOTP)
        {
            var customer = await _context.Customers.Where(x => x.CustomerId == newOTP.CustomerId).FirstOrDefaultAsync();
            var request = await _otpModel.ValidateOTP(customer.PhoneNumber, newOTP.Otp);
            if (!request.Status)
            {
                return BadRequest(request.Message);
            }
            await _smsService.SendSMS(customer.PhoneNumber, $"Hello, pleaseuse the following OTP {request.Data} to create your verify your phone number.");
            return Ok($"An OTP has been sent to {customer.PhoneNumber.Substring(0, 7)}******");
        }
    }
}
