using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Model
{
    internal class Adminmodelogloszenia
    {
        public string dodajogloszenie(string tytul, string opis)
        {
            return new StringBuilder(tytul).Append("; ").Append(opis).ToString();
        }
        public string gettytul(string ogloszenie)
        {
            for (int i = 0; i < ogloszenie.Length; i++)
            {
                if (ogloszenie[i] == ';')
                {
                    return ogloszenie.Substring(0,i);
                }
            }
            return "";
        }
        public string getopis(string ogloszenie) {
            for (int i = 0; i < ogloszenie.Length; i++)
            {
                if (ogloszenie[i] == ';')
                {
                    return ogloszenie.Substring(i+2);
                }
            }
            return "";
        }
        public string usunogloszenie(string ogloszenie)
        {
            return ogloszenie;
        }

    }
}
