using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YetAnotherTodo.Domain;

namespace YetAnotherTodo.EF.Repository
{
    public class TodoItemRepository : IDisposable
    {
        private bool _disposed; // = false

        public TodoItemRepository(DbContext context)
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

        public IQueryable<TodoItem> TodoItems
        {
            get
            {
                return Context.Set<TodoItem>();
            }
        }

        public async Task<TodoItem> FindByIdAsync(string id)
        {
            return await GetTodoItemAggregate(td => string.Equals(td.Id, id)).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TodoItem>> FindByUserNameAsync(string userName)
        {
            return await GetTodoItemAggregate(td => string.Equals(td.User.UserName, userName)).ToListAsync();
        }

        public async Task<TodoItem> FindByIdAndUserNameAsync(string id, string userName)
        {
            return await GetTodoItemAggregate(td => string.Equals(td.Id, id) && string.Equals(td.User.UserName, userName)).FirstOrDefaultAsync();
        }

        private IQueryable<TodoItem> GetTodoItemAggregate(Expression<Func<TodoItem, bool>> filter)
        {
            return TodoItems.Include(u => u.User).Where(filter);
        }

        public async Task CreateAsync(TodoItem item)
        {
            ThrowIfDisposed();

            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            Context.Set<TodoItem>().Add(item);
            await SaveChanges().ConfigureAwait(false);
        }

        public async Task UpdateAsync(TodoItem item)
        {
            ThrowIfDisposed();
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            Context.Entry(item).State = EntityState.Modified;
            await SaveChanges().ConfigureAwait(false);
        }

        public async Task DeleteAsync(TodoItem item)
        {
            ThrowIfDisposed();
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            Context.Set<TodoItem>().Remove(item);
            await SaveChanges().ConfigureAwait(false);
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