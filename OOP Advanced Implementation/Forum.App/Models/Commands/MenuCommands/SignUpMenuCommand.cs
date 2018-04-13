using System;
using System.Collections.Generic;
using System.Text;
using Forum.App.Contracts;

namespace Forum.App.Models.Commands.MenuCommands
{
    public class SignUpMenuCommand : MenuCommand
    {
        public SignUpMenuCommand(IMenuFactory menuFactory) : base(menuFactory)
        {
        }
    }
}
