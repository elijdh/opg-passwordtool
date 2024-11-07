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
using System.Windows.Forms;
using passwordTool.Core;
using Microsoft.WindowsAPICodePack.Dialogs;
using DataFormats = System.Windows.DataFormats;
using System.IO;
using DragDropEffects = System.Windows.DragDropEffects;
using DragEventArgs = System.Windows.DragEventArgs;


namespace passwordTool
{

    public partial class RemovePassword : Page
    {
        private string titlePart;
        private string descrip;
        private string selectedPath;
        private string operation;

        public RemovePassword(string title, string textLine, string operation)
        {
            InitializeComponent();

            titlePart = title;
            descrip = textLine;
            this.operation = operation;

            // for textboxes
            TitleTextBox.Text = $"'s {title} Tool";
            DescriptTextBox.Text = descrip;

        }

        void Button_Close(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window?.Close();
        }

        private void Button_Minimize(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.WindowState = WindowState.Minimized;
        }

        private void Button_Click_Folder(object sender, RoutedEventArgs e)
        {

            using (var folderDialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    selectedPath = folderDialog.SelectedPath;
                    SelectedPathTextBox.Text = $"Selected: {selectedPath}";
                }
            }
        }

        // for dragging functionality
        private void Button_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length > 0 && Directory.Exists(files[0]))
                {
                    e.Effects = DragDropEffects.Copy;
                }
                else
                {
                    e.Effects = DragDropEffects.None;
                }
            }
            else { e.Effects = DragDropEffects.None; }
        }

        private void Button_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length > 0 && Directory.Exists(files[0]))
                {
                    ErrorMessageTextBlock.Text = "";
                    selectedPath = files[0];
                    SelectedPathTextBox.Text = $"Selected: {selectedPath}";
                }
                else
                {
                    ShowErrorMessage("please drop a folder, not a file");
                }
            }
        }

        private void Button_Op_page(object sender, RoutedEventArgs e)
        {
            // passing to new page where password will be entered for files + removed
            if (System.IO.Directory.Exists(FolderPathTextBox.Text))
            {
                NavigateToPages(FolderPathTextBox.Text);
            }
            else if (!string.IsNullOrEmpty(selectedPath) && System.IO.Directory.Exists(selectedPath))
            {
                NavigateToPages(selectedPath);
            }
            else
            {
                ShowErrorMessage("Please Enter a valid Directory path or select a folder");
            }


        }

        // to actuall go to the pages 
        private void NavigateToPages(string path)
        {
            var operations = new PerformingOperations(path, operation, titlePart);
            NavigationService.Navigate(operations);

            // clear the backstack
            while (NavigationService.CanGoBack)
            {
                NavigationService.RemoveBackEntry();
            }

        }

        // text for error message
        private void ShowErrorMessage(string message)
        {
            ErrorMessageTextBlock.Text = $"ERROR: {message}";
        }

        private void Button_Click_Back(object sender, RoutedEventArgs e)
        {
            var mainWindow = AppManager.CurrentMainWindow as MainWindow;
            mainWindow.Show();
            if (mainWindow != null)
            {

                mainWindow.ButtonPanel.Visibility = Visibility.Visible; // Ensure button panel is shown
            }

            // Hide the current page
            this.Visibility = Visibility.Hidden;
        }


        private void FolderPathTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (FolderPathTextBox.Text == "Enter folder path")
            {
                FolderPathTextBox.Text = string.Empty;
                FolderPathTextBox.Foreground = new SolidColorBrush(Color.FromRgb(89, 89, 89));
            }
        }

        private void FolderPathTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FolderPathTextBox.Text))
            {
                FolderPathTextBox.Text = "Enter folder Path";
                FolderPathTextBox.Foreground = new SolidColorBrush(Color.FromRgb(89, 89, 89));
            }
        }

    }
}