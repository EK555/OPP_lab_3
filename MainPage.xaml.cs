using FinanceTrackerApp.Models;
using FinanceTrackerApp.Services;

namespace FinanceTrackerApp;

public partial class MainPage : ContentPage
{
    private readonly DatabaseService? _databaseService;

    // Конструктор  для правильной работы XAML
    public MainPage()
    {
        InitializeComponent();
        // Получаем сервис через DependencyService 
        _databaseService = new DatabaseService();
        LoadTransactions();
        UpdateBalance();
    }

    // Конструктор для передачи сервиса явно
    public MainPage(DatabaseService databaseService)
    {
        InitializeComponent();
        _databaseService = databaseService;
        LoadTransactions();
        UpdateBalance();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (_databaseService != null)
        {
            LoadTransactions();
            UpdateBalance();
        }
    }

    private void LoadTransactions()
    {
        if (_databaseService == null) return;
        var transactions = _databaseService.GetTransactions();
        TransactionsCollectionView.ItemsSource = transactions;
    }

    private void UpdateBalance()
    {
        if (_databaseService == null) return;
        var balance = _databaseService.GetBalance();
        BalanceLabel.Text = $"{balance:F2} ₽";

        if (balance < 0)
        {
            BalanceLabel.TextColor = Colors.Red;
        }
        else
        {
            BalanceLabel.TextColor = Colors.White;
        }
    }

    private async void OnAddClicked(object sender, EventArgs e)
    {
        if (_databaseService == null) return;
        await Navigation.PushAsync(new TransactionPage(_databaseService, null));
    }

    private async void OnTransactionTapped(object sender, TappedEventArgs e)
    {
        if (_databaseService == null) return;
        if (e.Parameter is Transaction tappedTransaction)
        {
            await Navigation.PushAsync(new TransactionPage(_databaseService, tappedTransaction));
        }
    }

    // Кнопка удаления
    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Transaction transactionToDelete)
        {
            bool answer = await DisplayAlert("Подтверждение", $"Удалить операцию \"{transactionToDelete.Category}\"?", "Да", "Нет");
            if (answer)
            {
                _databaseService?.DeleteTransaction(transactionToDelete.Id);
                LoadTransactions();
                UpdateBalance();
            }
        }
    }
}