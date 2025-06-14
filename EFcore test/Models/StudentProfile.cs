using System.ComponentModel.DataAnnotations;

namespace EFcore_test.Models
{
    /// <summary>
    /// StudentProfile - encja zawierająca szczegółowe dane studenta
    /// Relacja 1:1 ze Student (jeden profil dla jednego studenta)
    /// </summary>
    public class StudentProfile
    {
        // Klucz główny tabeli Profile
        public int Id { get; set; }

        [Required]                                     // NOT NULL constraint
        public int StudentId { get; set; }             // Foreign Key do tabeli Studenci

        [Required]                                     // NOT NULL constraint
        [StringLength(50)]                             // VARCHAR(50) w bazie danych
        public string Imie { get; set; } = string.Empty;

        [Required]                                     // NOT NULL constraint
        [StringLength(50)]                             // VARCHAR(50) w bazie danych
        public string Nazwisko { get; set; } = string.Empty;

        [Required]                                     // NOT NULL constraint
        public DateTime DataUrodzenia { get; set; }   // DATE w bazie danych

        [Range(1, 5)]                                  // Check constraint: wartość między 1-5
        public int RokStudiow { get; set; }

        // Navigation Property - pozwala nawigować z Profile do Student
        // EF używa tej właściwości do tworzenia JOIN-ów w zapytaniach
        public Student Student { get; set; } = null!;
    }
}