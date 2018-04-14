using System;
using System.Collections.Generic;
using System.Text;
using Forum.App.Contracts;

namespace Forum.App.Models.Commands.MenuCommands
{
    class SignUpCommand : MenuCommand
    {
        private IUserService userService;
        public SignUpCommand(IMenuFactory menuFactory, IUserService userService) : base(menuFactory)
        {
            this.userService = userService;
        }

        public override IMenu Execute(params string[] args)
        {
            var username = args[0];
            var password = args[1];

            var success = this.userService.TrySignUpUser(username, password);

            if (!success)
                throw new InvalidOperationException("Invalid login!");

            else return this.MenuFactory.CreateMenu("MainMenu");
        }
    }
}
