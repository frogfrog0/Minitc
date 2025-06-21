using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.Models.Data;
using LibraryManagement.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Model
{
    internal class Adminmodeluzytkownicy
    {
        private readonly LibraryContext _context;

        public Adminmodeluzytkownicy()
        {
            var connectionString = "Data Source=Library.db";
            var optionsBuilder = new DbContextOptionsBuilder<LibraryContext>();
            optionsBuilder.UseSqlite(connectionString);
            _context = new LibraryContext(optionsBuilder.Options);
        }

        // Get all users for admin panel
        public List<UserInfo> GetAllUsers()
        {
            try
            {
                var users = _context.Uzytkownicy
                    .Include(u => u.Adres)
                    .Include(u => u.Wypozyczenia)
                    .Select(u => new UserInfo
                    {
                        Id = u.Id,
                        FullName = $"{u.Imie} {u.Nazwisko}",
                        Email = u.Email,
                        Phone = u.NumTelefonu ?? "Brak",
                        City = u.Adres != null ? u.Adres.Miasto : "Brak",
                        IsBanned = u.Ban,
                        IsAdmin = u.IsAdmin,
                        Saldo = u.Saldo,
                        BorrowedBooksCount = u.Wypozyczenia.Count,
                        RegistrationDate = u.Id // Using ID as proxy for registration order
                    })
                    .OrderBy(u => u.FullName)
                    .ToList();

                return users;
            }
            catch (Exception ex)
            {
                return new List<UserInfo>();
            }
        }

        // Get specific user details
        public UserInfo? GetUserById(int userId)
        {
            try
            {
                var user = _context.Uzytkownicy
                    .Include(u => u.Adres)
                    .Include(u => u.Wypozyczenia)
                    .Where(u => u.Id == userId)
                    .Select(u => new UserInfo
                    {
                        Id = u.Id,
                        FullName = $"{u.Imie} {u.Nazwisko}",
                        Email = u.Email,
                        Phone = u.NumTelefonu ?? "Brak",
                        City = u.Adres != null ? u.Adres.Miasto : "Brak",
                        IsBanned = u.Ban,
                        IsAdmin = u.IsAdmin,
                        Saldo = u.Saldo,
                        BorrowedBooksCount = u.Wypozyczenia.Count,
                        RegistrationDate = u.Id
                    })
                    .FirstOrDefault();

                return user;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // Toggle user ban status
        public AdminActionResult ToggleBan(int userId)
        {
            try
            {
                var user = _context.Uzytkownicy.Find(userId);
                if (user == null)
                {
                    return new AdminActionResult { Success = false, ErrorMessage = "Nie znaleziono użytkownika" };
                }

                if (user.IsAdmin)
                {
                    return new AdminActionResult { Success = false, ErrorMessage = "Nie można zbanować administratora" };
                }

                user.Ban = !user.Ban;
                _context.SaveChanges();

                var action = user.Ban ? "zbanowany" : "odbanowany";
                return new AdminActionResult
                {
                    Success = true,
                    Message = $"Użytkownik został {action}",
                    UpdatedUser = GetUserById(userId)
                };
            }
            catch (Exception ex)
            {
                return new AdminActionResult { Success = false, ErrorMessage = "Błąd podczas zmiany statusu: " + ex.Message };
            }
        }

        // Set user admin status
        public AdminActionResult SetAdminStatus(int userId, bool isAdmin)
        {
            try
            {
                var user = _context.Uzytkownicy.Find(userId);
                if (user == null)
                {
                    return new AdminActionResult { Success = false, ErrorMessage = "Nie znaleziono użytkownika" };
                }

                // Prevent removing admin from current user
                if (UserSession.UserId == userId && !isAdmin)
                {
                    return new AdminActionResult { Success = false, ErrorMessage = "Nie można usunąć uprawnień administratora dla bieżącego użytkownika" };
                }

                user.IsAdmin = isAdmin;
                _context.SaveChanges();

                var action = isAdmin ? "nadano uprawnienia administratora" : "usunięto uprawnienia administratora";
                return new AdminActionResult
                {
                    Success = true,
                    Message = $"Użytkownikowi {action}",
                    UpdatedUser = GetUserById(userId)
                };
            }
            catch (Exception ex)
            {
                return new AdminActionResult { Success = false, ErrorMessage = "Błąd podczas zmiany uprawnień: " + ex.Message };
            }
        }

        // Adjust user balance
        public AdminActionResult AdjustSaldo(int userId, decimal amount)
        {
            try
            {
                var user = _context.Uzytkownicy.Find(userId);
                if (user == null)
                {
                    return new AdminActionResult { Success = false, ErrorMessage = "Nie znaleziono użytkownika" };
                }

                user.Saldo += amount;
                _context.SaveChanges();

                return new AdminActionResult
                {
                    Success = true,
                    Message = $"Saldo użytkownika zostało {(amount > 0 ? "zwiększone" : "zmniejszone")} o {Math.Abs(amount):C}",
                    UpdatedUser = GetUserById(userId)
                };
            }
            catch (Exception ex)
            {
                return new AdminActionResult { Success = false, ErrorMessage = "Błąd podczas zmiany salda: " + ex.Message };
            }
        }

        // Legacy methods for compatibility with existing presenter
        public string ustawuz(string userInfo)
        {
            // This was used to set selected user - now handled by presenter
            return userInfo;
        }

        public string dajbana(string userInfo)
        {
            // This was used to toggle ban - now handled by new methods
            return userInfo;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }

    // Data transfer objects
    public class UserInfo
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public bool IsBanned { get; set; }
        public bool IsAdmin { get; set; }
        public decimal Saldo { get; set; }
        public int BorrowedBooksCount { get; set; }
        public int RegistrationDate { get; set; }

        public override string ToString()
        {
            var status = IsBanned ? " [ZBANOWANY]" : "";
            var admin = IsAdmin ? " [ADMIN]" : "";
            return $"{FullName} - {Email} (Saldo: {Saldo:C}, Wypożyczenia: {BorrowedBooksCount}){admin}{status}";
        }
    }

    public class AdminActionResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public UserInfo? UpdatedUser { get; set; }
    }
}