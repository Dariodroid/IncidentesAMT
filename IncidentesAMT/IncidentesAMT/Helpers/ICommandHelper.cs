using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace IncidentesAMT.Helpers
{
    public static class ICommandHelper
    {
        public static bool CheckBeginExecute(this ICommand command)
        {
            return CheckBeginExecuteCommand(command);
        }

        public static bool CheckBeginExecuteCommand(ICommand command)
        {
            var canExecute = false;
            lock (command)
            {
                canExecute = command.CanExecute(null);
                if (canExecute)
                {
                    command.Execute(null);
                }
            }

            return canExecute;
        }
    }
}
