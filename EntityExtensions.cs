using System;
using System.Collections.Generic;
using System.Linq;
using TatlaCas.Asp.Domain.Models.Common;
using Microsoft.EntityFrameworkCore;

namespace TatlaCas.Asp.Core.Persistence;

public static class EntityExtensions
{
    public static void EnforceOnModelCreatingRules(this IEnumerable<Type> sqlTypes,
        ModelBuilder builder, Action<ModelBuilder,Type> finishEnforceModelCreating)
    {
        foreach (var typ in sqlTypes)
        {
            finishEnforceModelCreating(builder, typ);
        }
    }

    public static IEnumerable<Type> WhenPersistable(this IEnumerable<Type> types)
    {
        return types.Where(p => p.GetInterfaces().Contains(typeof(IPersistableEntity)));
    }

    public static void Test()
    {
           
    }
}