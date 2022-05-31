using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Helpers
{
    public class Audit
    {
        private readonly ILogger _logger;

        public Audit(ILogger<Audit> logger)
        {
            _logger = logger;
        }
        public void LogInfo(string message)
        {
            _logger.LogInformation(message);
        }
        public void LogError(string message)
        {
            _logger.LogWarning(message);
        }
        public void LogFatal(Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        public void LogRequest(string endpoint, string request, string idenitifer)
        {
            _logger.LogInformation($"Request: {endpoint} for [{idenitifer}] => {request}");
        }
        public void LogResponse(string endpoint, string response, string idenitifer)
        {
            _logger.LogInformation($"Response: {endpoint} for [{idenitifer}] => {response}");
        }
    }
}
