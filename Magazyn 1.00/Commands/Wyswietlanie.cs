using System;
using System.IO;
using System.Windows.Input;

namespace Magazyn
{
    class Wyswietlanie : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            string filePath = @"C:\Users\Public\TestFolder\WriteLines2.txt";

            if (!File.Exists(filePath))
            {
                Console.WriteLine("Plik tekstowy nie istnieje.");
                return;
            }

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                Console.WriteLine("Stan magazynowy");
                while ((line = reader.ReadLine()) != null)
                {
                    string[] fields = line.Split(separator: ',');

                    // Minimalna ilość pól oczekiwanych w pliku
                    if (fields.Length >= 4)
                    {
                        string productName = fields[0];
                        double price = Convert.ToDouble(fields[1]);
                        int quantity = Convert.ToInt32(fields[2]);
                        double weight = Convert.ToDouble(fields[3]);

                        Console.WriteLine("{0} - {1:C} - {2} szt. - {3} kg", productName, price, quantity, weight);

                        // Jeżeli istnieje pole opisu
                        if (fields.Length >= 5)
                        {
                            string description = fields[4];
                            Console.WriteLine("Opis: {0}", description);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Błąd w formacie pliku: {0}", line);
                    }
                }
            }

            string wysylkaPath = @"C:\Users\Public\TestFolder\WriteLines3.txt";

            if (!File.Exists(wysylkaPath))
            {
                Console.WriteLine("Plik tekstowy nie istnieje.");
                return;
            }

            using (StreamReader reader = new StreamReader(wysylkaPath))
            {
                string line;
                Console.WriteLine("Wysyłka");
                while ((line = reader.ReadLine()) != null)
                {
                    string[] fields = line.Split(',');

                    // Minimalna ilość pól oczekiwanych w pliku
                    if (fields.Length >= 3)
                    {
                        string productName = fields[0];
                        int quantity = Convert.ToInt32(fields[1]);
                        double shippingCost = Convert.ToDouble(fields[2]);

                        Console.WriteLine("{0} - {1} szt. - Koszt wysyłki: {2:C}", productName, quantity, shippingCost);
                    }
                    else
                    {
                        Console.WriteLine("Błąd w formacie pliku: {0}", line);
                    }
                }
            }
            Console.ReadLine();
        }

        public event EventHandler CanExecuteChanged;
    }
}
