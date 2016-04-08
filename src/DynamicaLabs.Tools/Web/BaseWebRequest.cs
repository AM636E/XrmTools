using System.Collections.Generic;
using System.Net;

namespace DynamicaLabs.Tools.Web
{
    public abstract class BaseWebRequest : IWebRequest
    {
        public abstract string Execute(string url, IDictionary<HttpRequestHeader, string> headers, string method = "GET");

        public virtual string Execute(string url, string method = "GET")
        {
            return Execute(url, new Dictionary<HttpRequestHeader, string>(), method);
        }
    }
}