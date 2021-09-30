using System;
using FluentNHibernate.Mapping;
using Microsoft.AspNetCore.Identity;

namespace NHibernate.AspNetCore.Identity.Mappings.Base {

    public class IdentityUserMap<TUser, TKey> : ClassMap<TUser>
        where TUser : IdentityUser<TKey>
        where TKey : IEquatable<TKey>
    {
        public IdentityUserMap() :this("Users", "Id", "UserName"){

        }
        public IdentityUserMap(string tableName = "Users", string idColumn = "Id", string userNameColumn = "UserName")
        {
            Table(tableName);
            Id(e => e.Id)
                .Column(idColumn)
                .GeneratedBy.Native();
            Map(e => e.UserName)
                .Column(userNameColumn);
            Map(e => e.NormalizedUserName);
            Map(e => e.Email);
            Map(e => e.NormalizedEmail);
            Map(e => e.EmailConfirmed);
            Map(e => e.PhoneNumber);
            Map(e => e.PhoneNumberConfirmed);
            Map(e => e.LockoutEnabled);
            Map(e => e.LockoutEnd);
            Map(e => e.AccessFailedCount);
            Map(e => e.ConcurrencyStamp);
            Map(e => e.PasswordHash);
            Map(e => e.TwoFactorEnabled);
            Map(e => e.SecurityStamp);
        }

    }

}
