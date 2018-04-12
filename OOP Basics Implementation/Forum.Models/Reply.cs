using System;
using System.Collections.Generic;
using System.Text;

namespace Forum.Models
{
    public class Reply : Model
    {
        public Reply(params string[] args) : base(args)
        {
            var id = int.Parse(args[0]);
            var content = args[1];
            var authorId = int.Parse(args[2]);
            var postId = int.Parse(args[3]);

            BindData(id, content, authorId, postId);
        }

        public Reply(int id, string content, int authorId, int postId)
        {
            BindData(id, content, authorId, postId);
        }

        public int Id { get; set; }
        public string Content { get; set; }
        public int AuthorId { get; set; }
        public int PostId { get; set; }

        public override string ToString()
        {
            var output = $"{this.Id};{this.Content};{this.AuthorId};{this.PostId}";
            return output;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="content"></param>
        /// <param name="authorId"></param>
        /// <param name="postId"></param>
        private void BindData(int id, string content, int authorId, int postId)
        {
            this.Id = id;
            this.Content = content;
            this.AuthorId = authorId;
            this.PostId = postId;
        }
    }
}
