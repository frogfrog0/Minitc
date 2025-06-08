using Microsoft.EntityFrameworkCore;
using EFcore_test.Data;
using EFcore_test.Models;

namespace EFcore_test
{
    class Program
    {
        static async Task Main(string[] args)
        {

            using var context = new ApplicationDbContext();

            // Tworzenie bazy danych z przykładowymi danymi
            await context.Database.EnsureCreatedAsync();
            Console.WriteLine("Baza danych gotowa\n");

            await ShowMenu(context);
        }

        static async Task ShowMenu(ApplicationDbContext context)
        {
            while (true)
            {
                Console.WriteLine("\n=== MENU ===");
                Console.WriteLine("1. Dodaj studenta");
                Console.WriteLine("2. Usuń studenta");
                Console.WriteLine("3. Aktualizuj dane studenta");
                Console.WriteLine("4. Zapisz studenta na kurs");
                Console.WriteLine("5. uczestnicy kursu");
                Console.WriteLine("6. dane studenta");
                Console.WriteLine("7. Studenci dane roku");
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
                        Console.WriteLine("Nieznana komenda");
                        break;
                }
            }
        }

        // 1. Dodaj studenta
        static async Task DodajStudenta(ApplicationDbContext context)
        {
            Console.WriteLine("\nDodawanie nowego studenta:");

            // Automatyczne wygenerowanie IdStudenta
            var maxId = await context.Studenci.AnyAsync()
                ? await context.Studenci.MaxAsync(s => s.IdStudenta)
                : 0;
            var nowyIdStudenta = maxId + 1;

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

            // Tworzenie studenta i profilu
            var student = new Student
            {
                IdStudenta = nowyIdStudenta,
                Profile = new StudentProfile
                {
                    Imie = imie,
                    Nazwisko = nazwisko,
                    DataUrodzenia = dataUr,
                    RokStudiow = rok
                }
            };

            context.Studenci.Add(student);
            await context.SaveChangesAsync();

            Console.WriteLine($"Student {imie} {nazwisko} dodany z ID: {nowyIdStudenta}");
        }

        // 2. Usuń studenta
        static async Task UsunStudenta(ApplicationDbContext context)
        {
            Console.Write("\nPodaj ID studenta do usunięcia: ");
            if (!int.TryParse(Console.ReadLine(), out var idStudenta))
            {
                Console.WriteLine("Nieprawidłowe ID!");
                return;
            }

            var student = await context.Studenci
                .Include(s => s.Profile)
                .FirstOrDefaultAsync(s => s.IdStudenta == idStudenta);

            if (student == null)
            {
                Console.WriteLine($"Nie znaleziono studenta o ID: {idStudenta}");
                return;
            }

            Console.Write($"Czy usunąć studenta {student.Profile.Imie} {student.Profile.Nazwisko}? (t/n): ");
            if (Console.ReadLine()?.ToLower() == "t")
            {
                context.Studenci.Remove(student);
                await context.SaveChangesAsync();
                Console.WriteLine("Student usunięty!");
            }
        }

        // 3. Aktualizuj dane studenta
        static async Task AktualizujStudenta(ApplicationDbContext context)
        {
            Console.Write("\nPodaj ID studenta: ");
            if (!int.TryParse(Console.ReadLine(), out var idStudenta))
            {
                Console.WriteLine("Nieprawidłowe ID!");
                return;
            }

            var student = await context.Studenci
                .Include(s => s.Profile)
                .FirstOrDefaultAsync(s => s.IdStudenta == idStudenta);

            if (student == null)
            {
                Console.WriteLine($"Nie znaleziono studenta o ID: {idStudenta}");
                return;
            }

            Console.WriteLine($"Aktualnie: {student.Profile.Imie} {student.Profile.Nazwisko}, Rok: {student.Profile.RokStudiow}");

            Console.Write("Nowe imię (Enter = bez zmiany): ");
            var noweImie = Console.ReadLine();
            if (!string.IsNullOrEmpty(noweImie))
                student.Profile.Imie = noweImie;

            Console.Write("Nowe nazwisko (Enter = bez zmiany): ");
            var noweNazwisko = Console.ReadLine();
            if (!string.IsNullOrEmpty(noweNazwisko))
                student.Profile.Nazwisko = noweNazwisko;

            Console.Write("Nowy rok studiów (Enter = bez zmiany): ");
            var nowyRokStr = Console.ReadLine();
            if (!string.IsNullOrEmpty(nowyRokStr) && int.TryParse(nowyRokStr, out var nowyRok) && nowyRok >= 1 && nowyRok <= 5)
                student.Profile.RokStudiow = nowyRok;

            await context.SaveChangesAsync();
            Console.WriteLine("Dane zaktualizowane!");
        }

