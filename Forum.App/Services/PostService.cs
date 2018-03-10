using Forum.App.UserInterface.ViewModels;
using Forum.Data;
using Forum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Forum.App.Services
{
    public static class PostService
    {
        public static PostViewModel GetPostViewModel(int postId)
        {
            var post = ForumData.GetInstance().Posts.Find(p => p.Id == postId);
            var postViewModel = new PostViewModel(post);

            return postViewModel;
        }

        public static bool TryAddReply(PostViewModel post, ReplyViewModel reply)
        {
            if (reply.Content is null)
                return false;

            var replyContent = string.Join("", reply.Content);

            if (string.IsNullOrWhiteSpace(replyContent))
                return false;

            var replies = ForumData.GetInstance().Replies;

            var author = ForumData.GetInstance().Users.Find(u => u.Username == reply.Author);
            var replyId = replies.Any() ? replies.Last().Id + 1 : 1;

            var replyModel = new Reply(replyId, replyContent, author.Id, post.PostId);
            replies.Add(replyModel);

            ForumData.GetInstance().SaveChanges();
            return true;
        }

        public static bool TrySavePost(PostViewModel postView)
        {
            var emptyCategory = string.IsNullOrWhiteSpace(postView.Category);
            var emptyTitle = string.IsNullOrWhiteSpace(postView.Title);
            var emptyContent = !postView.Content.Any();

            if (emptyCategory || emptyTitle || emptyContent)
                return false;

            var category = EnsureCategory(postView, ForumData.GetInstance());

            var posts = ForumData.GetInstance().Posts;
            var postId = posts.Any() ? posts.Last().Id + 1 : 1;

            var author = UserService.GetUser(postView.Author);

            var content = string.Join("", postView.Content);

            var post = new Post(postId, postView.Title, content, category.Id, author.Id);

            posts.Add(post);
            author.Posts.Add(post.Id);
            category.Posts.Add(post.Id);

            ForumData.GetInstance().SaveChanges();

            postView.PostId = postId;

            return true;
        }

        internal static Category GetCategory(int categoryId)
        {
            var category = ForumData.GetInstance().Categories.Find(c => c.Id == categoryId);
            return category;
        }

        internal static IList<ReplyViewModel> GetPostReplies(int postId)
        {
            var post = ForumData.GetInstance().Posts.Find(p => p.Id == postId);

            var replies = ForumData.GetInstance().Replies
                .Where(r => r.PostId == post.Id)
                .Select(r => new ReplyViewModel(r))
                .ToList();

            return replies;
        }

        internal static string[] GetAllGategoryNames()
        {
            var allCategories = ForumData.GetInstance()
                .Categories
                .Select(c => c.Name)
                .ToArray();

            return allCategories;
        }

        internal static IEnumerable<Post> GetPostsByCategory(int categoryId)
        {
            var postIds = ForumData.GetInstance().Categories
                .First(c => c.Id == categoryId).Posts.ToHashSet();

            var posts = ForumData.GetInstance()
                .Posts.Where(p => postIds.Contains(p.Id));

            return posts;
        }

        private static Category EnsureCategory(PostViewModel postView, ForumData forumData)
        {
            var categoryName = postView.Category;
            var category = forumData.Categories.FirstOrDefault(c => c.Name == categoryName);

            if(category is null)
            {
                var categories = forumData.Categories;
                var categoryId = categories.Any() ? categories.Last().Id + 1 : 1;
                category = new Category(categoryId, categoryName);

                forumData.Categories.Add(category);
            }

            return category;
        }
    }
}
