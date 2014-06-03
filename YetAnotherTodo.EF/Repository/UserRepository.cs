using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using YetAnotherTodo.Domain;

namespace YetAnotherTodo.EF.Repository
{
    public class UserRepository<TUser> : 
        IUserStore<TUser>, IUserPasswordStore<TUser>, IUserEmailStore<TUser>
        where TUser : User
    {
        private bool _disposed; // = false

        public UserRepository(DbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            Context = context;
            AutoSaveChanges = true;
        }

        public DbContext Context { get; private set; }

        public bool AutoSaveChanges { get; set; }
        public bool DisposeContext { get; set; } //Default = false

        public IQueryable<TUser> Users
        {
            get { return Context.Set<TUser>(); }
        }

        private Task<TUser> GetUserAggregateAsync(Expression<Func<TUser, bool>> filter)
        {
            return Users.AsQueryable().Include(u => u.Roles).FirstOrDefaultAsync(filter);
        }

        public Task<TUser> FindByIdAsync(string userId)
        {
            ThrowIfDisposed();
            return GetUserAggregateAsync(u => u.Id.Equals(userId));
        }

        public Task<TUser> FindByNameAsync(string userName)
        {
            ThrowIfDisposed();
            return GetUserAggregateAsync(u => u.UserName.Equals(userName));
        }

        public async Task CreateAsync(TUser user)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            
            Context.Set<TUser>().Add(user);
            await SaveChanges().ConfigureAwait(false);
        }

        public async Task UpdateAsync(TUser user)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            Context.Entry(user).State = EntityState.Modified;
            await SaveChanges().ConfigureAwait(false);
        }

        public async Task DeleteAsync(TUser user)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            Context.Set<TUser>().Remove(user);
            await SaveChanges().ConfigureAwait(false);
        }

        public Task<string> GetPasswordHashAsync(TUser user)
        {
            ThrowIfDisposed();
            
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(TUser user)
        {
            return Task.FromResult(user.PasswordHash != null);
        }

        public Task SetPasswordHashAsync(TUser user, string passwordHash)
        {
            ThrowIfDisposed();
            
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public Task SetEmailAsync(TUser user, string email)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.Email = email;
            return Task.FromResult(0);
        }

        public Task<string> GetEmailAsync(TUser user)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(TUser user)
        {
            return Task.FromResult(true);
        }

        public Task SetEmailConfirmedAsync(TUser user, bool confirmed)
        {
            return Task.FromResult(0);
        }

        public Task<TUser> FindByEmailAsync(string email)
        {
            return GetUserAggregateAsync(u => string.Equals(u.Email, email));
        }

        private async Task SaveChanges()
        {
            if (AutoSaveChanges)
            {
                await Context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (DisposeContext && disposing && Context != null)
            {
                Context.Dispose();
            }
            
            _disposed = true;
            Context = null;
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }
    }
}