using System.Windows.Controls;
using LibraryManagementSystem.ViewModels;

namespace LibraryManagementSystem.Views
{
    public partial class ReportsView : UserControl
    {
        public ReportsView()
        {
            InitializeComponent();
            DataContext = App.GetService<ReportsViewModel>();
        }
    }
}
