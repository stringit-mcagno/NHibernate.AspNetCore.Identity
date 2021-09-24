
using System;

namespace NHibernate.AspNetCore.Identity.Entities {
    public class IdentityUserLogin<TKey> : Microsoft.AspNetCore.Identity.IdentityUserLogin<TKey>
        where TKey : IEquatable<TKey>
    {
        public override bool Equals(object obj)
        {
            var userLogin = (IdentityUserLogin<TKey>) obj;
            return LoginProvider.Equals(userLogin.LoginProvider) &&
                   ProviderKey.Equals(userLogin.ProviderKey);
        }

        public override int GetHashCode()
        {
            return (LoginProvider + "|" + ProviderKey).GetHashCode();
        }
    }
}
