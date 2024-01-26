using Magazyn.Commands;
using System.Windows.Input;
namespace Magazyn
{
    class Menu
    {
        public const int MAX_COMMANDS = 8;

        static void Main(string[] args)
        {
            while (true)
            {

                bool exit = false;
                {
                    Console.WriteLine("| Wybierz jedną z opcji: |");
                    Console.WriteLine("|------------------------|");
                    Console.WriteLine("| 1. Wysyłka produktu    |");
                    Console.WriteLine("| 2. Usuwanie towaru     |");
                    Console.WriteLine("| 3. Dodawanie towaru    |");
                    Console.WriteLine("| 4. Wyświetlanie stanu  |");
                    Console.WriteLine("|    magazynowego        |");
                    Console.WriteLine("| 5. Aktualizacja towaru |");
                    Console.WriteLine("| 6. Zamknięcie programu |");
                    Console.WriteLine("|------------------------|");

                    int choice = Convert.ToInt32(Console.ReadLine());

                    ICommand[] commands = new ICommand[MAX_COMMANDS];
                    commands[5] = new EdycjaTowaru();
                    commands[4] = new Wyswietlanie();
                    commands[2] = new UsuwanieTowaru();
                    commands[3] = new DodawanieTowaru();
                    commands[1] = new WysylkaTowaru();

                    if (commands[choice] != null)
                    {
                        commands[choice].Execute(null);
                    }
                }
            }
        }
    }
}