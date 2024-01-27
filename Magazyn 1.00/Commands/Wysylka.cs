using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace Magazyn.Commands
{
    // Abstrakcyjna klasa bazowa reprezentująca artykuł w magazynie
    public abstract class Artykul
    {
        public string Nazwa { get; protected set; }
        public double Waga { get; protected set; }
        public double Cena { get; protected set; }

        public Artykul(string nazwa, double cena, double waga)
        {
            Nazwa = nazwa;
            Cena = cena;
            Waga = waga;
        }
    }

    // Klasa dziedzicząca po Artykul, reprezentująca artykuł 
    public class Produkty : Artykul
    {
        public string Kategoria { get; private set; }

        public Produkty(string nazwa, double cena, double waga, string kategoria) : base(nazwa, cena, waga)
        {
            Kategoria = kategoria;
        }
    }

    // Klasa implementująca interfejs ICommand, reprezentująca proces wysyłki towaru
    class WysylkaTowaru : ICommand
    {
        private Dictionary<string, Artykul> _artykuly = new Dictionary<string, Artykul>();

        private string _sciezkaPlikuOryginalnego = @"C:\Users\adam.bigdowski\source\repos\Magazyn11\stan.txt";
        private string _sciezkaPlikuWysylki = @"C:\Users\adam.bigdowski\source\repos\Magazyn11\wysylka.txt";

        // Konstruktor klasy WysylkaTowaru, inicjalizuje artykuły na podstawie pliku
        public WysylkaTowaru()
        {
            WczytajArtykulyZPliku();
        }

        // Implementacja interfejsu ICommand
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Console.Clear();

            WyswietlOpcjeArtykulow();

            string wybor = WczytajTekstOdUzytkownika("Podaj wybrane artykuły:");

            string[] wybraneArtykuly = wybor.Split(',');

            Dictionary<string, int> wybraneArtykulyZIlosciami = new Dictionary<string, int>();

            foreach (string artykul in wybraneArtykuly)
            {
                DodajArtykulZInterakcjiUzytkownika(artykul.Trim(), wybraneArtykulyZIlosciami);
            }

            double waga = wybraneArtykulyZIlosciami.Sum(kvp => _artykuly[kvp.Key].Waga * kvp.Value);
            double kosztWysylki = ObliczKosztWysylki(waga);

            if (!ZmniejszIloscArtykulowWPlikuOryginalnym(wybraneArtykulyZIlosciami))
            {
                Console.WriteLine("Naciśnij Enter, aby kontynuować.");
                Console.ReadLine();
                return; // Jeżeli ilość artykułów jest nieprawidłowa, wróć do menu
            }

            DodajWyslaneDoPlikuWysylki(wybraneArtykulyZIlosciami, kosztWysylki);

            Console.WriteLine($"Koszt wysyłki: {kosztWysylki} zł");
            Console.WriteLine("Artykuły zostały dodane do pliku tekstowego wraz z kosztem wysyłki.");

            Console.ReadLine();
        }

        // Pozostałe metody

        private void WczytajArtykulyZPliku()
        {
            try
            {
                string[] linie = File.ReadAllLines(_sciezkaPlikuOryginalnego);

                foreach (string linia in linie)
                {
                    string[] czesci = linia.Split(',');
                    string nazwaProduktu = czesci[0];
                    double waga = Convert.ToDouble(czesci[3]);
                    double cena = Convert.ToDouble(czesci[1]);

                    _artykuly.Add(nazwaProduktu, new Produkty(nazwaProduktu, cena, waga, "elektronika"));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas wczytywania artykułów: {ex.Message}");
            }
        }

        private void WyswietlOpcjeArtykulow()
        {
            Console.WriteLine("Wybierz artykuły do wysłania (oddzielone przecinkami):");

            foreach (var artykul in _artykuly)
            {
                Console.WriteLine($"{artykul.Key} - {artykul.Value.Waga} kg");
            }
        }

        private void DodajArtykulZInterakcjiUzytkownika(string nazwaArtykulu, Dictionary<string, int> wybraneArtykulyZIlosciami)
        {
            nazwaArtykulu = SprawdzPoprawnoscWyboruArtykulu(nazwaArtykulu);

            int ilosc = WczytajLiczbeOdUzytkownika($"Podaj ilość towaru {nazwaArtykulu}:");

            wybraneArtykulyZIlosciami.Add(nazwaArtykulu, ilosc);
        }

        private string SprawdzPoprawnoscWyboruArtykulu(string nazwaArtykulu)
        {
            if (!_artykuly.ContainsKey(nazwaArtykulu))
            {
                Console.WriteLine($"Nieprawidłowy wybór artykułu: {nazwaArtykulu}");

                nazwaArtykulu = WczytajTekstOdUzytkownika("Podaj poprawną nazwę artykułu:");
            }

            return nazwaArtykulu;
        }

        private double ObliczKosztWysylki(double waga)
        {
            if (waga <= 5)
            {
                return 10;
            }
            else if (waga <= 20)
            {
                return 20;
            }
            else
            {
                return 50;
            }
        }

        private void DodajWyslaneDoPlikuWysylki(Dictionary<string, int> wybraneArtykulyZIlosciami, double kosztWysylki)
        {
            try
            {
                using (StreamWriter sw = File.AppendText(_sciezkaPlikuWysylki))
                {
                    foreach (var kvp in wybraneArtykulyZIlosciami)
                    {
                        string pelnaNazwa = $"{kvp.Key.Trim()} {_artykuly[kvp.Key].Waga}";
                        sw.WriteLine($"{pelnaNazwa},{kvp.Value},{kosztWysylki}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas dodawania do pliku wysyłki: {ex.Message}");
            }
        }

        private bool ZmniejszIloscArtykulowWPlikuOryginalnym(Dictionary<string, int> wybraneArtykulyZIlosciami)
        {
            try
            {
                string[] linie = File.ReadAllLines(_sciezkaPlikuOryginalnego);

                for (int i = 0; i < linie.Length; i++)
                {
                    string[] czesci = linie[i].Split(',');

                    string nazwaArtykulu = czesci[0].Trim();

                    if (wybraneArtykulyZIlosciami.ContainsKey(nazwaArtykulu))
                    {
                        int iloscDoWyslania = wybraneArtykulyZIlosciami[nazwaArtykulu];
                        int aktualnaIlosc = Convert.ToInt32(czesci[2]);
                        int nowaIlosc = aktualnaIlosc - iloscDoWyslania;

                        if (nowaIlosc < 0)
                        {
                            Console.WriteLine($"Błąd: Próba wysłania większej ilości artykułu, niż dostępnej w magazynie ({nazwaArtykulu})");

                            int doZamowienia = Math.Abs(nowaIlosc);
                            Console.WriteLine($"Aby zrealizować zamówienie, należy zamówić {doZamowienia} sztuk artykułu {nazwaArtykulu}.");

                            Console.ReadLine();

                            return false; // Jeżeli ilość artykułów jest nieprawidłowa, wróć do menu
                        }

                        czesci[2] = nowaIlosc.ToString();
                        linie[i] = string.Join(",", czesci);
                    }
                }

                File.WriteAllLines(_sciezkaPlikuOryginalnego, linie);
                return true; // Poprawnie zmniejszono ilość artykułów
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas zmniejszania ilości artykułów: {ex.Message}");
                return false; // Jeżeli wystąpił błąd, wróć do menu
            }
        }

        private string WczytajTekstOdUzytkownika(string prompt)
        {
            Console.WriteLine(prompt);
            return Console.ReadLine();
        }

        private int WczytajLiczbeOdUzytkownika(string prompt)
        {
            int result;
            bool isValid;

            do
            {
                Console.WriteLine(prompt);
                isValid = int.TryParse(Console.ReadLine(), out result);

                if (!isValid)
                {
                    Console.WriteLine("Nieprawidłowa wartość. Podaj liczbę całkowitą.");
                }

            } while (!isValid);

            return result;
        }

        public event EventHandler CanExecuteChanged;
    }
}
/*Klasy i Obiekty:

Klasy Artykul oraz Produkty reprezentują abstrakcyjne pojęcia artykułu i produktu. Klasa Produkty dziedziczy po klasie Artykul, co jest przykładem hierarchii dziedziczenia.
Obiekty artykułów są tworzone i zarządzane w klasie WysylkaTowaru.
Hermetyzacja:

Właściwości klasy Artykul (np. Nazwa, Waga, Cena) są oznaczone jako protected set, co oznacza, że są dostępne tylko wewnątrz samej klasy i klas dziedziczących, co zapewnia hermetyzację danych.
Polimorfizm:

Metoda Execute w klasie WysylkaTowaru używa polimorfizmu, ponieważ obsługuje różne rodzaje artykułów (dziedziczących po Artykul) w sposób jednolity.
Interfejsy:

Klasa WysylkaTowaru implementuje interfejs ICommand, co pozwala na użycie jej instancji jako polecenia, co jest często wykorzystywane w aplikacjach z interfejsem użytkownika.
Dziedziczenie:

Klasa Produkty dziedziczy po klasie Artykul, co jest przykładem dziedziczenia, umożliwiając współdzielenie cech ogólnych artykułów.
Enkapsulacja:

Dostęp do prywatnych pól, takich jak _artykuly, jest kontrolowany poprzez metody publiczne, co jest przykładem enkapsulacji.
Zdarzenia:

Zastosowanie zdarzenia CanExecuteChanged w klasie WysylkaTowaru umożliwia obsługę zmian, co jest charakterystyczne dla programowania obiektowego.
Obsługa Błędów:

Kod zawiera obsługę błędów, takie jak próba wysłania większej ilości artykułów niż dostępnych w magazynie, co jest zgodne z zasadami programowania obiektowego.
Inicjalizacja Obiektów:

Konstruktor klasy WysylkaTowaru inicjalizuje artykuły na podstawie pliku, co jest przykładem inicjalizacji obiektów.
*/
