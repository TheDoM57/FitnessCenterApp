using System;
using System.Collections.Generic;
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
    /// Interaction logic for TrainerPage.xaml
    /// </summary>
    public partial class TrainerPage : Page
    {
        List<Client> clients;
        ListCollectionView clientsView;
        Trainer currentTrainer;
        Client selectedClient;
        public TrainerPage(Trainer trainer)
        {
            InitializeComponent();
            clients = DBmanagement.db.context.Client.Where(x => x.Requests.TrainerId == trainer.Id && x.Requests.StatusId != 1).ToList();
            clientsView = (ListCollectionView)CollectionViewSource.GetDefaultView(clients);
            ClientList.ItemsSource = clientsView;
            currentTrainer = trainer;
        }

        void onNavigating(object sender, NavigatingCancelEventArgs e)
        {
            clients = DBmanagement.db.context.Client.Where(x => x.Requests.TrainerId == currentTrainer.Id && x.Requests.StatusId != 1).ToList();
            clientsView.Refresh();
        }

        private int _lastSortedColumn = 0;

        private class NameComparer : System.Collections.IComparer
        {
            public int sortOrder = 1;
            public int Compare(object _x, object _y)
            {
                Client x = (Client)_x;
                Client y = (Client)_y;
                return x.Name.CompareTo(y.Name) * sortOrder;
            }
        }
        private void NameCM_Click(object sender, RoutedEventArgs e)
        {
            if (_lastSortedColumn == 2)
                ((NameComparer)clientsView.CustomSort).sortOrder *= -1;
            else
                clientsView.CustomSort = new NameComparer();
            clientsView.Refresh();
            _lastSortedColumn = 2;
        }

        private class SurnameComparer : System.Collections.IComparer
        {
            public int sortOrder = 1;
            public int Compare(object _x, object _y)
            {
                Client x = (Client)_x;
                Client y = (Client)_y;
                if (string.IsNullOrEmpty(x.Surname) || string.IsNullOrEmpty(y.Surname))
                    return ((string.IsNullOrEmpty(x.Surname) ? 1 : 0) - (string.IsNullOrEmpty(y.Surname) ? 1 : 0)) * sortOrder;
                return x.Surname.CompareTo(y.Surname) * sortOrder;
            }
        }
        private void SurnameCM_Click(object sender, RoutedEventArgs e)
        {
            if (_lastSortedColumn == 3)
                ((SurnameComparer)clientsView.CustomSort).sortOrder *= -1;
            else
                clientsView.CustomSort = new SurnameComparer();
            clientsView.Refresh();
            _lastSortedColumn = 3;
        }

        private class PatronymicComparer : System.Collections.IComparer
        {
            public int sortOrder = 1;
            public int Compare(object _x, object _y)
            {
                Client x = (Client)_x;
                Client y = (Client)_y;
                if (string.IsNullOrEmpty(x.Patronymic) || string.IsNullOrEmpty(y.Patronymic))
                    return ((string.IsNullOrEmpty(x.Patronymic) ? 1 : 0) - (string.IsNullOrEmpty(y.Patronymic) ? 1 : 0)) * sortOrder;
                return x.Patronymic.CompareTo(y.Patronymic) * sortOrder;
            }
        }
        private void PatronymicCM_Click(object sender, RoutedEventArgs e)
        {
            if (_lastSortedColumn == 4)
                ((PatronymicComparer)clientsView.CustomSort).sortOrder *= -1;
            else
                clientsView.CustomSort = new PatronymicComparer();
            clientsView.Refresh();
            _lastSortedColumn = 4;
        }

        private class BirthDateComparer : System.Collections.IComparer
        {
            public int sortOrder = 1;
            public int Compare(object _x, object _y)
            {
                Client x = (Client)_x;
                Client y = (Client)_y;
                return x.BirthDate.CompareTo(y.BirthDate) * sortOrder;
            }
        }

        private void BirthDateCM_Click(object sender, RoutedEventArgs e)
        {
            if (_lastSortedColumn == 5)
                ((BirthDateComparer)clientsView.CustomSort).sortOrder *= -1;
            else
                clientsView.CustomSort = new BirthDateComparer();
            clientsView.Refresh();
            _lastSortedColumn = 5;
        }

        private void AcceptBtn_Click(object sender, RoutedEventArgs e)
        {
            if (selectedClient == null)
            {
                MessageBox.Show("Клиент не выбран", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            NavigationService.Navigating -= onNavigating;

            NavigationService.Navigate(new AddExercise(currentTrainer, selectedClient));

            NavigationService.Navigating += onNavigating;
        }

        private void RejectBtn_Click(object sender, RoutedEventArgs e)
        {
            if (selectedClient == null)
            {
                MessageBox.Show("Клиент не выбран", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string userInput;
            using (InputWindow input = new InputWindow("Введите причину отказа", "Причина отказа"))
            {
                if (input.ShowDialog() == false)
                    return;
                userInput = input.input;
            }
            Requests req = selectedClient.Requests;
            req.StatusId = 1;
            req.Reason = userInput;
            clientsView.Remove(selectedClient);
            selectedClient = null;
            if (DBmanagement.db.context.SaveChanges() == 0)
                MessageBox.Show("Ошибка доступа к базе данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ClientList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView listView = (ListView)sender;
            selectedClient = (Client)listView.SelectedValue;
            if (selectedClient != null && selectedClient.Requests.StatusId == 2)
            {
                AcceptBtn.Content = "Изменить расписание ИЗ";
                RejectBtn.Style = FindResource("AuxularyDisabledButton") as Style;
                RejectBtn.IsHitTestVisible = false;
            }
            else
            {
                AcceptBtn.Content = "Принять запрос";
                RejectBtn.Style = FindResource("AuxularyButton") as Style;
                RejectBtn.IsHitTestVisible = true;
            }
        }
    }

}
