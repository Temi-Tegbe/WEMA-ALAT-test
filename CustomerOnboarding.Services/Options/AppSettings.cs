using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Services.Options
{
    public class AppSettings
    {
        public JWT JWT { get; set; }
        public GetBankServiceConfiguration getBankServiceConfiguration { get; set; }    
    }

    public class JWT
    {
        public string ValidAudience { get; set; }
        public string ValidIssuer { get; set; }
        public string Secret { get; set; }
    }

    public class GetBankServiceConfiguration
    {
        public string BaseUrl { get; set; }
        public string Auth { get; set; }
    }
}
