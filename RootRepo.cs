using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TatlaCas.Asp.Domain.Models.Common;
using TatlaCas.Asp.Domain.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Npgsql;
using TatlaCas.Asp.Domain;
using TatlaCas.Asp.Utils.Extensions;

namespace TatlaCas.Asp.Persistence.Npgsql
{
    public abstract class RootRepo<TEntity, TAppContext> : IRepo<TEntity>
        where TEntity : class, IEntity where TAppContext : AbstractDbContext
    {
        private readonly TAppContext _dbContext;
        private DbSet<TEntity> Items { get; }

        protected RootRepo(TAppContext dbContext)
        {
            _dbContext = dbContext;
            Items = _dbContext.Set<TEntity>();
        }

        #region Insert

        public async Task<int> InsertAsync(List<TEntity> input)
        {
            await InsertInternalAsync(input);
            return await SaveChangesAsync();
        }

        public async Task<int> InsertAsync(TEntity input)
        {
            await InsertInternalAsync(input);
            return await SaveChangesAsync();
        }


        protected virtual async Task InsertInternalAsync(List<TEntity> input)
        {
            if (!(input?.Count > 0)) return;
            foreach (var entity in input)
            {
                await InsertInternalAsync(entity);
            }
        }

        protected virtual async Task InsertInternalAsync(TEntity input)
        {
            await Items.AddAsync(input);
        }

        #endregion

        #region Update

        public virtual Task<bool> UpdateAsync(TEntity input)
        {
            return Task.Factory.StartNew(() =>
            {
                Items.Update(input);
                SaveChanges();
                return true;
            });
        }

        public virtual Task<bool> UpdateAsync(List<TEntity> input)
        {
            return Task.Factory.StartNew(() =>
            {
                Items.UpdateRange(input);
                SaveChanges();
                return true;
            });
        }

        public virtual Task<bool> UpdateFieldsWhereAsync(object fields, Expression<Func<TEntity, bool>> queryExpr)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    if (fields == null) throw new ArgumentException("fieldsStrings required");
                    var entity = Items.Where(queryExpr).ToList();
                    if (!(entity?.Count > 0)) return false;
                    var properties = fields.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
                    var fieldsWithVals = new StringBuilder();
                    var idVals = new StringBuilder();

                    var i = 0;
                    var j = 0;
                    var tableType = typeof(TEntity);
                    var objectList = new List<NpgsqlParameter>();
                    foreach (var prop in properties)
                    {
                        fieldsWithVals.Append($"\"{prop.Name}\" = @newVal{i}, ");
                        objectList.Add(new NpgsqlParameter($"@newVal{i}", prop.GetValue(fields, null)));

                        i++;
                    }

                    fieldsWithVals.Remove(fieldsWithVals.Length - 2, 2);
                    var ids = entity.Select(p => p.Id);
                    foreach (var id in ids)
                    {
                        idVals.Append($"@id{j}, ");
                        objectList.Add(new NpgsqlParameter($"@id{j}", id));
                        j++;
                    }

                    idVals.Remove(idVals.Length - 2, 2);

                    _dbContext.Database.BeginTransaction();
                    var updatedCount = _dbContext.Database.ExecuteSqlRaw(
                        $"update \"{tableType.Name}\" set {fieldsWithVals} where \"Id\" IN ({idVals}) ",
                        objectList);
                    _dbContext.Database.CommitTransaction();
                    return updatedCount > 0;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            });
        }

        #endregion

        #region Get Entities

        public Task<List<TEntity>> GetEntitiesAsync(int pageSize = -1, int page = 0,
            List<Expression<Func<TEntity, object>>> includeRelationships = null,
            List<OrderByExpr<TEntity>> orderByExpr = null,
            List<OrderByFieldNames> orderByStr = null, RepoLocale repoLocale = RepoLocale.Default)
        {
            return EntitiesWhereAsync(null, orderByExpr, orderByStr, pageSize, page, includeRelationships, repoLocale);
        }


        public Task<List<TEntity>> EntitiesWhereAsync(Expression<Func<TEntity, bool>> queryExpr,
            List<OrderByExpr<TEntity>> orderByExpr = null, List<OrderByFieldNames> orderByStr = null,
            int pageSize = -1, int page = 0, List<Expression<Func<TEntity, object>>> includeRelationships = null,
            RepoLocale repoLocale = RepoLocale.Default)
        {
            return GetEntitiesInternal(queryExpr, pageSize, page, orderByExpr, orderByStr,
                includeRelationships, repoLocale);
        }


        protected virtual Task<List<TEntity>> GetEntitiesInternal(Expression<Func<TEntity, bool>> query,
            int pageSize,
            int page, List<OrderByExpr<TEntity>> orderByExpr, List<OrderByFieldNames> orderByStr = null,
            List<Expression<Func<TEntity, object>>> includeRelationships = null,
            RepoLocale repoLocale = RepoLocale.Default)
        {
            var tableQuery = Items.AsQueryable();
            var orderedQueryable = OrderedQueryable(orderByExpr, orderByStr, tableQuery);

            if (orderedQueryable != null) tableQuery = orderedQueryable;

            if (query != null)
                tableQuery = tableQuery.Where(query);
            if (pageSize <= 0) return QueryAsync(tableQuery, includeRelationships, repoLocale);

            if (page > 1)
            {
                tableQuery = tableQuery.Skip(pageSize * (page - 1));
            }

            tableQuery = tableQuery.Take(pageSize);

            return QueryAsync(tableQuery, includeRelationships, repoLocale);
        }

        public Task<TEntity> FirstEntityOrDefaultAsync(Expression<Func<TEntity, bool>> queryExpr,
            List<Expression<Func<TEntity, object>>> includeRelationships = null,
            List<OrderByExpr<TEntity>> orderByExpr = null,
            List<OrderByFieldNames> orderByStr = null, RepoLocale repoLocale = RepoLocale.Default)
        {
            var tableQuery = Items.AsQueryable();
            var orderedQueryable = OrderedQueryable(orderByExpr, orderByStr, tableQuery);

            if (orderedQueryable != null) tableQuery = orderedQueryable;
            if (queryExpr != null)
                tableQuery = tableQuery.Where(queryExpr);
            if (!(includeRelationships?.Count > 0)) return tableQuery.FirstOrDefaultAsync();
            tableQuery = includeRelationships.Aggregate(tableQuery, (current, include) => current.Include(include));
            return tableQuery.FirstOrDefaultAsync();
        }

        public TEntity FirstEntityOrDefault(Expression<Func<TEntity, bool>> queryExpr,
            List<Expression<Func<TEntity, object>>> includeRelationships = null,
            List<OrderByExpr<TEntity>> orderByExpr = null,
            List<OrderByFieldNames> orderByStr = null, RepoLocale repoLocale = RepoLocale.Default)
        {
            var tableQuery = Items.AsQueryable();
            var orderedQueryable = OrderedQueryable(orderByExpr, orderByStr, tableQuery);

            if (orderedQueryable != null) tableQuery = orderedQueryable;

            if (queryExpr != null)
                tableQuery = tableQuery.Where(queryExpr);

            if (!(includeRelationships?.Count > 0)) return tableQuery.FirstOrDefault();
            tableQuery = includeRelationships.Aggregate(tableQuery, (current, include) => current.Include(include));

            return tableQuery.FirstOrDefault();
        }

        #endregion

        private IOrderedQueryable<TEntity> OrderedQueryable(List<OrderByExpr<TEntity>> orderByExpr,
            List<OrderByFieldNames> orderByStr,
            IQueryable<TEntity> tableQuery)
        {
            IOrderedQueryable<TEntity> orderedQueryable = null;
            if (orderByExpr?.Count > 0)
            {
                foreach (var expr in orderByExpr)
                {
                    if (orderedQueryable == null)
                        orderedQueryable = expr.Direction == OrderBy.Descending
                            ? tableQuery.OrderByDescending(expr.OrderByExpression)
                            : tableQuery.OrderBy(expr.OrderByExpression);
                    else
                        orderedQueryable = expr.Direction == OrderBy.Descending
                            ? orderedQueryable.ThenByDescending(expr.OrderByExpression)
                            : orderedQueryable.ThenBy(expr.OrderByExpression);
                }
            }

            if (!(orderByStr?.Count > 0)) return orderedQueryable;
            foreach (var expr in orderByStr)
            {
                try
                {
                    if (orderedQueryable == null)
                        orderedQueryable = expr.Direction == OrderBy.Descending
                            ? tableQuery.OrderByDescending(expr.FieldName)
                            : tableQuery.OrderBy(expr.FieldName);
                    else
                        orderedQueryable = expr.Direction == OrderBy.Descending
                            ? orderedQueryable.ThenByDescending(expr.FieldName)
                            : orderedQueryable.ThenBy(expr.FieldName);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            return orderedQueryable;
        }


        private Task<List<TEntity>> QueryAsync(IQueryable<TEntity> tableQuery,
            List<Expression<Func<TEntity, object>>> includeRelationships, RepoLocale repoLocale = RepoLocale.Default)
        {
            AddRepoSpecificIncludes(ref includeRelationships);
            if (!(includeRelationships?.Count > 0)) return tableQuery.ToListAsync();
            tableQuery = includeRelationships.Aggregate(tableQuery, (current, include) => current.Include(include));
            return FinalizeAndExecuteQuery(tableQuery, includeRelationships);
        }

        protected virtual void AddRepoSpecificIncludes(ref List<Expression<Func<TEntity, object>>> includeRelationships)
        {
        }

        protected virtual Task<List<TEntity>> FinalizeAndExecuteQuery(IQueryable<TEntity> tableQuery,
            List<Expression<Func<TEntity, object>>> includeRelationships, RepoLocale repoLocale = RepoLocale.Default)
        {
            return tableQuery?.ToListAsync();
        }

        #region Get Resources

        public async Task<List<IResource>> GetResourcesAsync(int pageSize = -1, int page = 0,
            List<Expression<Func<TEntity, object>>> includeRelationships = null,
            List<OrderByExpr<TEntity>> orderByExpr = null,
            List<OrderByFieldNames> orderByStr = null, RepoLocale repoLocale = RepoLocale.Default)
        {
            var entities = await GetEntitiesAsync(pageSize, page, includeRelationships, orderByExpr, orderByStr,
                repoLocale);
            return ToRes(entities);
        }


        public async Task<List<IResource>> ResourcesWhereAsync(Expression<Func<TEntity, bool>> queryExpr,
            int pageSize = -1, int page = 0, List<Expression<Func<TEntity, object>>> includeRelationships = null,
            List<OrderByExpr<TEntity>> orderByExpr = null, List<OrderByFieldNames> orderByStr = null,
            RepoLocale repoLocale = RepoLocale.Default)
        {
            var entities = await EntitiesWhereAsync(queryExpr, orderByExpr, orderByStr, pageSize, page,
                includeRelationships, repoLocale);
            return ToRes(entities);
        }

        #endregion


        public Task<int> CountAsync(Expression<Func<TEntity, bool>> queryExpr = null)
        {
            return queryExpr != null ? Items.CountAsync(queryExpr) : Items.CountAsync();
        }


        public async Task<IResource> FirstResourceOrDefaultAsync(Expression<Func<TEntity, bool>> queryExpr,
            List<Expression<Func<TEntity, object>>> includeRelationships = null,
            List<OrderByExpr<TEntity>> orderByExpr = null,
            List<OrderByFieldNames> orderByStr = null, RepoLocale repoLocale = RepoLocale.Default)
        {
            var entities =
                await FirstEntityOrDefaultAsync(queryExpr, includeRelationships, orderByExpr, orderByStr, repoLocale);
            return ToRes(entities);
        }


        #region To Resource/ To Entity

        public virtual List<IResource> ToRes(IEnumerable<TEntity> entities)
        {
            return entities.Select(ToRes).ToList();
        }

        public abstract IResource ToRes(TEntity entity);

        public abstract TEntity ToEntity(IResource resource);

        #endregion

        #region Save Changes

        protected virtual async Task<int> SaveChangesAsync()
        {
            try
            {
                return await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 0;
            }
        }

        protected virtual void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        #endregion
    }


    public abstract class RootReadOnlyRepo<TEntity, TAppContext> : RootRepo<TEntity, TAppContext>
        where TEntity : class, IEntity where TAppContext : AbstractDbContext
    {
        protected RootReadOnlyRepo(TAppContext appDbContext, IMapper mapper) : base(appDbContext)
        {
        }

        protected override Task InsertInternalAsync(List<TEntity> input)
        {
            throw new NotSupportedException();
        }

        protected override Task InsertInternalAsync(TEntity input)
        {
            throw new NotSupportedException();
        }

        public override Task<bool> UpdateAsync(TEntity input)
        {
            throw new NotSupportedException();
        }

        public override Task<bool> UpdateAsync(List<TEntity> input)
        {
            throw new NotSupportedException();
        }

        public override Task<bool> UpdateFieldsWhereAsync(object fields, Expression<Func<TEntity, bool>> queryExpr)
        {
            throw new NotSupportedException();
        }

        protected override Task<int> SaveChangesAsync()
        {
            throw new NotSupportedException();
        }

        protected override void SaveChanges()
        {
            throw new NotSupportedException();
        }
    }
}