﻿using Forum.App.Contracts;
using Forum.App.Models.ViewModels;
using Forum.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Forum.App.Services
{
    public class PostService : IPostService
    {
        private ForumData forumData;
        private IUserService userService;

        public PostService(ForumData forumData, IUserService userService)
        {
            this.forumData = forumData;
            this.userService = userService;
        }

        public int AddPost(int userId, string postTitle, string postCategory, string postContent)
        {
            throw new NotImplementedException();
        }

        public void AddReplyToPost(int postId, string replyContents, int userId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ICategoryInfoViewModel> GetAllCategories()
        {
            var categories = this.forumData
                .Categories
                .Select(c => new CategoryInfoViewModel(c.Id, c.Name, c.Posts.Count));

            return categories;
        }

        public string GetCategoryName(int categoryId)
        {
            var categoryName = this.forumData.Categories.Find(c => c.Id == categoryId)?.Name;

            if (categoryName is null)
                throw new ArgumentException($"Category with id {categoryId} not found!");

            return categoryName;
        }

        public IEnumerable<IPostInfoViewModel> GetCategoryPostsInfo(int categoryId)
        {
            var posts = forumData
                .Posts
                .Where(p => p.CategoryId == categoryId)
                ?.Select(p => new PostInfoViewModel(p.Id, p.Title, p.Replies.Count));

            return posts;
        }

        public IPostViewModel GetPostViewModel(int postId)
        {
            throw new NotImplementedException();
        }
    }
}
