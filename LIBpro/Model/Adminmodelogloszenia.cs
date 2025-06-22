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
    internal class Adminmodelogloszenia
    {
        private readonly LibraryContext _context;

        public Adminmodelogloszenia()
        {
            var connectionString = "Data Source=Library.db";
            var optionsBuilder = new DbContextOptionsBuilder<LibraryContext>();
            optionsBuilder.UseSqlite(connectionString);
            _context = new LibraryContext(optionsBuilder.Options);
        }

        // Get all announcements
        public List<AnnouncementInfo> GetAllAnnouncements()
        {
            try
            {
                var announcements = _context.Ogloszenia
                    .OrderByDescending(o => o.DataUtworzenia)
                    .Select(o => new AnnouncementInfo
                    {
                        Id = o.Id,
                        Tytul = o.Tytul,
                        Opis = o.Opis,
                        DataUtworzenia = o.DataUtworzenia
                    })
                    .ToList();

                return announcements;
            }
            catch (Exception ex)
            {
                return new List<AnnouncementInfo>();
            }
        }

        // Add new announcement
        public AnnouncementResult AddAnnouncement(string tytul, string opis)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tytul) || string.IsNullOrWhiteSpace(opis))
                {
                    return new AnnouncementResult { Success = false, ErrorMessage = "Tytuł i opis są wymagane" };
                }

                var announcement = new Ogloszenie
                {
                    Tytul = tytul.Trim(),
                    Opis = opis.Trim(),
                    DataUtworzenia = DateTime.Now
                };

                _context.Ogloszenia.Add(announcement);
                _context.SaveChanges();

                var announcementInfo = new AnnouncementInfo
                {
                    Id = announcement.Id,
                    Tytul = announcement.Tytul,
                    Opis = announcement.Opis,
                    DataUtworzenia = announcement.DataUtworzenia
                };

                return new AnnouncementResult
                {
                    Success = true,
                    Message = "Ogłoszenie zostało dodane",
                    Announcement = announcementInfo
                };
            }
            catch (Exception ex)
            {
                return new AnnouncementResult { Success = false, ErrorMessage = "Błąd podczas dodawania ogłoszenia: " + ex.Message };
            }
        }

        // Update existing announcement
        public AnnouncementResult UpdateAnnouncement(int id, string tytul, string opis)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tytul) || string.IsNullOrWhiteSpace(opis))
                {
                    return new AnnouncementResult { Success = false, ErrorMessage = "Tytuł i opis są wymagane" };
                }

                var announcement = _context.Ogloszenia.Find(id);
                if (announcement == null)
                {
                    return new AnnouncementResult { Success = false, ErrorMessage = "Nie znaleziono ogłoszenia" };
                }

                announcement.Tytul = tytul.Trim();
                announcement.Opis = opis.Trim();
                _context.SaveChanges();

                var announcementInfo = new AnnouncementInfo
                {
                    Id = announcement.Id,
                    Tytul = announcement.Tytul,
                    Opis = announcement.Opis,
                    DataUtworzenia = announcement.DataUtworzenia
                };

                return new AnnouncementResult
                {
                    Success = true,
                    Message = "Ogłoszenie zostało zaktualizowane",
                    Announcement = announcementInfo
                };
            }
            catch (Exception ex)
            {
                return new AnnouncementResult { Success = false, ErrorMessage = "Błąd podczas aktualizacji ogłoszenia: " + ex.Message };
            }
        }

        // Delete announcement
        public AnnouncementResult DeleteAnnouncement(int id)
        {
            try
            {
                var announcement = _context.Ogloszenia.Find(id);
                if (announcement == null)
                {
                    return new AnnouncementResult { Success = false, ErrorMessage = "Nie znaleziono ogłoszenia" };
                }

                _context.Ogloszenia.Remove(announcement);
                _context.SaveChanges();

                return new AnnouncementResult
                {
                    Success = true,
                    Message = "Ogłoszenie zostało usunięte"
                };
            }
            catch (Exception ex)
            {
                return new AnnouncementResult { Success = false, ErrorMessage = "Błąd podczas usuwania ogłoszenia: " + ex.Message };
            }
        }

        // Get specific announcement by ID
        public AnnouncementInfo? GetAnnouncementById(int id)
        {
            try
            {
                var announcement = _context.Ogloszenia
                    .Where(o => o.Id == id)
                    .Select(o => new AnnouncementInfo
                    {
                        Id = o.Id,
                        Tytul = o.Tytul,
                        Opis = o.Opis,
                        DataUtworzenia = o.DataUtworzenia
                    })
                    .FirstOrDefault();

                return announcement;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // Legacy methods for compatibility - these now work with structured data
        public string dodajogloszenie(string tytul, string opis)
        {
            var result = AddAnnouncement(tytul, opis);
            return result.Success ? result.Announcement?.ToString() ?? "" : "";
        }

        public string gettytul(string ogloszenie)
        {
            // This will be handled by the presenter now
            return "";
        }

        public string getopis(string ogloszenie)
        {
            // This will be handled by the presenter now
            return "";
        }

        public string usunogloszenie(string ogloszenie)
        {
            // This will be handled by the presenter now
            return ogloszenie;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }

    // Data transfer objects
    public class AnnouncementInfo
    {
        public int Id { get; set; }
        public string Tytul { get; set; } = string.Empty;
        public string Opis { get; set; } = string.Empty;
        public DateTime DataUtworzenia { get; set; }

        public override string ToString()
        {
            return $"{Tytul} ({DataUtworzenia:dd.MM.yyyy HH:mm})";
        }

        public string GetDisplayText()
        {
            return $"{Tytul}\n{DataUtworzenia:dd.MM.yyyy HH:mm}\n\n{Opis}";
        }
    }

    public class AnnouncementResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public AnnouncementInfo? Announcement { get; set; }
    }
}