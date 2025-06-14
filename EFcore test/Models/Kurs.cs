using System.ComponentModel.DataAnnotations;

namespace EFcore_test.Models
{
    /// <summary>
    /// Kurs - encja reprezentująca tabelę Kursy
    /// Relacja M:N ze Student (kurs może mieć wielu studentów)
    /// </summary>
    public class Kurs
    {
        // Klucz główny tabeli Kursy
        public int Id { get; set; }

        [Required]                                     // NOT NULL constraint
        [StringLength(10)]                             // VARCHAR(10) w bazie danych
        public string Nazwa { get; set; } = string.Empty;

        // Navigation Property - relacja M:N ze Student
        // EF używa tej kolekcji do zarządzania tabelą łączącą KursStudent
        public List<Student> Studenci { get; set; } = new List<Student>();
    }
}