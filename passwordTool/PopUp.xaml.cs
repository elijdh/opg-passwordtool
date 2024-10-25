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
using System.Windows.Shapes;

namespace passwordTool
{
    /// <summary>
    /// Interaction logic for popUp.xaml
    /// </summary>
    
    public partial class PopUp : Window
    {
        private List<string> files;
        private string opcode;

        public PopUp(List<string> files, string opcode)
        {
            
            InitializeComponent();
            ButtonPanel.Visibility = Visibility.Visible;    

            //initializing variables
            this.files = files;
            this.opcode = opcode;

            // assining values to textboxes

            SelectedPathTextBox.Text = $"{string.Join(Environment.NewLine, files)}";

        }

        // for dragging window
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        //for closing and minimizing 
        void Button_Close(object sender, RoutedEventArgs e) // also for skipping file
        {
            this.Close();
        }

        private void Button_Minimize(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
       
        // for changing password
        private void Button_Run_diffPassword(object sender, RoutedEventArgs e)
        {

            var page = new ErrorPassword(files, opcode);
            ButtonPanel.Visibility = Visibility.Hidden;
            errorPopUp.Navigate(page);


        }

        
            
        }

}

