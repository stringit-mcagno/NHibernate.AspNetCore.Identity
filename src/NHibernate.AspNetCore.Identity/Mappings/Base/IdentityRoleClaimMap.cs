using System;
using FluentNHibernate.Mapping;
using Microsoft.AspNetCore.Identity;

namespace NHibernate.AspNetCore.Identity.Mappings.Base {

    public class IdentityRoleClaimMap<TKey> : ClassMap<IdentityRoleClaim<TKey>>
        where TKey : IEquatable<TKey> {
        public IdentityRoleClaimMap() : this("RoleClaims", "Id") {
        }


        public IdentityRoleClaimMap(string tableName = "RoleClaims", string idColumnName = "Id")
            {

                Table(tableName);
                Id(e => e.Id)
                    .Column(idColumnName)
                    .GeneratedBy.Native();
                Map(e => e.ClaimType);
                Map(e => e.ClaimValue);
                Map(e => e.RoleId);
            }

        }

}
