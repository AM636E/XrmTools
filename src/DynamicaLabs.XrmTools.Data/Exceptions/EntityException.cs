using System;

namespace DynamicaLabs.XrmTools.Data.Exceptions
{
    public class EntityException : Exception
    {
        public EntityException()
        { }

        public EntityException(string message) : base(message)
        { }

        public EntityException(string message, string operation) 
            : this(String.Format("Error while {0} of entity: {1}", operation, message))
        { }
    }
}