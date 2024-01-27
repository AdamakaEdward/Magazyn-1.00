using Magazyn.Commands;
using System;
using System.Windows.Input;

namespace Magazyn
{
    class Menu
    {
        public const int MAX_COMMANDS = 8;

        static void Main(string[] args)
        {
            bool exit = false;

            while (!exit)
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

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    ICommand[] commands = new ICommand[MAX_COMMANDS];
                    commands[6] = new ZamkniecieProgramu();
                    commands[5] = new EdycjaTowaru();
                    commands[4] = new Wyswietlanie();
                    commands[3] = new DodawanieTowaru();
                    commands[2] = new UsuwanieTowaru();
                    commands[1] = new WysylkaTowaru();

                    if (choice >= 1 && choice <= MAX_COMMANDS)
                    {
                        if (commands[choice] != null)
                        {
                            commands[choice].Execute(null);
                        }

                    }
                    else
                    {
                        Console.WriteLine("Nieprawidłowy wybór. Podaj liczbę od 1 do {0}.", MAX_COMMANDS);
                    }
                }
                else
                {
                    Console.WriteLine("Nieprawidłowy wybór. Podaj liczbę od 1 do {0}.", MAX_COMMANDS);
                    Console.ReadLine(); // Aby użytkownik zobaczył komunikat i mógł spróbować ponownie
                }
            }
        }
    }
}
