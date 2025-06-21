using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LibraryManagement.Models.Entities.JunctionTables;

namespace LibraryManagement.Models.Entities
{
    [Table("UŻYTKOWNICY")]
    public class Uzytkownik
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        
        [Column("IMIĘ")]
        [Required]
        [MaxLength(50)]
        public string Imie { get; set; }
        
        [Column("NAZWISKO")]
        [Required]
        [MaxLength(50)]
        public string Nazwisko { get; set; }
        
        [Column("DATA_URODZENIA")]
        public DateTime? DataUrodzenia { get; set; }
        
        [Column("SALDO")]
        // SQLite handles decimal conversion in DbContext
        public decimal Saldo { get; set; }
        
        [Column("EMAIL")]
        [MaxLength(100)]
        public string Email { get; set; }
        
        [Column("BAN")]
        public bool Ban { get; set; }
        
        [Column("NUM_TELEFONU")]
        [MaxLength(20)]
        public string NumTelefonu { get; set; }

        [Column("HASLO")]
        [Required]
        [MaxLength(255)]
        public string Haslo { get; set; }

        [Column("ADMIN")]
        public bool IsAdmin { get; set; } = false;

        // Navigation properties
        public virtual Adres Adres { get; set; }
        public virtual ICollection<Wypozyczone> Wypozyczenia { get; set; } = new List<Wypozyczone>();
        public virtual ICollection<Ulubione> Ulubione { get; set; } = new List<Ulubione>();
    }
}