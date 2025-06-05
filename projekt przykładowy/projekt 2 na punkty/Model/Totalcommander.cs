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
            return DriveInfo.GetDrives()
                .Where(drive => drive.IsReady)
                .Select(drive => drive.Name)
                .ToArray();
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
                try
                {
                    bool koniec = true;
                    int pozstart = 0;
                    for (int i = this.aktualna_sciezka.Length - 2; i >= 0; i--)
                    {
                        if (this.aktualna_sciezka[i] == '\\')
                        {
                            koniec = false;
                            break;
                        }
                    }

                    // Get directories and files with exception handling
                    string[] directories;
                    string[] files;

                    try
                    {
                        directories = Directory.GetDirectories(this.aktualna_sciezka);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        MessageBox.Show("Access denied to this directory");
                        return new string[1];
                    }

                    try
                    {
                        files = Directory.GetFiles(this.aktualna_sciezka);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        MessageBox.Show("Access denied to read files in this directory");
                        return new string[1];
                    }

                    string[] tab = new string[directories.Length + files.Length + 1];

                    if (!koniec)
                    {
                        tab[0] = "..";
                        pozstart = 1;
                    }

                    // Process directories
                    for (int i = pozstart; i < directories.Length + pozstart; i++)
                    {
                        tab[i] = directories[i - pozstart] + "\\";
                        tab[i] = tab[i].Substring(this.aktualna_sciezka.Length);
                        tab[i] = "<D>" + tab[i];
                    }

                    // Process files
                    for (int i = pozstart + directories.Length; i < files.Length + pozstart + directories.Length; i++)
                    {
                        tab[i] = files[i - pozstart - directories.Length];
                        tab[i] = tab[i].Substring(this.aktualna_sciezka.Length);
                    }

                    this.pop_sciezka = this.aktualna_sciezka;
                    return tab;
                }
                catch (DirectoryNotFoundException)
                {
                    MessageBox.Show("Directory not found");
                    return new string[1];
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error accessing directory: {ex.Message}");
                    return new string[1];
                }
            }
            return new string[1];
        }
        public string setpath(string tekst)
        {
            if (tekst == "..")
            {
                int poz = 0;
                for (int i = this.aktualna_sciezka.Length - 3; i >= 0; i--)
                {
                    poz += 1;
                    if (this.aktualna_sciezka[i] == '\\' && i - 1 >= 0 && this.aktualna_sciezka[i - 1] != '\\')
                    {
                        break;
                    }
                }
                this.aktualna_sciezka = this.aktualna_sciezka.Substring(0, this.aktualna_sciezka.Length - poz - 1);
                return "d";
            }

            // Safety check for Substring
            if (tekst.Length < 3)
            {
                tekstdokopiowania = tekst;  // It's a short filename
                return "f";
            }

            tekst = tekst.Substring(3);
            if (tekst[tekst.Length - 1] != '\\')
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

            // Safety check for Substring
            if (tekst.Length < 3)
            {
                return "f";  // It's a short filename, don't navigate
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
                try
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

                    // Get directories and files with exception handling
                    string[] directories;
                    string[] files;

                    try
                    {
                        directories = Directory.GetDirectories(this.aktualna_sciezka2);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        MessageBox.Show("Access denied to this directory");
                        return new string[1];
                    }

                    try
                    {
                        files = Directory.GetFiles(this.aktualna_sciezka2);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        MessageBox.Show("Access denied to read files in this directory");
                        return new string[1];
                    }

                    string[] tab = new string[directories.Length + files.Length + 1];

                    if (!koniec)
                    {
                        tab[0] = "..";
                        pozstart = 1;
                    }

                    // Process directories
                    for (int i = pozstart; i < directories.Length + pozstart; i++)
                    {
                        tab[i] = directories[i - pozstart] + "\\";
                        tab[i] = tab[i].Substring(this.aktualna_sciezka2.Length);
                        tab[i] = "<D>" + tab[i];
                    }

                    // Process files
                    for (int i = pozstart + directories.Length; i < files.Length + pozstart + directories.Length; i++)
                    {
                        tab[i] = files[i - pozstart - directories.Length];
                        tab[i] = tab[i].Substring(this.aktualna_sciezka2.Length);
                    }

                    this.pop_sciezka2 = this.aktualna_sciezka2;
                    return tab;
                }
                catch (DirectoryNotFoundException)
                {
                    MessageBox.Show("Directory not found");
                    return new string[1];
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error accessing directory: {ex.Message}");
                    return new string[1];
                }
            }
            return new string[1];
        }
        public void Copy()
        {
            if (this.tekstdokopiowania != "" && this.aktualna_sciezka2 != "")
            {
                try
                {
                    string sourceFile = this.aktualna_sciezka + this.tekstdokopiowania;
                    string destinationFile = this.aktualna_sciezka2 + this.tekstdokopiowania;

                    File.Copy(sourceFile, destinationFile, overwrite: true);
                    MessageBox.Show("File copied successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Copy failed: {ex.Message}");
                }
            }
            else
                MessageBox.Show("brak zaznaczonego pliku");
        }
    }
}
