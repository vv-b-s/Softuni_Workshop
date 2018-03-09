using Forum.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forum.Data
{
    public class ForumData
    {
        public ForumData()
        {
            DataMapper.GetDataMapper();

            this.Users = DataMapper.LoadModels<User>();
            this.Categories = DataMapper.LoadModels<Category>();
            this.Posts = DataMapper.LoadModels<Post>();
            this.Replies = DataMapper.LoadModels<Reply>();
        }

        public List<Category> Categories { get; set; }
        public List<User> Users { get; set; }
        public List<Post> Posts { get; set; }
        public List<Reply> Replies { get; set; }

        public void SaveChanges()
        {
            DataMapper.SaveChanges(this.Users);
            DataMapper.SaveChanges(this.Categories);
            DataMapper.SaveChanges(this.Replies);
            DataMapper.SaveChanges(this.Posts);
        }

    }
}
