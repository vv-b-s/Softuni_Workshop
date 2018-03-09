﻿using System;
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

            var postIds = args[2]
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();

            BindData(id, name, postIds);
        }

        public Category(int id, string name, ICollection<int> posts)
        {
            BindData(id, name, posts);
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<int> PostIds { get; set; }

        public override string ToString()
        {
            var outout = $"{this.Id};{this.Name};{string.Join(',', this.PostIds)}";
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
            this.PostIds = posts;
        }
    }
}
