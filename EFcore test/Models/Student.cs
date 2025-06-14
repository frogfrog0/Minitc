using System.ComponentModel.DataAnnotations;

namespace EFcore_test.Models
{
    /// <summary>
    /// Student - główna encja reprezentująca tabelę Studenci
    /// Zawiera relacje 1:1 z Profile i M:N z Kurs
    /// </summary>
    public class Student
    {
        // Klucz główny (Primary Key) - EF rozpoznaje automatycznie przez nazwę "Id"
        public int Id { get; set; }                    // AUTO INCREMENT w bazie danych

        [Required]                                     // NOT NULL constraint w bazie
        public int IdStudenta { get; set; }            // Unikalny numer studenta (UNIQUE constraint)

        // === NAVIGATION PROPERTIES - Właściwości nawigacyjne ===

        // Relacja 1:1 z StudentProfile
        // EF tworzy Foreign Key w tabeli Profile wskazujący na Student.Id
        public StudentProfile Profile { get; set; } = null!;

        // Relacja M:N z Kurs
        // EF automatycznie tworzy tabelę łączącą KursStudent z kluczami obcymi:
        // - StudenciId (FK do Studenci.Id)
        // - KursyId (FK do Kursy.Id)
        public List<Kurs> Kursy { get; set; } = new List<Kurs>();
    }
}