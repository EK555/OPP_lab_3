using FinanceTrackerApp.Models;
using FinanceTrackerApp.Services;

namespace FinanceTrackerApp;

public partial class TransactionPage : ContentPage
{
    private readonly DatabaseService _databaseService;
    private Transaction? _editingTransaction;

    public TransactionPage(DatabaseService databaseService, Transaction? transaction = null)
    {
        InitializeComponent();
        _databaseService = databaseService;
        _editingTransaction = transaction;

        // Если редактируем существующую операцию
        if (_editingTransaction != null)
        {
            // Заполняем поля данными
            if (_editingTransaction.Type == "Income")
                TypePicker.SelectedIndex = 0;
            else
                TypePicker.SelectedIndex = 1;

            AmountEntry.Text = _editingTransaction.Amount.ToString();
            CategoryEntry.Text = _editingTransaction.Category;
            DatePicker.Date = _editingTransaction.Date;
            NoteEntry.Text = _editingTransaction.Note ?? string.Empty;

            Title = "Редактирование операции";
        }
        else
        {
            // Новая операция — ставим сегодняшнюю дату
            DatePicker.Date = DateTime.Now;
            Title = "Новая операция";
        }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        // Проверяем, что сумма введена
        if (string.IsNullOrWhiteSpace(AmountEntry.Text))
        {
            await DisplayAlert("Ошибка", "Введите сумму", "OK");
            return;
        }

        // Проверяем, что категория введена
        if (string.IsNullOrWhiteSpace(CategoryEntry.Text))
        {
            await DisplayAlert("Ошибка", "Введите категорию", "OK");
            return;
        }

        // Проверяем, что выбран тип операции
        if (TypePicker.SelectedIndex == -1)
        {
            await DisplayAlert("Ошибка", "Выберите тип операции", "OK");
            return;
        }

        // Получаем введённую сумму
        if (!decimal.TryParse(AmountEntry.Text, out decimal amount))
        {
            await DisplayAlert("Ошибка", "Введите корректную сумму", "OK");
            return;
        }

        // Определяем тип операции
        string type = TypePicker.SelectedIndex == 0 ? "Income" : "Expense";

        if (_editingTransaction != null)
        {
            // Редактируем существующую операцию
            _editingTransaction.Amount = amount;
            _editingTransaction.Category = CategoryEntry.Text;
            _editingTransaction.Type = type;
            _editingTransaction.Date = DatePicker.Date;
            _editingTransaction.Note = NoteEntry.Text;

            _databaseService.UpdateTransaction(_editingTransaction);
        }
        else
        {
            // Создаём новую операцию
            var newTransaction = new Transaction
            {
                Amount = amount,
                Category = CategoryEntry.Text,
                Type = type,
                Date = DatePicker.Date,
                Note = NoteEntry.Text
            };

            _databaseService.AddTransaction(newTransaction);
        }

        // Возвращаемся на главный экран
        await Navigation.PopAsync();
    }
}