using Forum.App.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forum.App.Models.Commands.PageNavigationCommands
{
    public abstract class PageNavigationCommand : ICommand
    {
        private ISession session;

        protected PageNavigationCommand(ISession session)
        {
            this.session = session;
        }

        protected ISession Session => this.session;

        public abstract IMenu Execute(params string[] args);
    }
}
