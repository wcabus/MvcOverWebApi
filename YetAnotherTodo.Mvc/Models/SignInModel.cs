using System.ComponentModel.DataAnnotations;

namespace YetAnotherTodo.Mvc.Models
{
    public class SignInModel
    {
        [Required, Display(Name = "Email address")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}