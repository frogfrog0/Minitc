using LibraryManagement.Models.Data;
using LibraryManagement.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace LibraryApp.Model
{
    internal class Rejestracjamodel
    {
        private readonly LibraryContext _context;

        public Rejestracjamodel()
        {
            var connectionString = "Data Source=Library.db";
            var optionsBuilder = new DbContextOptionsBuilder<LibraryContext>();
            optionsBuilder.UseSqlite(connectionString);
            _context = new LibraryContext(optionsBuilder.Options);
        }

        public RegistrationResult Register(string imie, string nazwisko, string nazwauz, string haslo,
            string email, string numtel, DateTime dataUrodzenia, string woj, string miasto,
            string ul, string numdom, string nummiesz, string kod1, string kod2)
        {
            try
            {
                // Basic validation
                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(haslo))
                {
                    return new RegistrationResult { Success = false, ErrorMessage = "Email i hasło są wymagane" };
                }

                // Check if email already exists
                if (_context.Uzytkownicy.Any(u => u.Email == email))
                {
                    return new RegistrationResult { Success = false, ErrorMessage = "Użytkownik z tym emailem już istnieje" };
                }

                // Create new user
                var newUser = new Uzytkownik
                {
                    Imie = imie,
                    Nazwisko = nazwisko,
                    Email = email,
                    Haslo = HashPassword(haslo),
                    NumTelefonu = numtel,
                    DataUrodzenia = dataUrodzenia,
                    Saldo = 0,
                    Ban = false,
                    IsAdmin = false
                };

                _context.Uzytkownicy.Add(newUser);
                _context.SaveChanges();

                // Create address
                var kodPocztowy = $"{kod1}-{kod2}";
                var adres = new Adres
                {
                    UzytkownikId = newUser.Id,
                    Miasto = miasto,
                    Ulica = ul,
                    Wojewodztwo = woj,
                    NrDomu = numdom,
                    NrMieszkania = nummiesz,
                    KodPocztowy = kodPocztowy
                };

                _context.Adresy.Add(adres);
                _context.SaveChanges();

                // Reload user with address
                var userWithAddress = _context.Uzytkownicy
                    .Include(u => u.Adres)
                    .First(u => u.Id == newUser.Id);

                // Set session
                UserSession.Login(userWithAddress);

                return new RegistrationResult
                {
                    Success = true,
                    User = userWithAddress
                };
            }
            catch (Exception ex)
            {
                return new RegistrationResult { Success = false, ErrorMessage = "Błąd podczas rejestracji: " + ex.Message };
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + "salt"));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }

    public class RegistrationResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public Uzytkownik? User { get; set; }
    }
}