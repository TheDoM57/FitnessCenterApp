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
    /// Логика взаимодействия для SchedulePage.xaml
    /// </summary>
    public partial class SchedulePage : Page
    {
        public SchedulePage()
        {
            InitializeComponent();
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new StartPage());
        }

        private void ScheduleBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SchedulePage());
        }

        private void TrainerListBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new TrainerListPage());
        }

        private void RequestsBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RequestsPage());
        }
    }
}
