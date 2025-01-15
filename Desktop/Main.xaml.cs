using Desktop.Repository;
using Desktop.utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace Desktop
{
    public partial class Main : Window, INotifyPropertyChanged
    {
        private string _username;

        public string UserName
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<TasksItem> Tasks { get; set; }
        public ObservableCollection<Category> Categories { get; set; }

        private string _selectedCategory;
        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged();
                UpdateFilteredTasks();
            }
        }

        private ObservableCollection<TasksItem> _filteredTasks;
        public ObservableCollection<TasksItem> FilteredTasks
        {
            get => _filteredTasks;
            set
            {
                _filteredTasks = value;
                OnPropertyChanged();
            }
        }

        public Main()
        {
            InitializeComponent();
            DataContext = this;

            // Получаем экземпляр Singleton через статическое свойство Instance
            var userRepository = UserRepository.Instance;

            // Проверяем текущего пользователя через userRepository.CurrentUser
            if (userRepository.CurrentUser != null)
            {
                UserName = userRepository.CurrentUser.Name;
            }

            Tasks = new ObservableCollection<TasksItem>
            {
                new TasksItem { Title = "Зарядка", Date = new DateTime(2024, 11, 11), Time = "8:00", IsCompleted = false, Description = "Утренняя зарядка для здоровья", Category = "Дом" },
                new TasksItem { Title = "Прогулка с собакой", Date = new DateTime(2024, 11, 11), Time = "09:00", IsCompleted = false, Description = "Прогулка с собакой на свежем воздухе.", Category = "Дом" },
                new TasksItem { Title = "Совещание", Date = new DateTime(2024, 11, 12), Time = "10:00", IsCompleted = false, Description = "Рабочее совещание в офисе.", Category = "Работа" },
                new TasksItem { Title = "Чтение лекций", Date = new DateTime(2024, 11, 13), Time = "14:00", IsCompleted = false, Description = "Просмотр материалов по учёбе.", Category = "Учёба" },
                new TasksItem { Title = "Поход в кафе", Date = new DateTime(2024, 11, 11), Time = "17:00", IsCompleted = false, Description = "Поход в кафе Милано с другом", Category = "Отдых" },
                new TasksItem { Title = "Бассейн", Date = new DateTime(2024, 11, 12), Time = "10:00", IsCompleted = false, Description = "Поход в бассейн.", Category = "Отдых" },
                new TasksItem { Title = "Написание дз", Date = new DateTime(2024, 11, 13), Time = "14:00", IsCompleted = false, Description = "Написать и придумать задание.", Category = "Учёба" }
            };

            Categories = new ObservableCollection<Category>
            {
                new Category { Name = "Все" },
                new Category { Name = "Дом" },
                new Category { Name = "Работа" },
                new Category { Name = "Учёба" },
                new Category { Name = "Отдых" }
            };

            SelectedCategory = null;
            UpdateFilteredTasks();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Task.SelectedItem is TasksItem selectedTask)
            {
                myName.Text = selectedTask.Title;
                Name_Zadacha.Text = $"{selectedTask.Date.ToShortDateString()} {selectedTask.Time}";
                Description.Text = selectedTask.Description;

                Delete.Visibility = Visibility.Visible;
                Done.Visibility = Visibility.Visible;

                TaskPanel.Visibility = Visibility.Visible;
            }
            else
            {
                TaskPanel.Visibility = Visibility.Collapsed;
                ClearTaskDetails();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (Task.SelectedItem is TasksItem task)
            {
                Tasks.Remove(task);
                UpdateFilteredTasks();

                TaskPanel.Visibility = Visibility.Collapsed;
                ClearTaskDetails();
            }
        }

        private void Done_Click(object sender, RoutedEventArgs e)
        {
            if (Task.SelectedItem is TasksItem selectedTask)
            {
                selectedTask.IsCompleted = true;
                MessageBox.Show($"Задача '{selectedTask.Title}' выполнена.");
                TaskPanel.Visibility = Visibility.Collapsed;
                ClearTaskDetails();
                var filteredTasks = FilteredTasks.ToList();
                FilteredTasks.Clear();
                foreach (var task in filteredTasks)
                {
                    FilteredTasks.Add(task);
                }
            }
        }

        private void ClearTaskDetails()
        {
            myName.Text = string.Empty;
            Name_Zadacha.Text = string.Empty;
            Description.Text = string.Empty;

            Delete.Visibility = Visibility.Collapsed;
            Done.Visibility = Visibility.Collapsed;
        }

        private void UpdateFilteredTasks()
        {
            if (!string.IsNullOrEmpty(SelectedCategory) && SelectedCategory != "Все")
            {
                FilteredTasks = new ObservableCollection<TasksItem>(
                    Tasks.Where(task => task.Category == SelectedCategory));
            }
            else
            {
                FilteredTasks = Tasks;
            }
            ClearTaskDetails();
        }

        private void CategorySelected(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBlock textBlock)
            {
                SelectedCategory = textBlock.Text;
                foreach (var category in Categories)
                {
                    category.IsSelected = category.Name == SelectedCategory;
                }
            }
        }

        public class Category : INotifyPropertyChanged
        {
            private bool _isSelected;

            public string Name { get; set; }
            public bool IsSelected
            {
                get => _isSelected;
                set
                {
                    _isSelected = value;
                    OnPropertyChanged();
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected void OnPropertyChanged([CallerMemberName] string name = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
