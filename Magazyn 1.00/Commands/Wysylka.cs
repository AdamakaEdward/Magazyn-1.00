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

        private string _sciezkaPlikuOryginalnego = @"C:\Users\Adam\source\repos\Magazyn-1.00\stan.txt.txt";
        private string _sciezkaPlikuWysylki = @"C:\Users\Adam\source\repos\Magazyn-1.00\wysylka.txt.txt";

        // Konstruktor klasy WysylkaTowaru, inicjalizuje artykuły na podstawie pliku
        public WysylkaTowaru()
        {
            WczytajArtykulyZPliku();
        }

        // Implementacja interfejsu ICommand

        /// <summary>
        /// Sprawdza, czy komenda może być wykonana.
        /// </summary>
        /// <param name="parameter">Parametr komendy (nieużywany).</param>
        /// <returns>Zawsze zwraca true, co oznacza, że komenda może być wykonana.</returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Wykonuje logikę związaną z wysyłką towaru.
        /// </summary>
        /// <param name="parameter">Parametr komendy (nieużywany).</param>
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

            DodajWyslaneDoPlikuWysylki(wybraneArtykulyZIlosciami, kosztWysylki);

            ZmniejszIloscArtykulowWPlikuOryginalnym(wybraneArtykulyZIlosciami);

            Console.WriteLine($"Koszt wysyłki: {kosztWysylki} zł");
            Console.WriteLine("Artykuły zostały dodane do pliku tekstowego wraz z kosztem wysyłki.");

            Console.ReadLine();
        }

        // Pozostałe metody

        /// <summary>
        /// Wczytuje artykuły z pliku do słownika _artykuly.
        /// </summary>
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

        /// <summary>
        /// Wyświetla opcje artykułów do wysłania.
        /// </summary>
        private void WyswietlOpcjeArtykulow()
        {
            Console.WriteLine("Wybierz artykuły do wysłania (oddzielone przecinkami):");

            foreach (var artykul in _artykuly)
            {
                Console.WriteLine($"{artykul.Key} - {artykul.Value.Waga} kg");
            }
        }

        /// <summary>
        /// Dodaje artykuł do słownika wybranych artykułów na podstawie interakcji z użytkownikiem.
        /// </summary>
        /// <param name="nazwaArtykulu">Nazwa artykułu podana przez użytkownika.</param>
        /// <param name="wybraneArtykulyZIlosciami">Słownik z ilościami wybranych artykułów.</param>
        private void DodajArtykulZInterakcjiUzytkownika(string nazwaArtykulu, Dictionary<string, int> wybraneArtykulyZIlosciami)
        {
            nazwaArtykulu = SprawdzPoprawnoscWyboruArtykulu(nazwaArtykulu);

            int ilosc = WczytajLiczbeOdUzytkownika($"Podaj ilość towaru {nazwaArtykulu}:");

            wybraneArtykulyZIlosciami.Add(nazwaArtykulu, ilosc);
        }

        /// <summary>
        /// Sprawdza poprawność wyboru artykułu podanego przez użytkownika.
        /// </summary>
        /// <param name="nazwaArtykulu">Nazwa artykułu podana przez użytkownika.</param>
        /// <returns>Poprawiona nazwa artykułu.</returns>
        private string SprawdzPoprawnoscWyboruArtykulu(string nazwaArtykulu)
        {
            if (!_artykuly.ContainsKey(nazwaArtykulu))
            {
                Console.WriteLine($"Nieprawidłowy wybór artykułu: {nazwaArtykulu}");
                Console.WriteLine("Podaj poprawną nazwę artykułu:");
                nazwaArtykulu = WczytajTekstOdUzytkownika("Podaj poprawną nazwę artykułu:");
            }

            return nazwaArtykulu;
        }

        /// <summary>
        /// Oblicza koszt wysyłki na podstawie wagi.
        /// </summary>
        /// <param name="waga">Waga artykułów.</param>
        /// <returns>Koszt wysyłki.</returns>
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

        /// <summary>
        /// Dodaje dane o wysłanych artykułach do pliku wysyłki.
        /// </summary>
        /// <param name="wybraneArtykulyZIlosciami">Słownik z ilościami wybranych artykułów.</param>
        /// <param name="kosztWysylki">Koszt wysyłki.</param>
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

        /// <summary>
        /// Aktualizuje ilość artykułów w magazynie po wysłaniu.
        /// </summary>
        /// <param name="wybraneArtykulyZIlosciami">Słownik z ilościami wybranych artykułów.</param>
        private void ZmniejszIloscArtykulowWPlikuOryginalnym(Dictionary<string, int> wybraneArtykulyZIlosciami)
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
                            continue;
                        }

                        czesci[2] = nowaIlosc.ToString();
                        linie[i] = string.Join(",", czesci);
                    }
                }

                File.WriteAllLines(_sciezkaPlikuOryginalnego, linie);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas zmniejszania ilości artykułów: {ex.Message}");
            }
        }

        /// <summary>
        /// Wyświetla prompt i wczytuje tekst od użytkownika.
        /// </summary>
        /// <param name="prompt">Komunikat do wyświetlenia użytkownikowi.</param>
        /// <returns>Wczytany tekst od użytkownika.</returns>
        private string WczytajTekstOdUzytkownika(string prompt)
        {
            Console.WriteLine(prompt);
            return Console.ReadLine();
        }

        /// <summary>
        /// Wyświetla prompt i wczytuje liczbę całkowitą od użytkownika.
        /// </summary>
        /// <param name="prompt">Komunikat do wyświetlenia użytkownikowi.</param>
        /// <returns>Wczytana liczba całkowita.</returns>
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

        // Zdarzenie interfejsu ICommand
        public event EventHandler CanExecuteChanged;
    }
}
