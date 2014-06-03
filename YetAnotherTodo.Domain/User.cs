using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;

namespace YetAnotherTodo.Domain
{
    public class User : IUser<string>
    {
        /// <summary>
        /// Creates a new empty User.
        /// </summary>
        public User()
        {
            Id = Guid.NewGuid().ToString();

            Roles = new List<UserRole>();
        }

        /// <summary>
        /// Creates a new User entity with the given <paramref name="userName"/>
        /// </summary>
        /// <param name="userName"></param>
        public User(string userName) : this()
        {
            UserName = userName;
        }

        /// <summary>
        /// The Id
        /// </summary>
        public string Id { get; set; } //Implements IUser<string>.Id{get}

        /// <summary>
        /// The username 
        /// </summary>
        public string UserName { get; set; } //Implements IUser<string>.UserName{get;set;}

        /// <summary>
        /// The users email address
        /// </summary>
        public string Email { get; set; }
        
        /// <summary>
        /// The salted/hashed form of the user password
        /// </summary>
        public virtual string PasswordHash { get; set; }

        /// <summary>
        /// A random value that should change whenever a users credentials have changed (password changed, login removed)
        /// </summary>
        public virtual string SecurityStamp { get; set; }

        /// <summary>
        /// The users first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The users surname
        /// </summary>
        public string LastName { get; set; }

        public ICollection<UserRole> Roles { get; set; }
    }
}
