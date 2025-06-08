using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFcore_test.Models
{
    public class Student
    {
        public int Id { get; set; }                    // Klucz główny (automatyczny)

        [Required]
        public int IdStudenta { get; set; }            // Unikalny numer studenta (1, 2, 3...)

        // Relacja 1:1 z Profile
        public StudentProfile Profile { get; set; } = null!;

        // Relacja M:N z Kursami
        public List<Kurs> Kursy { get; set; } = new List<Kurs>();
    }
}
