using CustomerOnboarding.Domain.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Domain.Model
{
    public class OTPModel : ManagerBase<OTPModel>
    {
        private readonly AppDbContext _context;

        public OTPModel (AppDbContext context) : base (context)
        {
            _context = context;
        }
        public long OTPId { get; set; } 
        public int OTPAttempts { get; set; }
        public int OTPResets { get; set; }
        public string OTP { get; set; }
        public DateTime OTPSentTime { get; set; }
        public bool OTPSent { get; set; }
        public bool OTPUsed { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime ExpirationTime  { get; set; }

    

        public string SetOTP(string phoneNumber)
        {
            
            var code = new Random().Next(100000, 999999).ToString();
            OTP = code;
            OTPSentTime = DateTime.Now;
            OTPSent = true;
            PhoneNumber = phoneNumber;
            ExpirationTime = DateTime.Now.AddMinutes(15);
            return code;

        }

        public bool HasExpired()
        {
            if (DateTime.Now > ExpirationTime)
            {
                return true;
            }

            return false;
        }

        public async Task<ResponseModel<string>> ValidateOTP(string phoneNumber, string otp)
        {

            bool response;
            var verify =  _context.OTPModels.Where(e => e.PhoneNumber == phoneNumber && e.OTP == otp).FirstOrDefault();
            if (verify == null)
            {
                return new ResponseModel<string> { Message = "Oops, looks like you entered an invalid OTP. Please try again.", Status = false };

            }
            if (verify.OTPUsed == true)
            {
                return new ResponseModel<string> { Message = "OTP has been used", Status = false };
            }
            if (verify.HasExpired())
            {
                verify.OTPUsed = false;
                response = await Schema<OTPModel>().UpdateAsync(verify);
                return new ResponseModel<string> { Message = "OTP has expired", Status = false };
            }
            verify.OTPUsed = false;

            response = await Schema<OTPModel>().UpdateAsync(verify);
            return new ResponseModel<string>
            {
                Data = verify.PhoneNumber,
                Status = response,
                Message = response ? "OTP has expired" : "Oops something went wrong while completing email verification"
            };

            //OTPAttempts++;
            //if (otp != OTP)
            //{
            //    return false;
            //}
            //OTPUsed = true;
            //return true;
        }

    }
}
