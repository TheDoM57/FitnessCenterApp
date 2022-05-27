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
            if (LoginTextBox.Text == null || !PhoneNumberManagement.IsNumberCorrect(LoginTextBox.Text))
            {
                MessageBox.Show("Некорректный номер телефона", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (PasswordTextBox.Password == null)
            {
                MessageBox.Show("Введите пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            long phoneNumber = PhoneNumberManagement.StringToNumber(LoginTextBox.Text);
            Users activeUser;
            try
            {
                 activeUser = db.context.Users.First(x => x.PhoneNumber == phoneNumber);
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("Такой пользователь отсутствует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            byte[] salt = new byte[16];
            Array.Copy(activeUser.Password, 24, salt, 0, 16);
            byte[] hash = PasswordManagement.getPasswordHashWithSalt(PasswordTextBox.Password, salt);

            // Для избежания перебора по времени отклика
            int differentBytesCount = 0;
            for (int i = 0; i < 40; ++i)
            {
                if (hash[i] != activeUser.Password[i])
                    ++differentBytesCount;
            }
            if (differentBytesCount > 0)
            {
                MessageBox.Show("Неверный пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            switch (activeUser.RoleId)
            {
                case 1:
                    this.NavigationService.Navigate(new SchedulePageAdmin());
                    break;
                case 2:
                case 3:
                    this.NavigationService.Navigate(new PostLoginPage(activeUser));
                    break;
            }
        }
    }
}
