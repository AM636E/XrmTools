using System;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;

namespace DynamicaLabs.XrmTools.Data
{
    public class FetchXmlQueryConverter : IQueryConverter
    {
        private readonly DefaultEntityRepository _defaultEntityRepository;
        public FetchXmlQueryConverter(DefaultEntityRepository defaultEntityRepository)
        {
            _defaultEntityRepository = defaultEntityRepository;
        }

        public QueryBase ConvertQuery(QueryBase query)
        {
            if (!(query is QueryExpression))
                throw new ArgumentException("Query must be QueryExpressions", "query");
            var q = (QueryExpression)query;

            var conversionRequest = new QueryExpressionToFetchXmlRequest
            {
                Query = q
            };

            var service = _defaultEntityRepository.GetOrganizationService();

            var result = (QueryExpressionToFetchXmlResponse)service.Execute(conversionRequest);

            return new FetchExpression(result.FetchXml);
        }
    }
}