        // 4. Zapisz studenta na kurs
        static async Task ZapiszStudentaNaKurs(ApplicationDbContext context)
        {
            Console.Write("\nPodaj ID studenta: ");
            if (!int.TryParse(Console.ReadLine(), out var idStudenta))
            {
                Console.WriteLine("Nieprawidłowe ID!");
                return;
            }

            var student = await context.Studenci
                .Include(s => s.Profile)
                .Include(s => s.Kursy)
                .FirstOrDefaultAsync(s => s.IdStudenta == idStudenta);

            if (student == null)
            {
                Console.WriteLine($"Nie znaleziono studenta o ID: {idStudenta}");
                return;
            }

            // Pokaż dostępne kursy
            var kursy = await context.Kursy.ToListAsync();
            Console.WriteLine("\nDostępne kursy:");
            foreach (var kurs in kursy)
            {
                var zapisany = student.Kursy.Any(k => k.Id == kurs.Id) ? " [ZAPISANY]" : "";
                Console.WriteLine($"{kurs.Id}. {kurs.Nazwa}{zapisany}");
            }

            Console.Write("Wybierz ID kursu: ");
            if (!int.TryParse(Console.ReadLine(), out var idKursu))
            {
                Console.WriteLine("Nieprawidłowe ID kursu!");
                return;
            }

            var wybranyKurs = kursy.FirstOrDefault(k => k.Id == idKursu);
            if (wybranyKurs == null)
            {
                Console.WriteLine("Nie znaleziono kursu!");
                return;
            }

            if (student.Kursy.Any(k => k.Id == idKursu))
            {
                Console.WriteLine("Student już jest zapisany na ten kurs!");
                return;
            }

            student.Kursy.Add(wybranyKurs);
            await context.SaveChangesAsync();
            Console.WriteLine($"Student {student.Profile.Imie} {student.Profile.Nazwisko} zapisany na kurs {wybranyKurs.Nazwa}");
        }

        // 5. Wypisz studentów kursu
        static async Task WypiszStudentowKursu(ApplicationDbContext context)
        {
            var kursy = await context.Kursy.ToListAsync();
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

            var kurs = await context.Kursy
                .Include(k => k.Studenci)
                .ThenInclude(s => s.Profile)
                .FirstOrDefaultAsync(k => k.Id == idKursu);

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

            foreach (var student in kurs.Studenci.OrderBy(s => s.Profile.Nazwisko))
            {
                Console.WriteLine($"ID: {student.IdStudenta}, {student.Profile.Imie} {student.Profile.Nazwisko}, Rok: {student.Profile.RokStudiow}");
            }
        }

        // 6. Wypisz dane studenta z kursami
        static async Task WypiszDaneStudenta(ApplicationDbContext context)
        {
            Console.Write("\nPodaj ID studenta: ");
            if (!int.TryParse(Console.ReadLine(), out var idStudenta))
            {
                Console.WriteLine("Nieprawidłowe ID!");
                return;
            }

            var student = await context.Studenci
                .Include(s => s.Profile)
                .Include(s => s.Kursy)
                .FirstOrDefaultAsync(s => s.IdStudenta == idStudenta);

            if (student == null)
            {
                Console.WriteLine($"Nie znaleziono studenta o ID: {idStudenta}");
                return;
            }

            Console.WriteLine($"\nDane studenta:");
            Console.WriteLine($"ID: {student.IdStudenta}");
            Console.WriteLine($"Imię i nazwisko: {student.Profile.Imie} {student.Profile.Nazwisko}");
            Console.WriteLine($"Data urodzenia: {student.Profile.DataUrodzenia:yyyy-MM-dd}");
            Console.WriteLine($"Rok studiów: {student.Profile.RokStudiow}");

            Console.WriteLine($"\nKursy ({student.Kursy.Count}):");
            if (student.Kursy.Any())
            {
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

        // 7. Wypisz studentów danego roku
        static async Task WypiszStudentowRoku(ApplicationDbContext context)
        {
            Console.Write("\nPodaj rok studiów (1-5): ");
            if (!int.TryParse(Console.ReadLine(), out var rok) || rok < 1 || rok > 5)
            {
                Console.WriteLine("Rok musi być między 1-5!");
                return;
            }

            var studenci = await context.Studenci
                .Include(s => s.Profile)
                .Where(s => s.Profile.RokStudiow == rok)
                .OrderBy(s => s.Profile.Nazwisko)
                .ToListAsync();

            Console.WriteLine($"\nStudenci {rok} roku:");
            if (!studenci.Any())
            {
                Console.WriteLine($"Brak studentów na {rok} roku.");
                return;
            }

            foreach (var student in studenci)
            {
                Console.WriteLine($"ID: {student.IdStudenta}, {student.Profile.Imie} {student.Profile.Nazwisko}");
            }
        }

        // 8. Pokaż wszystkich studentów
        static async Task PokazWszystkichStudentow(ApplicationDbContext context)
        {
            var studenci = await context.Studenci
                .Include(s => s.Profile)
                .OrderBy(s => s.IdStudenta)
                .ToListAsync();

            Console.WriteLine("\nWszyscy studenci:");
            if (!studenci.Any())
            {
                Console.WriteLine("Brak studentów w bazie.");
                return;
            }

            foreach (var student in studenci)
            {
                Console.WriteLine($"ID: {student.IdStudenta}, {student.Profile.Imie} {student.Profile.Nazwisko}, Rok: {student.Profile.RokStudiow}");
            }
        }
    }
}