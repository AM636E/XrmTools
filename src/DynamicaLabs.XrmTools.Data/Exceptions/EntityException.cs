using System;

namespace DynamicaLabs.XrmTools.Data.Exceptions
{
    public class EntityException : Exception
    {
        public EntityException()
        {
        }

        public EntityException(string message) : base(message)
        {
        }

        public EntityException(string message, string operation)
            : this($"Error while {operation} of entity: {message}")
        {
        }
    }
}