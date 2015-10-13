using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Filters;
using Newtonsoft.Json;
using static System.String;

namespace DynamicaLabs.WebTools
{
    public class LogGenerator : List<Func<HttpActionExecutedContext, string>>, ILogGenerator
    {
        private static readonly List<Func<HttpActionExecutedContext, string>> AdditionalStrings = new List
            <Func<HttpActionExecutedContext, string>>
        {
            // Exception type, place and message.
            context =>
                $"\r\n{context.Exception.GetType().FullName} in {context.ActionContext.ControllerContext.Controller} : {context.Exception.Message}",
            // Display incoming data.
            context =>
                $@"Parameters: {Join(",${newline}\r\n",
                    context.ActionContext.ActionArguments.Select(
                        a => $"\"{a.Key}\" = {JsonConvert.SerializeObject(a.Value, Formatting.Indented)}"))}",
            // Stack trace.
            context => $"$\r\nStack Trace: {context.Exception.StackTrace}"
        };

        public LogGenerator()
        {
            AdditionalStrings.ForEach(Add);
        }

        public LogGenerator(List<Func<HttpActionExecutedContext, string>> additionalStrings) :
            this()
        {
            additionalStrings.ForEach(s => AdditionalStrings.Add(s));
        }

        public LogGenerator(Func<HttpActionExecutedContext, string> additionString) :
            this(new List<Func<HttpActionExecutedContext, string>> {additionString})
        {
        }

        public string GenerateLogMessage(HttpActionExecutedContext actionExecutedContext)
            => Join("\n", AdditionalStrings.Select(a => a(actionExecutedContext)));
    }
}