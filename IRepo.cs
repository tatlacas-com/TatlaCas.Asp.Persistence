using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TatlaCas.Asp.Domain;
using TatlaCas.Asp.Domain.Models.Common;
using TatlaCas.Asp.Domain.Resources;

namespace TatlaCas.Asp.Persistence.Npgsql
{
    public interface IRepo<TEntity> where TEntity : IEntity
    {
        #region Insert

        Task<int> InsertAsync(List<TEntity> input);
        Task<int> InsertAsync(TEntity input);

        #endregion

        /*#region Insert or Update

        TEntity InsertOrUpdate(TEntity input);
        void InsertOrUpdate(List<TEntity> input);
        Task InsertOrUpdateAsync(List<TEntity> input);
        Task InsertOrUpdateAsync(TEntity input);

        #endregion*/

        #region Update

        Task<bool> UpdateAsync(TEntity input);
        Task<bool> UpdateAsync(List<TEntity> input);

        #endregion

        #region Update Where

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fields">Anonymous Type object with the Fields to update</param>
        /// <param name="queryExpr">Query with the condition to prompt update</param>
        /// <returns></returns>
        Task<bool> UpdateFieldsWhereAsync(object fields, Expression<Func<TEntity, bool>> queryExpr);

        #endregion


        #region Get Entities

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageSize">Maximum number of records to return. Set -1 to return all</param>
        /// <param name="page">page number, starts at 1</param>
        /// <param name="includeRelationships"></param>
        /// <param name="orderByExpr"></param>
        /// <param name="orderByStr"></param>
        /// <returns></returns>
        Task<List<TEntity>> GetEntitiesAsync(int pageSize = -1, int page = 0, List<string> includeRelationships = null,
            List<OrderByExpr<TEntity>> orderByExpr = null, List<OrderByFieldNames> orderByStr = null);

        #endregion

        #region Get Entities Where

        Task<List<TEntity>> EntitiesWhereAsync(Expression<Func<TEntity, bool>> queryExpr,
            List<OrderByExpr<TEntity>> orderByExpr = null, List<OrderByFieldNames> orderByStr = null,
            int pageSize = -1, int page = 0, List<string> includeRelationships = null);

        #endregion

        #region Get First Entity

        Task<TEntity> FirstEntityOrDefaultAsync(Expression<Func<TEntity, bool>> queryExpr,
            List<string> includeRelationships = null, List<OrderByExpr<TEntity>> orderByExpr = null,
            List<OrderByFieldNames> orderByStr = null);

        TEntity FirstEntityOrDefault(Expression<Func<TEntity, bool>> queryExpr,
            List<string> includeRelationships = null, List<OrderByExpr<TEntity>> orderByExpr = null,
            List<OrderByFieldNames> orderByStr = null);

        #endregion

        #region Get Resources

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageSize">Maximum number of records to return. Set -1 to return all</param>
        /// <param name="page">page number, starts at 1</param>
        /// <param name="includeRelationships"></param>
        /// <returns></returns>
        Task<List<IResource>> GetResourcesAsync(int pageSize = -1, int page = 0,
            List<string> includeRelationships = null, List<OrderByExpr<TEntity>> orderByExpr = null,
            List<OrderByFieldNames> orderByStr = null);

        #endregion

        #region Get Resources Where

        Task<List<IResource>> ResourcesWhereAsync(Expression<Func<TEntity, bool>> queryExpr, int pageSize = -1,
            int page = 0, List<string> includeRelationships = null, List<OrderByExpr<TEntity>> orderByExpr = null,
            List<OrderByFieldNames> orderByStr = null);

        #endregion

        #region Get First Resource

        Task<IResource> FirstResourceOrDefaultAsync(Expression<Func<TEntity, bool>> queryExpr,
            List<string> includeRelationships = null, List<OrderByExpr<TEntity>> orderByExpr = null,
            List<OrderByFieldNames> orderByStr = null);

        #endregion

        #region Entities To Resources

        List<IResource> ToRes(IEnumerable<TEntity> entities);
        IResource ToRes(TEntity entity);
        TEntity ToEntity(IResource resource);

        #endregion

        #region Count

        Task<int> CountAsync(Expression<Func<TEntity, bool>> queryExpr = null);

        #endregion
    }

    public class OrderByExpr<TEntity> where TEntity : IEntity
    {
        public Expression<Func<TEntity, object>> OrderByExpression { get; set; }
        public OrderBy Direction { get; set; } = OrderBy.Ascending;
    }

    public class OrderByFieldNames
    {
        public string FieldName { get; set; }
        public OrderBy Direction { get; set; } = OrderBy.Ascending;
    }
}