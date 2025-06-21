using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Model
{
    internal class Adminmodel
    {
        //wczytanie listy książek
        public string[] listagat()
        {
            string[] lista = new string[9];
            lista[0] = "historyczny";
            lista[1] = "fantazy";
            lista[2] = "romans";
            lista[3] = "Science-fiction";
            lista[4] = "Horror";
            lista[5] = "Triller";
            lista[6] = "Poradnik";
            lista[7] = "Manga";
            lista[8] = "Kryminalny";
            return lista;
        }
        public string dodajgat(string gatunek, string resztagat) {
            for(int i=0; i<resztagat.Length-gatunek.Length+1; i++)
            {
                if (resztagat.Substring(i, gatunek.Length) == gatunek)
                {
                    return resztagat;
                }
            }
            if (resztagat != "")
            {
                string tekst;
                tekst = new StringBuilder(", ").Append(gatunek).ToString();
                return new StringBuilder(resztagat).Append(tekst).ToString();
            }
            else
            return new StringBuilder(resztagat).Append(gatunek).ToString();
        }
        public string dodajaut(string imie, string nazwisko, string resztaaut)
        {
            string pelnanazwa = imie + " ";
            pelnanazwa = new StringBuilder(pelnanazwa).Append(nazwisko).ToString();
            for (int i = 0; i < resztaaut.Length - pelnanazwa.Length + 1; i++)
            {
                if (resztaaut.Substring(i, pelnanazwa.Length) == pelnanazwa)
                {
                    return resztaaut;
                }
            }
            if (resztaaut != "")
            {
                string tekst;
                tekst = new StringBuilder(", ").Append(pelnanazwa).ToString();
                return new StringBuilder(resztaaut).Append(tekst).ToString();
            }
            else
                return new StringBuilder(resztaaut).Append(pelnanazwa).ToString();
        }
        public string[] rodz()
        {
            string[] rodzaje = new string[3];
            rodzaje[0] = "książka";
            rodzaje[1] = "album";
            rodzaje[2] = "komiks";
            return rodzaje;
        }
        public string[] rodzokl()
        {
            string[] rodzaje = new string[2];
            rodzaje[0] = "twarda";
            rodzaje[1] = "miękka";
            return rodzaje;
        }
        public string wyczyscautorow()
        {
            return "";
        }
        public string wyczyscgatunki()
        {
            return "";
        }
        public string dodajksiaz(string tytul,string rodzaj, string rodzajokl, DateTimePicker data_wyd, string ISBN, string gatunki, string autorzy, int ile)
        {
            string ksiazka = tytul;
            ksiazka = new StringBuilder(ksiazka).Append("; ").Append(rodzaj).Append("; ").Append(rodzajokl).Append("; ").Append(data_wyd.Value.ToLongDateString()).Append("; ").Append(ISBN).Append("; ").Append(gatunki).Append("; ").Append(autorzy).Append("; ").Append(ile.ToString()).ToString();
            
            // tutaj jest dodanie książki do bazy danych (edycja będzie wyglądała podobnie
            
            return ksiazka;
        }
        public string tytul(string tekst)
        {
            string wynik="";
            for (int i=0; i<tekst.Length; i++)
            {
                if (tekst[i] == ';')
                {
                    wynik=tekst.Substring(0,i);
                    break;
                }
            }
            return wynik;
        }
        public string rodzaj(string tekst)
        {
            string wynik = "";
            int pozstart = 0;
            int ile = 0;
            for (int i = 0; i < tekst.Length; i++)
            {
                if (tekst[i] == ';')
                {
                    ile++;
                    if (ile == 2)
                    {
                    wynik = tekst.Substring(pozstart+2, i - pozstart-2);
                        break;
                    }
                    pozstart = i;
                }
            }
            return wynik;
        }
        public string rodzajokladki(string tekst)
        {
            string wynik = "";
            int pozstart = 0;
            int ile = 0;
            for (int i = 0; i < tekst.Length; i++)
            {
                if (tekst[i] == ';')
                {
                    ile++;
                    if (ile == 3)
                    {
                        wynik = tekst.Substring(pozstart + 2, i -pozstart-2);
                        break;
                    }
                    if (ile == 2)
                    pozstart = i;
                }
            }
            return wynik;
        }
        public DateTime getdata_wyd(string tekst)
        {
            string wynik = "";
            int pozstart = 0;
            int ile = 0;
            for (int i = 0; i < tekst.Length; i++)
            {
                if (tekst[i] == ';')
                {
                    ile++;
                    if (ile == 4)
                    {
                        wynik = tekst.Substring(pozstart + 2, i - pozstart - 2);
                        break;
                    }
                    if (ile == 3)
                        pozstart = i;
                }
            }
            return DateTime.Parse(wynik);
        }
        public string getISBN(string tekst)
        {
            string wynik = "";
            int pozstart = 0;
            int ile = 0;
            for (int i = 0; i < tekst.Length; i++)
            {
                if (tekst[i] == ';')
                {
                    ile++;
                    if (ile == 5)
                    {
                        wynik = tekst.Substring(pozstart + 2, i - pozstart - 2);
                        break;
                    }
                    if (ile == 4)
                        pozstart = i;
                }
            }
            return wynik;
        }
        public string gatunki(string tekst)
        {
            string wynik = "";
            int pozstart = 0;
            int ile = 0;
            for (int i = 0; i < tekst.Length; i++)
            {
                if (tekst[i] == ';')
                {
                    ile++;
                    if (ile == 6)
                    {
                        wynik = tekst.Substring(pozstart + 2, i -pozstart-2);
                        break;
                    }
                    if (ile == 5)
                        pozstart = i;
                }
            }
            return wynik;
        }
        public string autorzy(string tekst)
        {
            string wynik = "";
            int pozstart = 0;
            int ile = 0;
            for (int i = 0; i < tekst.Length; i++)
            {
                if (tekst[i] == ';')
                {
                    ile++;
                    if (ile == 7)
                    {
                        wynik = tekst.Substring(pozstart + 2, i - pozstart - 2);
                        break;
                    }
                    if (ile == 6)
                        pozstart = i;
                }
            }
            return wynik;
        }
        public int getilosc(string tekst)
        {
            string wynik = "";
            int pozstart = 0;
            int ile = 0;
            for (int i = 0; i < tekst.Length; i++)
            {
                if (tekst[i] == ';')
                {
                    ile++;
                    if (ile == 7)
                        pozstart = i;
                }
            }
            wynik = tekst.Substring(pozstart+1);
            return int.Parse(wynik);
        }
        public int usunksiaz(int indeks)
        {
            //usunięcie książki z bazy danych
            return indeks;
        }
    }
}
