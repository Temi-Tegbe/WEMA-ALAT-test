using CustomerOnboarding.Helpers;
using CustomerOnboarding.Services.Options;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Services.Service
{
    public class GetBankService
    {
        private readonly AppSettings _appSettings;
        
        public GetBankService(IOptions<AppSettings> options)
        {
            _appSettings = options.Value;
        }

        public List<GetBankServiceResponseModel> GetBanks()
        {
            List<GetBankServiceResponseModel> responseModel = null;
            var client = new RestClient(_appSettings.getBankServiceConfiguration.BaseUrl);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Ocp-Apim-Subscription-Key", _appSettings.getBankServiceConfiguration.Auth);
            IRestResponse response = (IRestResponse)client.ExecuteAsync(request);
            if (!response.IsSuccessful)
            {
                return responseModel;
            }

            //responseModel = JsonConvert.DeserializeObject<GetBankServiceResponseModel>(response.Content);
            responseModel = JsonConvert.DeserializeObject<List<GetBankServiceResponseModel>>(response.Content);
           //responseModel = JsonConvert.DeserializeObject<List<GetBankServiceResponseModel>>(response.Content);
            return responseModel;
        }
    }
}
