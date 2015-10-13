using System;

namespace DynamicaLabs.XrmTools.Core.Configuration.Exceptions
{
    internal class InvalidRequestException : Exception
    {
        public InvalidRequestException(string message) : base(message)
        {
        }
    }
}