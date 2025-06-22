using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models.Entities
{
    [Table("OGLOSZENIA")]
    public class Ogloszenie
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        
        [Column("TYTUL")]
        [Required]
        [MaxLength(200)]
        public string Tytul { get; set; }
        
        [Column("OPIS")]
        [Required]
        [MaxLength(1000)]
        public string Opis { get; set; }
        
        [Column("DATA_UTWORZENIA")]
        [Required]
        public DateTime DataUtworzenia { get; set; } = DateTime.Now;
    }
}