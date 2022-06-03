using FitnessCenterApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace FitnessCenterApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        Core db = DBmanagement.db;
        User user;
        Client newClient;
        Trainer newTrainer;
        BitmapSource photo;
        Photo photoTableField;
        bool isNewUser;

        private void CommonConstructor()
        {
            InitializeComponent();
            //Заполнение открывающегося списка "Пол"
            GenderComboBox.ItemsSource = db.context.Gender.ToList();
        }
        public RegistrationPage()
        {
            CommonConstructor();
            newClient = new Client();
            DataContext = newClient; // взаимосвязь с XAML
            user = null;
            isNewUser = true;
            BtnReg.Click += RegisterClient;
            photoTableField = null;
        }
        public RegistrationPage(Client client)
        {
            CommonConstructor();
            if (client == null)
            {
                newClient = new Client();
                user = null;
                isNewUser = true;
            }
            else
            {
                newClient = client;
                try
                {
                    user = newClient.User.First(x => x.RoleId == 1); // Клиент
                    PhoneNumber.Text = PhoneNumberManagement.NumberToFormattedString(user.PhoneNumber);
                }
                catch (InvalidOperationException)
                {
                    user = null;
                }
                BitmapSource _photo;
                (_photo, photoTableField) = DBmanagement.LoadPhoto(newClient.PhotoId);
                if (_photo != null)
                    UserPhoto.Source = _photo;
                BtnReg.Content = "Обновить информацию";
                isNewUser = false;
            }
            DataContext = newClient; // взаимосвязь с XAML
            BtnReg.Click += RegisterClient;
        }
        public RegistrationPage(Trainer trainer)
        {
            CommonConstructor();
            if (trainer == null)
            {
                newTrainer = new Trainer();
                user = null;
                isNewUser = true;
            }
            else
            {
                newTrainer = trainer;
                try
                {
                    user = newTrainer.User.First(x => x.RoleId == 2); // Тренер
                    PhoneNumber.Text = PhoneNumberManagement.NumberToFormattedString(user.PhoneNumber);
                }
                catch (InvalidOperationException)
                {
                    user = null;
                }
                BitmapSource _photo;
                (_photo, photoTableField) = DBmanagement.LoadPhoto(newTrainer.PhotoId);
                if (_photo != null)
                    UserPhoto.Source = _photo;
                BtnReg.Content = "Обновить информацию";
                isNewUser = false;
            }
            DataContext = newTrainer;
            SpecializationText.Visibility = Visibility.Visible;
            SpecializationComboBox.Visibility = Visibility.Visible;
            WorkLengthText.Visibility = Visibility.Visible;
            WorkLength.Visibility = Visibility.Visible;
            Date.Text = "Дата приёма на работу";
            DatePick.SetBinding(DatePicker.SelectedDateProperty, new Binding("EmploymentDate"));
            List<Specialization> specializations = db.context.Specialization.ToList();
            specializations.Sort(Comparer.CompareSpecialization);
            SpecializationComboBox.ItemsSource = specializations;
            BtnReg.Click += RegisterTrainer;
        }

        private bool onClick_Common()
        {
            string phoneNumberText = PhoneNumber.Text;
            long phoneNumber = PhoneNumberManagement.StringToNumber(phoneNumberText);
            if (!PhoneNumberManagement.IsNumberCorrect(phoneNumberText))
            {
                MessageBox.Show("Некорректный номер телефона", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (user == null && db.context.User.Count(x => x.PhoneNumber == phoneNumber) > 0)
            {
                MessageBox.Show("Такой пользователь уже существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (photo != null)
            {
                DBmanagement.StorePhoto(photo, photoTableField);
            }
            return true;
        }

        private void GeneratePhoneNumberAndPassword()
        {

            user.PhoneNumber = PhoneNumberManagement.StringToNumber(PhoneNumber.Text);
            byte[] dynamicSalt = new byte[16];
            using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetBytes(dynamicSalt);
            }
            user.Password = PasswordManagement.getPasswordHashWithSalt(Password.Password, dynamicSalt);
        }

        private void UpdatePhoneNumberAndPassword()
        {
            long phoneNumber = PhoneNumberManagement.StringToNumber(PhoneNumber.Text);
            if (user.PhoneNumber != phoneNumber)
                user.PhoneNumber = phoneNumber;
            if (!string.IsNullOrEmpty(Password.Password))
            {
                byte[] dynamicSalt = new byte[16];
                using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
                {
                    rngCsp.GetBytes(dynamicSalt);
                }
                user.Password = PasswordManagement.getPasswordHashWithSalt(Password.Password, dynamicSalt);
            }
        }

        private void RegisterClient(object sender, RoutedEventArgs e)
        {
            if (GenderComboBox.SelectedValue == null || newClient.Name == null || newClient.Surname == null || newClient.BirthDate == null ||
                PhoneNumber.Text == null || user == null && Password.Password == null)
            {
                MessageBox.Show("Заполните все поля, чтобы продолжить", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!onClick_Common())
                return;
            if (photo != null)
                newClient.PhotoId = photoTableField.Id;
            if (isNewUser)
            {
                db.context.Client.Add(newClient);
                if (db.context.SaveChanges() == 0)
                {
                    MessageBox.Show("Ошибка доступа к базе данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            if (user == null)
            {
                user = new User();
                user.UserId = newClient.Id;
                user.RoleId = 1; // Клиент
                GeneratePhoneNumberAndPassword();
                db.context.User.Add(user);
            }
            else
            {
                UpdatePhoneNumberAndPassword();
            }
            if (db.context.SaveChanges() == 0)
            {
                MessageBox.Show("Ошибка доступа к базе данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (isNewUser)
            {
                MessageBox.Show("Регистрация прошла успешно!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                user = null;
                newClient = new Client();
                DataContext = newClient;
                photoTableField = null;
            }
            else
            {
                MessageBox.Show("Данные обновлены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void RegisterTrainer(object sender, RoutedEventArgs e)
        {
            if (GenderComboBox.SelectedValue == null || SpecializationComboBox.SelectedValue == null ||
                newTrainer.Name == null || newTrainer.EmploymentDate == null ||
                user == null && (PhoneNumber.Text == null || Password.Password == null))
            {
                MessageBox.Show("Заполните все поля, чтобы продолжить", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!onClick_Common())
                return;
            if (photo != null)
                newTrainer.PhotoId = photoTableField.Id;
            if (isNewUser)
            {
                db.context.Trainer.Add(newTrainer);
                if (db.context.SaveChanges() == 0)
                {
                    MessageBox.Show("Ошибка доступа к базе данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            if (user == null)
            {
                user = new User();
                user.UserId = newTrainer.Id;
                user.RoleId = 2; // Тренер
                GeneratePhoneNumberAndPassword();
                db.context.User.Add(user);
            }
            else
            {
                UpdatePhoneNumberAndPassword();
            }
            if (db.context.SaveChanges() == 0)
            {
                MessageBox.Show("Ошибка доступа к базе данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (isNewUser)
            {
                MessageBox.Show("Регистрация прошла успешно!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                user = null;
                newTrainer = new Trainer();
                DataContext = newTrainer;
                photoTableField = null;
            }
            else
            {
                MessageBox.Show("Данные обновлены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private static readonly string[] ImageExtensions = new string[] {".jpg", ".jpeg", ".jpe", ".png", ".bmp", ".gif", ".tiff", ".ico"};
        private bool isImage(string path)
        {
            return ImageExtensions.Contains(System.IO.Path.GetExtension(path).ToLower());
        }

        private void Photo_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            string fileTypes = string.Join(";*", ImageExtensions).Insert(0, "*");
            openFileDialog.Filter = "Image files (" + fileTypes + ")|" + fileTypes + "|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == false)
                return;

            try
            {
                BitmapImage _photo = new BitmapImage();
                _photo.BeginInit();
                _photo.UriSource = new Uri(openFileDialog.FileName);
                _photo.DecodePixelHeight = 200;
                _photo.EndInit();
                if (_photo == null)
                    throw new NullReferenceException();

                photo = _photo;
            }
            catch (Exception)
            {
                MessageBox.Show("Фото не может быть загружено", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            UserPhoto.Source = photo;
        }

        // Функциональность не работает https://stackoverflow.com/questions/1422166/e-data-getdata-is-always-null
        /*
        private void Photo_Drop(object sender, DragEventArgs e)
        {
            
            ImageSource image = e.Data.GetData(typeof(ImageSource)) as ImageSource;
            if (image == false)
            {
                MessageBox.Show("Фото не может быть загружено", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            photo = (BitmapSource) image;
            UserPhoto.Source = photo;
        }
        */
    }
}
