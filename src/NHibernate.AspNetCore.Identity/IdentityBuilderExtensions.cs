using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace NHibernate.AspNetCore.Identity {

    public static class IdentityBuilderExtensions {

        public static IdentityBuilder AddHibernateStores(
            this IdentityBuilder builder
        ) {
            AddStores(builder.Services, builder.UserType, builder.RoleType);
            return builder;
        }

        private static void AddStores(
            IServiceCollection services,
            System.Type userType,
            System.Type roleType
        ) {
            if (roleType != null) {
                var userKeyType = userType.GetProperty("Id").PropertyType;
                //var userKeyType = userType.GenericTypeArguments[0];
                // register user store type
                var userStoreServiceType = typeof(IUserStore<>)
                    .MakeGenericType(userType);
                var userStoreImplType = typeof(UserStore<,,>)
                    .MakeGenericType(userType, roleType, userKeyType);
                services.AddScoped(userStoreServiceType, userStoreImplType);
                // add role store type
                var roleKeyType = roleType.GetProperty("Id").PropertyType;
                var roleStoreSvcType = typeof(IRoleStore<>)
                    .MakeGenericType(roleType);
                var roleStoreImplType = typeof(RoleStore<,>)
                    .MakeGenericType(roleType, roleKeyType);
                services.AddScoped(roleStoreSvcType, roleStoreImplType);
            }
            else {
                // register user only store type
                var userKeyType = userType.GetProperty("Id").PropertyType;
                var userStoreServiceType = typeof(IUserStore<>)
                    .MakeGenericType(userType);
                var userStoreImplType = typeof(UserOnlyStore<,>)
                    .MakeGenericType(userType, userKeyType);
                services.AddScoped(userStoreServiceType, userStoreImplType);
            }
        }
    }
}
