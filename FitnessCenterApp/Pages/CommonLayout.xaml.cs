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
    public enum CommonLayoutGoToType
    {
        ChangeFrameContent,
        ReplaceItself
    };

    public partial class CommonLayout : Page
    {
        object _onButton1Press;
        private CommonLayoutGoToType _button1GoToType;
        object _onButton2Press;
        private CommonLayoutGoToType _button2GoToType;
        private void commonConstructor(string header, Page subframe, string button1, Page button1Pressed, CommonLayoutGoToType button1GoToType)
        {
            InitializeComponent();
            SubTitle.Text = header;
            _button1GoToType = button1GoToType;
            _onButton1Press = button1Pressed;
            if (button1Pressed == null)
            {
                BtnReturnMain.Visibility = Visibility.Hidden;
            }
            else
            {
                BtnReturnMain.Content = button1;
            }
            SubFrame.Navigate(subframe);
        }
        public CommonLayout(string header, Page subframe, string button1, Page button1Pressed, CommonLayoutGoToType button1GoToType)
        {
            commonConstructor(header, subframe, button1, button1Pressed, button1GoToType);
            BtnReturnSecondary.Visibility = Visibility.Hidden;
        }

        public CommonLayout(string header, Page subframe, string button1, Page button1Pressed, CommonLayoutGoToType button1GoToType, string button2, Page button2Pressed, CommonLayoutGoToType button2GoToType)
        {
            commonConstructor(header, subframe, button1, button1Pressed, button1GoToType);
            BtnReturnSecondary.Content = button2;
            _onButton2Press = button2Pressed;
            _button2GoToType = button2GoToType;
        }

        private void Btn_click(object goTo, CommonLayoutGoToType goToType)
        {
            switch (goToType)
            {
                case CommonLayoutGoToType.ChangeFrameContent:
                    SubFrame.Navigate(goTo);
                    break;
                case CommonLayoutGoToType.ReplaceItself:
                    NavigationService.Navigate(goTo);
                    break;
            }
        }

        private void BtnReturnMain_Click(object sender, RoutedEventArgs e)
        {
            if (_onButton1Press == null)
                return;

            Btn_click(_onButton1Press, _button1GoToType);
        }

        private void BtnReturnSecondary_Click(object sender, RoutedEventArgs e)
        {
            if (_onButton2Press == null)
                return;

            Btn_click(_onButton2Press, _button2GoToType);
        }
    }
}
