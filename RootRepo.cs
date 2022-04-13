using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TatlaCas.Asp.Domain.Models.Common;
using Microsoft.EntityFrameworkCore;

namespace TatlaCas.Asp.Persistence;

public abstract class RootRepo<TEntity, TAppContext> : IRepo<TEntity>
    where TEntity : class, IEntity where TAppContext : AbstractDbContext
{
    public readonly TAppContext _dbContext;
    public DbContext DbContext => _dbContext;

    public DbSet<TEntity> Items { get; }

    protected RootRepo(TAppContext dbContext)
    {
        this._dbContext = dbContext;
        Items = this._dbContext.Set<TEntity>();
    }

    public void DetachAll()
    {
        var entityEntries = _dbContext.ChangeTracker.Entries().ToList();

        foreach (var entityEntry in entityEntries)
            entityEntry.State = EntityState.Detached;
    }

    #region Insert

    public async Task<int> InsertAsync(List<TEntity> input,Func<TEntity,Expression<Func<TEntity, bool>>> ifExistingPredicate = null)
    {
        await InsertInternalAsync(input,ifExistingPredicate);
        return await SaveChangesAsync();
    }

    public async Task<int> InsertAsync(TEntity input,Func<TEntity,Expression<Func<TEntity, bool>>> ifExistingPredicate = null)
    {
        await InsertInternalAsync(input,ifExistingPredicate);
        return await SaveChangesAsync();
    }


    protected virtual async Task InsertInternalAsync(List<TEntity> input,Func<TEntity,Expression<Func<TEntity, bool>>> alreadyExistsPredicate)
    {
        if (!(input?.Count > 0)) return;
        foreach (var entity in input)
        {
            await InsertInternalAsync(entity,alreadyExistsPredicate);
        }
    }

    protected virtual async Task InsertInternalAsync(TEntity input,Func<TEntity,Expression<Func<TEntity, bool>>> alreadyExistsPredicate)
    {
        if (alreadyExistsPredicate != null && await Items.AnyAsync(alreadyExistsPredicate(input))) return;
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
        Func<DbSet<TEntity>, IQueryable<TEntity>> customizeQuery = null)
    {
        return GetEntitiesInternal(pageSize, page, customizeQuery);
    }


    protected virtual Task<List<TEntity>> GetEntitiesInternal(int pageSize,
        int page, Func<DbSet<TEntity>, IQueryable<TEntity>> customizeQuery = null)
    {
        if (pageSize <= 0) return QueryAsync(Items, customizeQuery).ToListAsync();
        var queryable = QueryAsync(Items, customizeQuery);
        if (page > 1)
        {
            queryable = queryable.Skip(pageSize * (page - 1));
        }

        return queryable.Take(pageSize).ToListAsync();
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


    private IQueryable<TEntity> QueryAsync(DbSet<TEntity> tableQuery,
        Func<DbSet<TEntity>, IQueryable<TEntity>> customizeQuery = null)
    {
        return customizeQuery != null ? customizeQuery(tableQuery) : tableQuery;
    }

    public Task<int> CountAsync(Func<DbSet<TEntity>, IQueryable<TEntity>> customizeQuery = null)
    {
        return customizeQuery != null ? customizeQuery(Items).CountAsync() : Items.CountAsync();
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

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}


public abstract class RootReadOnlyRepo<TEntity, TAppContext> : RootRepo<TEntity, TAppContext>
    where TEntity : class, IEntity where TAppContext : AbstractDbContext
{
    protected RootReadOnlyRepo(TAppContext appDbContext) : base(appDbContext)
    {
    }

    protected override Task InsertInternalAsync(List<TEntity> input,Func<TEntity,Expression<Func<TEntity, bool>>> ifExistingPredicate)
    {
        throw new NotSupportedException();
    }

    protected override Task InsertInternalAsync(TEntity input,Func<TEntity,Expression<Func<TEntity, bool>>> ifExistingPredicate)
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