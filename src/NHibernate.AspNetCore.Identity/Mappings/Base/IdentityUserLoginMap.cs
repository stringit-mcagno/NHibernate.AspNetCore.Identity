using System;
using FluentNHibernate.Mapping;
using NHibernate.AspNetCore.Identity.Entities;

namespace NHibernate.AspNetCore.Identity.Mappings.Base {

    public class IdentityUserLoginMap<TKey> : ClassMap<IdentityUserLogin<TKey>> where TKey : IEquatable<TKey> {

        public IdentityUserLoginMap(): this("UserLogins") {
        }

        public IdentityUserLoginMap(string tableName = "UserLogins") {
            Table(tableName);
            CompositeId()
                .KeyProperty(e => e.LoginProvider)
                .KeyProperty(e => e.ProviderKey);
            Map(e => e.ProviderDisplayName);
            Map(e => e.UserId);
        }

    }



}
