using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;
using static System.String;

namespace DynamicaLabs.WebTools.Handlers
{
    /// <summary>
    ///     Handler that format exception message.
    /// </summary>
    public class CustomExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            var tmp = context.Exception;
            var messages = new List<string>();
            while (tmp != null)
            {
                messages.Add(tmp.Message);
                tmp = tmp.InnerException;
            }
            var result = new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new ObjectContent<ErrorResponse>(new ErrorResponse
                {
                    Message = Join(" -> ", messages)
                }, new JsonMediaTypeFormatter())
            });

            if (context.Exception is NullReferenceException)
                ((ErrorResponse) ((ObjectContent<ErrorResponse>) result.Response.Content).Value).Message =
                    "Empty request(NullReferenceException)";

            context.Result = result;
        }

        private class ErrorResponse
        {
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public string Message { get; set; }
        }
    }
}