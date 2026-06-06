using DadaGrid.Models;
using System;
using System.Linq;
using System.Windows;

namespace DadaGrid
{
    public partial class BookEditWindow : Window
    {
        private Books _editingBook;
        private bool _isEditMode;

        public BookEditWindow(Books book = null)
        {
            InitializeComponent();
            
            if (book != null)
            {
                _editingBook = book;
                _isEditMode = true;
                Title = "Редактирование книги";
                LoadBookData();
            }
            else
            {
                _isEditMode = false;
                Title = "Добавление новой книги";
            }
            
            LoadGenres();
        }

        private void LoadGenres()
        {
            var context = ExamEntitiesEntities.GetContext();
            var genres = context.Genres.ToList();
            GenreComboBox.ItemsSource = genres;
            GenreComboBox.DisplayMemberPath = "Name";
            GenreComboBox.SelectedValuePath = "Id";
        }

        private void LoadBookData()
        {
            if (_editingBook != null)
            {
                TitleTextBox.Text = _editingBook.Title;
                AuthorTextBox.Text = _editingBook.Author;
                YearTextBox.Text = _editingBook.YearOfPublication.ToString();
                PriceTextBox.Text = _editingBook.Price.ToString("F2");
                GenreComboBox.SelectedValue = _editingBook.GenreId;
            }
        }

        public Books GetBook()
        {
            if (_editingBook == null)
                _editingBook = new Books();
            
            _editingBook.Title = TitleTextBox.Text.Trim();
            _editingBook.Author = AuthorTextBox.Text.Trim();
            
            if (int.TryParse(YearTextBox.Text, out int year))
                _editingBook.YearOfPublication = year;
            
            if (decimal.TryParse(PriceTextBox.Text, out decimal price))
                _editingBook.Price = price;
            
            if (GenreComboBox.SelectedValue != null)
                _editingBook.GenreId = (int)GenreComboBox.SelectedValue;
            
            return _editingBook;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput())
                return;
            
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(TitleTextBox.Text))
            {
                MessageBox.Show("Введите название книги.", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                TitleTextBox.Focus();
                return false;
            }
            
            if (string.IsNullOrWhiteSpace(AuthorTextBox.Text))
            {
                MessageBox.Show("Введите автора книги.", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                AuthorTextBox.Focus();
                return false;
            }
            
            if (!int.TryParse(YearTextBox.Text, out int year) || year < 0 || year > DateTime.Now.Year)
            {
                MessageBox.Show($"Введите корректный год публикации (от 0 до {DateTime.Now.Year}).", 
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                YearTextBox.Focus();
                return false;
            }
            
            if (!decimal.TryParse(PriceTextBox.Text, out decimal price) || price < 0)
            {
                MessageBox.Show("Введите корректную цену (положительное число).", 
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                PriceTextBox.Focus();
                return false;
            }
            
            if (GenreComboBox.SelectedValue == null)
            {
                MessageBox.Show("Выберите жанр книги.", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                GenreComboBox.Focus();
                return false;
            }
            
            return true;
        }
    }
}
