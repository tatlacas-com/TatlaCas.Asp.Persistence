using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TatlaCas.Asp.Domain.Models.Common;
using Microsoft.EntityFrameworkCore;
using TatlaCas.Asp.Core.Util.Resources;

namespace TatlaCas.Asp.Core.Persistence
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

        /*public virtual Task<bool> UpdateFieldsWhereAsync(object fields, Expression<Func<TEntity, bool>> queryExpr)
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
        }*/

        #endregion

        #region Get Entities

        public Task<List<TEntity>> GetEntitiesAsync(int pageSize = -1, int page = 0,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> customizeQuery = null)
        {
            return GetEntitiesInternal(pageSize, page, customizeQuery);
        }


        protected virtual Task<List<TEntity>> GetEntitiesInternal(int pageSize,
            int page, Func<IQueryable<TEntity>, IQueryable<TEntity>> customizeQuery = null)
        {
            var tableQuery = Items.AsQueryable();

            if (pageSize <= 0) return QueryAsync(tableQuery, customizeQuery);

            if (page > 1)
            {
                tableQuery = tableQuery.Skip(pageSize * (page - 1));
            }

            tableQuery = tableQuery.Take(pageSize);

            return QueryAsync(tableQuery, customizeQuery);
        }

        public Task<TEntity> FirstEntityOrDefaultAsync(
            Func<IQueryable<TEntity>, IQueryable<TEntity>> customizeQuery = null)
        {
            var tableQuery = Items.AsQueryable();
            if (customizeQuery != null)
                tableQuery = customizeQuery(tableQuery);
            return tableQuery.FirstOrDefaultAsync();
        }


        public TEntity FirstEntityOrDefault(Func<IQueryable<TEntity>, IQueryable<TEntity>> customizeQuery = null)
        {
            var tableQuery = Items.AsQueryable();

            if (customizeQuery != null)
                tableQuery = customizeQuery(tableQuery);
            return tableQuery.FirstOrDefault();
        }

        #endregion


        private Task<List<TEntity>> QueryAsync(IQueryable<TEntity> tableQuery,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> customizeQuery = null)
        {
            if (customizeQuery != null)
                tableQuery = customizeQuery(tableQuery);
            return tableQuery.ToListAsync();
        }

        public Task<int> CountAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> customizeQuery = null)
        {
            var tableQuery = Items.AsQueryable();
            return customizeQuery != null ? customizeQuery(tableQuery).CountAsync() : tableQuery.CountAsync();
        }

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

       
        protected override Task<int> SaveChangesAsync()
        {
            throw new NotSupportedException();
        }

        protected override void SaveChanges()
        {
            throw new NotSupportedException();
        }
    }

    public abstract class RootRepo<TEntity, TResource, TAppContext> : RootRepo<TEntity, TAppContext>
        where TEntity : class, IEntity
        where TResource : IResource
        where TAppContext : AbstractDbContext
    {
        protected readonly IMapper Mapper;
        public bool ResourceIsDisplayFormModel { get; set; }
        public string ViewKey { get; set; }

        public RootRepo(TAppContext dbContext, IMapper mapper) : base(dbContext)
        {
            Mapper = mapper;
        }
    }
}