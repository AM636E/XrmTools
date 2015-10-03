using System;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace DynamicaLabs.XrmTools.Data
{
    /// <summary>
    /// Decorated entity repository with insert and update checks.
    /// </summary>
    public class ConfigurableEntityRepository : IEntityRepository
    {
        public IEnumerable<Entity> GetEntities(string entityType, ColumnSet columnSet)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Entity> GetEntities(string entityType, QueryBase query)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Entity> GetAccessableEntities(string entityType, string userId, ColumnSet columnSet)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Entity> GetAccessableEntities(QueryBase query, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Entity GetEntityById(string entityType, Guid id)
        {
            throw new NotImplementedException();
        }

        public Entity GetEntityById(string entityType, Guid id, Guid userGuid)
        {
            throw new NotImplementedException();
        }

        public T GetEntityById<T>(string entityType, Guid id)
        {
            throw new NotImplementedException();
        }

        public T GetEntityById<T>(string entityType, Guid id, Guid userGuid)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetEntities<T>(QueryBase query)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetEntities<T>(QueryBase query, Guid userGuid)
        {
            throw new NotImplementedException();
        }

        public Entity UpdateEntity(string entityType, Entity entity)
        {
            throw new NotImplementedException();
        }

        public Entity UpdateEntity(string entityType, Entity entity, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Entity CreateEntity(string entityType, Entity entity)
        {
            throw new NotImplementedException();
        }

        public TEntity CreateEntity<TEntity>(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public TObject CreateEntity<TObject>(TObject entity, Guid userIGuid)
        {
            throw new NotImplementedException();
        }

        public Entity CreateEntity(string entityType, Entity entity, Guid userId)
        {
            throw new NotImplementedException();
        }

        public void DeleteEntity(string entityType, Guid entityGuid)
        {
            throw new NotImplementedException();
        }

        public void DeleteEntity(string entityType, Guid entityGuid, Guid userGuid)
        {
            throw new NotImplementedException();
        }

        public void DisableEntity(string entityType, Guid entityGuid)
        {
            throw new NotImplementedException();
        }

        public void DisableEntity(string entityType, Guid entityGuid, Guid userGuid)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> QueryByAttributes<TEntity>(string entityName, IDictionary<string, string> attributes, bool onlyActive = true,
            bool strict = true)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> QueryByAttributes<TEntity>(string entityName, IDictionary<string, string> attributes, Guid userid, bool onlyActive = true,
            bool strict = true)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> QueryByAttributes<TEntity>(string entityName, IDictionary<string, string> attributes, bool onlyActive = true)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> QueryByAttributes<TEntity>(string entityName, IDictionary<string, string> attributes, Guid userid, bool onlyActive = true)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> QueryByAttributes<TEntity>(string entityName, IDictionary<string, string> attributes)
        {
            throw new NotImplementedException();
        }
    }
}