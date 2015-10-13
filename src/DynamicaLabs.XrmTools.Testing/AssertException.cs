using System;

namespace DynamicaLabs.XrmTools.Testing
{
    public class AssertException : Exception
    {
        public AssertException(string s) : base(s)
        {
        }
    }
}