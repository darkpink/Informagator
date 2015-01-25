using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Informagator.Manager.Commands
{
    public static class ThreadControlCommandManager
    {
        public static ICommand StartThread { get { return new StartThreadCommand(); } }

        public static ICommand StopThread { get { return new StopThreadCommand(); } }

        public static ICommand UpdateConfiguration { get { return new UpdateConfigurationCommand(); } }
    }
}
