﻿namespace Forum.App.Controllers
{
    using Forum.App.Controllers.Contracts;
    using Forum.App.Services;
    using Forum.App.UserInterface;
    using Forum.App.UserInterface.Contracts;
    using Forum.App.Views;

    public class LogInController : IController, IReadUserInfoController
    {
        public LogInController()
        {
            this.ResetLogin();
        }

        private enum Command { ReadUsername, ReadPassword, LogIn, Back }

        public string Username { get; private set; }

        private string Password { get; set; }

        private bool Error { get; set; }

        public MenuState ExecuteCommand(int index)
        {
            switch ((Command)index)
            {
                case Command.ReadUsername:
                    {
                        this.ReadUsername();
                        return MenuState.Login;
                    }

                case Command.ReadPassword:
                    {
                        this.ReadPassword();
                        return MenuState.Login;
                    }

                case Command.LogIn:
                    {
                        var loggedIn = UserService.TryLogInUser(this.Username, this.Password);
                        this.Error = !loggedIn;

                        return loggedIn ? MenuState.SuccessfulLogIn : MenuState.Error;
                    }

                case Command.Back:
                    {
                        this.ResetLogin();
                        return MenuState.Back;
                    }
            }

            throw new InvalidCommandException();
        }

        public IView GetView(string userName) =>
            new LogInView(this.Error, this.Username, this.Password.Length);

        public void ReadPassword()
        {
            this.Password = ForumViewEngine.ReadRow();
            ForumViewEngine.HideCursor();
        }

        public void ReadUsername()
        {
            this.Username = ForumViewEngine.ReadRow();
            ForumViewEngine.HideCursor();
        }

        private void ResetLogin()
        {
            this.Error = false;
            this.Username = string.Empty;
            this.Password = string.Empty;
        }
    }
}
