using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TatlaCas.Asp.Domain.Models.Common;
using TatlaCas.Asp.Domain.Repos;
using TatlaCas.Asp.Domain.Resources;
using TatlaCas.Asp.Domain.Utils;
using TatlaCas.Asp.Utils;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace TatlaCas.Asp.Persistence.Npgsql
{
    public abstract class BaseRepo<TEntity, TResource, TAppContext> : IRepo<TEntity, TResource>
        where TEntity : class, IEntity where TResource : IResource where TAppContext : AbstractDbContext
    {
        private readonly TAppContext AdminDbContext;
        private DbSet<TEntity> Items { get; }
        private readonly IMapper _mapper;

        protected BaseRepo(TAppContext adminDbContext, IMapper mapper)
        {
            AdminDbContext = adminDbContext;
            _mapper = mapper;
            Items = AdminDbContext.Set<TEntity>();
        }

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

                    AdminDbContext.Database.BeginTransaction();
                    var updatedCount = AdminDbContext.Database.ExecuteSqlRaw(
                        $"update \"{tableType.Name}\" set {fieldsWithVals} where \"Id\" IN ({idVals}) ",
                        objectList);
                    AdminDbContext.Database.CommitTransaction();
                    return updatedCount > 0;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            });
        }

        public Task<List<TEntity>> GetEntitiesAsync(int pageSize = -1, int page = 0,
            List<string> includeRelationships = null)
        {
            return EntitiesWhereAsync(null, pageSize, page, includeRelationships);
        }

        public Task<List<TEntity>> EntitiesWhereAsync(Expression<Func<TEntity, bool>> queryExpr,
            int pageSize = -1, int page = 0, List<string> includeRelationships = null)
        {
            return EntitiesWhereAsync(queryExpr, null, pageSize: pageSize, page: page,
                includeRelationships: includeRelationships);
        }

        public Task<List<TEntity>> EntitiesWhereAsync(Expression<Func<TEntity, bool>> queryExpr,
            List<OrderByExpr<TEntity>> orderByExpr, List<OrderByFieldNames> orderByStr = null,
            int pageSize = -1, int page = 0, List<string> includeRelationships = null)
        {
            return GetEntitiesInternal(queryExpr, pageSize, page, orderByExpr, orderByStr,
                includeRelationships);
        }


        protected virtual Task<List<TEntity>> GetEntitiesInternal(Expression<Func<TEntity, bool>> query,
            int pageSize,
            int page, List<OrderByExpr<TEntity>> orderByExpr, List<OrderByFieldNames> orderByStr = null,
            List<string> includeRelationships = null)
        {
            var tableQuery = Items.AsQueryable();
            var orderedQueryable = OrderedQueryable(orderByExpr, orderByStr, tableQuery);

            if (orderedQueryable != null) tableQuery = orderedQueryable;

            if (query != null)
                tableQuery = tableQuery.Where(query);
            if (pageSize <= 0) return QueryAsync(tableQuery, includeRelationships);

            if (page > 1)
            {
                tableQuery = tableQuery.Skip(pageSize * (page - 1));
            }

            tableQuery = tableQuery.Take(pageSize);

            return QueryAsync(tableQuery, includeRelationships);
        }

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
            List<string> includeRelationships)
        {
            if (!(includeRelationships?.Count > 0)) return tableQuery.ToListAsync();
            foreach (var include in includeRelationships)
            {
                tableQuery = tableQuery.Include(include);
            }

            return tableQuery?.ToListAsync();
        }

        public Task<TEntity> FirstEntityOrDefaultAsync(Expression<Func<TEntity, bool>> queryExpr,
            List<string> includeRelationships = null)
        {
            return Items.FirstOrDefaultAsync(queryExpr);
        }

        public TEntity FirstEntityOrDefault(Expression<Func<TEntity, bool>> queryExpr,
            List<string> includeRelationships = null)
        {
            return Items.FirstOrDefault(queryExpr);
        }

        public Task<TEntity> FirstEntityOrDefaultAsync(Expression<Func<TEntity, bool>> queryExpr,
            List<OrderByExpr<TEntity>> orderByExpr, List<OrderByFieldNames> orderByStr = null,
            List<string> includeRelationships = null)
        {
            var tableQuery = Items.AsQueryable();
            var orderedQueryable = OrderedQueryable(orderByExpr, orderByStr, tableQuery);

            if (orderedQueryable != null) tableQuery = orderedQueryable;
            return tableQuery.FirstOrDefaultAsync(queryExpr);
        }

        public async Task<List<TResource>> GetResourcesAsync(int pageSize = -1, int page = 0,
            List<string> includeRelationships = null)
        {
            var entities = await GetEntitiesAsync(pageSize, page, includeRelationships);
            return ToRes(entities);
        }


        public async Task<List<TResource>> ResourcesWhereAsync(Expression<Func<TEntity, bool>> queryExpr,
            int pageSize = -1, int page = 0, List<string> includeRelationships = null)
        {
            var entities = await EntitiesWhereAsync(queryExpr, pageSize, page, includeRelationships);
            return ToRes(entities);
        }

        public async Task<List<TResource>> ResourcesAsync(int pageSize = -1, int page = 0,
            List<string> includeRelationships = null)
        {
            var entities = await EntitiesWhereAsync(null, pageSize, page, includeRelationships);
            return ToRes(entities);
        }

        public Task<int> CountAsync(Expression<Func<TEntity, bool>> queryExpr = null)
        {
            return queryExpr != null ? Items.CountAsync(queryExpr) : Items.CountAsync();
        }

        public async Task<List<TResource>> ResourcesWhereAsync(Expression<Func<TEntity, bool>> queryExpr,
            List<OrderByExpr<TEntity>> orderByExpr, List<OrderByFieldNames> orderByStr = null,
            int pageSize = -1, int page = 0, List<string> includeRelationships = null)
        {
            var entities =
                await EntitiesWhereAsync(queryExpr, orderByExpr, orderByStr, pageSize, page, includeRelationships);
            return ToRes(entities);
        }

        public async Task<TResource> FirstResourceOrDefaultAsync(Expression<Func<TEntity, bool>> queryExpr,
            List<string> includeRelationships = null)
        {
            var entities = await FirstEntityOrDefaultAsync(queryExpr, includeRelationships);
            return ToRes(entities);
        }

        public virtual List<TResource> ToRes(IEnumerable<TEntity> entities)
        {
            return entities.Select(ToRes).ToList();
        }

        public virtual TResource ToRes(TEntity entity)
        {
            return _mapper.Map<TEntity, TResource>(entity);
        }

        public virtual TEntity ToEntity(TResource resource)
        {
            return _mapper.Map<TResource, TEntity>(resource);
        }

        protected virtual async Task<int> SaveChangesAsync()
        {
            try
            {
                return await AdminDbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 0;
            }
        }

        protected virtual void SaveChanges()
        {
            AdminDbContext.SaveChanges();
        }
    }


    public abstract class ReadOnlyRepo<TEntity, TResource, TAppContext> : BaseRepo<TEntity, TResource, TAppContext>
        where TEntity : class, IEntity where TResource : IResource where TAppContext : AbstractDbContext
    {
        protected ReadOnlyRepo(TAppContext appDbContext, IMapper mapper) : base(appDbContext, mapper)
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