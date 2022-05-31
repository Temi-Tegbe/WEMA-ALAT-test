using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Domain.Helpers
{
   
        public class ResponseModel
        {
            public string Message { get; set; }
            public bool Status { get; set; }
        }

        public class ResponseDataModel
        {
            public string ResponseMessage { get; set; }
            public bool Status { get; set; }
        }

        public class ResponseModel<T> : ResponseModel
        {
            public T Data { get; set; }
        }

      
    }

