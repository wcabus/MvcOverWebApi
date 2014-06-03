using System;

namespace YetAnotherTodo.Domain
{
    public class TodoItem
    {
        /// <summary>
        /// Creates a new TodoItem
        /// </summary>
        public TodoItem()
        {
            Id = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// The key 
        /// </summary>
        public string Id { get; set; }
        
        /// <summary>
        /// The description of the todo-item
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Is it done?
        /// </summary>
        public bool Done { get; set; }

        /// <summary>
        /// The user that owns this todo-item
        /// </summary>
        public User User { get; set; }
    }
}