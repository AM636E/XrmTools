using System.Collections.Generic;
using System.Net;

namespace DynamicaLabs.Tools.Web
{
    /// <summary>
    /// Represents web request.
    /// </summary>
    public interface IWebRequest
    {
        /// <summary>
        /// Execute web request.
        /// </summary>
        /// <param name="url">Destination url.</param>
        /// <param name="headers">Headers.</param>
        /// <param name="method">Http verb</param>
        /// <returns>String result of request.</returns>
        string Execute(string url, IDictionary<HttpRequestHeader, string> headers, string method = "GET");
        string Execute(string url, string method = "GET");
    }
}