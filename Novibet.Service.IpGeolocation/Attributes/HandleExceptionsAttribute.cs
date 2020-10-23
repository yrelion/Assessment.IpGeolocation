using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Novibet.Service.IpGeolocation.Common.Models;
using Novibet.Service.IpGeolocation.Models;

namespace Novibet.Service.IpGeolocation.Attributes
{
    public class HandleExceptionsAttribute : Attribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;

            // Catch-all exception handler
            if (!(exception is ApiException))
            {
                var genericMessage = new UnknownErrorException().Message;
                BuildErrorModel(context, genericMessage);
                return;
            }

            // Handle ApiException only
            BuildErrorModel(context, exception.Message);
        }

        private void BuildErrorModel(ExceptionContext context, string message)
        {
            var response = new ApiErrorResponse
            {
                TraceId = context.HttpContext.TraceIdentifier,
                Messages = new List<string> { message }
            };

            context.Result = new BadRequestObjectResult(response);
        }
    }
}
