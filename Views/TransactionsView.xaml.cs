using System.Windows.Controls;
using LibraryManagementSystem.ViewModels;

namespace LibraryManagementSystem.Views
{
    public partial class TransactionsView : UserControl
    {
        public TransactionsView()
        {
            InitializeComponent();
            DataContext = App.GetService<TransactionViewModel>();
        }
    }
}
