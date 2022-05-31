using CustomerOnboarding.Domain.Helpers;
using CustomerOnboarding.Domain.Model;
using CustomerOnboarding.Domain.Model.DTO;
using CustomerOnboarding.Helpers;
using CustomerOnboarding.Services.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Services.Service
{
    public class CustomerService
    {

        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly AppSettings appSettings;
        private readonly OTPModel _otpModel;

        public CustomerService(AppDbContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IConfiguration configuration, IOptions<AppSettings> options, OTPModel otpModel)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            appSettings = options.Value;
            _otpModel = otpModel;

        }

        public async Task<Response<dynamic>> AddAsync(CustomerRegistrationDTO customer, ApplicationUser userInfo)
        {
            var existing = await GetCustomerDetails(customer.EmailAddress);
            if (existing != null)
            {
                return Response<dynamic>.Send(false, "User already exists", HttpStatusCode.Conflict);
            }
            var newCustomer = Customer.CreateCustomer(customer); ;
            await _context.Customers.AddAsync(
                new Customer
                {
                    
                    EmailAddress = customer.EmailAddress,
                    DateRegistered = DateTime.Now,
                    StateId = customer.StateId,
                    LocalGovernmentId = customer.LocalGovernmentId,
                    PhoneNumber = customer.PhoneNumber,
                    User = userInfo


                }
                );
            await _context.SaveChangesAsync();
            return Response<dynamic>.Send(true, "Added Successfully");
        }

        public async Task<Response<dynamic>> Login(CustomerLoginDTO login)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(login.EmailAddress);
            if (user != null && await _userManager.CheckPasswordAsync(user, login.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim (ClaimTypes.Name, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

                };
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));

                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.JWT.Secret));
                var token = new JwtSecurityToken(
                    issuer: appSettings.JWT.ValidIssuer,
                    audience: appSettings.JWT.ValidAudience,
                    expires: DateTime.Now.AddMinutes(120),
                     claims: authClaims,
                     signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                     );
                return Response<dynamic>.Send(true, "Logged in Suucessfully", payload: new { token = new JwtSecurityTokenHandler().WriteToken(token) });

            }

            return Response<dynamic>.Send(false, "Log in failed", HttpStatusCode.Unauthorized);
        }

        public async Task<Customer> GetCustomerDetails(string login)
        {
            var result = await _context.Customers.FirstOrDefaultAsync(x => x.EmailAddress.ToLower() == login.ToLower());
            return result;
        }

        public async Task<PagedQueryResult<Customer>> GetAllCustomers(PagedQueryRequest request)
        {
            var allCustomers =  _context.Customers;
            return allCustomers.ToPagedResult(request.PageNumber, request.PageSize);
        }
      
        //public async Task<Customer> OTP(long customerId, string email)
        //{
        //    var customer = await _context.Customers.Where(x => x.CustomerId == customerId).FirstOrDefaultAsync();
        //    if (customer != null)
        //    {
        //        var otp = _otpModel.SetOTP();
        //        await _context.SaveChangesAsync();

        //    }
        //}

        //public async Task<ResponseModel<string>> ValidateOTP(long customerId, string otp)
        //{
        //    var customer = await _context.Customers.Where(x => x.UserId == customerId).FirstOrDefaultAsync();
        //    if (customer == null)
        //    {
        //        return new ResponseModel<string> { Message = "Customer not found.", Status = false };
        //    }
        //    var sendOtp = _otpModel.SetOTP();
        //    _context.Update(sendOtp);
        //   await  _context.SaveChangesAsync();
          
        //}

    }
}
