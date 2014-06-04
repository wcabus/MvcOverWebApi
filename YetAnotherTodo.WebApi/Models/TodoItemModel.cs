using System.ComponentModel.DataAnnotations;

namespace YetAnotherTodo.WebApi.Models
{
    public class TodoItemModel
    {
        [Required]
        public string Description { get; set; } 
    }
}