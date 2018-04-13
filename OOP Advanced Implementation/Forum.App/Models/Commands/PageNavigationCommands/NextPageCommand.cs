using Forum.App.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forum.App.Models.Commands.PageNavigationCommands
{
    public class NextPageCommand : PageNavigationCommand
    {
        public NextPageCommand(ISession session) : base(session) { }

        public override IMenu Execute(params string[] args)
        {
            var currentMenu = this.Session.CurrentMenu;

            if (currentMenu is IPaginatedMenu paginatedMenu)
                paginatedMenu.ChangePage();

            return currentMenu;
        }
    }
}
