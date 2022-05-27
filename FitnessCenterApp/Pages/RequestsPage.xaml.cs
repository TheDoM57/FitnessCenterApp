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
    /// Логика взаимодействия для TrainerListPage.xaml
    /// </summary>
    public partial class RequestsPage : Page, IPostLoginPage
    {
        Core db;
        List<Trainer> arrayTrainer;
        Users user;
        public int currentGymType {
            get { return _currentGymType; }
            set
            {
                if (_currentGymType == value)
                    return;
                _currentGymType = value;
                List<Trainer> trainersOfCurrentGym = arrayTrainer.FindAll(arrayTrainer => arrayTrainer.SpecializationId == value);
                TrainerListView.ItemsSource = trainersOfCurrentGym;
            }
        }
        private int _currentGymType;
        private void initTrainetListPage(Core database, Users currentUser)
        {
            InitializeComponent();
            user = currentUser;
            db = database;
            arrayTrainer = database.context.Trainer.ToList();
            foreach (var item in arrayTrainer)
            {
                Console.WriteLine(item.Name);
            }
            switch (user.RoleId)
            {
                case 2:
                {
                    BtnSendRequest.Content = "Отправить запрос";
                    break;
                }
                case 3:
                {
                    BtnSendRequest.Content = "Принять запрос";
                    break;
                }
            }
        }
        public RequestsPage(Users currentUser)
        {
            initTrainetListPage(new Core(), currentUser);
        }
        public RequestsPage(Core db, Users currentUser)
        {
            initTrainetListPage(db, currentUser);
        }

        private void BtnRejectRequest_Click(object sender, RoutedEventArgs e)
        {
            Trainer trainer = TrainerListView.SelectedItem as Trainer;
            if (trainer == null)
            {
                MessageBox.Show("Тренер не выбран", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBox.Show("Запрос уже отклонён или не был отправлен", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void BtnSendRequest_Click(object sender, RoutedEventArgs e)
        {
            Trainer trainer = TrainerListView.SelectedItem as Trainer;
            if (trainer == null)
            {
                MessageBox.Show("Тренер не выбран", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            // = db.context.Requests.Where(r => (r.ClientId == 0) && (r.TrainerId == trainer.Id));

            MessageBox.Show("Запрос уже отправлен", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
