using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
    /// Interaction logic for AddExercise.xaml
    /// </summary>
    public partial class AddExercise : Page
    {
        Core db = new Core(); // Отдельная база для возможности не сохранять изменения
        List<Exercise> exercises;
        ObservableCollection<Exercise> exercisesView;
        Trainer trainer;
        Client client;
        public AddExercise(Trainer currentTrainer, Client currentClient)
        {
            InitializeComponent();
            trainer = currentTrainer;
            client = currentClient;
            exercises = db.context.Exercise.Where(x => x.TrainerId == trainer.Id && x.ClientId == client.Id).ToList();
            exercisesView = new ObservableCollection<Exercise>(exercises);
            ExercisesGrid.ItemsSource = exercisesView;
            exercisesView.CollectionChanged += onCollectionChange;
        }

        private void onCollectionChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Exercise newItem in e.NewItems)
                {
                    newItem.ClientId = client.Id;
                    newItem.TrainerId = trainer.Id;
                    newItem.BeginAt = DateTime.Now;
                    db.context.Exercise.Add(newItem);
                }
            }

            if (e.OldItems != null)
            {
                foreach (Exercise oldItem in e.OldItems)
                {
                    db.context.Exercise.Remove(oldItem);
                }
            }
        }

        private void AddExercise_Click(object sender, RoutedEventArgs e)
        {
            exercisesView.Add(new Exercise());
        }
        private void RmExercise_Click(object sender, RoutedEventArgs e)
        {
            Exercise selected = (Exercise)ExercisesGrid.SelectedValue;
            if (selected == null)
            {
                MessageBox.Show("Выберите упражнения для удаления", "Выберите упражнения", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            exercisesView.Remove(selected);
        }

        private void SaveExercise_Click(object sender, RoutedEventArgs e)
        {
            if (client.Requests.StatusId != 2)
                db.context.Requests.Find(client.RequestId).StatusId = 2;
            db.context.SaveChanges();
        }

        /*

        int _lastSortedColumn;

        private class TypeComparer : System.Collections.IComparer
        {
            public int sortOrder = 1;
            public int Compare(object _x, object _y)
            {
                Exercise x = (Exercise)_x;
                Exercise y = (Exercise)_y;
                return (x.ExerciseType.CompareTo(y.ExerciseType)) * sortOrder;
            }
        }
        private void TypeCM_Click(object sender, RoutedEventArgs e)
        {
            if (_lastSortedColumn == 1)
                ((TypeComparer)exercisesView.CustomSort).sortOrder *= -1;
            else
                exercisesView.CustomSort = new TypeComparer();
            exercisesView.Refresh();
            _lastSortedColumn = 1;
        }

        private class RepeatsComparer : System.Collections.IComparer
        {
            public int sortOrder = 1;
            public int Compare(object _x, object _y)
            {
                Exercise x = (Exercise)_x;
                Exercise y = (Exercise)_y;
                return (x.RepeatOnceEvery - y.RepeatOnceEvery) * sortOrder;
            }
        }
        private void RepeatOnceEveryCM_Click(object sender, RoutedEventArgs e)
        {
            if (_lastSortedColumn == 2)
                ((RepeatsComparer)exercisesView.CustomSort).sortOrder *= -1;
            else
                exercisesView.CustomSort = new RepeatsComparer();
            exercisesView.Refresh();
            _lastSortedColumn = 2;
        }

        private class NumberOfSetsComparer : System.Collections.IComparer
        {
            public int sortOrder = 1;
            public int Compare(object _x, object _y)
            {
                Exercise x = (Exercise)_x;
                Exercise y = (Exercise)_y;
                return (x.NumberOfSets - y.NumberOfSets) * sortOrder;
            }
        }
        private void NumberOfSetsCM_Click(object sender, RoutedEventArgs e)
        {
            if (_lastSortedColumn == 3)
                ((NumberOfSetsComparer)exercisesView.CustomSort).sortOrder *= -1;
            else
                exercisesView.CustomSort = new NumberOfSetsComparer();
            exercisesView.Refresh();
            _lastSortedColumn = 3;
        }

        private class RepeatsPerSetComparer : System.Collections.IComparer
        {
            public int sortOrder = 1;
            public int Compare(object _x, object _y)
            {
                Exercise x = (Exercise)_x;
                Exercise y = (Exercise)_y;
                return (x.RepeatsPerSet - y.RepeatsPerSet) * sortOrder;
            }
        }
        private void RepeatsCM_Click(object sender, RoutedEventArgs e)
        {
            if (_lastSortedColumn == 4)
                ((RepeatsPerSetComparer)exercisesView.CustomSort).sortOrder *= -1;
            else
                exercisesView.CustomSort = new RepeatsPerSetComparer();
            exercisesView.Refresh();
            _lastSortedColumn = 4;
        }
        */
    }
}
