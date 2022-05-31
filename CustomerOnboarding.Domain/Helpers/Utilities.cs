using CustomerOnboarding.Domain.Model.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Domain.Helpers
{
    public class Utilities
    {

        public static T MapTo<T>(dynamic data)
        {
            var dataString = JsonConvert.SerializeObject(data);
            return JsonConvert.DeserializeObject<T>(dataString);
        }

        public static List<PostHeader> GeneratePostHeaders(string key, string value, List<PostHeader> postHeaders = null)
        {
            if (postHeaders == null) postHeaders = new List<PostHeader>();
            var header = new PostHeader
            {
                key = key,
                value = value
            };
            postHeaders.Add(header);
            return postHeaders;
        }
    }
}
