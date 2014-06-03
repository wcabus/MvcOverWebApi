using System.Data.Entity.ModelConfiguration;
using YetAnotherTodo.Domain;

namespace YetAnotherTodo.EF.TableMapping
{
    public class UserRoleMap : EntityTypeConfiguration<UserRole>
    {
        public UserRoleMap()
        {
            ToTable("UserRoles");

            HasKey(ur => new {ur.UserId, ur.RoleId});
        }
    }
}