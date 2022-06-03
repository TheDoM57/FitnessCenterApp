using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FitnessCenterApp.Model;

namespace FitnessCenterApp.Pages
{
    /// <summary>
    /// Interaction logic for AdminConsole.xaml
    /// </summary>
    public partial class AdminConsole : Page
    {
        List<Trainer> trainers;
        ListCollectionView trainersView;
        Trainer selectedTrainer;
        int lastEditor;
        public AdminConsole()
        {
            InitializeComponent();
            trainers = DBmanagement.db.context.Trainer.ToList();
            trainersView = (ListCollectionView)CollectionViewSource.GetDefaultView(trainers);
            TrainerList.ItemsSource = trainersView;
        }

        void onNavigating(object sender, NavigatingCancelEventArgs e)
        {
            trainers = DBmanagement.db.context.Trainer.ToList();
            switch (lastEditor)
            {
                case 1:
                    trainersView = (ListCollectionView)CollectionViewSource.GetDefaultView(trainers);
                    TrainerList.ItemsSource = trainersView;
                    break;
                case 2:
                    trainersView.Refresh();
                    break;
            }
        }

        private void updateTrainers()
        {
            
            
        }

        private void TrainerList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedTrainer = (Trainer) TrainerList.SelectedItem;
        }

        private void CreateBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigating -= onNavigating;
            }
            catch (Exception)
            { }
            NavigationService.Navigate(new RegistrationPage((Trainer) null));
            NavigationService.Navigating += onNavigating;
            lastEditor = 1;
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (selectedTrainer == null)
            {
                MessageBox.Show("Выберите тренера для его изменения", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                NavigationService.Navigating -= onNavigating;
            }
            catch (Exception)
            {}
            NavigationService.Navigate(new RegistrationPage(selectedTrainer));
            NavigationService.Navigating += onNavigating;
            lastEditor = 2;
        }

        private void RemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (selectedTrainer == null)
            {
                MessageBox.Show("Выберите тренера для его изменения", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBoxResult answer = MessageBox.Show("Вы уверены, что хотите удалить тренера? (необратимая операция)", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (answer == MessageBoxResult.Yes)
            {
                DBmanagement.db.context.Trainer.Remove(selectedTrainer);
                if (DBmanagement.db.context.SaveChanges() == 0)
                {
                    MessageBox.Show("Ошибка доступа к базе данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                trainersView.Remove(selectedTrainer);
            }
        }
        private int _lastSortedColumn = 0;
        private class NoComparer : System.Collections.IComparer
        {
            public int sortOrder = 1;
            public int Compare(object _x, object _y)
            {
                Trainer x = (Trainer)_x;
                Trainer y = (Trainer)_y;
                return (x.Id - y.Id) * sortOrder;
            }
        }
        private void NoCM_Click(object sender, RoutedEventArgs e)
        {
            if (_lastSortedColumn == 1)
                ((NoComparer)trainersView.CustomSort).sortOrder *= -1;
            else
                trainersView.CustomSort = new NoComparer();
            trainersView.Refresh();
            _lastSortedColumn = 1;
        }

        private class NameComparer : System.Collections.IComparer
        {
            public int sortOrder = 1;
            public int Compare(object _x, object _y)
            {
                Trainer x = (Trainer)_x;
                Trainer y = (Trainer)_y;
                return x.Name.CompareTo(y.Name) * sortOrder;
            }
        }
        private void NameCM_Click(object sender, RoutedEventArgs e)
        {
            if (_lastSortedColumn == 2)
                ((NameComparer)trainersView.CustomSort).sortOrder *= -1;
            else
                trainersView.CustomSort = new NameComparer();
            trainersView.Refresh();
            _lastSortedColumn = 2;
        }

        private class SurnameComparer : System.Collections.IComparer
        {
            public int sortOrder = 1;
            public int Compare(object _x, object _y)
            {
                Trainer x = (Trainer)_x;
                Trainer y = (Trainer)_y;
                if (string.IsNullOrEmpty(x.Surname) || string.IsNullOrEmpty(y.Surname))
                    return ((string.IsNullOrEmpty(x.Surname) ? 1 : 0) - (string.IsNullOrEmpty(y.Surname) ? 1 : 0)) * sortOrder;
                return x.Surname.CompareTo(y.Surname) * sortOrder;
            }
        }
        private void SurnameCM_Click(object sender, RoutedEventArgs e)
        {
            if (_lastSortedColumn == 3)
                ((SurnameComparer)trainersView.CustomSort).sortOrder *= -1;
            else
                trainersView.CustomSort = new SurnameComparer();
            trainersView.Refresh();
            _lastSortedColumn = 3;
        }

        private class PatronymicComparer : System.Collections.IComparer
        {
            public int sortOrder = 1;
            public int Compare(object _x, object _y)
            {
                Trainer x = (Trainer)_x;
                Trainer y = (Trainer)_y;
                if (string.IsNullOrEmpty(x.Patronymic) || string.IsNullOrEmpty(y.Patronymic))
                    return ((string.IsNullOrEmpty(x.Patronymic) ? 1 : 0) - (string.IsNullOrEmpty(y.Patronymic) ? 1 : 0)) * sortOrder;
                return x.Patronymic.CompareTo(y.Patronymic) * sortOrder;
            }
        }
        private void PatronymicCM_Click(object sender, RoutedEventArgs e)
        {
            if (_lastSortedColumn == 4)
                ((PatronymicComparer)trainersView.CustomSort).sortOrder *= -1;
            else
                trainersView.CustomSort = new PatronymicComparer();
            trainersView.Refresh();
            _lastSortedColumn = 4;
        }

        private class SpecializationComparer : System.Collections.IComparer
        {
            public int sortOrder = 1;
            public int Compare(object _x, object _y)
            {
                Trainer x = (Trainer)_x;
                Trainer y = (Trainer)_y;
                return Comparer.CompareSpecialization(x.Specialization, y.Specialization) * sortOrder;
            }
        }
        private void SpecializationCM_Click(object sender, RoutedEventArgs e)
        {
            if (_lastSortedColumn == 5)
                ((SpecializationComparer)trainersView.CustomSort).sortOrder *= -1;
            else
                trainersView.CustomSort = new SpecializationComparer();
            trainersView.Refresh();
            _lastSortedColumn = 5;
        }

        private class WorkLengthComparer : System.Collections.IComparer
        {
            public int sortOrder = 1;
            public int Compare(object _x, object _y)
            {
                Trainer x = (Trainer)_x;
                Trainer y = (Trainer)_y;
                int length1 = (int)((DateTime.Now - x.EmploymentDate).Days / 365.2425) + x.PreviousWorkLength;
                int length2 = (int)((DateTime.Now - y.EmploymentDate).Days / 365.2425) + y.PreviousWorkLength;
                return (length1 - length2) * sortOrder;
            }
        }

        private void WorkLengthCM_Click(object sender, RoutedEventArgs e)
        {
            if (_lastSortedColumn == 6)
                ((WorkLengthComparer)trainersView.CustomSort).sortOrder *= -1;
            else
                trainersView.CustomSort = new WorkLengthComparer();
            trainersView.Refresh();
            _lastSortedColumn = 6;
        }

        private class EmploymentDateComparer : System.Collections.IComparer
        {
            public int sortOrder = 1;
            public int Compare(object _x, object _y)
            {
                Trainer x = (Trainer)_x;
                Trainer y = (Trainer)_y;
                return x.EmploymentDate.CompareTo(y.EmploymentDate) * sortOrder;
            }
        }

        private void EmploymentDateCM_Click(object sender, RoutedEventArgs e)
        {
            if (_lastSortedColumn == 7)
                ((EmploymentDateComparer)trainersView.CustomSort).sortOrder *= -1;
            else
                trainersView.CustomSort = new EmploymentDateComparer();
            trainersView.Refresh();
            _lastSortedColumn = 7;
        }
    }
}
