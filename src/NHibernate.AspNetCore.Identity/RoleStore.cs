using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using NHibernate.Linq;

namespace NHibernate.AspNetCore.Identity {

    public class RoleStore<TRole, TKey>
        : RoleStoreBase<TRole, TKey, IdentityUserRole<TKey>, IdentityRoleClaim<TKey>> where TRole : IdentityRole<TKey> where TKey : IEquatable<TKey> {

        private readonly ISession _session;

        public RoleStore(
            ISession session,
            IdentityErrorDescriber describer = null
        ) : base(describer) {
            this._session = session ?? throw new ArgumentNullException(nameof(session));
        }

        public override async Task<IdentityResult> CreateAsync(
            TRole role,
            CancellationToken cancellationToken = default
        ) {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null) {
                throw new ArgumentNullException(nameof(role));
            }
            await _session.SaveAsync(role, cancellationToken);
            await FlushChangesAsync(cancellationToken);
            return IdentityResult.Success;
        }

        public override async Task<IdentityResult> UpdateAsync(
            TRole role,
            CancellationToken cancellationToken = default
        ) {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null) {
                throw new ArgumentNullException(nameof(role));
            }
            var exists = await Roles.AnyAsync(
                r => r.Id.Equals(role.Id),
                cancellationToken
            );
            if (!exists) {
                return IdentityResult.Failed(
                    new IdentityError {
                        Code = "RoleNotExist",
                        Description = $"Role with {role.Id} does not exists."
                    }
                );
            }
            role.ConcurrencyStamp = Guid.NewGuid().ToString("N");
            await _session.MergeAsync(role, cancellationToken);
            await FlushChangesAsync(cancellationToken);
            return IdentityResult.Success;
        }

        public override async Task<IdentityResult> DeleteAsync(
            TRole role,
            CancellationToken cancellationToken = default
        ) {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null) {
                throw new ArgumentNullException(nameof(role));
            }
            await _session.DeleteAsync(role, cancellationToken);
            await FlushChangesAsync(cancellationToken);
            return IdentityResult.Success;
        }

        public override Task<string> GetRoleIdAsync(
            TRole role,
            CancellationToken cancellationToken = default
        ) {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null) {
                throw new ArgumentNullException(nameof(role));
            }
            return Task.FromResult(role.Id.ToString());
        }

        public override Task<string> GetRoleNameAsync(
            TRole role,
            CancellationToken cancellationToken = default
        ) {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null) {
                throw new ArgumentNullException(nameof(role));
            }
            return Task.FromResult(role.Name);
        }

        public override async Task<TRole> FindByIdAsync(
            string roleId,
            CancellationToken cancellationToken = default
        ) {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            var id = roleId;
            var role = await _session.GetAsync<TRole>(id, cancellationToken);
            return role;
        }

        public override async Task<TRole> FindByNameAsync(
            string normalizedRoleName,
            CancellationToken cancellationToken = default
        ) {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            var role = await Roles
                .FirstOrDefaultAsync(
                    r => r.NormalizedName == normalizedRoleName,
                    cancellationToken
                );
            return role;
        }

        public override IQueryable<TRole> Roles => _session.Query<TRole>();

        private IQueryable<IdentityRoleClaim<TKey>> RoleClaims => _session.Query<IdentityRoleClaim<TKey>>();

        public override async Task<IList<Claim>> GetClaimsAsync(
            TRole role,
            CancellationToken cancellationToken = default
        ) {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null) {
                throw new ArgumentNullException(nameof(role));
            }

            var claims = await RoleClaims
                .Where(rc => rc.RoleId.Equals(role.Id))
                .Select(c => new Claim(c.ClaimType, c.ClaimValue))
                .ToListAsync(cancellationToken);
            return claims;
        }

        public override async Task AddClaimAsync(
            TRole role,
            Claim claim,
            CancellationToken cancellationToken = default
        ) {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null) {
                throw new ArgumentNullException(nameof(role));
            }
            if (claim == null) {
                throw new ArgumentNullException(nameof(claim));
            }
            var roleClaim = CreateRoleClaim(role, claim);
            await _session.SaveAsync(roleClaim, cancellationToken);
            await FlushChangesAsync(cancellationToken);
        }

        public override async Task RemoveClaimAsync(
            TRole role,
            Claim claim,
            CancellationToken cancellationToken = default
        ) {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null) {
                throw new ArgumentNullException(nameof(role));
            }
            if (claim == null) {
                throw new ArgumentNullException(nameof(claim));
            }
            var claims = await RoleClaims.Where(
                    rc => rc.RoleId.Equals(role.Id)
                        && rc.ClaimValue == claim.Value &&
                        rc.ClaimType == claim.Type
                )
                .ToListAsync(cancellationToken);
            foreach (var c in claims) {
                await _session.DeleteAsync(c, cancellationToken);
            }
            await FlushChangesAsync(cancellationToken);
        }

        private async Task FlushChangesAsync(
            CancellationToken cancellationToken = default
        ) {
            await _session.FlushAsync(cancellationToken);
            _session.Clear();
        }

    }
}
