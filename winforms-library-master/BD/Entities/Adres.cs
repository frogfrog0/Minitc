using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models.Entities
{
    [Table("ADRES")]
    public class Adres
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        
        [Column("MIASTO")]
        [Required]
        [MaxLength(100)]
        public string Miasto { get; set; } = "Gliwice"; // Default value
        
        [Column("ULICA")]
        [Required]
        [MaxLength(100)]
        public string Ulica { get; set; }
        
        [Column("WOJEWÓDZTWO")]
        [MaxLength(50)]
        public string Wojewodztwo { get; set; } = "Śląskie"; // Default value
        
        [Column("NR_DOMU")]
        [MaxLength(10)]
        public string NrDomu { get; set; }
        
        [Column("NR_MIESZKANIA")]
        [MaxLength(10)]
        public string NrMieszkania { get; set; }
        
        [Column("KOD_POCZTOWY")]
        [MaxLength(10)]
        public string KodPocztowy { get; set; }
        
        // Foreign key for 1:1 relationship with User
        [ForeignKey("Uzytkownik")]
        public int UzytkownikId { get; set; }
        
        // Navigation property
        public virtual Uzytkownik Uzytkownik { get; set; }
    }
}