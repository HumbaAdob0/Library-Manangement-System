using LibraryManagementSystem.Services;

namespace LibraryManagementSystem.ViewModels;

public class LoginViewModel : ObservableObject
{
    private readonly AuthenticationService _authenticationService;
    private readonly UserSession _session;
    private readonly AsyncRelayCommand _signInCommand;

    private string _username = string.Empty;
    private string _password = string.Empty;
    private string _errorMessage = string.Empty;
    private bool _isBusy;

    public LoginViewModel(AuthenticationService authenticationService, UserSession session)
    {
        _authenticationService = authenticationService;
        _session = session;
        _signInCommand = new AsyncRelayCommand(SignInAsync, CanSignIn);
    }

    public event Action? LoginSucceeded;

    public string Username
    {
        get => _username;
        set
        {
            if (SetProperty(ref _username, value))
            {
                ErrorMessage = string.Empty;
                _signInCommand.RaiseCanExecuteChanged();
            }
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            if (SetProperty(ref _password, value))
            {
                ErrorMessage = string.Empty;
                _signInCommand.RaiseCanExecuteChanged();
            }
        }
    }

    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            if (SetProperty(ref _isBusy, value))
            {
                _signInCommand.RaiseCanExecuteChanged();
            }
        }
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }

    public AsyncRelayCommand SignInCommand => _signInCommand;

    private bool CanSignIn()
    {
        return !IsBusy && !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
    }

    private async Task SignInAsync()
    {
        IsBusy = true;
        ErrorMessage = string.Empty;

        try
        {
            var user = await _authenticationService.SignInAsync(Username, Password);
            if (user == null)
            {
                ErrorMessage = "Invalid username or password.";
                return;
            }

            _session.CurrentUser = user;
            LoginSucceeded?.Invoke();
        }
        catch
        {
            ErrorMessage = "Unable to sign in right now. Please try again.";
        }
        finally
        {
            IsBusy = false;
        }
    }
}
