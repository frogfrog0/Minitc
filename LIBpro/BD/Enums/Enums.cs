namespace LibraryManagement.Models.Enums
{
    public enum RodzajKsiazki
    {
        Książka,
        Komiks,
        Album
    }

    public enum RodzajOkladki
    {
        Twarda,
        Miękka
    }

    public enum Uszkodzenia
    {
        Brak,
        Lekkie,
        Znaczące,
        Do_naprawy
    }

    public enum StatusKsiazki
    {
        Dostępna,
        Wypożyczona,
        Inne
    }
}