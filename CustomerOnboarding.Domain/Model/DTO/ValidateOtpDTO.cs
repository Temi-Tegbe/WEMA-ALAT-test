using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Domain.Model.DTO
{
    public class ValidateOtpDTO
    {
        public string Otp { get; set; }
        public long CustomerId { get; set; }
    }
}
