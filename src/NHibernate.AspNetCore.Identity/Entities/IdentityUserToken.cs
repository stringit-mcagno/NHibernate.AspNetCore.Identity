using System;

namespace NHibernate.AspNetCore.Identity.Entities {
    public class IdentityUserToken<TKey> : Microsoft.AspNetCore.Identity.IdentityUserToken<TKey>
        where TKey : IEquatable<TKey>
    {
        public override bool Equals(object obj) {
            var x = (IdentityUserToken<TKey>) obj;
            return UserId.Equals(x.UserId) &&
                   LoginProvider.Equals(x.LoginProvider) &&
                   Name.Equals(x.Name);
        }

        public override int GetHashCode()
        {
            return (UserId + "|" + LoginProvider + "|" + Name).GetHashCode();
        }

    }
}
