using System;
using FluentNHibernate.Mapping;
using Microsoft.AspNetCore.Identity;

namespace NHibernate.AspNetCore.Identity.Mappings.Base {
    public class IdentityUserClaimMap<TKey> : ClassMap<IdentityUserClaim<TKey>>
        where TKey : IEquatable<TKey>
    {

        public IdentityUserClaimMap(): this("UserClaims", "Id") {}

        public IdentityUserClaimMap(string tableName = "UserClaims", string idColumn = "Id")
        {
            Table(tableName);
            Id(e => e.Id)
                .Column(idColumn)
                .GeneratedBy.Native();
            Map(e => e.ClaimType);
            Map(e => e.ClaimValue);
            Map(e => e.UserId);

        }

    }

}
