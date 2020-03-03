using System;
using System.Collections.Generic;
using System.Linq;
using TatlaCas.Asp.Domain.Models.Common;
using Microsoft.EntityFrameworkCore;

namespace TatlaCas.Asp.Persistence.Npgsql
{
    public static class EntityExtensions
    {
        public static void EnforceOnModelCreatingRules(this IEnumerable<Type> sqlTypes, ModelBuilder builder)
        {
            foreach (var typ in sqlTypes)
            {
                
                builder.Entity(typ).ToTable(typ.Name);
                var tbl = (IPersistableEntity) Activator.CreateInstance(typ);
                tbl.OnModelCreating(builder);
                builder.UseIdentityAlwaysColumns();
            }
        }

        public static IEnumerable<Type> WhenPersistable(this IEnumerable<Type> types)
        {
            return types.Where(p => p.GetInterfaces().Contains(typeof(IPersistableEntity)));
        }
    }
}