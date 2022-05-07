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
    public partial class TrainerListPage : Page, IPostLoginPage
    {
        Core db;
        List<Trainer> arrayTrainer;
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
        private void initTrainetListPage(Core db)
        {
            InitializeComponent();

            arrayTrainer = db.context.Trainer.ToList();
            foreach (var item in arrayTrainer)
            {
                Console.WriteLine(item.Name);
            }
        }
        public TrainerListPage()
        {
            initTrainetListPage(new Core());
        }
        public TrainerListPage(Core db)
        {
            initTrainetListPage(db);
        }
    }
}
