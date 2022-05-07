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
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        Core db = new Core();
        public LoginPage()
        {
            InitializeComponent();
        }

        private void BtnReturnMain_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new StartPage());
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            int result = db.context.Users.Where(x => x.UserName == LoginTextBox.Text && x.UserPassword == PasswordTextBox.Password).Count();

            if (result == 0)
            {
                MessageBox.Show("Такой пользователь отсутствует", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                Users activeUser = db.context.Users.Where(x => x.UserName == LoginTextBox.Text && x.UserPassword == PasswordTextBox.Password).FirstOrDefault();
                switch (activeUser.RoleId)
                {
                    case 1:
                        this.NavigationService.Navigate(new SchedulePageAdmin());
                        break;
                    case 2:
                        this.NavigationService.Navigate(new PostLoginPage());
                        break;
                }
            }
        }
    }
}
