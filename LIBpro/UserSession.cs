using LibraryManagement.Models.Entities;

namespace LibraryApp
{
    public static class UserSession
    {
        private static Uzytkownik? _currentUser;
        
        public static Uzytkownik? CurrentUser 
        { 
            get => _currentUser;
            private set => _currentUser = value;
        }
        
        public static bool IsLoggedIn => _currentUser != null;
        
        public static bool IsAdmin => _currentUser?.IsAdmin ?? false;
        
        public static int? UserId => _currentUser?.Id;
        
        public static string? UserFullName => 
            _currentUser != null ? $"{_currentUser.Imie} {_currentUser.Nazwisko}" : null;
        
        public static void Login(Uzytkownik user)
        {
            _currentUser = user;
        }
        
        public static void Logout()
        {
            _currentUser = null;
        }
        
        public static void UpdateCurrentUser(Uzytkownik updatedUser)
        {
            if (_currentUser?.Id == updatedUser.Id)
            {
                _currentUser = updatedUser;
            }
        }
    }
}