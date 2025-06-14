using Microsoft.EntityFrameworkCore;
using EFcore_test.Data;
using EFcore_test.Models;

namespace EFcore_test
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Tworzenie kontekstu Entity Framework
            // 'using' automatycznie zwalnia zasoby po zakończeniu
            using var context = new ApplicationDbContext();

            // EnsureCreatedAsync() - sprawdza czy baza istnieje
            // Jeśli NIE istnieje - tworzy bazę i stosuje seed data
            // Jeśli istnieje - nie robi nic (bezpieczne wielokrotne wywoływanie)
            await context.Database.EnsureCreatedAsync();
            Console.WriteLine("Baza danych gotowa\n");

            await ShowMenu(context);
        }

        /// <summary>
        /// Główne menu aplikacji - pętla obsługująca wybory użytkownika
        /// </summary>
        static async Task ShowMenu(ApplicationDbContext context)
        {
            while (true)
            {
                Console.WriteLine("\n=== MENU ===");
                Console.WriteLine("1. Dodaj studenta");
                Console.WriteLine("2. Usuń studenta");
                Console.WriteLine("3. Aktualizuj dane studenta");
                Console.WriteLine("4. Zapisz studenta na kurs");
                Console.WriteLine("5. uczestnicy kursu");           // Uproszczona nazwa
                Console.WriteLine("6. dane studenta");              // Uproszczona nazwa
                Console.WriteLine("7. Studenci dane roku");         // Uproszczona nazwa
                Console.WriteLine("8. Wszyscy studenci");
                Console.WriteLine("0. Wyjście");
                Console.Write("\nWybierz opcję (0-8): ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": await DodajStudenta(context); break;
                    case "2": await UsunStudenta(context); break;
                    case "3": await AktualizujStudenta(context); break;
                    case "4": await ZapiszStudentaNaKurs(context); break;
                    case "5": await WypiszStudentowKursu(context); break;
                    case "6": await WypiszDaneStudenta(context); break;
                    case "7": await WypiszStudentowRoku(context); break;
                    case "8": await PokazWszystkichStudentow(context); break;
                    case "0":
                        Console.WriteLine("Do widzenia!");
                        return;
                    default:
                        Console.WriteLine("Nieznana komenda");        // Zmienione z "Nieprawidłowa opcja"
                        break;
                }
            }
        }

        // ========== CREATE - Dodawanie nowego studenta ==========
        /// <summary>
        /// Dodaje nowego studenta wraz z profilem do bazy danych
        /// Automatycznie generuje IdStudenta (kolejny numer)
        /// </summary>
        static async Task DodajStudenta(ApplicationDbContext context)
        {
            Console.WriteLine("\nDodawanie nowego studenta:");

            // LINQ zapytanie - znajdź największy IdStudenta w bazie
            // AnyAsync() - sprawdza czy są jakiekolwiek rekordy (async)
            // MaxAsync() - znajduje maksymalną wartość (async)
            var maxId = await context.Studenci.AnyAsync()
                ? await context.Studenci.MaxAsync(s => s.IdStudenta)  // Jeśli są studenci - znajdź max
                : 0;                                                   // Jeśli baza pusta - zaczynaj od 0
            var nowyIdStudenta = maxId + 1;  // Następny numer studenta (17, 18, 19...)

            // Pobieranie danych od użytkownika...
            Console.Write("Imię: ");
            var imie = Console.ReadLine() ?? "";
            Console.Write("Nazwisko: ");
            var nazwisko = Console.ReadLine() ?? "";
            Console.Write("Data urodzenia (YYYY-MM-DD): ");
            if (!DateTime.TryParse(Console.ReadLine(), out var dataUr))
            {
                Console.WriteLine("Nieprawidłowy format daty!");
                return;
            }
            Console.Write("Rok studiów (1-5): ");
            if (!int.TryParse(Console.ReadLine(), out var rok) || rok < 1 || rok > 5)
            {
                Console.WriteLine("Rok musi być między 1-5!");
                return;
            }

            // Tworzenie nowego obiektu Student z zagnieżdżonym Profile
            // Relacja 1:1 - jeden student ma jeden profil
            var student = new Student
            {
                IdStudenta = nowyIdStudenta,
                Profile = new StudentProfile          // Tworzenie profilu w tym samym czasie
                {
                    Imie = imie,
                    Nazwisko = nazwisko,
                    DataUrodzenia = dataUr,
                    RokStudiow = rok
                    // StudentId zostanie automatycznie ustawione przez EF (klucz obcy)
                }
            };

            // Add() - dodaje obiekt do Change Trackera EF
            // Obiekt dostaje stan "Added" - zostanie INSERT-owany do bazy
            context.Studenci.Add(student);

            // SaveChangesAsync() - zapisuje wszystkie zmiany do bazy danych
            // EF automatycznie generuje i wykonuje zapytania SQL:
            // 1. INSERT INTO Studenci (IdStudenta) VALUES (@nowyIdStudenta)
            // 2. INSERT INTO Profile (StudentId, Imie, Nazwisko, DataUrodzenia, RokStudiow) VALUES (...)
            // 3. Ustawia klucze obce między tabelami
            await context.SaveChangesAsync();

            // Po SaveChanges() obiekt student.Id zostanie automatycznie ustawiony
            Console.WriteLine($"Student {imie} {nazwisko} dodany z ID: {nowyIdStudenta}");
        }

        // ========== DELETE - Usuwanie studenta ==========
        /// <summary>
        /// Usuwa studenta z bazy danych wraz z jego profilem
        /// Wykorzystuje Cascade Delete - usunięcie studenta automatycznie usuwa profil
        /// </summary>
        static async Task UsunStudenta(ApplicationDbContext context)
        {
            Console.Write("\nPodaj ID studenta do usunięcia: ");
            if (!int.TryParse(Console.ReadLine(), out var idStudenta))
            {
                Console.WriteLine("Nieprawidłowe ID!");
                return;
            }

            // LINQ + Include() - zapytanie z join
            // Include(s => s.Profile) - Eager Loading (ładuje profil razem ze studentem)
            // EF wygeneruje: SELECT * FROM Studenci s JOIN Profile p ON s.Id = p.StudentId WHERE s.IdStudenta = @idStudenta
            var student = await context.Studenci
                .Include(s => s.Profile)               // Ładuj profil razem ze studentem
                .FirstOrDefaultAsync(s => s.IdStudenta == idStudenta);  // WHERE IdStudenta = @idStudenta

            if (student == null)
            {
                Console.WriteLine($"Nie znaleziono studenta o ID: {idStudenta}");
                return;
            }

            // Potwierdzenie usunięcia - wyświetl dane studenta z załadowanego profilu
            Console.Write($"Czy usunąć studenta {student.Profile.Imie} {student.Profile.Nazwisko}? (t/n): ");
            if (Console.ReadLine()?.ToLower() == "t")
            {
                // Remove() - oznacza obiekt jako "Deleted" w Change Trackerze
                context.Studenci.Remove(student);

                // SaveChangesAsync() wygeneruje i wykona:
                // 1. DELETE FROM Profile WHERE StudentId = @studentId (cascade delete)
                // 2. DELETE FROM Studenci WHERE Id = @id
                // EF automatycznie obsługuje kolejność usuwania (relacje Foreign Key)
                await context.SaveChangesAsync();
                Console.WriteLine("Student usunięty!");
            }
        }

        // ========== UPDATE - Aktualizacja danych ==========
        /// <summary>
        /// Aktualizuje dane studenta w profilu
        /// Wykorzystuje Change Tracking - EF automatycznie wykrywa zmienione pola
        /// </summary>
        static async Task AktualizujStudenta(ApplicationDbContext context)
        {
            Console.Write("\nPodaj ID studenta: ");
            if (!int.TryParse(Console.ReadLine(), out var idStudenta))
            {
                Console.WriteLine("Nieprawidłowe ID!");
                return;
            }

            // Znajdź studenta z profilem (Eager Loading)
            // Include() tworzy JOIN między tabelami Studenci i Profile
            var student = await context.Studenci
                .Include(s => s.Profile)               // JOIN z tabelą Profile
                .FirstOrDefaultAsync(s => s.IdStudenta == idStudenta);

            if (student == null)
            {
                Console.WriteLine($"Nie znaleziono studenta o ID: {idStudenta}");
                return;
            }

            // Wyświetl aktualne dane
            Console.WriteLine($"Aktualnie: {student.Profile.Imie} {student.Profile.Nazwisko}, Rok: {student.Profile.RokStudiow}");

            // Modyfikacja właściwości obiektu
            // EF Change Tracker automatycznie wykrywa zmiany w załadowanych obiektach
            Console.Write("Nowe imię (Enter = bez zmiany): ");
            var noweImie = Console.ReadLine();
            if (!string.IsNullOrEmpty(noweImie))
                student.Profile.Imie = noweImie;       // Change Tracker: Profile = "Modified"

            Console.Write("Nowe nazwisko (Enter = bez zmiany): ");
            var noweNazwisko = Console.ReadLine();
            if (!string.IsNullOrEmpty(noweNazwisko))
                student.Profile.Nazwisko = noweNazwisko;  // Change Tracker: Profile = "Modified"

            Console.Write("Nowy rok studiów (Enter = bez zmiany): ");
            var nowyRokStr = Console.ReadLine();
            if (!string.IsNullOrEmpty(nowyRokStr) && int.TryParse(nowyRokStr, out var nowyRok) && nowyRok >= 1 && nowyRok <= 5)
                student.Profile.RokStudiow = nowyRok;     // Change Tracker: Profile = "Modified"

            // SaveChangesAsync() - EF wykrywa zmienione pola i generuje:
            // UPDATE Profile SET Imie = @imie, Nazwisko = @nazwisko, RokStudiow = @rok 
            // WHERE StudentId = @studentId
            // Aktualizuje TYLKO zmienione pola!
            await context.SaveChangesAsync();
            Console.WriteLine("Dane zaktualizowane!");
        }

        // ========== Many-to-Many - Zapisywanie na kurs ==========
        /// <summary>
        /// Zapisuje studenta na wybrany kurs (relacja Many-to-Many)
        /// EF automatycznie zarządza tabelą łączącą KursStudent
        /// </summary>
        static async Task ZapiszStudentaNaKurs(ApplicationDbContext context)
        {
            Console.Write("\nPodaj ID studenta: ");
            if (!int.TryParse(Console.ReadLine(), out var idStudenta))
            {
                Console.WriteLine("Nieprawidłowe ID!");
                return;
            }

            // Ładowanie studenta z relacjami - Multiple Include()
            var student = await context.Studenci
                .Include(s => s.Profile)               // JOIN Profile (1:1)
                .Include(s => s.Kursy)                 // JOIN przez tabelę łączącą KursStudent (M:N)
                .FirstOrDefaultAsync(s => s.IdStudenta == idStudenta);
            // EF wygeneruje złożone zapytanie:
            // SELECT * FROM Studenci s 
            // LEFT JOIN Profile p ON s.Id = p.StudentId 
            // LEFT JOIN KursStudent ks ON s.Id = ks.StudenciId 
            // LEFT JOIN Kursy k ON ks.KursyId = k.Id 
            // WHERE s.IdStudenta = @idStudenta

            if (student == null)
            {
                Console.WriteLine($"Nie znaleziono studenta o ID: {idStudenta}");
                return;
            }

            // Pobranie wszystkich dostępnych kursów
            var kursy = await context.Kursy.ToListAsync();    // SELECT * FROM Kursy
            Console.WriteLine("\nDostępne kursy:");
            foreach (var kurs in kursy)
            {
                // Any() - sprawdza czy student jest już zapisany na kurs
                // Działa na załadowanych danych (student.Kursy) - bez dodatkowego zapytania do bazy
                var zapisany = student.Kursy.Any(k => k.Id == kurs.Id) ? " [ZAPISANY]" : "";
                Console.WriteLine($"{kurs.Id}. {kurs.Nazwa}{zapisany}");
            }

            Console.Write("Wybierz ID kursu: ");
            if (!int.TryParse(Console.ReadLine(), out var idKursu))
            {
                Console.WriteLine("Nieprawidłowe ID kursu!");
                return;
            }

            // Znajdź wybrany kurs w załadowanej liście
            var wybranyKurs = kursy.FirstOrDefault(k => k.Id == idKursu);
            if (wybranyKurs == null)
            {
                Console.WriteLine("Nie znaleziono kursu!");
                return;
            }

            // Sprawdzenie duplikatów przed dodaniem
            if (student.Kursy.Any(k => k.Id == idKursu))
            {
                Console.WriteLine("Student już jest zapisany na ten kurs!");
                return;
            }

            // Dodanie kursu do kolekcji studenta - relacja Many-to-Many
            // EF automatycznie zarządza tabelą łączącą KursStudent
            student.Kursy.Add(wybranyKurs);

            // SaveChangesAsync() wygeneruje i wykona:
            // INSERT INTO KursStudent (StudenciId, KursyId) VALUES (@studentDbId, @kursId)
            await context.SaveChangesAsync();
            Console.WriteLine($"Student {student.Profile.Imie} {student.Profile.Nazwisko} zapisany na kurs {wybranyKurs.Nazwa}");
        }

        // ========== Zapytania z JOIN-ami - Studenci kursu ==========
        /// <summary>
        /// Wyświetla wszystkich studentów zapisanych na wybrany kurs
        /// Wykorzystuje ThenInclude dla zagnieżdżonych relacji
        /// </summary>
        static async Task WypiszStudentowKursu(ApplicationDbContext context)
        {
            // Proste zapytanie - pobierz wszystkie kursy
            var kursy = await context.Kursy.ToListAsync();    // SELECT * FROM Kursy
            Console.WriteLine("\nDostępne kursy:");
            foreach (var k in kursy)
            {
                Console.WriteLine($"{k.Id}. {k.Nazwa}");
            }

            Console.Write("Wybierz ID kursu: ");
            if (!int.TryParse(Console.ReadLine(), out var idKursu))
            {
                Console.WriteLine("Nieprawidłowe ID!");
                return;
            }

            // Złożone zapytanie z wielopoziomowym Include()
            var kurs = await context.Kursy
                .Include(k => k.Studenci)              // JOIN przez tabelę KursStudent (M:N)
                .ThenInclude(s => s.Profile)           // Następnie JOIN z Profile (1:1)
                .FirstOrDefaultAsync(k => k.Id == idKursu);
            // EF wygeneruje: 
            // SELECT * FROM Kursy k 
            // LEFT JOIN KursStudent ks ON k.Id = ks.KursyId 
            // LEFT JOIN Studenci s ON ks.StudenciId = s.Id 
            // LEFT JOIN Profile p ON s.Id = p.StudentId 
            // WHERE k.Id = @idKursu

            if (kurs == null)
            {
                Console.WriteLine("Nie znaleziono kursu!");
                return;
            }

            Console.WriteLine($"\nStudenci kursu {kurs.Nazwa}:");
            if (!kurs.Studenci.Any())
            {
                Console.WriteLine("Brak studentów na tym kursie.");
                return;
            }

            // OrderBy() na załadowanych danych - sortowanie w pamięci (nie w SQL)
            foreach (var student in kurs.Studenci.OrderBy(s => s.Profile.Nazwisko))
            {
                Console.WriteLine($"ID: {student.IdStudenta}, {student.Profile.Imie} {student.Profile.Nazwisko}, Rok: {student.Profile.RokStudiow}");
            }
        }

        // ========== Szczegółowe dane studenta ==========
        /// <summary>
        /// Wyświetla kompletne dane studenta wraz z listą jego kursów
        /// Pokazuje jak wykorzystywać załadowane relacje
        /// </summary>
        static async Task WypiszDaneStudenta(ApplicationDbContext context)
        {
            Console.Write("\nPodaj ID studenta: ");
            if (!int.TryParse(Console.ReadLine(), out var idStudenta))
            {
                Console.WriteLine("Nieprawidłowe ID!");
                return;
            }

            // Załaduj studenta z wszystkimi powiązanymi danymi
            var student = await context.Studenci
                .Include(s => s.Profile)               // Profil studenta (1:1)
                .Include(s => s.Kursy)                 // Lista kursów studenta (M:N)
                .FirstOrDefaultAsync(s => s.IdStudenta == idStudenta);

            if (student == null)
            {
                Console.WriteLine($"Nie znaleziono studenta o ID: {idStudenta}");
                return;
            }

            // Wyświetl dane z załadowanego profilu (relacja 1:1)
            Console.WriteLine($"\nDane studenta:");
            Console.WriteLine($"ID: {student.IdStudenta}");
            Console.WriteLine($"Imię i nazwisko: {student.Profile.Imie} {student.Profile.Nazwisko}");
            Console.WriteLine($"Data urodzenia: {student.Profile.DataUrodzenia:yyyy-MM-dd}");
            Console.WriteLine($"Rok studiów: {student.Profile.RokStudiow}");

            // Wyświetl kursy z załadowanej kolekcji (relacja M:N)
            Console.WriteLine($"\nKursy ({student.Kursy.Count}):");
            if (student.Kursy.Any())
            {
                // Sortowanie w pamięci - dane już załadowane z bazy
                foreach (var kurs in student.Kursy.OrderBy(k => k.Nazwa))
                {
                    Console.WriteLine($"• {kurs.Nazwa}");
                }
            }
            else
            {
                Console.WriteLine("Brak zapisów na kursy.");
            }
        }

        // ========== Zapytanie z filtrowaniem ==========
        /// <summary>
        /// Znajduje wszystkich studentów z określonego roku studiów
        /// Wykorzystuje Where() dla filtrowania danych
        /// </summary>
        static async Task WypiszStudentowRoku(ApplicationDbContext context)
        {
            Console.Write("\nPodaj rok studiów (1-5): ");
            if (!int.TryParse(Console.ReadLine(), out var rok) || rok < 1 || rok > 5)
            {
                Console.WriteLine("Rok musi być między 1-5!");
                return;
            }

            // LINQ zapytanie z WHERE i ORDER BY
            var studenci = await context.Studenci
                .Include(s => s.Profile)               // JOIN z Profile
                .Where(s => s.Profile.RokStudiow == rok)  // WHERE Profile.RokStudiow = @rok
                .OrderBy(s => s.Profile.Nazwisko)        // ORDER BY Profile.Nazwisko
                .ToListAsync();                           // Wykonanie zapytania
            // EF wygeneruje:
            // SELECT * FROM Studenci s 
            // JOIN Profile p ON s.Id = p.StudentId 
            // WHERE p.RokStudiow = @rok 
            // ORDER BY p.Nazwisko

            Console.WriteLine($"\nStudenci {rok} roku:");
            if (!studenci.Any())
            {
                Console.WriteLine($"Brak studentów na {rok} roku.");
                return;
            }

            // Wyświetl wyniki - dane już posortowane przez bazę danych
            foreach (var student in studenci)
            {
                Console.WriteLine($"ID: {student.IdStudenta}, {student.Profile.Imie} {student.Profile.Nazwisko}");
            }
        }

        // ========== Podstawowe zapytanie SELECT ==========
        /// <summary>
        /// Pobiera i wyświetla wszystkich studentów z bazy danych
        /// Podstawowe zapytanie z JOIN i sortowaniem
        /// </summary>
        static async Task PokazWszystkichStudentow(ApplicationDbContext context)
        {
            // Proste zapytanie z JOIN i sortowaniem
            var studenci = await context.Studenci
                .Include(s => s.Profile)               // Eager Loading - ładuj Profile razem ze Student
                .OrderBy(s => s.IdStudenta)            // Sortowanie po IdStudenta
                .ToListAsync();                        // Wykonanie zapytania i konwersja do List<Student>
            // EF wygeneruje:
            // SELECT * FROM Studenci s 
            // LEFT JOIN Profile p ON s.Id = p.StudentId 
            // ORDER BY s.IdStudenta

            Console.WriteLine("\nWszyscy studenci:");
            if (!studenci.Any())
            {
                Console.WriteLine("Brak studentów w bazie.");
                return;
            }

            // Iteracja przez wyniki - korzystanie z załadowanych relacji
            foreach (var student in studenci)
            {
                // Dostęp do danych profilu bez dodatkowych zapytań do bazy (dzięki Include)
                Console.WriteLine($"ID: {student.IdStudenta}, {student.Profile.Imie} {student.Profile.Nazwisko}, Rok: {student.Profile.RokStudiow}");
            }
        }
    }
}