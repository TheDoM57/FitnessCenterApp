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
    public partial class RequestsPage : Page
    {
        Core db;
        User user;
        ListCollectionView trainersList;
        Trainer trainerRequestSentTo;
        private void initTrainerListPage(Core database, User currentUser)
        {
            InitializeComponent();
            user = currentUser;
            db = database;
            trainersList = (ListCollectionView)CollectionViewSource.GetDefaultView(database.context.Trainer.ToList());
            TrainersList.ItemsSource = trainersList;
            setRequestedTrainer(user.Client.Requests);
        }
        public RequestsPage(User currentUser)
        {
            initTrainerListPage(DBmanagement.db, currentUser);
        }

        private void BtnRejectRequest_Click(object sender, RoutedEventArgs e)
        {
            Requests req = user.Client.Requests;
            if (req == null)
            {
                MessageBox.Show("Запрос уже отклонён или не был отправлен", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBoxResult answer = MessageBox.Show("Вы уверены, что хотите отклонить заявку?", "Вы уверены?", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (answer == MessageBoxResult.No)
                return;
            user.Client.RequestId = null;
            db.context.Requests.Remove(req);
            if (db.context.SaveChanges() == 0)
            {
                MessageBox.Show("Ошибка доступа к базе данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            trainersList.AddNewItem(trainerRequestSentTo);
            trainerRequestSentTo = null;
            ChoosenTrainer.Visibility = Visibility.Collapsed;
            ReqBtn.Click -= BtnRejectRequest_Click;
            ReqBtn.Click += BtnSendRequest_Click;
            ReqBtn.Content = "Отправить заявку";
        }

        private void setRequestedTrainer(Requests req)
        {
            if (req == null)
                return;
            trainerRequestSentTo = req.Trainer;
            trainersList.Remove(trainerRequestSentTo);
            ChoosenTrainer.DataContext = trainerRequestSentTo;
            RequestState.Text = db.context.RequestStatus.Find(req.StatusId).Name;
            if (req.StatusId == 1)
            {
                ReasonTitle.Text = "Причина отказа";
            }
            ReasonText.Text = req.Reason;
            ChoosenTrainer.Visibility = Visibility.Visible;
            ReqBtn.Click -= BtnSendRequest_Click;
            ReqBtn.Click += BtnRejectRequest_Click;
            ReqBtn.Content = "Отклонить заявку";
        }

        private void BtnSendRequest_Click(object sender, RoutedEventArgs e)
        {
            Trainer trainer = TrainersList.SelectedItem as Trainer;
            if (trainer == null)
            {
                MessageBox.Show("Тренер не выбран", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (db.context.Requests.Count(x => x.ClientId == user.UserId && x.TrainerId == trainer.Id) > 0)
                MessageBox.Show("Запрос уже отправлен", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

            string userInput;
            using (InputWindow input = new InputWindow("Введите цель(и), которую(ых) вы хотите добиться тренировками:", "Введите цель"))
            {
                if (input.ShowDialog() == false)
                    return;
                userInput = input.input;
            }
            Requests req = new Requests();
            req.TrainerId = trainer.Id;
            req.ClientId = (int) user.UserId;
            req.StatusId = 0;
            req.Reason = userInput;
            db.context.Requests.Add(req);
            if (db.context.SaveChanges() == 0)
            {
                MessageBox.Show("Ошибка доступа к базе данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            user.Client.RequestId = req.Id;
            if (db.context.SaveChanges() == 0)
            {
                MessageBox.Show("Ошибка доступа к базе данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            setRequestedTrainer(req);
        }
    }
}
