using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Model
{
    public class Exemplar
    {
        public string Name { get; set; }
        public DateTime? BorrowingDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public bool HasReturnDatePassed => ReturnDate < DateTime.Now && ReturnDate.HasValue;
        public bool IsAvailable => !BorrowingDate.HasValue;
        // tu trzeba będzie dorobić tworzenie zamówienia
    }
}
