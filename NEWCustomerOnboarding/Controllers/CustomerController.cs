using CustomerOnboarding.Domain.Model;
using CustomerOnboarding.Domain.Model.DTO;
using CustomerOnboarding.Helpers;
using CustomerOnboarding.Services.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NEWCustomerOnboarding.Controllers
{

    [Route("api/[controller]")]
    [ApiController]

    public class CustomerController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly CustomerService _customerService;
        private readonly AppDbContext _context;
        private readonly Audit _audit;

        public CustomerController(IConfiguration configuration, CustomerService customerService, AppDbContext context, Audit audit, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _configuration = configuration;
            _customerService = customerService;
            _context = context;
            _audit = audit;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost]
        [Route("list-all-Customers")]
        [Produces(typeof(ResponseWrapper<PagedQueryResult<Customer>>))]
        public async Task<PagedQueryResult<Customer>> GetAllCustomers(PagedQueryRequest request)
        {
            try
            {


                var allCustomers = await _customerService.GetAllCustomers(request);
                return allCustomers;
            }
            catch (Exception ex)
            {
                _audit.LogFatal(ex);
                throw ex;
            }
        }

        [HttpPost]
        [Route("register-customer")]
        //[Authorize(Roles = "SuperAdmin")]
        [Produces("application/json", "application/xml", Type = typeof(Response<dynamic>))]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerRegistrationDTO model)
        {
            Response<dynamic> responseBody = new Response<dynamic>();

            ApplicationUser adminExist = await _userManager.FindByEmailAsync(model.EmailAddress);
            if (adminExist != null)
            {
                responseBody.Message = "An Administrator with this Email Address already exist";
                responseBody.Status = "Failed";
                responseBody.Payload = null;
                return Conflict(responseBody);
            }

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.EmailAddress,
                UserName = model.EmailAddress,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            IdentityResult result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                responseBody.Message = "Administrator registration was not successful. Please try again.";
                responseBody.Status = "Failed";
                responseBody.Payload = null;
                return BadRequest(responseBody);
            }

            await _customerService.AddAsync(model, user);

            if (!await _roleManager.RoleExistsAsync("Admin"))
                await _roleManager.CreateAsync(new ApplicationRole() { Name = "Admin" });

            if (await _roleManager.RoleExistsAsync("Admin"))
                await _userManager.AddToRoleAsync(user, "Admin");

            responseBody.Message = $"An Admin with email {user.UserName} has been provisioned";
            responseBody.Status = "Success";
            responseBody.Payload = null;
            return Created("", responseBody);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("authorize-admin")]
        //[Authorize(Roles = "Admin")]
        [Produces("application/json", "application/xml", Type = typeof(Response<long>))]
        public async Task<IActionResult> AuthorizeCustomer([FromBody] CustomerLoginDTO request)
        {
            var response = await _customerService.Login(request);
            return Ok(response);
            //return response.IsSuccess? Ok(response): StatusCode((int)response.StatusCode, response);
        }
    }
}
