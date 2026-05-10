using LibraryManagementSystem.Services;

namespace LibraryManagementSystem.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IAuthenticationService _authService;
        private string _currentUserName = string.Empty;
        private string _currentUserRole = string.Empty;

        public MainViewModel(IAuthenticationService authService)
        {
            _authService = authService;
            LoadCurrentUser();
        }

        public string CurrentUserName
        {
            get => _currentUserName;
            set => SetProperty(ref _currentUserName, value);
        }

        public string CurrentUserRole
        {
            get => _currentUserRole;
            set => SetProperty(ref _currentUserRole, value);
        }

        private void LoadCurrentUser()
        {
            if (_authService.CurrentUser != null)
            {
                CurrentUserName = _authService.CurrentUser.FullName;
                CurrentUserRole = _authService.CurrentUser.Role;
            }
        }
    }
}
