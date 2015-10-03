namespace DynamicaLabs.XrmTools.Data
{
    public interface IXrmConnectionStringProvider
    {
        string GetUsername();
        string GetPassword();
        string GetUrl();
    }
}