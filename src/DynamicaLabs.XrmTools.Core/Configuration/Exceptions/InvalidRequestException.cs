using System;

namespace DynamicaLabs.XrmTools.Core.Configuration.Exceptions
{
    class InvalidRequestException : Exception
    {
        public InvalidRequestException(string message) : base(message) { }
    }
}
