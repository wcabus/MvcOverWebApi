using System.Data.Entity.ModelConfiguration;
using YetAnotherTodo.Domain;

namespace YetAnotherTodo.EF.TableMapping
{
    public class TodoItemMap : EntityTypeConfiguration<TodoItem>
    {
        public TodoItemMap()
        {
            ToTable("TodoItems");

            Property(t => t.Description).IsUnicode().IsRequired();

            HasRequired(t => t.User).WithMany().Map(m => m.MapKey("UserId"));
        }
    }
}