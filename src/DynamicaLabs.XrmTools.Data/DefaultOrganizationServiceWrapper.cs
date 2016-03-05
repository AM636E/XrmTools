using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.ServiceModel.Description;
using DynamicaLabs.XrmTools.Construction;
using DynamicaLabs.XrmTools.Core;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Client;
using Microsoft.Xrm.Client.Services;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;

namespace DynamicaLabs.XrmTools.Data
{
    public class DefaultOrganizationServiceWrapper : IOrganizationServiceWrapper
    {
        private OrganizationService _service;
        private readonly string _connectionString;
        private IEntityConstructor _entityConstructor;
        private readonly string _password;
        private readonly string _uri;
        private readonly string _userName;

        public DefaultOrganizationServiceWrapper() { }

        public DefaultOrganizationServiceWrapper(IXrmConnectionStringProvider connectionStringProvider,
            IEntityConstructor entityConstructor)
        {
            _entityConstructor = entityConstructor;
            _uri = connectionStringProvider.GetUrl();
            _userName = connectionStringProvider.GetUsername();
            _password = connectionStringProvider.GetPassword();
            _connectionString = $"Url={_uri}; Username={_userName}; Password={_password}";
        }
        
        public IEnumerable<Entity> GetAccessableEntities(QueryBase query, Guid userId)
        {
            using (var proxy = CreateProxy())
            {
                proxy.CallerId = userId;

                return proxy.RetrieveMultiple(query).Entities;
            }
        }

        public virtual Entity GetEntityById(string entityType, Guid id)
        {
            using (var service = CreateOrganizationService())
            {
                return service.Retrieve(entityType, id, new ColumnSet(true));
            }
        }

        public Entity GetEntityById(string entityType, Guid id, Guid userGuid)
        {
            using (var proxy = CreateProxy())
            {
                proxy.CallerId = userGuid;

                return proxy.Retrieve(entityType, id, new ColumnSet(true));
            }
        }

        public T GetEntityById<T>(string entityType, Guid id)
        {
            using (var service = CreateOrganizationService())
            {
                return _entityConstructor.ConstructObject<T>(
                    service.Retrieve(
                        entityType,
                        id,
                        _entityConstructor.CreateColumnSet<T>()));
            }
        }

        public T GetEntityById<T>(string entityType, Guid id, Guid userGuid)
        {
            using (var proxy = CreateProxy())
            {
                proxy.CallerId = userGuid;
                return _entityConstructor.ConstructObject<T>(
                    proxy.Retrieve(
                        entityType,
                        id,
                        _entityConstructor.CreateColumnSet<T>()));
            }
        }

        public IEnumerable<T> GetEntities<T>(QueryBase query)
        {
            using (var service = CreateOrganizationService())
            {
                var columnset = _entityConstructor.CreateColumnSet<T>();
                var isQe = false;
                // ReSharper disable CanBeReplacedWithTryCastAndCheckForNull
                if (query is QueryExpression)
                {
                    isQe = true;
                    ((QueryExpression)query).ColumnSet = columnset;
                }
                else if (query is QueryByAttribute) { ((QueryByAttribute)query).ColumnSet = columnset; }
                if (Limit > 0)
                {
                    if (isQe) ((QueryExpression)query).TopCount = Limit;
                    else ((QueryByAttribute)query).TopCount = Limit;
                }
                // ReSharper restore CanBeReplacedWithTryCastAndCheckForNull
                var entities = service.RetrieveMultiple(query).Entities;

                return entities.Select(e => _entityConstructor.ConstructObject<T>(e));
            }
        }

        public IEnumerable<T> GetEntities<T>(QueryBase query, Guid userGuid)
        {
            using (var service = CreateProxy())
            {
                service.CallerId = userGuid;
                var columnset = _entityConstructor.CreateColumnSet<T>();
                // ReSharper disable CanBeReplacedWithTryCastAndCheckForNull
                var isQe = false;
                // ReSharper disable CanBeReplacedWithTryCastAndCheckForNull
                if (query is QueryExpression)
                {
                    isQe = true;
                    ((QueryExpression)query).ColumnSet = columnset;
                }
                else if (query is QueryByAttribute) { ((QueryByAttribute)query).ColumnSet = columnset; }
                if (Limit > 0)
                {
                    if (isQe) ((QueryExpression)query).TopCount = Limit;
                    else ((QueryByAttribute)query).TopCount = Limit;
                }
                // ReSharper restore CanBeReplacedWithTryCastAndCheckForNull
                var entities = service.RetrieveMultiple(query).Entities;

                return entities.Select(e => _entityConstructor.ConstructObject<T>(e));
            }
        }

