using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Helpers
{
    public class ResponseWrapper<T>
    {
        public T Data { get; }

        public string Message { get; }

        public List<string> Messages { get; }

        public bool HasMessage { get; }

        public DateTime TimeGenerated { get; }

        protected internal ResponseWrapper(T data, string message, bool hasMessage)
        {
            Data = data;
            Message = message;
            HasMessage = hasMessage;
            TimeGenerated = DateTime.UtcNow;
        }

        protected internal ResponseWrapper(T data, List<string> messages, bool hasMessage)
        {
            Data = data;
            Messages = messages;
            HasMessage = hasMessage;
            TimeGenerated = DateTime.UtcNow;
        }
    }

    public class ResponseWrapper : ResponseWrapper<string>
    {
        protected ResponseWrapper(List<string> errorMessages, bool hasError)
            : base(null, errorMessages, hasError)
        {
        }

        protected ResponseWrapper(string errorMessage, bool hasError)
            : base(null, errorMessage, hasError)
        {
        }

        public static ResponseWrapper<T> Ok<T>(T result)
        {
            string nullRes = null;

            return new ResponseWrapper<T>(result, nullRes, false);
        }

        public static ResponseWrapper Ok()
        {
            string nullRes = null;

            return new ResponseWrapper(nullRes, false);
        }

        public static ResponseWrapper Error(string errorMessage)
        {
            return new ResponseWrapper(errorMessage, true);
        }

        public static ResponseWrapper ErrorList(List<string> errorMessages)
        {
            return new ResponseWrapper(errorMessages, true);
        }

        public static ResponseWrapper Unauthorized(string message)
        {
            return new ResponseWrapper(message, true);
        }
    }
}
