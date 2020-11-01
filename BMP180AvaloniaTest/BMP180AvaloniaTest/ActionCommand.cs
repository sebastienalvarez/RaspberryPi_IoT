using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace BMP180AvaloniaTest
{
    internal class ActionCommand : ICommand
    {
        // PROPRIETES
        private Action<object> commandLogic;

        // EVENEMENT
        public event EventHandler CanExecuteChanged;

        // CONSTRUCTEUR
        public ActionCommand(Action<object> a_commandLogic)
        {
            commandLogic = a_commandLogic;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            commandLogic(parameter);
        }
    }
}
