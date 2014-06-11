namespace YetAnotherTodo.Mvc.Models
{
    public class TodoItem
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public bool Done { get; set; }
        public User User { get; set; }
    }

    public class User
    {
        public string Id { get; set; }
    }
}