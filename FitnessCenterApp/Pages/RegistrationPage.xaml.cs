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
using System.Security.Cryptography;

namespace FitnessCenterApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        Core db = new Core();
        Client newPerson;
        public RegistrationPage()
        {
            InitializeComponent();
            //Заполнение открывающегося списка "Пол"
            GenderComboBox.ItemsSource = db.context.Gender.ToList();
            GenderComboBox.DisplayMemberPath = "Name";
            GenderComboBox.SelectedValuePath = "IdGender";
            newPerson = new Client();
            this.DataContext = newPerson; //взаимосвязь с XAML
        }

        private void BtnReg_Click(object sender, RoutedEventArgs e)
        {
            string phoneNumberText = PhoneNumber.Text;
            string password = Password.Password;
            if (GenderComboBox.SelectedValue == null || newPerson.Name == null || newPerson.Surname == null || newPerson.BirthDate == null ||
                phoneNumberText == null || password == null)
            {
                MessageBox.Show("Заполните все поля для регистрации", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!PhoneNumberManagement.IsNumberCorrect(phoneNumberText))
            {
                MessageBox.Show("Некорректный номер телефона", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            long phoneNumber = PhoneNumberManagement.StringToNumber(phoneNumberText);
            if (db.context.Users.Count(x => x.PhoneNumber == phoneNumber) > 0)
            {
                MessageBox.Show("Такой пользователь уже существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            byte[] dynamicSalt = new byte[16];
            using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetBytes(dynamicSalt);
            }
            Users user = new Users();
            user.PhoneNumber = phoneNumber;
            user.Password = PasswordManagement.getPasswordHashWithSalt(password, dynamicSalt);
            user.RoleId = 2; // Клиент
            db.context.Client.Add(newPerson);
            if (db.context.SaveChanges() == 0)
            {
                MessageBox.Show("Ошибка доступа к базе данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            user.UserId = newPerson.Id;
            db.context.Users.Add(user);
            if (db.context.SaveChanges() == 0)
            {
                MessageBox.Show("Ошибка доступа к базе данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBox.Show("Вы зарегестрировались!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigationService.Navigate(new LoginPage());
        }

        private void BtnReturnMain_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new StartPage());
        }
    }
}
