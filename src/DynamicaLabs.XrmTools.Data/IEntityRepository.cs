using System;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace DynamicaLabs.XrmTools.Data
{
    /// <summary>
    ///     Represents basic entity repository.
    ///     Exposes methods for common entity operations:
    ///     Retrieve by id, by query, generic retrieve with or without impersonation.
    /// </summary>
    public interface IEntityRepository
    {
        /// <summary>
        ///     Get entities all entitites with given columnset.
        /// </summary>
        /// <param name="entityType">Entity Type Name</param>
        /// <param name="columnSet">Entity Column set</param>
        /// <returns></returns>
        IEnumerable<Entity> GetEntities(string entityType, ColumnSet columnSet);

        /// <summary>
        ///     Get Entitities by query.
        /// </summary>
        /// <param name="entityType">Entity Type Name</param>
        /// <param name="query">Query for entities.</param>
        /// <returns></returns>
        IEnumerable<Entity> GetEntities(string entityType, QueryBase query);

        /// <summary>
        ///     Get all entities with impersonation.
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="userId"></param>
        /// <param name="columnSet"></param>
        /// <returns></returns>
        IEnumerable<Entity> GetAccessableEntities(string entityType, string userId, ColumnSet columnSet);

        /// <summary>
        ///     Get entities by query with impersonation.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<Entity> GetAccessableEntities(QueryBase query, Guid userId);

        /// <summary>
        ///     Get single crm entity by id.
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Entity GetEntityById(string entityType, Guid id);

        /// <summary>
        ///     Get single crm entity by id with impersonation.
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="id"></param>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        Entity GetEntityById(string entityType, Guid id, Guid userGuid);

        /// <summary>
        ///     Get generic object from crm entity by id.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityType"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetEntityById<T>(string entityType, Guid id);

        /// <summary>
        ///     Get generic object from crm entity by id with impersonation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityType"></param>
        /// <param name="id"></param>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        T GetEntityById<T>(string entityType, Guid id, Guid userGuid);

        /// <summary>
        ///     Get crm entities by query and construct generic objects.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        IEnumerable<T> GetEntities<T>(QueryBase query);

        /// <summary>
        ///     Get crm entities by query with impersonation and construct generic objects.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        IEnumerable<T> GetEntities<T>(QueryBase query, Guid userGuid);

        /// <summary>
        ///     Update crm entity.
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        Entity UpdateEntity(string entityType, Entity entity);

        /// <summary>
        ///     Update crm entity with impersonation.
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="entity"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Entity UpdateEntity(string entityType, Entity entity, Guid userId);

        TObject UpdateEntity<TObject>(TObject tobject);
        TObject UpdateEntity<TObject>(TObject tobject, Guid userId);

        /// <summary>
        ///     Create crm entity.
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        Entity CreateEntity(string entityType, Entity entity);

        TObject CreateEntity<TObject>(TObject entity);
        TObject CreateEntity<TObject>(TObject entity, Guid userIGuid);

        /// <summary>
        ///     Create crm entity with impersonation.
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="entity"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Entity CreateEntity(string entityType, Entity entity, Guid userId);

        /// <summary>
        ///     Delete crm entity.
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="entityGuid"></param>
        void DeleteEntity(string entityType, Guid entityGuid);

        /// <summary>
        ///     Delete crm entity with impersonation.
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="entityGuid"></param>
        /// <param name="userGuid"></param>
        void DeleteEntity(string entityType, Guid entityGuid, Guid userGuid);

        /// <summary>
        ///     Disable crm entity.
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="entityGuid"></param>
        void DisableEntity(string entityType, Guid entityGuid);

        /// <summary>
        ///     Disable crm entity with impersonation.
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="entityGuid"></param>
        /// <param name="userGuid"></param>
        void DisableEntity(string entityType, Guid entityGuid, Guid userGuid);

        IEnumerable<Entity> QueryByAttributes(string entityName, IDictionary<string, object> attributes,
            bool onlyActive = true, bool strict = true);

        IEnumerable<Entity> QueryByAttributes(string entityName, IDictionary<string, object> attributes, Guid userId,
            bool onlyActive = true, bool strict = true);

        IEnumerable<TEntity> QueryByAttributes<TEntity>(string entityName, IDictionary<string, object> attributes,
            bool onlyActive = true, bool strict = true);

        IEnumerable<TEntity> QueryByAttributes<TEntity>(string entityName, IDictionary<string, object> attributes,
            Guid userid, bool onlyActive = true, bool strict = true);
    }
}