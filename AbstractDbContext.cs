using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TatlaCas.Asp.Core.Util.Extensions;

namespace TatlaCas.Asp.Core.Persistence
{
    public abstract class AbstractDbContext : DbContext
    {
        public AbstractDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
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
}