using CustomerOnboarding.Domain.Helpers;
using CustomerOnboarding.Domain.Model.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Domain.Model
{
    public class Customer
    {
        [Key]
        [Required]
        public long CustomerId { get; set; }

        [Required]
        public long UserId { get; set; }

        public ApplicationUser User { get; set; }
        public string EmailAddress { get; set; }
     public string StateId { get; set; }
        public string LocalGovernmentId { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateRegistered { get; set; }
        public DateTime LastModified { get; set; }
        public bool isVerified { get; set; }

        public static Customer CreateCustomer(CustomerRegistrationDTO customer)
        {
            var newCustomer = Utilities.MapTo<Customer>(customer);
            newCustomer.DateRegistered = DateTime.Now;
            newCustomer.LastModified = DateTime.Now;
            return newCustomer;
        }


    }
}
