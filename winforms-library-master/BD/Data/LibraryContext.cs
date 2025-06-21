using Microsoft.EntityFrameworkCore;
using LibraryManagement.Models.Entities;
using LibraryManagement.Models.Entities.JunctionTables;

namespace LibraryManagement.Models.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
        {
        }
        
        public DbSet<Ksiazka> Ksiazki { get; set; }
        public DbSet<Autor> Autorzy { get; set; }
        public DbSet<Uzytkownik> Uzytkownicy { get; set; }
        public DbSet<Adres> Adresy { get; set; }
        public DbSet<Gatunek> Gatunki { get; set; }
        public DbSet<KsiazkaAutor> KsiazkiAutorzy { get; set; }
        public DbSet<KsiazkaGatunek> KsiazkiGatunki { get; set; }
        public DbSet<Wypozyczone> Wypozyczone { get; set; }
        public DbSet<Ulubione> Ulubione { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configure enum to string conversion for all book enums
            modelBuilder.Entity<Ksiazka>()
                .Property(k => k.Rodzaj)
                .HasConversion<string>();
                
            modelBuilder.Entity<Ksiazka>()
                .Property(k => k.RodzajOkladki)
                .HasConversion<string>();
                
            modelBuilder.Entity<Ksiazka>()
                .Property(k => k.Uszkodzenia)
                .HasConversion<string>();
                
            modelBuilder.Entity<Ksiazka>()
                .Property(k => k.Status)
                .HasConversion<string>();
            
            // SQLite-specific: Configure decimal precision for currency fields
            modelBuilder.Entity<Ksiazka>()
                .Property(k => k.Cena)
                .HasColumnType("TEXT")
                .HasConversion<string>();
                
            modelBuilder.Entity<Uzytkownik>()
                .Property(u => u.Saldo)
                .HasColumnType("TEXT")
                .HasConversion<string>();
            
            // Configure composite primary keys for junction tables
            modelBuilder.Entity<KsiazkaAutor>()
                .HasKey(ka => new { ka.KsiazkaId, ka.AutorId });
                
            modelBuilder.Entity<KsiazkaGatunek>()
                .HasKey(kg => new { kg.KsiazkaId, kg.GatunekId });
                
            modelBuilder.Entity<Ulubione>()
                .HasKey(u => new { u.KsiazkaId, u.UzytkownikId });
            
            // Configure 1:1 relationship between User and Address
            modelBuilder.Entity<Uzytkownik>()
                .HasOne(u => u.Adres)
                .WithOne(a => a.Uzytkownik)
                .HasForeignKey<Adres>(a => a.UzytkownikId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Configure many-to-many relationships
            modelBuilder.Entity<KsiazkaAutor>()
                .HasOne(ka => ka.Ksiazka)
                .WithMany(k => k.KsiazkaAutorzy)
                .HasForeignKey(ka => ka.KsiazkaId)
                .OnDelete(DeleteBehavior.Cascade);
                
            modelBuilder.Entity<KsiazkaAutor>()
                .HasOne(ka => ka.Autor)
                .WithMany(a => a.KsiazkaAutorzy)
                .HasForeignKey(ka => ka.AutorId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<KsiazkaGatunek>()
                .HasOne(kg => kg.Ksiazka)
                .WithMany(k => k.KsiazkaGatunki)
                .HasForeignKey(kg => kg.KsiazkaId)
                .OnDelete(DeleteBehavior.Cascade);
                
            modelBuilder.Entity<KsiazkaGatunek>()
                .HasOne(kg => kg.Gatunek)
                .WithMany(g => g.KsiazkaGatunki)
                .HasForeignKey(kg => kg.GatunekId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Configure one-to-many relationships for loans
            modelBuilder.Entity<Wypozyczone>()
                .HasOne(w => w.Ksiazka)
                .WithMany(k => k.Wypozyczenia)
                .HasForeignKey(w => w.KsiazkaId)
                .OnDelete(DeleteBehavior.Cascade);
                
            modelBuilder.Entity<Wypozyczone>()
                .HasOne(w => w.Uzytkownik)
                .WithMany(u => u.Wypozyczenia)
                .HasForeignKey(w => w.UzytkownikId)
                .OnDelete(DeleteBehavior.Cascade);
                
            // SQLite: Ensure a book can only be borrowed by one person at a time
            modelBuilder.Entity<Wypozyczone>()
                .HasIndex(w => w.KsiazkaId)
                .IsUnique();
            
            modelBuilder.Entity<Ulubione>()
                .HasOne(u => u.Ksiazka)
                .WithMany(k => k.Ulubione)
                .HasForeignKey(u => u.KsiazkaId)
                .OnDelete(DeleteBehavior.Cascade);
                
            modelBuilder.Entity<Ulubione>()
                .HasOne(u => u.Uzytkownik)
                .WithMany(uz => uz.Ulubione)
                .HasForeignKey(u => u.UzytkownikId)
                .OnDelete(DeleteBehavior.Cascade);
                
            // Business logic constraints (NOT performance indexes)
            // Unique email constraint
            modelBuilder.Entity<Uzytkownik>()
                .HasIndex(u => u.Email)
                .IsUnique();
                
            // Unique genre names constraint
            modelBuilder.Entity<Gatunek>()
                .HasIndex(g => g.Gatunki)
                .IsUnique();
                
            // Seed data for Gatunki
            modelBuilder.Entity<Gatunek>().HasData(
                new Gatunek { Id = 1, Gatunki = "Poezja" },
                new Gatunek { Id = 2, Gatunki = "Dramat" },
                new Gatunek { Id = 3, Gatunki = "Słowniki i encyklopedie" },
                new Gatunek { Id = 4, Gatunki = "Psychologia" },
                new Gatunek { Id = 5, Gatunki = "Książki kucharskie" },
                new Gatunek { Id = 6, Gatunki = "Historia" },
                new Gatunek { Id = 7, Gatunki = "Filozofia" },
                new Gatunek { Id = 8, Gatunki = "Medycyna" },
                new Gatunek { Id = 9, Gatunki = "Nauki ścisłe i przyrodnicze" },
                new Gatunek { Id = 10, Gatunki = "Biografie" },
                new Gatunek { Id = 11, Gatunki = "Fantastyka" },
                new Gatunek { Id = 12, Gatunki = "Literatura dziecięca" },
                new Gatunek { Id = 13, Gatunki = "Literatura wojenna" },
                new Gatunek { Id = 14, Gatunki = "SciFi" }
            );
        }
    }
}