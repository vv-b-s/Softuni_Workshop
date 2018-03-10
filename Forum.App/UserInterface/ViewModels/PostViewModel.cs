namespace Forum.App.UserInterface.ViewModels
{
    using Forum.App.Services;
    using Forum.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PostViewModel
    {
        private const int LINE_LENGHT = 37;

        public PostViewModel() { }

        public PostViewModel(Post post)
        {
            this.PostId = post.Id;
            this.Title = post.Title;
            this.Content = GetLines(post.Content);
            this.Author = UserService.GetUser(post.AuthorId).Username;
            this.Category = PostService.GetCategory(post.CategoryId).Name;
            this.Replies = PostService.GetPostReplies(post.Id);
        }

        public int PostId { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public string Category { get; set; }

        public IList<string> Content { get; set; }

        public IList<ReplyViewModel> Replies { get; set; }

        /// <summary>
        /// Wraps the text by a given constant so it appears pretty
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private IList<string> GetLines(string content)
        {
            var contentChars = content.ToCharArray();
            var lines = new List<string>();

            for (int i = 0; i < content.Length; i+=LINE_LENGHT)
            {
                var line = contentChars.Skip(i).Take(LINE_LENGHT);
                lines.Add(string.Join("", line));
            }

            return lines;
        }

    }
}
