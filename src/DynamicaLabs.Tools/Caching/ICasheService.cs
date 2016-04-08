namespace DynamicaLabs.Tools.Caching
{
    public interface ICasheService
    {
        TObject Get<TObject>(string key);
        void Set<TObject>(string key, TObject data);
    }
}