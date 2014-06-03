using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;

namespace YetAnotherTodo.Domain
{
    public class Role : IRole<string>
    {
        /// <summary>
        /// Creates a new role with a unique Id
        /// </summary>
        public Role()
        {
            Id = Guid.NewGuid().ToString();
            Users = new List<UserRole>();
        }

        /// <summary>
        /// Creates a new Role with the given name
        /// </summary>
        /// <param name="name"></param>
        public Role(string name) : this()
        {
            Name = name;
        }

        public string Id { get; set; }
        public string Name { get; set; }

        public ICollection<UserRole> Users { get; set; }
    }
}