using Forum.App.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forum.App.Models.Commands.MenuCommands
{
    public abstract class MenuCommand : ICommand
    {
        private IMenuFactory menuFactory;

        protected MenuCommand(IMenuFactory menuFactory)
        {
            this.menuFactory = menuFactory;
        }

        public virtual IMenu Execute(params string[] args)
        {
            var menuName = GetMenuName();
            var menu = this.menuFactory.CreateMenu(menuName);
            return menu;
        }

        protected string GetMenuName()
        {
            var commandName = this.GetType().Name;
            var menuName = commandName.Substring(0, commandName.LastIndexOf("Command"));
            return menuName;
        }
    }
}
