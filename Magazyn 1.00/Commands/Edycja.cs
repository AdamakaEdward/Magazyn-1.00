using System;
using System.IO;
using System.Windows.Input;

namespace Magazyn.Commands
{
    internal class EdycjaTowaru : ICommand
    {
        private readonly Wyswietlanie wyswietlanieCommand = new Wyswietlanie();



        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            // Wywołaj metodę Execute z Wyswietlanie przed rozpoczęciem edycji
            wyswietlanieCommand.Execute(null);

            string filePath = @"C:\Users\adam.bigdowski\source\repos\Magazyn11\stan.txt";

            // Odczytaj wszystkie linie z pliku
            string[] lines = File.ReadAllLines(filePath);

            // Wybierz indeks linii do edycji
            Console.WriteLine("Wybierz numer linii do edycji (od 1 do " + lines.Length + "):");
            int lineNumber;
            if (int.TryParse(Console.ReadLine(), out lineNumber) && lineNumber >= 1 && lineNumber <= lines.Length)
            {
                // Wybierz dane do edycji
                string[] data = lines[lineNumber - 1].Split(',');

                Console.WriteLine("Wybrane dane do edycji: " + string.Join(", ", data));

                // Edytuj dane
                for (int j = 0; j < data.Length; j++)
                {
                    string newValue;
                    do
                    {
                        Console.Write("Wprowadź nową wartość dla elementu " + (j + 1) + ": ");
                        newValue = Console.ReadLine();

                        if (string.IsNullOrWhiteSpace(newValue))
                        {
                            Console.WriteLine("Wartość nie może być pusta. Spróbuj ponownie.");
                        }
                    } while (string.IsNullOrWhiteSpace(newValue));

                    data[j] = newValue;
                }

                // Zaktualizuj linię w pliku
                lines[lineNumber - 1] = string.Join(",", data);

                // Zapisz zaktualizowane dane z powrotem do pliku
                File.WriteAllLines(filePath, lines);

                Console.WriteLine("Dane zostały pomyślnie zaktualizowane.");
            }
            else
            {
                Console.WriteLine("Nieprawidłowy numer linii.");
            }

            // Wywołaj metodę Execute z Wyswietlanie po zakończeniu edycji
            wyswietlanieCommand.Execute(null);
        }

        public event EventHandler CanExecuteChanged;
    }
}
