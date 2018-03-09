using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Forum.Models
{
    public class User : Model
    {
        public User(params string[] args) : base(args)
        {
            var id = int.Parse(args[0]);
            var username = args[1];
            var password = args[2];

            var posts = args[3]
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();

            BindData(id,username,password,posts);
        }

        public User(int id, string username, string password, ICollection<int> posts)
        {
            BindData(id, username, password, posts);
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public ICollection<int> PostIds { get; set; }

        public override string ToString()
        {
            var output = $"{this.Id};{this.Username};{this.Password};{string.Join(',', this.PostIds)}";
            return output;
        }

        /// <summary>
        /// Using this method to fill in the main data attributes without repeating code.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="posts"></param>
        private void BindData(int id, string username, string password, ICollection<int> posts)
        {
            this.Id = id;
            this.Username = username;
            this.Password = password;
            this.PostIds = posts;
        }
    }
}
