using LibraryApp.View;
using LibraryManagement.Models.Data;
using LibraryManagement.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace LibraryApp.Model
{
    internal class Logowaniemodel
    {
        private readonly LibraryContext _context;

        public Logowaniemodel()
        {
            // Create DbContext - we'll improve this with DI later if needed
            var connectionString = "Data Source=Library.db";
            var optionsBuilder = new DbContextOptionsBuilder<LibraryContext>();
            optionsBuilder.UseSqlite(connectionString);
            _context = new LibraryContext(optionsBuilder.Options);
        }

        public LoginResult Login(string email, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                {
                    return new LoginResult { Success = false, ErrorMessage = "Email i hasło są wymagane" };
                }

                var hashedPassword = HashPassword(password);
                var user = _context.Uzytkownicy
                    .Include(u => u.Adres)
                    .FirstOrDefault(u => u.Email == email && u.Haslo == hashedPassword);

                if (user == null)
                {
                    return new LoginResult { Success = false, ErrorMessage = "Nieprawidłowy email lub hasło" };
                }

                if (user.Ban)
                {
                    return new LoginResult { Success = false, ErrorMessage = "Konto zostało zablokowane" };
                }

                // Set session
                UserSession.Login(user);

                return new LoginResult
                {
                    Success = true,
                    User = user,
                    IsAdmin = user.IsAdmin
                };
            }
            catch (Exception ex)
            {
                return new LoginResult { Success = false, ErrorMessage = "Błąd podczas logowania: " + ex.Message };
            }
        }

        private string HashPassword(string password)
        {
            // Basic hashing - in production would use proper password hashing
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

    public class LoginResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public Uzytkownik? User { get; set; }
        public bool IsAdmin { get; set; }
    }
}