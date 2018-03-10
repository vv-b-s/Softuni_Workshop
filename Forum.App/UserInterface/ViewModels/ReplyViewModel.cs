namespace Forum.App.UserInterface.ViewModels
{
    using Forum.App.Services;
    using Forum.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ReplyViewModel
    {
        private const int LINE_LENGHT = 37;

        public ReplyViewModel() { }

        public ReplyViewModel(Reply reply)
        {
            this.Author = UserService.GetUser(reply.AuthorId).Username;
            this.Content = GetLines(reply.Content);
        }


        public string Author { get; set; }

        public IList<string> Content { get; set; }

        /// <summary>
        /// Wraps the text by a given constant so it appears pretty
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private IList<string> GetLines(string content)
        {
            var contentChars = content.ToCharArray();
            var lines = new List<string>();

            for (int i = 0; i < content.Length; i += LINE_LENGHT)
            {
                var line = contentChars.Skip(i).Take(LINE_LENGHT);
                lines.Add(string.Join("", line));
            }

            return lines;
        }
    }
}