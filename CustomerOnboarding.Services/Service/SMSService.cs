using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Services.Service
{
    public class SMSService
    {
        SMSSettings _smsSettings;
        public SMSService(SMSSettings smsSettings)
        {
            _smsSettings = smsSettings;
        }
        public async Task SendSMS(string phoneNumber, string text)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(_smsSettings.BasURL);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("App", _smsSettings.APIKey);

            string SENDER = "OTP Verification";

            string message = $@"
            {{
                ""messages"": [
                {{
                    ""from"": ""{SENDER}"",
                    ""destinations"":
                    [
                        {{
                            ""to"": ""{phoneNumber}""
                        }}
                  ],
                  ""text"": ""{text}""
                }}
              ]
            }}";

            HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Post, "sms/phoneotp/send");
            httpRequest.Content = new StringContent(message, Encoding.UTF8, "application/json");

            var response = await client.SendAsync(httpRequest);
            var responseContent = await response.Content.ReadAsStringAsync();

        }
    }
}
