using System.Windows.Input;

namespace Magazyn.Commands
{
    public class ZamkniecieProgramu : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Console.WriteLine("Program zostanie zamknięty. Naciśnij Enter.");
            Console.ReadLine();
            Environment.Exit(0);
        }

        public event EventHandler CanExecuteChanged;
    }
}