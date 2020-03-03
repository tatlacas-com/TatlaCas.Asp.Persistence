using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TatlaCas.Asp.Utils;

namespace TatlaCas.Asp.Persistence.Npgsql
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

        protected void EnforceOnModelCreatingRules(List<Type> types, ModelBuilder builder)
        {
            types.ForEach(typ =>
            {
                typ.Assembly
                    .CreatableTypesInNs(typ.Namespace)
                    .WhenPersistable()
                    .EnforceOnModelCreatingRules(builder);
            });
        }
    }
}