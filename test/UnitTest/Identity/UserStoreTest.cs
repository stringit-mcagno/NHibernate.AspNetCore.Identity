using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using NUnit.Framework;
using NHibernate.AspNetCore.Identity;
using NHibernate.NetCore;
using NHibernate;
using NHibernate.Linq;
using NHIdentityUser = NHibernate.AspNetCore.Identity.IdentityUser;
using NHIdentityRole = NHibernate.AspNetCore.Identity.IdentityRole;

namespace UnitTest.Identity {

    [TestFixture]
    public class UserStoreTest : BaseTest, IDisposable {

        private readonly UserStore<NHIdentityUser, NHIdentityRole, string> _store;
        private readonly ISessionFactory _sessionFactory;

        public UserStoreTest() {
            var builder = new LoggingBuilder();
            var loggerFactory = builder.BuildLoggerFactory();
            loggerFactory.UseAsHibernateLoggerFactory();
            var cfg = ConfigNHibernate();
            cfg.AddIdentityMappings();
            AddXmlMapping(cfg);
            _sessionFactory = cfg.BuildSessionFactory();
            _store = new UserStore<NHIdentityUser, NHIdentityRole, string>(
                _sessionFactory.OpenSession(),
                new IdentityErrorDescriber()
            );
        }

        public void Dispose() {
            _store?.Dispose();
            _sessionFactory?.Dispose();
        }

        [Test]
        public async Task _01_CanQueryAllUsers() {
            var users = await _store.Users.ToListAsync();
            Assert.NotNull(users);
            Assert.True(users.Count >= 0);
        }

        [Test]
        public async Task _02_CanDoCurd() {
            var user = new NHIdentityUser {
                UserName = "Beginor",
                NormalizedUserName = "BEGINOR",
                Email = "beginor@qq.com",
                PhoneNumber = "02000000000",
                PhoneNumberConfirmed = true,
                LockoutEnabled = false,
                LockoutEnd = null,
                AccessFailedCount = 0,
                NormalizedEmail = "BEGINOR@QQ.COM",
                PasswordHash = null,
                SecurityStamp = null
            };
            var result = await _store.CreateAsync(user);
            Assert.True(result.Succeeded);
            var id = user.Id;
            Assert.IsNotEmpty(id);
            Assert.IsNotEmpty(user.ConcurrencyStamp);

            user.LockoutEnabled = true;
            user.LockoutEnd = DateTimeOffset.UtcNow.AddMinutes(20);
            result = await _store.UpdateAsync(user);
            Assert.True(result.Succeeded);

            var lockouts = await _store.Users
                .Where(u => u.LockoutEnabled)
                .CountAsync();
            Assert.True(lockouts > 0);

            user = await _store.FindByEmailAsync(user.NormalizedEmail);
            Assert.True(user.Id == id);

            user = await _store.FindByNameAsync(user.NormalizedUserName);
            Assert.True(user.Id == id);

            user = await _store.FindByIdAsync(id);
            Assert.True(user.Id == id);

            var claim = new Claim("Test", Guid.NewGuid().ToString("N"));
            await _store.AddClaimsAsync(user, new [] { claim });
            var claims = await _store.GetClaimsAsync(user);
            Assert.True(claims.Count > 0);

            var users = await _store.GetUsersForClaimAsync(claim);
            Assert.IsNotEmpty(users);

            await _store.RemoveClaimsAsync(user, claims);

            var loginInfo = new Microsoft.AspNetCore.Identity.UserLoginInfo(
                "test",
                Guid.NewGuid().ToString("N"),
                "Test"
            );
            await _store.AddLoginAsync(user, loginInfo);
            await _store.SetTokenAsync(
                user,
                loginInfo.LoginProvider,
                loginInfo.ProviderDisplayName,
                loginInfo.ProviderKey,
                CancellationToken.None
            );

            await _store.RemoveTokenAsync(
                user,
                loginInfo.LoginProvider,
                loginInfo.ProviderDisplayName,
                CancellationToken.None
            );

            await _store.RemoveLoginAsync(
                user,
                loginInfo.LoginProvider,
                loginInfo.ProviderKey
            );

            result = await _store.DeleteAsync(user);
            Assert.True(result.Succeeded);
        }

        [Test]
        public async Task _03_CanGetRolesForUser() {
            using var session = _sessionFactory.OpenSession();
            var user = new NHIdentityUser { Id = "1579928865223010012" };
            // Assert.IsNotNull(user);
            // var userId = user.Id;
            // var query = from userRole in session.Query<IdentityUserRole>()
            //     join role in session.Query<NHIdentityRole>() on userRole.RoleId equals role.Id
            //     where userRole.UserId == userId
            //     select role.Name;
            // var roles = await query.ToListAsync(CancellationToken.None);
            var roles = await _store.GetRolesAsync(user);
            Console.WriteLine(roles.Count);
        }
    }

}
