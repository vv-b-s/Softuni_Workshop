using Forum.App.Contracts;
using Forum.App.Models.ViewModels;
using Forum.Data;
using Forum.DataModels;
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
            var emptyCategory = string.IsNullOrWhiteSpace(postCategory);
            var emptyTitle = string.IsNullOrWhiteSpace(postTitle);
            var emptyContent = string.IsNullOrWhiteSpace(postContent);

            if (emptyCategory || emptyContent || emptyTitle)
                throw new ArgumentException("All fields must be filled!");

            var category = this.EnsureCategory(postCategory);
            var postId = this.forumData.Posts.Any() ? forumData.Posts.Last().Id + 1 : 1;

            var author = this.userService.GetUserById(userId);

            var post = new Post(postId, postTitle, postContent, category.Id, userId, new List<int>());

            this.forumData.Posts.Add(post);
            author.Posts.Add(post.Id);
            category.Posts.Add(postId);
            this.forumData.SaveChanges();

            return post.Id;
        }

        public void AddReplyToPost(int postId, string replyContents, int userId)
        {
            var post = this.forumData.Posts.FirstOrDefault(p => p.Id == postId);
            var user = this.forumData.Users.FirstOrDefault(u => u.Id == userId);

            var replyId = this.forumData.Replies.Any() ? this.forumData.Replies.Last().Id + 1 : 1;
            var reply = new Reply(replyId, replyContents, userId, postId);

            this.forumData.Replies.Add(reply);
            post.Replies.Add(replyId);
            this.forumData.SaveChanges();
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
            var post = this.forumData.Posts.FirstOrDefault(p => p.Id == postId);
            var postViewModel = new PostViewModel(post.Title, this.userService.GetUserName(post.AuthorId), post.Content, this.GetPostReplies(postId));

            return postViewModel;
        }

        private IEnumerable<IReplyViewModel> GetPostReplies(int postId)
        {
            var replies = this.forumData.Replies.Where(r => r.PostId == postId).Select(r => new ReplyViewModel(this.userService.GetUserName(r.AuthorId), r.Content));

            return replies;
        }

        private Category EnsureCategory(string postCategory)
        {
            var category = this.forumData.Categories.FirstOrDefault(c => c.Name == postCategory);

            if(category is null)
            {
                var categoryId = this.forumData.Categories.Any() ? forumData.Categories.Last().Id + 1 : 1;
                category = new Category(categoryId, postCategory, new List<int>());
                forumData.Categories.Add(category);
            }

            return category;
        }
    }
}
