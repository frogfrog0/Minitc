using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LibraryManagement.Models.Enums;
using LibraryManagement.Models.Entities.JunctionTables;

namespace LibraryManagement.Models.Entities
{
    [Table("KSIĄŻKI")]
    public class Ksiazka
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        
        [Column("TYTUŁ")]
        [Required]
        [MaxLength(200)]
        public string Tytul { get; set; }
        
        [Column("CENA")]
        // SQLite handles decimal conversion in DbContext
        public decimal Cena { get; set; } = 0; // Default value
        
        [Column("RODZAJ")]
        [Required]
        public RodzajKsiazki Rodzaj { get; set; }
        
        [Column("RODZAJ_OKŁADKI")]
        [Required]
        public RodzajOkladki RodzajOkladki { get; set; }
        
        [Column("USZKODZENIA")]
        [Required]
        public Uszkodzenia Uszkodzenia { get; set; } = Uszkodzenia.Brak; // Default: no damage
        
        [Column("STATUS")]
        [Required]
        public StatusKsiazki Status { get; set; } = StatusKsiazki.Dostępna; // Default: available

        [Column("ISBN")]
        [MaxLength(20)]
        public string? ISBN { get; set; }

        [Column("ROK_WYDANIA")]
        public int? RokWydania { get; set; }

        // Navigation properties for many-to-many relationships
        public virtual ICollection<KsiazkaAutor> KsiazkaAutorzy { get; set; } = new List<KsiazkaAutor>();
        public virtual ICollection<KsiazkaGatunek> KsiazkaGatunki { get; set; } = new List<KsiazkaGatunek>();
        public virtual ICollection<Wypozyczone> Wypozyczenia { get; set; } = new List<Wypozyczone>();
        public virtual ICollection<Ulubione> Ulubione { get; set; } = new List<Ulubione>();
    }
}