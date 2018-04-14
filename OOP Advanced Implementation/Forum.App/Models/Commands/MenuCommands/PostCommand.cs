using Forum.App.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forum.App.Models.Commands.MenuCommands
{
    public class PostCommand : ICommand
    {
        private ISession session;
        private IPostService postService;
        private ICommandFactory commandFactory;

        public PostCommand(ISession session, IPostService postService, ICommandFactory commandFactory )
        {
            this.postService = postService;
            this.session = session;
            this.commandFactory = commandFactory;
        }

        public IMenu Execute(params string[] args)
        {
            var userId = this.session.UserId;

            var postTitle = args[0];
            var postCategory = args[1];
            var postContent = args[2];

            var postId = this.postService.AddPost(userId, postTitle, postCategory, postContent);
            var viewPostCommand = this.commandFactory.CreateCommand("ViewPostMenu");
            return viewPostCommand.Execute(postId.ToString());
        }
    }
}
