using System;

namespace YetAnotherTodo.WebApi.Models.Output
{
    public class TodoItem
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public bool Done { get; set; }
        public User User { get; set; }

        public static Func<Domain.TodoItem, TodoItem> Project = item => new TodoItem
        {
            Id = item.Id,
            Description = item.Description,
            Done = item.Done,
            User = new User { Id = item.User.Id }
        };
    }
}