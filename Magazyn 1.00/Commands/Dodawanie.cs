using System;
using System.IO;
using System.Windows.Input;

public class DodawanieTowaru : ICommand
{
    public bool CanExecute(object parameter)
    {
        return true;
    }

    public void Execute(object parameter)
    {
        string filePath = @"C:\Users\adam.bigdowski\source\repos\Magazyn11\stan.txt";


        string productName;
        do
        {
            Console.WriteLine("Podaj nazwę towaru:");
            productName = Console.ReadLine().Replace(" ", "_").Replace(",", ".");
            if (string.IsNullOrWhiteSpace(productName))
            {
                Console.WriteLine("Nazwa nie może być pusta. Spróbuj ponownie.");
            }
        } while (string.IsNullOrWhiteSpace(productName));

        double price;
        do
        {
            Console.WriteLine("Podaj cenę towaru:");
            if (!double.TryParse(Console.ReadLine(), out price) || price <= 0)
            {
                Console.WriteLine("Nieprawidłowa cena. Cena musi być liczbą większą od zera. Spróbuj ponownie.");
            }
        } while (price <= 0);

        int quantity;
        do
        {
            Console.WriteLine("Podaj ilość towaru:");
            if (!int.TryParse(Console.ReadLine(), out quantity) || quantity <= 0)
            {
                Console.WriteLine("Nieprawidłowa ilość. Ilość musi być liczbą całkowitą większą od zera. Spróbuj ponownie.");
            }
        } while (quantity <= 0);

        double weight;
        do
        {
            Console.WriteLine("Podaj wagę towaru:");
            if (!double.TryParse(Console.ReadLine(), out weight) || weight <= 0)
            {
                Console.WriteLine("Nieprawidłowa waga. Waga musi być liczbą większą od zera. Spróbuj ponownie.");
            }
        } while (weight <= 0);
        // Sprawdź czy podano opis, jeśli nie, ustaw pusty opis
        Console.WriteLine("Podaj opis towaru:");
        string description = Console.ReadLine();

        using (StreamWriter writer = new StreamWriter(filePath, append: true))
        {
            writer.WriteLine("{0},{1},{2},{3},{4}", productName, price, quantity, weight, description);
        }

        Console.WriteLine("Towar został dodany do pliku tekstowego.");
        Console.ReadLine();
    }

    public event EventHandler CanExecuteChanged;
}

public class DodawanieTowaruZOpisem : ICommand
{
    private string description;

    public DodawanieTowaruZOpisem(string desc)
    {
        description = desc;
    }

    public bool CanExecute(object parameter)
    {
        return true;
    }

    public void Execute(object parameter)
    {
        // Dodaj logikę dodawania opisu do pliku, nie dodawaj informacji o produkcie
        string filePath = @"C:\Users\adam.bigdowski\source\repos\Magazyn11\stan.txt";
        using (StreamWriter writer = new StreamWriter(filePath, true))
        {
            writer.WriteLine("Opis: {0}", description);
        }

        Console.WriteLine("Opis został dodany do pliku tekstowego.");
        Console.ReadLine();
    }

    public event EventHandler CanExecuteChanged;
}
