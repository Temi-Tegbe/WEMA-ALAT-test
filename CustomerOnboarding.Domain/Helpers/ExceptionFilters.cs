using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Domain.Helpers
{
    public class ExceptionFilters : ExceptionFilterAttribute
    {
        // private Audit _audit;

        //  public ExceptionFilters(Audit audit)
        // {
        //     _audit = audit;
        // }

        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            // _audit.LogFatal(exception);
            // TODO: log your exception here
            //var geenricMessage = "Dear customer, we tried processing your request. However, there seems to be a connectivity issue. We advise you try again shortly.";
            var geenricMessage = "Dear customer, we tried processing your request. However, there seems to be a connectivity issue. We advise you try again shortly.";
            //var result = new ObjectResult(Response<dynamic>.Send(false, geenricMessage, HttpStatusCode.InternalServerError));
            var result = new ObjectResult(context.Exception);
            result.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Result = result;
            context.ExceptionHandled = true; //optional 
        }
    }
}
