using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazyn
{
    class Menu
    {

        static void Main(string[] args)
        {

            bool exit = false;
            {
                Console.WriteLine("| Wybierz jedną z opcji: |");
                Console.WriteLine("|------------------------|");
                Console.WriteLine("| 1. Wysyłka produktu    |");
                Console.WriteLine("| 2. Usuwanie towaru     |");
                Console.WriteLine("| 3. Dodawanie towaru    |");
                Console.WriteLine("| 4. Wprowadzanie danych |");
                Console.WriteLine("|    artykułów           |");
                Console.WriteLine("| 5. Wyświetlanie stanu  |");
                Console.WriteLine("|    magazynowego        |");
                Console.WriteLine("| 6. Aktualizacja towaru |");
                Console.WriteLine("| 7. Zamknięcie programu |");
                Console.WriteLine("|------------------------|");

                int choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        WysylkaProduktu();
                        break;
                    case 2:
                        UsuwanieTowaru();
                        break;
                    case 3:
                        DodawanieTowaru();
                        break;
                    case 4:
                        WprowadzanieDanychArtykulow();
                        break;
                    case 5:
                        WyswietlanieStanuMagazynowego();
                        break;
                    case 6:
                        AktualizacjaTowaru();
                        break;
                    case 7:
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Nieprawidłowy wybór. Spróbuj ponownie.");
                        break;
                }
            }
        }

        static void WysylkaProduktu()
        {
            Console.WriteLine("Funkcja Wysyłka Produktu");
        }

        static void UsuwanieTowaru()
        {
            Console.WriteLine("Funkcja Usuwanie Towaru");
        }

        static void DodawanieTowaru()
        {
            Console.WriteLine("Funkcja Dodawanie Towaru");
        }

        static void WprowadzanieDanychArtykulow()
        {
            Console.WriteLine("Funkcja Wprowadzanie Danych Artykułów");
        }

        static void WyswietlanieStanuMagazynowego()
        {
            Console.WriteLine("Funkcja Wyświetlanie Stanu Magazynowego");
        }

        static void AktualizacjaTowaru()
        {
            Console.WriteLine("Funkcja Aktualizacja Towaru");
        }
    }

}
