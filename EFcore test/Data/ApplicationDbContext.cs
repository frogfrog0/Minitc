using System;
using EFcore_test.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EFcore_test.Data
{
    /// <summary>
    /// ApplicationDbContext - główna klasa Entity Framework
    /// Reprezentuje sesję z bazą danych i pozwala na komunikację z nią
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        // DbSet<T> reprezentuje tabelę w bazie danych
        // Każdy DbSet to kolekcja encji danego typu
        public DbSet<Student> Studenci { get; set; } = null!;        // Tabela "Studenci"
        public DbSet<StudentProfile> Profile { get; set; } = null!;   // Tabela "Profile"
        public DbSet<Kurs> Kursy { get; set; } = null!;              // Tabela "Kursy"

        /// <summary>
        /// OnConfiguring - konfiguruje połączenie z bazą danych
        /// Wywoływane automatycznie przez EF przy tworzeniu kontekstu
        /// </summary>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // UseSqlite - konfiguruje EF do używania bazy SQLite
            // "Data Source=studenci.db" - plik bazy danych będzie w folderze aplikacji
            optionsBuilder.UseSqlite("Data Source=studenci.db");

            // LogTo - opcjonalne logowanie SQL do konsoli (zakomentowane dla czystości)
            // optionsBuilder.LogTo(Console.WriteLine, LogLevel.Error);
        }

        /// <summary>
        /// OnModelCreating - konfiguruje model danych (relacje, constrainty, seed data)
        /// Wywoływane podczas tworzenia bazy lub migracji
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ========== KONFIGURACJA RELACJI ==========

            // Relacja 1:1 między Student a StudentProfile
            modelBuilder.Entity<Student>()
                .HasOne(s => s.Profile)                // Student ma jeden profil
                .WithOne(p => p.Student)               // Profil należy do jednego studenta
                .HasForeignKey<StudentProfile>(p => p.StudentId); // Klucz obcy w tabeli Profile
            // EF utworzy constraint FOREIGN KEY w bazie danych

            // Unikalność IdStudenta - każdy student ma unikalny numer
            modelBuilder.Entity<Student>()
                .HasIndex(s => s.IdStudenta)           // Tworzy indeks na kolumnie IdStudenta
                .IsUnique();                           // Zapewnia unikalność (UNIQUE constraint)

            // Relacja Many-to-Many między Student a Kurs
            modelBuilder.Entity<Student>()
                .HasMany(s => s.Kursy)                 // Student ma wiele kursów
                .WithMany(k => k.Studenci);            // Kurs ma wielu studentów
                                                       // EF automatycznie tworzy tabelę łączącą "KursStudent" z kluczami obcymi

            // ========== SEED DATA - dane przykładowe ==========
            // HasData dodaje przykładowe dane do bazy podczas pierwszego utworzenia

            // Kursy - 3 przykładowe kursy
            modelBuilder.Entity<Kurs>().HasData(
                new Kurs { Id = 1, Nazwa = "MD" },
                new Kurs { Id = 2, Nazwa = "ZSI" },
                new Kurs { Id = 3, Nazwa = "SSI" }
            );

            // UWAGA: Komentarz o migracjach - tylko informacyjny dla dewelopera
            //Add-Migration AddMoreStudents
            //Update-Database

            // Studenci - tylko klucze, szczegóły w Profile
            // 16 studentów: 4 pierwotni + 12 dodanych przez migrację
            modelBuilder.Entity<Student>().HasData(
                new Student { Id = 1, IdStudenta = 1 },
                new Student { Id = 2, IdStudenta = 2 },
                new Student { Id = 3, IdStudenta = 3 },
                new Student { Id = 4, IdStudenta = 4 },
                new Student { Id = 5, IdStudenta = 5 },
                new Student { Id = 6, IdStudenta = 6 },
                new Student { Id = 7, IdStudenta = 7 },
                new Student { Id = 8, IdStudenta = 8 },
                new Student { Id = 9, IdStudenta = 9 },
                new Student { Id = 10, IdStudenta = 10 },
                new Student { Id = 11, IdStudenta = 11 },
                new Student { Id = 12, IdStudenta = 12 },
                new Student { Id = 13, IdStudenta = 13 },
                new Student { Id = 14, IdStudenta = 14 },
                new Student { Id = 15, IdStudenta = 15 },
                new Student { Id = 16, IdStudenta = 16 }
            );

            // Profile studentów - szczegółowe dane osobowe
            // Każdy profil ma StudentId jako klucz obcy do tabeli Student
            modelBuilder.Entity<StudentProfile>().HasData(
                new StudentProfile
                {
                    Id = 1,
                    StudentId = 1,                     // Klucz obcy do Student (Id=1)
                    Imie = "Jan",
                    Nazwisko = "Kowalski",
                    DataUrodzenia = new DateTime(2000, 5, 15),
                    RokStudiow = 2
                },
                new StudentProfile
                {
                    Id = 2,
                    StudentId = 2,                     // Klucz obcy do Student (Id=2)
                    Imie = "Anna",
                    Nazwisko = "Nowak",
                    DataUrodzenia = new DateTime(2001, 3, 22),
                    RokStudiow = 1
                },
                new StudentProfile
                {
                    Id = 3,
                    StudentId = 3,                     // Klucz obcy do Student (Id=3)
                    Imie = "Piotr",
                    Nazwisko = "Zieliński",
                    DataUrodzenia = new DateTime(1999, 8, 10),
                    RokStudiow = 3
                },
                new StudentProfile
                {
                    Id = 4,
                    StudentId = 4,                     // Klucz obcy do Student (Id=4)
                    Imie = "Maria",
                    Nazwisko = "Wiśniewska",
                    DataUrodzenia = new DateTime(2000, 12, 5),
                    RokStudiow = 2
                },
                // === 12 DODATKOWYCH STUDENTÓW (dodanych przez migrację) ===
                new StudentProfile
                {
                    Id = 5,
                    StudentId = 5,
                    Imie = "Tomasz",
                    Nazwisko = "Lewandowski",
                    DataUrodzenia = new DateTime(1998, 7, 18),
                    RokStudiow = 4
                },
                new StudentProfile
                {
                    Id = 6,
                    StudentId = 6,
                    Imie = "Katarzyna",
                    Nazwisko = "Wójcik",
                    DataUrodzenia = new DateTime(2002, 1, 9),
                    RokStudiow = 1
                },
                new StudentProfile
                {
                    Id = 7,
                    StudentId = 7,
                    Imie = "Michał",
                    Nazwisko = "Kowalczyk",
                    DataUrodzenia = new DateTime(2000, 9, 3),
                    RokStudiow = 2
                },
                new StudentProfile
                {
                    Id = 8,
                    StudentId = 8,
                    Imie = "Magdalena",
                    Nazwisko = "Jankowska",
                    DataUrodzenia = new DateTime(1999, 4, 27),
                    RokStudiow = 3
                },
                new StudentProfile
                {
                    Id = 9,
                    StudentId = 9,
                    Imie = "Łukasz",
                    Nazwisko = "Mazur",
                    DataUrodzenia = new DateTime(2001, 11, 14),
                    RokStudiow = 1
                },
                new StudentProfile
                {
                    Id = 10,
                    StudentId = 10,
                    Imie = "Agnieszka",
                    Nazwisko = "Krawczyk",
                    DataUrodzenia = new DateTime(1998, 2, 20),
                    RokStudiow = 5
                },
                new StudentProfile
                {
                    Id = 11,
                    StudentId = 11,
                    Imie = "Paweł",
                    Nazwisko = "Szymański",
                    DataUrodzenia = new DateTime(2000, 6, 8),
                    RokStudiow = 2
                },
                new StudentProfile
                {
                    Id = 12,
                    StudentId = 12,
                    Imie = "Joanna",
                    Nazwisko = "Dąbrowska",
                    DataUrodzenia = new DateTime(2002, 10, 12),
                    RokStudiow = 1
                },
                new StudentProfile
                {
                    Id = 13,
                    StudentId = 13,
                    Imie = "Marcin",
                    Nazwisko = "Zając",
                    DataUrodzenia = new DateTime(1999, 12, 30),
                    RokStudiow = 3
                },
                new StudentProfile
                {
                    Id = 14,
                    StudentId = 14,
                    Imie = "Natalia",
                    Nazwisko = "Kozłowska",
                    DataUrodzenia = new DateTime(2001, 5, 16),
                    RokStudiow = 2
                },
                new StudentProfile
                {
                    Id = 15,
                    StudentId = 15,
                    Imie = "Adam",
                    Nazwisko = "Jankowski",
                    DataUrodzenia = new DateTime(1998, 8, 24),
                    RokStudiow = 4
                },
                new StudentProfile
                {
                    Id = 16,
                    StudentId = 16,
                    Imie = "Aleksandra",
                    Nazwisko = "Wojciechowska",
                    DataUrodzenia = new DateTime(2000, 3, 7),
                    RokStudiow = 2
                }
            );
        }
    }
}