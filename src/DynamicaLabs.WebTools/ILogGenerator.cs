using System;
using System.Collections.Generic;
using System.Web.Http.Filters;

namespace DynamicaLabs.WebTools
{
    public interface ILogGenerator : IList<Func<HttpActionExecutedContext, string>>
    {
        string GenerateLogMessage(HttpActionExecutedContext actionExecutedContext);
        void AddFirst(Func<HttpActionExecutedContext, string> func);
        void AddLast(Func<HttpActionExecutedContext, string> func);
    }
}