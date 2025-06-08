using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace EFcore_test.Models
{
    public class Kurs
    {
        public int Id { get; set; }                    // Klucz główny

        [Required]
        [StringLength(10)]
        public string Nazwa { get; set; } = string.Empty;

        // Relacja M:N ze Studentami
        public List<Student> Studenci { get; set; } = new List<Student>();
    }
}

