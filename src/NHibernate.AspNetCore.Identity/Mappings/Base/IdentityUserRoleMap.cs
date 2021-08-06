using System;
using FluentNHibernate.Mapping;
using NHibernate.AspNetCore.Identity.Entities;

namespace NHibernate.AspNetCore.Identity.Mappings.Base {

    public class IdentityUserRoleMap<TKey> : ClassMap<IdentityUserRole<TKey>> where TKey : IEquatable<TKey>
    {
        public IdentityUserRoleMap() :this ("UserRoles") {

        }
        public IdentityUserRoleMap(string tableName = "UserRoles")
        {
            Table(tableName);
            /*Map(e => e.UserId);
            Map(e => e.RoleId);*/
            CompositeId()
                .KeyProperty(e => e.UserId)
                .KeyProperty(e => e.RoleId);
        }

    }

}
