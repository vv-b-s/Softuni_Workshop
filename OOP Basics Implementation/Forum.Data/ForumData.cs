using Forum.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forum.Data
{
    /**
     * Yet another singleton class. There is only one forum data instance needed
     */
    public class ForumData :IDisposable
    {
        private static ForumData instance;

        private ForumData()
        {
            DataMapper.GetInstance();

            this.Users = DataMapper.LoadModels<User>();
            this.Categories = DataMapper.LoadModels<Category>();
            this.Posts = DataMapper.LoadModels<Post>();
            this.Replies = DataMapper.LoadModels<Reply>();
        }

        public List<Category> Categories { get; set; }
        public List<User> Users { get; set; }
        public List<Post> Posts { get; set; }
        public List<Reply> Replies { get; set; }

        public static ForumData GetInstance()
        {
            if (instance is null)
                instance = new ForumData();

            return instance;
        }

        ~ForumData()
        {
            this.Dispose();
        }

        public void Dispose()
        {
            this.SaveChanges();

            this.Categories = null;
            this.Users = null;
            this.Posts = null;
            this.Replies = null;
            instance = null;
        }

        public void SaveChanges()
        {
            DataMapper.SaveChanges(this.Users);
            DataMapper.SaveChanges(this.Categories);
            DataMapper.SaveChanges(this.Replies);
            DataMapper.SaveChanges(this.Posts);
        }
    }
}
