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
    /// Interaction logic for PostLoginPage.xaml
    /// </summary>
    public partial class PostLoginPage : Page
    {
        Core db = new Core();
        private IPostLoginPage[] _pages = new IPostLoginPage[3];
        private String[] _pageNames = new String[3];
        private Users user;
        private enum CurrentPage
        {
            SchedulePage = 0,
            TrainerListPage = 1,
            RequestsPage = 2,
        };
        private CurrentPage _currentPage;
        private CurrentPage currentPage
        {
            get { return _currentPage; }
            set
            {
                if (_currentPage == value)
                    return;
                _currentPage = value;
                SubFrame.Navigate(_pages[(int) value]);
                TitleText.Text = _pageNames[(int) value];
            }
        }
        public PostLoginPage(Users currentUser)
        {
            InitializeComponent();
            user = currentUser;
            Console.WriteLine(currentUser.RoleId);
            _pages[0] = new SchedulePage();
            _pages[1] = new TrainerListPage();
            _pages[2] = new RequestsPage(db, currentUser);
            _pageNames[0] = "Расписание";
            _pageNames[1] = "Мои тренеры";
            _pageNames[2] = "Отправить заявку";
            currentPage = CurrentPage.SchedulePage;
            GymTypeComboBox.ItemsSource = db.context.Specialization.ToList();
            GymTypeComboBox.DisplayMemberPath = "SpecializationName";
            GymTypeComboBox.SelectedValuePath = "Id";
        }

        private void ScheduleBtn_Click(object sender, RoutedEventArgs e)
        {
            currentPage = CurrentPage.SchedulePage;
        }

        private void TrainerListBtn_Click(object sender, RoutedEventArgs e)
        {
            currentPage = CurrentPage.TrainerListPage;
        }

        private void RequestsBtn_Click(object sender, RoutedEventArgs e)
        {
            currentPage = CurrentPage.RequestsPage;
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new StartPage());
        }

        private void SetCurrentGymForSubPages (int currentGymType)
        {
            foreach (var page in _pages)
            {
                page.currentGymType = currentGymType;
            }
        }

        private void GymTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetCurrentGymForSubPages((int) GymTypeComboBox.SelectedValue);
        }
    }
}
