namespace DynamicaLabs.XrmTools.Core
{
    public interface ICrmAuthRepository
    {
        SystemUser FindUser(string userName, string password);
    }
}