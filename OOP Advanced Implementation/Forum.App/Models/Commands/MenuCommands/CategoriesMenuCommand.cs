using Forum.App.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forum.App.Models.Commands.MenuCommands
{
    public class CategoriesMenuCommand : MenuCommand
    {
        public CategoriesMenuCommand(IMenuFactory menuFactory) : base(menuFactory) { }
    }
}
