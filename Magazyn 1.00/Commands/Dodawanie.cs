using System.Windows.Input;

public class DodawanieTowaru : ICommand
{
    public bool CanExecute(object parameter)
    {
        return true;
    }

    public void Execute(object parameter)
    {
        string filePath = @"C:\Users\Public\TestFolder\WriteLines2.txt";

        Console.WriteLine("Podaj nazwę towaru:");
        string productName = Console.ReadLine().Replace(" ", "_");

        Console.WriteLine("Podaj cenę towaru:");
        double price = Convert.ToDouble(Console.ReadLine());

        Console.WriteLine("Podaj ilość towaru:");
        int quantity = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("Podaj wagę towaru:");
        double weight = Convert.ToDouble(Console.ReadLine());

        Console.WriteLine("Podaj opis towaru:");
        string description = Console.ReadLine();

        using (StreamWriter writer = new StreamWriter(filePath, true))
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
        string filePath = @"C:\Users\Public\TestFolder\WriteLines2.txt";
        using (StreamWriter writer = new StreamWriter(filePath, true))
        {
            writer.WriteLine("Opis: {0}", description);
        }

        Console.WriteLine("Opis został dodany do pliku tekstowego.");
        Console.ReadLine();
    }

    public event EventHandler CanExecuteChanged;
}
