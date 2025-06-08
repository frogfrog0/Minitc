using System;
using EFcore_test.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace EFcore_test.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Student> Studenci { get; set; } = null!;
        public DbSet<StudentProfile> Profile { get; set; } = null!;
        public DbSet<Kurs> Kursy { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=studenci.db");
            //optionsBuilder.LogTo(Console.WriteLine, LogLevel.Error);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Konfiguracja relacji 1:1 Student -> Profile
            modelBuilder.Entity<Student>()
                .HasOne(s => s.Profile)                // Student ma jeden profil
                .WithOne(p => p.Student)               // Profil należy do jednego studenta
                .HasForeignKey<StudentProfile>(p => p.StudentId); // Klucz obcy w Profile

            // Konfiguracja unikalności IdStudenta
            modelBuilder.Entity<Student>()
                .HasIndex(s => s.IdStudenta)
                .IsUnique();

            // Konfiguracja relacji M:N Student <-> Kurs
            modelBuilder.Entity<Student>()
                .HasMany(s => s.Kursy)                 // Student ma wiele kursów
                .WithMany(k => k.Studenci);            // Kurs ma wielu studentów
                                                       // EF automatycznie tworzy tabelę łączącą

            // Dane przykładowe - Kursy
            modelBuilder.Entity<Kurs>().HasData(
                new Kurs { Id = 1, Nazwa = "MD" },
                new Kurs { Id = 2, Nazwa = "ZSI" },
                new Kurs { Id = 3, Nazwa = "SSI" }
            );

            // Dane przykładowe - Studenci
            modelBuilder.Entity<Student>().HasData(
                new Student { Id = 1, IdStudenta = 1 },
                new Student { Id = 2, IdStudenta = 2 },
                new Student { Id = 3, IdStudenta = 3 },
                new Student { Id = 4, IdStudenta = 4 }
            );

            // Dane przykładowe - Profile studentów
            modelBuilder.Entity<StudentProfile>().HasData(
                new StudentProfile
                {
                    Id = 1,
                    StudentId = 1,
                    Imie = "Jan",
                    Nazwisko = "Kowalski",
                    DataUrodzenia = new DateTime(2000, 5, 15),
                    RokStudiow = 2
                },
                new StudentProfile
                {
                    Id = 2,
                    StudentId = 2,
                    Imie = "Anna",
                    Nazwisko = "Nowak",
                    DataUrodzenia = new DateTime(2001, 3, 22),
                    RokStudiow = 1
                },
                new StudentProfile
                {
                    Id = 3,
                    StudentId = 3,
                    Imie = "Piotr",
                    Nazwisko = "Zieliński",
                    DataUrodzenia = new DateTime(1999, 8, 10),
                    RokStudiow = 3
                },
                new StudentProfile
                {
                    Id = 4,
                    StudentId = 4,
                    Imie = "Maria",
                    Nazwisko = "Wiśniewska",
                    DataUrodzenia = new DateTime(2000, 12, 5),
                    RokStudiow = 2
                }
            );
        }
    }
}
