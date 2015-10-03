using Microsoft.Xrm.Sdk;

namespace DynamicaLabs.XrmTools.Construction
{
    public interface IFieldHandler<in TField, out TResult>
    {
        TResult HandleField(TField fieldValue);
        TResult HandleField(TField field, Entity entity);
    }
}