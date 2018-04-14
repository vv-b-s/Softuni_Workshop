using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Forum.App.Models.ViewModels
{
    public abstract class ContentViewModel
    {
        private const int LineLength = 37;

        protected ContentViewModel(string content)
        {
            this.Content = GetLines(content);
        }

        public string[] Content { get; }
        
        private string[] GetLines(string content)
        {
            var contentChars = content.ToCharArray();

            var lines = new List<string>();

            for (int i = 0; i < content.Length; i+=LineLength)
            {
                var row = contentChars.Skip(i).Take(LineLength).ToArray();
                var rowString = new string(row);
                lines.Add(rowString);
            }

            return lines.ToArray();
        }
    }
}
