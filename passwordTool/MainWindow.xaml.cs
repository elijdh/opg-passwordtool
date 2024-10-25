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

        private void Button_Click_remove(object sender, RoutedEventArgs e)
        {

            var page = new RemovePassword("Removing Password", "This tool will Remove the same password from all files in a given directory", "R");
            ButtonPanel.Visibility = Visibility.Hidden;
            _Buttons.Navigate(page);
        }

        private void Button_Click_add(object sender, RoutedEventArgs e)
        {

            var page = new RemovePassword("Adding Password", "This tool will add the same password to all files in a given directory", "A");
            ButtonPanel.Visibility = Visibility.Hidden;
            _Buttons.Navigate(page);
        }

        private void Button_Click_open(object sender, RoutedEventArgs e)
        {

            var page = new RemovePassword("Opening Documents", "This tool will open all of the files in a given directory protected with the same password", "O");
            ButtonPanel.Visibility = Visibility.Hidden;
            _Buttons.Navigate(page);
        }


        // _------------------------------using diff pages ------------------------------------
        //private void Button_Click_remove(object sender, RoutedEventArgs e)
        //{
        //    ButtonPanel.Visibility = Visibility.Collapsed; // gets rid of buttons when navigating to new page
        //    _Buttons.Navigate(new Uri("RemovePassword.xaml", UriKind.Relative)); // actually navigates to new page
        //}

        //private void Button_Click_add(object sender, RoutedEventArgs e)
        //{
        //    ButtonPanel.Visibility = Visibility.Collapsed;
        //    _Buttons.Navigate(new Uri("AddPassword.xaml", UriKind.Relative));
        //}

        //private void Button_Click_open(object sender, RoutedEventArgs e)
        //{
        //    ButtonPanel.Visibility = Visibility.Collapsed;
        //    _Buttons.Navigate(new Uri("OpenDocs.xaml", UriKind.Relative));
        //}


        void Button_Close (object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Minimize(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

    }
}
