﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Helpers
{
    public class GetBankServiceResponseModel
    {

            public Result result { get; set; }
            public string errorMessage { get; set; }
            public string[] errorMessages { get; set; }
            public bool hasError { get; set; }
            public DateTime timeGenerated { get; set; }
        }

        public class Result
        {
            public string bankName { get; set; }
            public string bankCode { get; set; }
        }

    }

