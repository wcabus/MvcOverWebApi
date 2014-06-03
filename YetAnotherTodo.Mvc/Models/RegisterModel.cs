using System.ComponentModel.DataAnnotations;

namespace YetAnotherTodo.Mvc.Models
{
    public class RegisterModel
    {
        [Required, DataType(DataType.EmailAddress), MaxLength(256), Display(Name = "Email address")]
        public string Email { get; set; }

        [Required, MaxLength(128), Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required, MaxLength(128), Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required, MaxLength(100), MinLength(6)]
        public string Password { get; set; }
        
        [Required, Compare("Password"), Display(Name = "Password (again)")]
        public string ConfirmPassword { get; set; }
    }
}