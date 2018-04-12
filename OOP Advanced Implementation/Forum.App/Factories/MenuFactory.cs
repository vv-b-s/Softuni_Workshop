using Forum.App.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Forum.App.Factories
{
    public class MenuFactory : IMenuFactory
    {
        private IServiceProvider serviceProvider;

        public MenuFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IMenu CreateMenu(string menuName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var menuType = assembly.GetTypes()
               .FirstOrDefault(t => t.Name == menuName + "Menu");

            if(!typeof(IMenu).IsAssignableFrom(menuType))
                throw new InvalidOperationException($"{menuType} is not a command");

            var ctorParams = menuType.GetConstructors().First().GetParameters();
            var args = new object[ctorParams.Length];

            for (int i = 0; i < args.Length; i++)
                args[i] = this.serviceProvider.GetService(ctorParams[i].ParameterType);

            var menu = Activator.CreateInstance(menuType, args) as IMenu;

            return menu;
        }
    }
}
