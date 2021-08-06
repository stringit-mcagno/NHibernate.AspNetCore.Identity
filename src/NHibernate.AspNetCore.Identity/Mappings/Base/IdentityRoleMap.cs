using System;
using FluentNHibernate.Mapping;
using Microsoft.AspNetCore.Identity;

namespace NHibernate.AspNetCore.Identity.Mappings.Base
{
    public class IdentityRoleMap<TKey> : ClassMap<IdentityRole<TKey>>
        where TKey : IEquatable<TKey>
    {

        public IdentityRoleMap() : this("Roles", "Id") {

        }

        public IdentityRoleMap(string tableName = "Roles", string keyColumnName = "Id") {
            Table(tableName);
            Id(e => e.Id)
                .Column(keyColumnName)
                .GeneratedBy.Native();
            Map(e => e.Name);
            Map(e => e.NormalizedName);
            Map(e => e.ConcurrencyStamp);
        }
    }
}