        public int Limit { get; set; }

        public IEnumerable<Entity> GetEntities(string entityType, ColumnSet columnSet)
        {
            return GetEntities(entityType, new QueryExpression(entityType) { ColumnSet = columnSet });
        }

        public IEnumerable<Entity> GetEntities(string entityType, QueryBase query)
        {
            using (var service = CreateOrganizationService())
            {
                // ReSharper disable CanBeReplacedWithTryCastAndCheckForNull
                if (query is QueryExpression && Limit > 0) { ((QueryExpression)query).TopCount = Limit; }
                else if (query is QueryByAttribute && Limit > 0) { ((QueryByAttribute)query).TopCount = Limit; }
                return service.RetrieveMultiple(query).Entities;
            }
        }

        public IEnumerable<Entity> GetAccessableEntities(string entityType, string userId, ColumnSet columnSet)
        {
            var credentials = new ClientCredentials();
            credentials.Windows.ClientCredential = CredentialCache.DefaultNetworkCredentials;
            credentials.Windows.AllowedImpersonationLevel = TokenImpersonationLevel.Impersonation;
            credentials.UserName.UserName = _userName;
            credentials.UserName.Password = _password;
            using (var proxy = CreateProxy())
            {
                proxy.CallerId = Guid.Parse(userId);
                var query = new QueryExpression(entityType) { ColumnSet = columnSet};
                if (Limit > 0) query.TopCount = Limit;
                // Retrieve only active entities.
                if (entityType != "systemuser")
                {
                    query.Criteria.AddCondition("statecode", ConditionOperator.Equal, 0);
                }
                return proxy.RetrieveMultiple(query).Entities;
            }
        }

        public virtual Entity UpdateEntity(string entityType, Entity entity)
        {
            using (var service = CreateOrganizationService())
            {
                if (string.IsNullOrEmpty(entity.LogicalName))
                {
                    entity.LogicalName = entityType;
                }
                var tmp = entity.Id;
                service.Update(entity);

                entity = service.Retrieve(entityType, tmp, new ColumnSet(true));
                return entity;
            }
        }

        public virtual Entity UpdateEntity(string entityType, Entity entity, Guid userId)
        {
            entity.LogicalName = entityType;
            using (var proxy = CreateProxy())
            {
                proxy.CallerId = userId;

                proxy.Update(entity);

                return GetEntityById(entityType, entity.Id);
            }
        }

        public TObject UpdateEntity<TObject>(TObject tobject)
        {
            var entity = _entityConstructor.ConstructEntity(tobject);
            return _entityConstructor.ConstructObject<TObject>(UpdateEntity(entity.LogicalName, entity));
        }

        public TObject UpdateEntity<TObject>(TObject tobject, Guid userId)
        {
            var entity = _entityConstructor.ConstructEntity(tobject);
            return _entityConstructor.ConstructObject<TObject>(UpdateEntity(entity.LogicalName, entity, userId));
        }

        public TObject CreateEntity<TObject>(TObject entity)
        {
            var crmEntity = GUtil(entity);
            return _entityConstructor.ConstructObject<TObject>(CreateEntity(crmEntity.LogicalName, crmEntity));
        }

        public TObject CreateEntity<TObject>(TObject entity, Guid userIGuid)
        {
            var crmEntity = GUtil(entity);
            return _entityConstructor.ConstructObject<TObject>(CreateEntity(crmEntity.LogicalName, crmEntity, userIGuid));
        }

        public virtual Entity CreateEntity(string entityType, Entity entity, Guid userId)
        {
            entity.LogicalName = entityType;
            using (var proxy = CreateProxy())
            {
                proxy.CallerId = userId;

                var e = proxy.Create(entity);

                return proxy.Retrieve(entityType, e, new ColumnSet(true));
            }
        }


        public virtual Entity CreateEntity(string entityType, Entity entity)
        {
            if (string.IsNullOrEmpty(entity.LogicalName))
            {
                entity.LogicalName = entityType;
            }
            using (var service = CreateOrganizationService())
            {
                entity.Id = service.Create(entity);

                return service.Retrieve(entityType, entity.Id, new ColumnSet(true));
            }
        }

