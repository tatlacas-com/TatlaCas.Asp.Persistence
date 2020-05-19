using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TatlaCas.Asp.Domain.Models.Common;
using TatlaCas.Asp.Core.Util.Resources;

namespace TatlaCas.Asp.Core.Persistence
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

        /*/// <summary>
        /// 
        /// </summary>
        /// <param name="fields">Anonymous Type object with the Fields to update</param>
        /// <param name="queryExpr">Query with the condition to prompt update</param>
        /// <returns></returns>
        Task<bool> UpdateFieldsWhereAsync(object fields, Expression<Func<TEntity, bool>> queryExpr);*/

        #endregion


        #region Get Entities

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageSize">Maximum number of records to return. Set -1 to return all</param>
        /// <param name="page">page number, starts at 1</param>
        /// <param name="customizeQuery"></param>
        /// <returns></returns>
        Task<List<TEntity>> GetEntitiesAsync(int pageSize = -1, int page = 0,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> customizeQuery = null);

        #endregion

      
        #region Get First Entity

        Task<TEntity> FirstEntityOrDefaultAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> customizeQuery = null);

        TEntity FirstEntityOrDefault(Func<IQueryable<TEntity>, IQueryable<TEntity>> customizeQuery = null);

        #endregion


        #region Count

        Task<int> CountAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> customizeQuery = null);

        #endregion
    }

  

    public interface IRepo<TEntity, TResource> : IRepo<TEntity> where TEntity : class, IEntity
        where TResource : IResource
    {
    }

}