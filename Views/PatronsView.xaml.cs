using System.Windows.Controls;
using LibraryManagementSystem.ViewModels;

namespace LibraryManagementSystem.Views
{
    public partial class PatronsView : UserControl
    {
        public PatronsView()
        {
            InitializeComponent();
            // Use the ViewModel resolved from the app's DI container so it shares the same service instances
            DataContext = App.GetService<PatronManagementViewModel>();
        }
    }
}
