#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TatlaCas.Asp.Util.Extensions;
using TatlaCas.Asp.Domain.Models.Common;

namespace TatlaCas.Asp.Persistence;

public abstract class AbstractDbContext : DbContext
{
    public AbstractDbContext(DbContextOptions options) : base(options)
    {
        SavingChanges += OnSavingChanges;
    }

    private void OnSavingChanges(object? sender, SavingChangesEventArgs e)
    {
        SetCreatedAtUpdatedAt();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }


    private void SetCreatedAtUpdatedAt()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is IPersistableEntity && e.State is EntityState.Added or EntityState.Modified);

        foreach (var entityEntry in entries)
        {
            ((IPersistableEntity)entityEntry.Entity).UpdatedAt =  DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);

            if (entityEntry.State == EntityState.Added)
            {
                ((IPersistableEntity)entityEntry.Entity).CreatedAt =  DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
            }
        }
    }


    protected void EnforceOnModelCreatingRules(List<Type> types, ModelBuilder builder,
        Action<ModelBuilder, Type> finishEnforceModelCreating, bool includeSubDirTypes = false)
    {
        types.ForEach(typ =>
        {
            typ.Assembly
                .CreatableTypesInNs(typ.Namespace, includeSubDirTypes)
                .WhenPersistable()
                .EnforceOnModelCreatingRules(builder, finishEnforceModelCreating);
        });
    }
}