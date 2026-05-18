using System.Collections.ObjectModel;
using System.Windows.Media;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;

namespace LibraryManagementSystem.ViewModels;

public class MainViewModel : ObservableObject
{
    private readonly UserSession _session;
    private object? _currentView;

    public MainViewModel(UserSession session)
    {
        _session = session;
        SignOutCommand = new RelayCommand(SignOut);
        BuildCards();
        
        // Load Overview as the default landing page
        LoadDefaultView();
    }

    public ObservableCollection<DashboardCardViewModel> Cards { get; } = new();

    public object? CurrentView
    {
        get => _currentView;
        set => SetProperty(ref _currentView, value);
    }

    public string DisplayName => _session.CurrentUser?.Username ?? "User";

    public string RoleLabel => _session.CurrentUser?.Role == UserRole.Admin ? "Administrator" : "Librarian";

    public string WelcomeMessage => $"Welcome, {DisplayName}!";

    public RelayCommand SignOutCommand { get; }

    public event Action? SignOutRequested;
    public event Action<string>? NavigationRequested;

    private void LoadDefaultView()
    {
        // Trigger navigation to Overview on startup
        NavigateTo("Overview");
    }

    private void BuildCards()
    {
        var role = _session.CurrentUser?.Role ?? UserRole.Librarian;
        var isAdmin = role == UserRole.Admin;

        Cards.Add(new DashboardCardViewModel(
            "Overview",
            "Dashboard and analytics",
            "📊",
            320,
            180,
            new SolidColorBrush(Color.FromRgb(207, 199, 182)),
            true,
            new RelayCommand(() => NavigateTo("Overview"))));

        Cards.Add(new DashboardCardViewModel(
            "Books",
            "Manage titles and inventory",
            "📚",
            360,
            200,
            new SolidColorBrush(Color.FromRgb(234, 217, 199)),
            true,
            new RelayCommand(() => NavigateTo("Books"))));

        Cards.Add(new DashboardCardViewModel(
            "Patrons",
            "View memberships and details",
            "👥",
            300,
            200,
            new SolidColorBrush(Color.FromRgb(216, 195, 178)),
            true,
            new RelayCommand(() => NavigateTo("Patrons"))));

        Cards.Add(new DashboardCardViewModel(
            "Transactions",
            "Checkouts, returns, and fines",
            "🔄",
            420,
            220,
            new SolidColorBrush(Color.FromRgb(226, 211, 195)),
            true,
            new RelayCommand(() => NavigateTo("Transactions"))));

        Cards.Add(new DashboardCardViewModel(
            "Users & Roles",
            "Admin access controls",
            "🔐",
            300,
            180,
            new SolidColorBrush(Color.FromRgb(212, 193, 176)),
            isAdmin,
            new RelayCommand(() => NavigateTo("Users"))));

        Cards.Add(new DashboardCardViewModel(
            "Settings",
            "System preferences",
            "⚙️",
            280,
            180,
            new SolidColorBrush(Color.FromRgb(230, 218, 206)),
            isAdmin,
            new RelayCommand(() => NavigateTo("Settings"))));
    }

    private void NavigateTo(string viewName)
    {
        NavigationRequested?.Invoke(viewName);
    }

    private void SignOut()
    {
        _session.CurrentUser = null;
        SignOutRequested?.Invoke();
    }
}
