using System;
using System.Web.Http.Filters;

namespace DynamicaLabs.WebTools
{
    public class LogString
    {
        public uint Weight { get; set; }
        public Func<HttpActionExecutedContext, string> Generator;
    }
}