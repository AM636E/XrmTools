namespace DynamicaLabs.XrmTools.Core
{
    public class SystemUser
    {
        public string LogicalName
        {
            get { return "systemuser"; }
        }

        public string UserName { get; set; }
        public string Password { get; set; }
        public string Id { get; set; }

        public override string ToString()
        {
            return string.Format("[{0}, {1}:{2}]", Id, UserName, Password);
        }
    }
}