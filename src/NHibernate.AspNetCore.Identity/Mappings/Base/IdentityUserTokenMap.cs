using System;
using FluentNHibernate.Mapping;
using NHibernate.AspNetCore.Identity.Entities;

namespace NHibernate.AspNetCore.Identity.Mappings.Base {

    public class IdentityUserTokenMap<TKey> : ClassMap<IdentityUserToken<TKey>>
        where TKey : IEquatable<TKey>
    {
        public IdentityUserTokenMap(): this("UserTokens") {

        }
        public IdentityUserTokenMap(string tableName = "UserTokens") {
            Table(tableName);
            /*Map(e => e.UserId);
            Map(e => e.LoginProvider);
            Map(e => e.Name);*/
            CompositeId()
                .KeyProperty(e => e.UserId)
                .KeyProperty(e => e.LoginProvider)
                .KeyProperty(e => e.Name);
        }

    }



}
