using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace projekt_2_na_punkty.Model
{
    internal class Totalcommander
    {
        string aktualna_sciezka = "";
        string pop_sciezka = "";
        string aktualna_sciezka2 = "";
        string pop_sciezka2 = "";
        string tekstdokopiowania = "";
        public string[] Dyski()
        {
            return Environment.GetLogicalDrives();
        }
        public string acdysk(string scierz)
        {
            this.aktualna_sciezka=scierz;
            return this.aktualna_sciezka;
        }
        public string acdysk2(string scierz)
        {
            this.aktualna_sciezka2 = scierz;
            return this.aktualna_sciezka2;
        }
        public string acpath()
        {
            return this.aktualna_sciezka;
        }
        public string acpath2()
        {
            return this.aktualna_sciezka2;
        }
        public string[] treecontent()
        {
            if (this.pop_sciezka != this.aktualna_sciezka)
            {
                bool koniec = true;
                int pozstart = 0;
                for(int i=this.aktualna_sciezka.Length-2; i>=0; i--)
                {
                    if (this.aktualna_sciezka[i] == '\\')
                    {
                        koniec = false;
                        break;
                    }
                }
                string[] tab = new string[Directory.GetDirectories(this.aktualna_sciezka).Length+Directory.GetFiles(this.aktualna_sciezka).Length + 1];
                if (!koniec)
                {
                    tab[0] = "..";
                    pozstart = 1;
                }
                for(int i=pozstart; i < Directory.GetDirectories(this.aktualna_sciezka).Length +pozstart; i++)
                {
                    tab[i] = Directory.GetDirectories(this.aktualna_sciezka)[i-pozstart ] + "\\" ;
                    tab[i] = tab[i].Substring(this.aktualna_sciezka.Length);
                    tab[i] = "<" + aktualna_sciezka[0] + ">" + tab[i];
                }
                for (int i = pozstart + Directory.GetDirectories(this.aktualna_sciezka).Length; i < Directory.GetFiles(this.aktualna_sciezka).Length+ pozstart + Directory.GetDirectories(this.aktualna_sciezka).Length; i++)
                {
                    tab[i] = Directory.GetFiles(this.aktualna_sciezka)[i - pozstart - Directory.GetDirectories(this.aktualna_sciezka).Length];
                    tab[i] = tab[i].Substring(this.aktualna_sciezka.Length);
                    tab[i] = "<" + aktualna_sciezka[0] + ">" + tab[i];
                }
                this.pop_sciezka = this.aktualna_sciezka;
               return tab;
            }
            return new string[1];
        }
        public string setpath(string tekst)
        {
            if (tekst == "..")
            {
                int poz = 0;
                for (int i=this.aktualna_sciezka.Length-3; i>=0; i--) {
                        poz += 1; 
                if (this.aktualna_sciezka[i] == '\\' && i-1>=0 && this.aktualna_sciezka[i-1]!='\\')
                    {
                        break;
                    }
                }
                this.aktualna_sciezka = this.aktualna_sciezka.Substring(0,this.aktualna_sciezka.Length - poz-1);
                return "d";
            }
            tekst = tekst.Substring(3);
            if (tekst[tekst.Length-1] != '\\')
            {
                tekstdokopiowania = tekst;
                return "f";
            }
            tekstdokopiowania = "";
            this.aktualna_sciezka += tekst;
            return "d";
            
        }
        public string setpath2(string tekst)
        {
            if (tekst == "..")
            {
                int poz = 0;
                for (int i = this.aktualna_sciezka2.Length - 3; i >= 0; i--)
                {
                    poz += 1;
                    if (this.aktualna_sciezka2[i] == '\\' && i - 1 >= 0 && this.aktualna_sciezka2[i - 1] != '\\')
                    {
                        break;
                    }
                }
                this.aktualna_sciezka2 = this.aktualna_sciezka2.Substring(0, this.aktualna_sciezka2.Length - poz - 1);
                return "d";
            }
            tekst = tekst.Substring(3);
            if (tekst[tekst.Length - 1] != '\\')
            {
                return "f";
            }
            this.aktualna_sciezka2 += tekst;
            return "d";

        }
        public string[] treecontent2()
        {
            if (this.pop_sciezka2 != this.aktualna_sciezka2)
            {
                bool koniec = true;
                int pozstart = 0;
                for (int i = this.aktualna_sciezka2.Length - 2; i >= 0; i--)
                {
                    if (this.aktualna_sciezka2[i] == '\\')
                    {
                        koniec = false;
                        break;
                    }
                }
                string[] tab = new string[Directory.GetDirectories(this.aktualna_sciezka2).Length + Directory.GetFiles(this.aktualna_sciezka2).Length + 1];
                if (!koniec)
                {
                    tab[0] = "..";
                    pozstart = 1;
                }
                for (int i = pozstart; i < Directory.GetDirectories(this.aktualna_sciezka2).Length + pozstart; i++)
                {
                    tab[i] = Directory.GetDirectories(this.aktualna_sciezka2)[i - pozstart] + "\\";
                    tab[i] = tab[i].Substring(this.aktualna_sciezka2.Length);
                    tab[i] = "<" + aktualna_sciezka2[0] + ">" + tab[i];
                }
                for (int i = pozstart + Directory.GetDirectories(this.aktualna_sciezka2).Length; i < Directory.GetFiles(this.aktualna_sciezka2).Length + pozstart + Directory.GetDirectories(this.aktualna_sciezka2).Length; i++)
                {
                    tab[i] = Directory.GetFiles(this.aktualna_sciezka2)[i - pozstart - Directory.GetDirectories(this.aktualna_sciezka2).Length];
                    tab[i] = tab[i].Substring(this.aktualna_sciezka2.Length);
                    tab[i] = "<" + aktualna_sciezka2[0] + ">" + tab[i];
                }
                this.pop_sciezka2 = this.aktualna_sciezka2;
                return tab;
            }
            return new string[1];
        }
        public void Copy()
        {
            if (this.tekstdokopiowania != "" && this.aktualna_sciezka2!="")
                File.Copy(this.aktualna_sciezka + this.tekstdokopiowania, this.aktualna_sciezka2);
            else
                MessageBox.Show("brak zaznaczonego pliku");
        }
    }
}
