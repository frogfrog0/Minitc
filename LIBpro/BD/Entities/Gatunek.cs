using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LibraryManagement.Models.Entities.JunctionTables;

namespace LibraryManagement.Models.Entities
{
    [Table("GATUNEK")]
    public class Gatunek
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        
        [Column("GATUNKI")]
        [Required]
        [MaxLength(50)]
        public string Gatunki { get; set; }
        
        // Navigation property
        public virtual ICollection<KsiazkaGatunek> KsiazkaGatunki { get; set; } = new List<KsiazkaGatunek>();
    }
}