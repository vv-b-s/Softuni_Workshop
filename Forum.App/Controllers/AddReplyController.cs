namespace Forum.App.Controllers
{
    using Forum.App.Controllers.Contracts;
    using Forum.App.Services;
    using Forum.App.UserInterface;
    using Forum.App.UserInterface.Contracts;
    using Forum.App.UserInterface.Input;
    using Forum.App.UserInterface.ViewModels;
    using Forum.App.Views;
    using System.Collections.Generic;
    using System.Linq;

    public class AddReplyController : IController
    {
        private const int TEXT_AREA_WIDTH = 37;
        private const int TEXT_AREA_HEIGHT = 6;
        private const int POST_MAX_LENGTH = 220;

        private static int centerTop = Position.ConsoleCenter().Top;
        private static int centerLeft = Position.ConsoleCenter().Left;

        public AddReplyController()
        {
            ResetReply();
        }

        private enum Command { Write, Submit }

        public PostViewModel Post { get; private set; }

        public ReplyViewModel Reply { get; private set; }

        public TextArea TextArea { get; set; }

        public bool Error { get; set; }


        public void ResetReply()
        {
            this.Error = false;
            this.Reply = new ReplyViewModel();
            this.Post = null;
            this.TextArea = new TextArea(centerLeft - 18, centerTop - 6, TEXT_AREA_WIDTH, TEXT_AREA_HEIGHT, POST_MAX_LENGTH);
        }

        public MenuState ExecuteCommand(int index)
        {
            switch ((Command)index)
            {
                case Command.Write:
                    this.TextArea.Write();
                    this.Reply.Content = this.TextArea.Lines.ToList();
                    return MenuState.AddReply;


                case Command.Submit:
                    var replyAdded = PostService.TryAddReply(this.Post, this.Reply);

                    if(!replyAdded)
                    {
                        this.Error = true;
                        return MenuState.Rerender;
                    }

                    return MenuState.ReplyAdded;
            }

            throw new InvalidCommandException();
        }

        public IView GetView(string userName)
        {
            this.Reply.Author = userName;
            this.Post.Content = new List<string>();

            return new AddReplyView(this.Post, this.Reply, this.TextArea, this.Error);
        }

        public void SetPost(int postId) => this.Post = PostService.GetPostViewModel(postId);
    }
}
