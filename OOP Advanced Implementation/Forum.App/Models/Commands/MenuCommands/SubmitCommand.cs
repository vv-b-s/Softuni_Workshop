using Forum.App.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forum.App.Models.Commands.MenuCommands
{
    public class SubmitCommand : ICommand
    {
        private ISession session;
        private IPostService postService;
        private ICommandFactory commandFactory;

        public SubmitCommand(ISession session, IPostService postService, ICommandFactory commandFactory)
        {
            this.postService = postService;
            this.session = session;
            this.commandFactory = commandFactory;
        }

        public IMenu Execute(params string[] args)
        {
            var userId = this.session.UserId;

            var postId = int.Parse(args[0]);
            var replyContent = args[1];
            this.postService.AddReplyToPost(postId, replyContent, userId);

            var viewPostCommand = this.commandFactory.CreateCommand("ViewPostMenu");
            return viewPostCommand.Execute(postId.ToString());
        }
    }
}
