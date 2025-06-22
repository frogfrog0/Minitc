using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LibraryManagement.Models.Entities;

namespace LibraryManagement.Models.Entities.JunctionTables
{
    // Junction Tables for Many-to-Many relationships

    [Table("KSIĄŻKI_AUTORZY")]
    public class KsiazkaAutor
    {
        [Column("ID_KSIĄŻKI")]
        public int KsiazkaId { get; set; }
        
        [Column("ID_AUTORA")]
        public int AutorId { get; set; }
        
        // Navigation properties
        [ForeignKey("KsiazkaId")]
        public virtual Ksiazka Ksiazka { get; set; }
        
        [ForeignKey("AutorId")]
        public virtual Autor Autor { get; set; }
    }

    [Table("KSIĄŻKI_GATUNKI")]
    public class KsiazkaGatunek
    {
        [Column("ID_KSIĄŻKI")]
        public int KsiazkaId { get; set; }
        
        [Column("ID_GATUNKU")]
        public int GatunekId { get; set; }
        
        // Navigation properties
        [ForeignKey("KsiazkaId")]
        public virtual Ksiazka Ksiazka { get; set; }
        
        [ForeignKey("GatunekId")]
        public virtual Gatunek Gatunek { get; set; }
    }

    [Table("WYPOŻYCZONE")]
    public class Wypozyczone
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        
        [Column("ID_KSIĄŻKI")]
        public int KsiazkaId { get; set; }
        
        [Column("ID_UŻYTKOWNIKA")]
        public int UzytkownikId { get; set; }
        
        [Column("DATA_WYPOŻYCZENIA")]
        [Required]
        public DateTime DataWypozyczenia { get; set; }
        
        [Column("TERMIN_ODDANIA")]
        [Required]
        public DateTime TerminOddania { get; set; }
        
        // Navigation properties
        [ForeignKey("KsiazkaId")]
        public virtual Ksiazka Ksiazka { get; set; }
        
        [ForeignKey("UzytkownikId")]
        public virtual Uzytkownik Uzytkownik { get; set; }
    }

    [Table("ULUBIONE")]
    public class Ulubione
    {
        [Column("ID_KSIĄŻKI")]
        public int KsiazkaId { get; set; }
        
        [Column("ID_UŻYTKOWNIKA")]
        public int UzytkownikId { get; set; }
        
        // Navigation properties
        [ForeignKey("KsiazkaId")]
        public virtual Ksiazka Ksiazka { get; set; }
        
        [ForeignKey("UzytkownikId")]
        public virtual Uzytkownik Uzytkownik { get; set; }
    }
}