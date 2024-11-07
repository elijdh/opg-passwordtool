using passwordTool.Core;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace passwordTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private weakEventHandler<MainWindow> _weakHandler;
        public MainWindow()
        {

            InitializeComponent();
            AppManager.CurrentMainWindow = this;
            ButtonPanel.Visibility = Visibility.Visible;


        }

        // to move window around
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void NavigateToPage(string descrip, string msg, string opcode)
        {
            var page = new RemovePassword(descrip, msg, opcode);
            ButtonPanel.Visibility = Visibility.Hidden;
            _Buttons.Navigate(page);


        }

        private void Button_Click_remove(object sender, RoutedEventArgs e)
        {

            NavigateToPage("Removing Password", "This tool will Remove the same password from all files in a given directory", "R");
        }

        private void Button_Click_add(object sender, RoutedEventArgs e)
        {

            NavigateToPage("Adding Password", "This tool will add the same password to all files in a given directory", "A");

        }

        private void Button_Click_open(object sender, RoutedEventArgs e)
        {

            NavigateToPage("Opening Documents", "This tool will open all of the files in a given directory protected with the same password", "O");
        }

        void Button_Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Minimize(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

    }
}