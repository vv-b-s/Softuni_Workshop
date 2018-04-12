namespace Forum.App.Factories
{
	using Contracts;
    using System;
    using System.Linq;
    using System.Reflection;

    public class CommandFactory : ICommandFactory
	{
        private IServiceProvider serviceProvider;

        public CommandFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

		public ICommand CreateCommand(string commandName)
		{
            var assembly = Assembly.GetExecutingAssembly();
            var commandType = assembly.GetTypes()
                .FirstOrDefault(t => t.Name == commandName + "Command");

            if (commandType is null)
                throw new InvalidOperationException("Command not found!");

            //Checks if commandType implements ICommand
            if (!typeof(ICommand).IsAssignableFrom(commandType))
                throw new InvalidOperationException($"{commandType} is not a command!");

            var ctorParams = commandType.GetConstructors().First().GetParameters();
            var args = new object[ctorParams.Length];

            for (int i = 0; i < args.Length; i++)
                args[i] = this.serviceProvider.GetService(ctorParams[i].ParameterType);

            var command = Activator.CreateInstance(commandType, args) as ICommand;

            return command;
		}
	}
}
