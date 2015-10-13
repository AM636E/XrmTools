using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using NLog;
using static System.String;

namespace DynamicaLabs.WebTools.Filters
{
    public class LoggingFilter : IExceptionFilter, IActionFilter
    {
        private readonly ILogGenerator _logGenerator;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public LoggingFilter(): this(new LogGenerator())
        { }

        public LoggingFilter(ILogGenerator logGenerator)
        {
            _logGenerator = logGenerator;
        }

        public LoggingFilter(Func<HttpActionExecutedContext, string> additionString) :
            this(new LogGenerator(additionString))
        { }

        public Task<HttpResponseMessage> ExecuteActionFilterAsync(HttpActionContext actionContext,
            CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            return continuation.Invoke();
        }

        public bool AllowMultiple => false;

        public Task ExecuteExceptionFilterAsync(HttpActionExecutedContext actionExecutedContext,
            CancellationToken cancellationToken)
        {
            Logger.Error(Join("${newline}\r\n", _logGenerator.GenerateLogMessage(actionExecutedContext)));

            var task = new Task<HttpResponseMessage>(() =>
            {
                var data = new
                {
                    message = actionExecutedContext.Exception.Message
                };
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new ObjectContent(typeof(object), data, new JsonMediaTypeFormatter())
                };
                return response;
            });
            task.RunSynchronously();
            return task;
        }
    }
}