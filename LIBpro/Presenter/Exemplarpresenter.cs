using LibraryApp.Model;
using LibraryApp.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Presenter
{
    internal class Exemplarpresenter
    {
        ExemplarsForm _exemplarsform;
        BorrowingService _borrowingservice;
        public Exemplarpresenter(ExemplarsForm form, BorrowingService borrowingService) {
        
            _exemplarsform = form;
            _borrowingservice = borrowingService;
            _exemplarsform.porzycz += porzyczanie;
            _exemplarsform.zwroc += zwracanie;
            _exemplarsform.przedluz += przedluzanie;
        }
        private void porzyczanie(Exemplar exemplar)
        {
            _borrowingservice.BorrowBook(exemplar.BookId,(int) UserSession.UserId);
        }
        private void zwracanie(Exemplar exemplar)
        {
            _borrowingservice.ReturnBook(exemplar.BookId, (int)UserSession.UserId);
        }
        private void przedluzanie(Exemplar exemplar, int dlugosc)
        {
            _borrowingservice.ExtendBorrowing(exemplar.BookId, (int)UserSession.UserId, dlugosc);
        }
    }
}
