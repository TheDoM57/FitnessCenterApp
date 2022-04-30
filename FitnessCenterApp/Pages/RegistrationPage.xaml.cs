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
            this.DataContext = newPerson;//взаимосвязь с XAML
            db.context.Client.Add(newPerson);
        }

        private void BtnReg_Click(object sender, RoutedEventArgs e)
        {
           
            if (db.context.SaveChanges()>0)
            {
                MessageBox.Show("Вы зарегестрировались!", "Успех", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void BtnReturnMain_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new StartPage());
        }

        private void GenderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int activeGender = Convert.ToInt32(GenderComboBox.SelectedValue);
            newPerson.GenderId = activeGender;
        }
    }
}
