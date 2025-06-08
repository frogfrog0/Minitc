using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace EFcore_test.Models
{
    public class StudentProfile
    {
        public int Id { get; set; }                    // Klucz główny

        [Required]
        public int StudentId { get; set; }             // Klucz obcy do Student

        [Required]
        [StringLength(50)]
        public string Imie { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Nazwisko { get; set; } = string.Empty;

        [Required]
        public DateTime DataUrodzenia { get; set; }

        [Range(1, 5)]
        public int RokStudiow { get; set; }

        // Nawigacja do Student
        public Student Student { get; set; } = null!;
    }
}
