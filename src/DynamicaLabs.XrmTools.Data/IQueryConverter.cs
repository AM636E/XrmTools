using Microsoft.Xrm.Sdk.Query;

namespace DynamicaLabs.XrmTools.Data
{
    public interface IQueryConverter
    {
        /// <summary>
        /// Converts one query type to another.
        /// Like QueryExpression to FetchXmlQuery
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        QueryBase ConvertQuery(QueryBase query);
    }
}