using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Informagator.Manager
{
    public class NavigationCommands
    {
        private static Dictionary<string, RoutedUICommand> Commands = new Dictionary<string, RoutedUICommand>();

        public static RoutedUICommand CancelEdit { get { return Commands["CancelEdit"]; } }
        
        public static RoutedUICommand SaveEdit { get { return Commands["SaveEdit"]; } }

        public static RoutedUICommand GotoMachineList { get { return Commands["GotoMachineList"]; } }

        public static RoutedUICommand GotoMachineEdit { get { return Commands["GotoMachineEdit"]; } }

        public static RoutedUICommand GotoProcessList { get { return Commands["GotoProcessList"]; } }

        public static RoutedUICommand GotoProcessEdit { get { return Commands["GotoProcessEdit"]; } }
        
        public static RoutedUICommand GotoAssemblyList { get { return Commands["GotoAssemblyList"]; } }

        public static RoutedUICommand GotoAssemblyEdit { get { return Commands["GotoAssemblyEdit"]; } }

        public static RoutedUICommand GotoGlobalSettings { get { return Commands["GotoGlobalSettings"]; } }

        static NavigationCommands()
        {
            foreach (string commandName in GetCommands())
            {
                Commands.Add(commandName, new RoutedUICommand(commandName, commandName, typeof(NavigationCommands)));
                CommandBinding binding = new CommandBinding(Commands[commandName]);
                CommandManager.RegisterClassCommandBinding(typeof(NavigationCommands), binding);
            }
        }

        private static IEnumerable<string> GetCommands()
        {
            IEnumerable<string> result;

            result = typeof(NavigationCommands).GetProperties(BindingFlags.Public | BindingFlags.Static)
                                               .Where(prop => prop.PropertyType == typeof(RoutedUICommand))
                                               .Select(prop => prop.Name);

            return result;
        }
    }
}
