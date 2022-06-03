using System;
using System.Windows;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using FitnessCenterApp.Pages;

namespace FitnessCenterApp
{
    /// <summary>
    /// Логика взаимодействия для InputWindow.xaml
    /// </summary>
    /// 

    public partial class InputWindow : Window, IDisposable
    {
        [DllImport("user32.dll")]
        static extern uint GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

        const int GWL_EXSTYLE = -20;
        const int WS_EX_DLGMODALFRAME = 0x0001;

        public string input { get; private set; }

        protected void InputWindow_OnSourceInitialized(object sender, EventArgs e)
        {
            IntPtr hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_EXSTYLE, GetWindowLong(hwnd, GWL_EXSTYLE) | WS_EX_DLGMODALFRAME);
        }
        public InputWindow(string message, string title)
        {
            InitializeComponent();
            if (title == null)
                Title = "Введите текст";
            else
                Title = title;

            if (message == null)
                Message.Text = "Введите текст, запрошенный приложением: ";
            else
                Message.Text = message;
            this.SourceInitialized += InputWindow_OnSourceInitialized; // Убирает иконку
        }


        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Hidden;
            DialogResult = false;
        }

        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Hidden;
            DialogResult = true;
        }

        private void UserInput_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            input = UserInput.Text;
            if (string.IsNullOrEmpty(input))
            {
                OkBtn.Style = FindResource("DialogueDisabledButton") as Style;
                OkBtn.IsHitTestVisible = false;
            }
            else
            {
                OkBtn.Style = FindResource("DialogueButton") as Style;
                OkBtn.IsHitTestVisible = true;
            }
        }
        public void Dispose()
        {
            Close();
            GC.SuppressFinalize(this);
        }
    }
}
