using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LibraryManagement.Models.Entities.JunctionTables;

namespace LibraryManagement.Models.Entities
{
    [Table("AUTORZY")]
    public class Autor
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
        
        [Column("NARODOWOŚĆ")]
        [MaxLength(50)]
        public string Narodowosc { get; set; }
        
        [Column("DATA_URODZENIA")]
        public DateTime? DataUrodzenia { get; set; }
        
        [Column("DATA_ŚMIERCI")]
        public DateTime? DataSmierci { get; set; }
        
        // Navigation property
        public virtual ICollection<KsiazkaAutor> KsiazkaAutorzy { get; set; } = new List<KsiazkaAutor>();
    }
}