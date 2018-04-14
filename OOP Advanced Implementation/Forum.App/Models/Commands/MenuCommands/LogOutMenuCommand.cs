using System;
using System.Collections.Generic;
using System.Text;
using Forum.App.Contracts;

namespace Forum.App.Models.Commands.MenuCommands
{
    public class LogOutMenuCommand : MenuCommand
    {
        private ISession session;

        public LogOutMenuCommand(IMenuFactory menuFactory, ISession session) : base(menuFactory)
        {
            this.session = session;
        }

        public override IMenu Execute(params string[] args)
        {
            this.session.Reset();

            return this.MenuFactory.CreateMenu("MainMenu");
        }
    }
}
