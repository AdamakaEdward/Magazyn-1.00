using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazyn_1._00
{
    class Towar
    {
        public double waga;
        public double wielkosc;
    }

    class Produkt : Towar
    {
        public string nazwa;
        public int ilosc;

        public Produkt(string nazwa, int ilosc, double waga, double wielkosc)
        {
            this.nazwa = nazwa;
            this.ilosc = ilosc;
            this.waga = waga;
            this.wielkosc = wielkosc;
        }

        public void DodajProdukt()
        {
            Console.WriteLine("Dodaj produkt:");
            Console.Write("Nazwa: ");
            nazwa = Console.ReadLine();
            Console.Write("Ilość: ");
            ilosc = Convert.ToInt32(Console.ReadLine());
            Console.Write("Waga: ");
            waga = Convert.ToDouble(Console.ReadLine());
            Console.Write("Wielkość: ");
            wielkosc = Convert.ToDouble(Console.ReadLine());
        }

        public void ZapiszDoPliku()
            {
                using (StreamWriter sw = new StreamWriter("magazyn.txt"))
                {
                    sw.WriteLine("Nazwa\tIlość\tWaga\tWielkość");
                    sw.WriteLine($"{nazwa}\t{ilosc}\t{waga}\t{wielkosc}");
                }

                Console.WriteLine("Produkty zostały zapisane do pliku.");
            }

            public void WyswietlStanMagazynowy()
            {
                Console.WriteLine("Stan magazynowy:");

                using (StreamReader sr = new StreamReader("magazyn.txt"))
                {
                    string line;

                    while ((line = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(line);
                    }
                }
            
            }

        class Program
        {
            static void Test(string[] args)
            {
                Produkt p = new Produkt("", 0, 0, 0);
            }
        }
    }
}