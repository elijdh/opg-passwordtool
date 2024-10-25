﻿using System;
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
    /// Interaction logic for SimpleErrorPopUp.xaml
    /// </summary>
    public partial class SimpleErrorPopUp : Window
    {

        public SimpleErrorPopUp(string filepath, string opcode, string msg)
        {
            InitializeComponent();

            if (opcode=="A") {
                TitleTextBox.Text = "Error";
            } else {TitleTextBox.Text = "Update"; }

            MessageTextBox.Text = msg;

        }

        // for moving page around the screen
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


        
    }
}
