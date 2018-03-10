﻿namespace Forum.App.Controllers
{
    using Forum.App.Controllers.Contracts;
    using Forum.App.Services;
    using Forum.App.UserInterface;
    using Forum.App.UserInterface.Contracts;
    using Forum.App.Views;

    public class PostDetailsController : IController, IUserRestrictedController
    {

        private enum Command { Back, AddReply }

        public bool LoggedInUser { get; private set; }

        public int PostId { get; private set; }

        public MenuState ExecuteCommand(int index)
        {
            switch ((Command)index)
            {
                case Command.Back:
                    ForumViewEngine.ResetBuffer();
                    return MenuState.Back;

                case Command.AddReply:
                    return MenuState.AddReplyToPost;
            }

            throw new InvalidCommandException();
        }

        public IView GetView(string userName)
        {
            var postViewModel = PostService.GetPostViewModel(this.PostId);
            return new PostDetailsView(postViewModel, this.LoggedInUser);
        }

        public void UserLogIn() => this.LoggedInUser = true;

        public void UserLogOut() => this.LoggedInUser = false;

        public void SetPostId(int postId) => this.PostId = postId;
    }
}
