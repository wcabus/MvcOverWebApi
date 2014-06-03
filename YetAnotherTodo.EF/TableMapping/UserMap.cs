using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using YetAnotherTodo.Domain;

namespace YetAnotherTodo.EF.TableMapping
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            ToTable("Users");

            Property(u => u.UserName)
                .IsUnicode()
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("UserNameIndex") { IsUnique = true }));

            Property(u => u.Email).IsUnicode().IsRequired().HasMaxLength(256);

            Property(u => u.FirstName).IsUnicode().HasMaxLength(128);
            Property(u => u.LastName).IsUnicode().HasMaxLength(128);

            //Foreign keys
            HasMany(u => u.Roles).WithRequired().HasForeignKey(ur => ur.UserId);
        }
    }
}