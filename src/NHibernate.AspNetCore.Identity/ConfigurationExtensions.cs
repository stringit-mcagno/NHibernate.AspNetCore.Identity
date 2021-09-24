using System;
using FluentNHibernate.Cfg;
using Microsoft.AspNetCore.Identity;
using NHibernate.AspNetCore.Identity.Mappings;
using NHibernate.AspNetCore.Identity.Mappings.Base;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;

namespace NHibernate.AspNetCore.Identity {

    public static class ConfigurationExtensions {

        public static void AddIdentityMappings<TUser, TKey>(this MappingConfiguration cfg)
            where TKey : IEquatable<TKey>
            where TUser : IdentityUser<TKey> {
            cfg.FluentMappings.Add<IdentityRoleMap<TKey>>();
            cfg.FluentMappings.Add<IdentityRoleClaimMap<TKey>>();
            cfg.FluentMappings.Add<IdentityUserMap<TUser, TKey>>();
            cfg.FluentMappings.Add<IdentityUserClaimMap<TKey>>();
            cfg.FluentMappings.Add<IdentityUserLoginMap<TKey>>();
            cfg.FluentMappings.Add<IdentityUserRoleMap<TKey>>();
            cfg.FluentMappings.Add<IdentityUserTokenMap<TKey>>();
        }
    }

}