        public virtual void DeleteEntity(string entityType, Guid entityGuid)
        {
            using (var context = CreateOrganizationService())
            {
                context.Delete(entityType, entityGuid);
            }
        }

        public void DeleteEntity(string entityType, Guid entityGuid, Guid userGuid)
        {
            using (var context = CreateProxy())
            {
                context.CallerId = userGuid;

                context.Delete(entityType, entityGuid);
            }
        }

        public virtual void DisableEntity(string entityType, Guid entityGuid)
        {
            using (var service = CreateOrganizationService())
            {
                service.Execute(new SetStateRequest
                {
                    EntityMoniker = new EntityReference(entityType, entityGuid),
                    State = new OptionSetValue(1),
                    Status = new OptionSetValue(-1)
                });
            }
        }

        public void DisableEntity(string entityType, Guid entityGuid, Guid userGuid)
        {
            using (var proxy = CreateProxy())
            {
                proxy.CallerId = userGuid;
                proxy.Execute(new SetStateRequest
                {
                    EntityMoniker = new EntityReference(entityType, entityGuid),
                    State = new OptionSetValue(1),
                    Status = new OptionSetValue(-1)
                });
            }
        }

        public IEnumerable<Entity> QueryByAttributes(string entityName, IDictionary<string, object> attributes,
            bool onlyActive = true, bool strict = true)
        {
            var expr = Utils.ToQueryExpression(entityName, attributes, strict);
            expr.ColumnSet = new ColumnSet(true);
            if (onlyActive)
            {
                expr.Criteria.AddCondition(new ConditionExpression("statecode", ConditionOperator.Equal, 0));
            }
            return GetEntities(entityName, expr);
        }

        public IEnumerable<Entity> QueryByAttributes(string entityName, IDictionary<string, object> attributes,
            Guid userId, bool onlyActive = true,
            bool strict = true)
        {
            var expr = Utils.ToQueryExpression(entityName, attributes, strict);
            expr.ColumnSet = new ColumnSet(true);
            if (onlyActive)
            {
                expr.Criteria.AddCondition(new ConditionExpression("statecode", ConditionOperator.Equal, 0));
            }
            return GetAccessableEntities(expr, userId);
        }

        public IEnumerable<TEntity> QueryByAttributes<TEntity>(string entityName, IDictionary<string, object> attributes,
            bool onlyActive = true, bool strict = true)
        {
            var expr = Utils.ToQueryExpression(entityName, attributes, strict);
            if (onlyActive)
            {
                expr.Criteria.AddCondition(new ConditionExpression("statecode", ConditionOperator.Equal, 0));
            }
            return GetEntities<TEntity>(expr);
        }

        public IEnumerable<TEntity> QueryByAttributes<TEntity>(string entityName, IDictionary<string, object> attributes,
            Guid userid, bool onlyActive = true, bool strict = true)
        {
            var expr = Utils.ToQueryExpression(entityName, attributes, strict);
            if (onlyActive)
            {
                expr.Criteria.AddCondition(new ConditionExpression("statecode", ConditionOperator.Equal, 0));
            }

            return GetEntities<TEntity>(expr);
        }

        private Entity GUtil<TObject>(TObject entity)
        {
            return _entityConstructor.ConstructEntity(entity);
        }

        protected OrganizationService CreateOrganizationService()
        {
            return _service ?? new OrganizationService(CrmConnection.Parse(_connectionString));
        }

        protected OrganizationServiceProxy CreateProxy()
        {
            var credentials = new ClientCredentials();
            credentials.Windows.ClientCredential = CredentialCache.DefaultNetworkCredentials;
            credentials.Windows.AllowedImpersonationLevel = TokenImpersonationLevel.Impersonation;
            credentials.UserName.UserName = _userName;
            credentials.UserName.Password = _password;
            return new OrganizationServiceProxy(new Uri(_uri + "/XRMServices/2011/Organization.svc"), null, credentials,
                null);
        }

        public IOrganizationService GetOrganizationService()
        {
            return CreateOrganizationService();
        }

        public static DefaultOrganizationServiceWrapper FromOrganizationService(OrganizationService service,
            IEntityConstructor constructor)
        {
            return new DefaultOrganizationServiceWrapper
            {
                _service = service,
                _entityConstructor = constructor
            };
        }
    }
}