using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLib.Models
{
    /// <summary>
    /// The User class represents a user entity with properties for username and password.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Represents the username of the user.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Represents the password of the user.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Initializes the Username and Password properties when creating a new User object.
        /// </summary>
        /// <param name="username">Username of the user.</param>
        /// <param name="password">Password of the user.</param>
        public User(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
