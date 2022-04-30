using FitnessCenterApp.Model;
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

namespace FitnessCenterApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для TrainerListPage.xaml
    /// </summary>
    public partial class TrainerListPage : Page
    {
        Core db = new Core();
        List<Trainer> arrayTrainer;
        public TrainerListPage()
        {
            InitializeComponent();

            arrayTrainer = db.context.Trainer.ToList();
            foreach (var item in arrayTrainer)
            {
                Console.WriteLine(item.Name);
            }
            TrainerListView.ItemsSource = arrayTrainer;

            TrainerGymTypeComboBox.ItemsSource = db.context.Specialization.ToList();
            TrainerGymTypeComboBox.DisplayMemberPath = "Specialization";           
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new StartPage());
        }

        private void TrainerListBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new TrainerListPage());
        }

        private void ScheduleBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SchedulePage());
        }

        private void RequestsBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RequestsPage());
        }

        private void TrainerGymTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TrainerGymTypeComboBox.SelectedValuePath = "Id";
            int specialization = Convert.ToInt32(TrainerGymTypeComboBox.SelectedValue);
            arrayTrainer= arrayTrainer.Where(x => x.SpecializationId == specialization).ToList();
            TrainerListView.ItemsSource = arrayTrainer;
        }
    }
}
