namespace DynamicaLabs.XrmTools.Core
{
    public class SystemUser
    {
        public string LogicalName => "systemuser";

        public string UserName { get; set; }
        public string Password { get; set; }
        public string Id { get; set; }

        public override string ToString()
        {
            return $"[{Id}, {UserName}:{Password}]";
        }
    }
}