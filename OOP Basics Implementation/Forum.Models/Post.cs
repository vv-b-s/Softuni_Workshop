using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Forum.Models
{
    public class Post : Model
    {
        public Post(params string[] args) : base(args)
        {
            var id = int.Parse(args[0]);
            var title = args[1];
            var content = args[2];
            var categoryId = int.Parse(args[3]);
            var authorId = int.Parse(args[4]);

            var replyIds = new int[0];

            if (args.Length > 5)
                replyIds = args[5]
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToArray();

            BindData(id, title, content, categoryId, authorId, replyIds);
        }

        public Post(int id, string title, string content, int categoryId, int authorId) : this(id, title, content, categoryId, authorId, new int[0]) { }

        public Post(int id, string title, string content, int categoryId, int authorId, ICollection<int> replyIds)
        {
            BindData(id, title, content, categoryId, authorId, replyIds);
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int CategoryId { get; set; }
        public int AuthorId { get; set; }
        public IList<int> ReplyIds { get; set; }

        public override string ToString()
        {
            var output = $"{this.Id};{this.Title};{this.Content};{this.CategoryId};{this.AuthorId};{string.Join(',', this.ReplyIds)}";
            return output;
        }

        /// <summary>
        /// Using this method to fill in the main data attributes without repeating code.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="categoryId"></param>
        /// <param name="authorId"></param>
        /// <param name="replyIds"></param>
        private void BindData(int id, string title, string content, int categoryId, int authorId, ICollection<int> replyIds)
        {
            this.Id = id;
            this.Title = title;
            this.Content = content;
            this.CategoryId = categoryId;
            this.AuthorId = authorId;
            this.ReplyIds = replyIds.ToList();
        }
    }
}
