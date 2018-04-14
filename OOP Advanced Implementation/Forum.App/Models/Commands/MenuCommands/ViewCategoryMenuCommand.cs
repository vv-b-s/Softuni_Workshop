using System;
using System.Collections.Generic;
using System.Text;
using Forum.App.Contracts;

namespace Forum.App.Models.Commands.MenuCommands
{
    public class ViewCategoryMenuCommand : MenuCommand
    {
        public ViewCategoryMenuCommand(IMenuFactory menuFactory) : base(menuFactory) { }

        public override IMenu Execute(params string[] args)
        {
            var categoryId = int.Parse(args[0]);

            var menu = base.Execute(args) as IIdHoldingMenu;
            menu.SetId(categoryId);

            return menu;
        }
    }
}
