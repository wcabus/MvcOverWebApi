using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using YetAnotherTodo.Domain;
using YetAnotherTodo.EF.TableMapping;

namespace YetAnotherTodo.EF
{
    public class YetAnotherTodoDbContext<TUser> : DbContext where TUser : User
    {
        public YetAnotherTodoDbContext()
            : base("YetAnotherTodoConnectionString")
        {
            //Disable lazy loading and proxy creation
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;

            //Database initialization
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<YetAnotherTodoDbContext<TUser>>());
        }

        public static YetAnotherTodoDbContext<TUser> Create()
        {
            return new YetAnotherTodoDbContext<TUser>();
        }

        public IDbSet<TUser> Users
        {
            get { return Set<TUser>(); }
        }

        public IDbSet<Role> Roles
        {
            get { return Set<Role>(); }
        }

        public IDbSet<TodoItem> TodoItems
        {
            get { return Set<TodoItem>(); }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserRoleMap());
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new RoleMap());
            modelBuilder.Configurations.Add(new TodoItemMap());
        }

        protected override DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            var errors = new List<DbValidationError>();

            //Have we added a new, non-null item?
            if (entityEntry != null && entityEntry.State == EntityState.Added)
            {
                CheckIfValidUserWasAdded(entityEntry.Entity as User, errors);
                CheckIfValidRoleWasAdded(entityEntry.Entity as Role, errors);
            }

            //If there are any errors, return the error list
            if (errors.Any())
            {
                return new DbEntityValidationResult(entityEntry, errors);
            }

            return base.ValidateEntity(entityEntry, items);
        }

        private void CheckIfValidUserWasAdded(User user, List<DbValidationError> errors)
        {
            if (user == null)
            {
                return;
            }

            if (Users.Any(u => string.Equals(u.UserName, user.UserName)))
            {
                errors.Add(new DbValidationError("User", string.Format("There already is another user with user name \"{0}\".", user.UserName)));
            }

            //Email must be unique too
            if (Users.Any(u => string.Equals(u.Email, user.Email)))
            {
                errors.Add(new DbValidationError("User", string.Format("There already is another user that has registered with the email address {0}.", user.Email)));
            }
        }

        private void CheckIfValidRoleWasAdded(Role role, List<DbValidationError> errors)
        {
            if (role == null)
            {
                return;
            }

            if (Roles.Any(u => string.Equals(u.Name, role.Name)))
            {
                errors.Add(new DbValidationError("Role", string.Format("There already is another role with the name \"{0}\".", role.Name)));
            }
        }
    }
}
