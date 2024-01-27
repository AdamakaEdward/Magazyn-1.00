using System;
using System.IO;
using System.Windows.Input;

namespace Magazyn.Commands
{

    class UsuwanieTowaru : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            string filePath = @"C:\Users\adam.bigdowski\source\repos\Magazyn11\stan.txt";
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                Console.WriteLine(line);
            }
            Console.WriteLine("Podaj nazwę towaru, który chcesz usunąć:");
            string productName = Console.ReadLine();

            File.WriteAllLines(filePath, lines.Where(line => !line.Contains(productName)));

            Console.WriteLine("Towar został usunięty z bazy.");
            Console.ReadLine();
        }

        public event EventHandler CanExecuteChanged;
    }
}