using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Helpers
{
    public class Response<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public T? Payload { get; set; }

        public bool IsSuccess { get; set; }


        public static Response<T> Send(bool Success, string message, HttpStatusCode statusCode = HttpStatusCode.OK, dynamic payload = null)
        {
            return new Response<T>
            {
                IsSuccess = Success,
                Message = message,
                Payload = (T)payload,
                Status = Success ? "Success" : "Failed",
                StatusCode = statusCode
            };
        }

    }
}
