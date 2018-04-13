using System;
using System.Collections.Generic;
using System.Text;
using Forum.App.Contracts;

namespace Forum.App.Models.Commands.PageNavigationCommands
{
    public class PreviousPageCommand : PageNavigationCommand
    {
        public PreviousPageCommand(ISession session) : base(session) { }

        public override IMenu Execute(params string[] args)
        {
            var currentMenu = this.Session.CurrentMenu;

            if (currentMenu is IPaginatedMenu paginatedMenu)
                paginatedMenu.ChangePage(false);

            return currentMenu;
        }
    }
}
