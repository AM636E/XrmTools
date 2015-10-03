using System;

namespace DynamicaLabs.XrmTools.Core.Exceptions
{
    public class XrmServiceException : Exception
    {
        public XrmServiceException(string message) : base(message)
        {
        }

        public XrmServiceException(string message, params object[] sParams) : base(string.Format(message, sParams))
        {
        }
    }
}