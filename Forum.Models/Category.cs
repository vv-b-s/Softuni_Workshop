using System;
using System.Collections.Generic;
using System.Linq;

namespace Forum.Models
{
    public class Category : Model
    {
        public Category(params string[] args) : base(args)
        {
            var id = int.Parse(args[0]);
            var name = args[1];

            var postIds = new int[0];

            if (args.Length > 2)
                postIds = args[2]
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToArray();

            BindData(id, name, postIds);
        }

        public Category(int id, string name) : this(id, name, new int[0]) { }

        public Category(int id, string name, ICollection<int> posts)
        {
            BindData(id, name, posts);
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public IList<int> Posts { get; set; }

        public override string ToString()
        {
            var outout = $"{this.Id};{this.Name};{string.Join(',', this.Posts)}";
            return outout;
        }

        /// <summary>
        /// Using this method to fill in the main data attributes without repeating code.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="posts"></param>
        private void BindData(int id, string name, ICollection<int> posts)
        {
            this.Id = id;
            this.Name = name;
            this.Posts = posts.ToList();
        }
    }
}
