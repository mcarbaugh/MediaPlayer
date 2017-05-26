using System;
using System.Windows.Input;

namespace MediaPlayer.UserInterface.ViewModel.Utilities
{
    public class Command : ICommand
    {
        private Action _execute;

        public Command(Action execute)
        {
            if (execute == null)
                throw new ArgumentNullException("Failed to locate an executable action.");

            _execute = execute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _execute();
        }
    }
}
