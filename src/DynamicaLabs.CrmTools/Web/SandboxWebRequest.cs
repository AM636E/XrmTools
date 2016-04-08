using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using DynamicaLabs.Tools.Logging;
using DynamicaLabs.Tools.Web;

namespace DynamicaLabs.CrmTools.Web
{

    /// <summary>
    /// Web request with no use of System.Web 
    /// so it's safe to use in sandboxed environments ( like crm online ).
    /// </summary>
    public sealed class SandboxWebRequest : BaseWebRequest
    {
        private readonly ILogger _logger;
        private readonly int _tries;

        /// <summary>
        /// Initializes a new instance of SandboxWebRequest
        /// </summary>
        /// <param name="logger">Logger to use.</param>
        /// <param name="tries">Count of tries before exception.</param>
        public SandboxWebRequest(ILogger logger, int tries = 5)
        {
            _logger = logger;
            _tries = tries;
        }

        private string ExecuteInternal(string url, IDictionary<HttpRequestHeader, string> headers, string method = "GET", int tries = 3)
        {
            // TODO: Implement other HTTP verbs (POST, PUT, DELETE).
            var exc = new Exception("Failed to execute request.");
            for (var i = 0; i < tries; i++)
            {
                try
                {
                    using (var client = new WebClient())
                    {
                        var hheaders = new WebHeaderCollection();
                        foreach (var header in headers)
                        {
                            hheaders.Add(header.Key, header.Value);
                        }
                        client.Headers = hheaders;
                        var response = client.DownloadData(url);
                        var sresp = Encoding.UTF8.GetString(response);
                        return sresp;
                    }
                }
                catch (WebException ex)
                {
                    var str = string.Empty;
                    var r = ex.Response?.GetResponseStream();
                    if (r != null)
                    {
                        using (var reader = new StreamReader(r))
                        {
                            str = reader.ReadToEnd();
                        }
                        ex.Response.Close();
                    }
                    if (ex.Status == WebExceptionStatus.Timeout)
                    {
                        _logger.LogError($"The timeout elapsed while attempting to issue the request.: {ex.Message}");
                        exc = new WebException("The timeout elapsed while attempting to issue the request.", ex);
                    }
                    else
                    {
                        _logger.LogError(
                            $"A Web exception occurred while attempting to issue the request. {ex.Message}: {str}");
                        exc =
                            new WebException(
                                $"A Web exception occurred while attempting to issue the request. {ex.Message}: {str}",
                                ex);
                    }
                }
                Thread.Sleep(500);
            }

            throw exc;
        }

        public override string Execute(string url, IDictionary<HttpRequestHeader, string> headers, string method = "GET")
        {
            try
            {
                return ExecuteInternal(url, headers, method, _tries);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while executing web request: {url} {ex.Message}");
                throw new WebException(ex.Message, ex);
            }
        }
    }
}