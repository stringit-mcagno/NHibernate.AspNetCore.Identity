using System;

namespace NHibernate.AspNetCore.Identity.Entities {
    public class IdentityUserRole<TKey> : Microsoft.AspNetCore.Identity.IdentityUserRole<TKey>
        where TKey : IEquatable<TKey>
    {

        public override bool Equals(object obj) {
            var userRole = (IdentityUserRole<TKey>)obj;
            return UserId.Equals(userRole.UserId) &&
                   RoleId.Equals(userRole.RoleId);
        }

        public override int GetHashCode()
        {
            return (UserId + "|" + RoleId).GetHashCode();
        }
    }

}
