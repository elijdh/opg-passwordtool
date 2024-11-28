using System;
using System.Windows;
using System.Windows.Media;
using System.IO;
using Spire.Doc;
using Spire.Pdf;
using Word = Microsoft.Office.Interop.Word;
using System.Collections.Generic;
using Page = System.Windows.Controls.Page;
using System.Windows.Media.Imaging;

namespace passwordTool.Core
{
    /// <summary>
    /// Interaction logic for PerformingOperations.xaml
    /// </summary>
    public partial class PerformingOperations : Page
    {
        private string folderpath;
        private string opCode; // either r= remove, a=add, or o=open
        private string title;
        private string password;
        public PerformingOperations(String path, string operation, string TitlePart)
        {
            InitializeComponent();
            folderpath = path;
            opCode = operation.ToUpper();
            title = TitlePart;

            //setting tectblock display for folderpath and operation code
            SelectedPathTextBox.Text = $"Selected Folder: {folderpath}";
            TitleTextBox.Text = $"{TitlePart} Tool";

            // for prompt on first page
            if (operation == "R") { PasswordPrompt.Text = "Please Enter the password you would like to remove from the files below:"; }
            else if (operation == "A") { PasswordPrompt.Text = "Please Enter the password you would like to add to the files below:"; }
            else if (operation == "O") { PasswordPrompt.Text = "Please Enter the password for the files you would like to open below:"; }
            else { PasswordPrompt.Text = "Please Enter the password below:"; }

        }
        private bool isPasswordVisible = false;

        private void TogglePasswordVisibility(object sender, RoutedEventArgs e)
        {
            isPasswordVisible = !isPasswordVisible;

            if (isPasswordVisible)
            {
                // Show the TextBox and hide the PasswordBox
                passwordTextBox.Visibility = Visibility.Visible;
                passwordBox.Visibility = Visibility.Collapsed;

                // Update the icon to indicate the password is visible
                toggleImage.Source = new BitmapImage(new Uri("../assets/passwordVisibility1.png", UriKind.Relative));

                // Set the TextBox text to match the PasswordBox password
                passwordTextBox.Text = passwordBox.Password;
            }
            else
            {
                // Show the PasswordBox and hide the TextBox
                passwordTextBox.Visibility = Visibility.Collapsed;
                passwordBox.Visibility = Visibility.Visible;

                // Update the icon to indicate the password is hidden
                toggleImage.Source = new BitmapImage(new Uri("../assets/passwordVisibility.png", UriKind.Relative));

                // Sync the PasswordBox with the TextBox's text
                passwordBox.Password = passwordTextBox.Text;
            }
        }
        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            // Hide placeholder when the PasswordBox is focused
            placeholderText.Visibility = Visibility.Collapsed;
        }

        private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            // Show placeholder if the PasswordBox is empty and loses focus
            placeholderText.Visibility = string.IsNullOrEmpty(passwordBox.Password) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            // Update placeholder visibility based on content
            placeholderText.Visibility = string.IsNullOrEmpty(passwordBox.Password) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void Button_Click_Back(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }

        }
        public void Button_Close(object sender, RoutedEventArgs e)
        {
            var window = System.Windows.Window.GetWindow(this);
            window?.Close();
        }


        // for button openeing + minimizing
        private void Button_Minimize(object sender, RoutedEventArgs e)
        {
            var window = System.Windows.Window.GetWindow(this);
            window.WindowState = WindowState.Minimized;
        }

        // to show error message
        private void ShowErrorMessage(string message)
        {
            ErrorMessageTextBlock.Text = $"ERROR: {message}";
        }

        public void goMainPage()
        {
            // getting rid of backstack 
            while (NavigationService.CanGoBack)
            {
                NavigationService.RemoveBackEntry();
            }

            // going to main page
            var mainWindow = AppManager.CurrentMainWindow as MainWindow;
            mainWindow.Show();
            if (mainWindow != null)
            {

                mainWindow.ButtonPanel.Visibility = Visibility.Visible; // Ensure button panel is shown
            }

            // Hide the current page
            this.Visibility = Visibility.Hidden;


        }

        // when continue 
        private void Button_Run_code(object sender, RoutedEventArgs e)
        {

            password = passwordBox.Password;

            if ((string.IsNullOrEmpty(password) && password.Trim().Length == 0) || (password == "Enter your password"))
            {
                ShowErrorMessage("No password entered. Enter a valid password");
            }
            else
            {
                PasswordTool(folderpath, opCode, password);
            }

        }


        // to open error pop up page
        private static void ErrorPage(List<string> filepath, string opcode)
        {
            PopUp errorPage = new PopUp(filepath, opcode);
            Window mainWindow = System.Windows.Application.Current.MainWindow;
            if (mainWindow != null)
            {
                errorPage.Owner = mainWindow;
                errorPage.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }

            errorPage.ShowDialog();

        }

        //simple error page pop up
        private static void SimpleErrorPage(string folderpath, string opcode, string ErrMsg)
        {
            SimpleErrorPopUp errorPage = new SimpleErrorPopUp(folderpath, opcode, ErrMsg);

            // puts pop up in middle of window
            System.Windows.Window mainWindow = System.Windows.Application.Current.MainWindow;
            if (mainWindow != null)
            {
                errorPage.Owner = mainWindow;
                errorPage.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }

            // Show the error page
            errorPage.ShowDialog();


        }

        //open word document 
        static void OpenWordDocument(string filePath, string password, string opcode, List<string> errorfiles)
        {
            Word.Application wordApp = new Word.Application();
            Word.Document doc = null;

            try
            {
                // Open the Word document with the provided password
                doc = wordApp.Documents.Open(filePath, ReadOnly: false, PasswordDocument: password);

                // Make Word visible to the user
                wordApp.Visible = true;
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                if (!Path.GetFileName(filePath).StartsWith("~$"))
                {
                    errorfiles.Add(filePath);
                }

            }
            finally
            {
                // Properly release COM objects
                if (doc != null)
                {
                    //doc.Close(false); // Close the document without saving changes
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(doc);
                }

                if (wordApp != null)
                {
                    //wordApp.Quit(); // Quit the Word application
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(wordApp);

                }
            }
        }

        // openeing pdf documents
        static void OpenPdfDocument(string filepath, string password, string opcode)
        {


        }

        private void ErrorFiles(List<string> files)
        {
            ErrorPage(files, opCode);
        }


        public void PasswordTool(string folderpath, string opcode, string password)
        {

            List<string> errorFiles = new List<string>();

            //for opening files
            if (opcode == "O")
            {
                foreach (string filepath in Directory.GetFiles(folderpath, "*.docx"))
                {
                    OpenWordDocument(filepath, password, opcode, errorFiles);
                }

                // to deal with error files
                if (errorFiles.Count > 0)
                {
                    ErrorPage(errorFiles, opcode);
                }

                SimpleErrorPage(folderpath, "Message", "All of the files have been opened.");
                goMainPage();
            }

            //to add password to files
            else if (opcode == "A")
            {
                // iterates through all .docx files
                foreach (string filepath in Directory.GetFiles(folderpath, "*.docx"))
                {
                    //loads files, tries encrypting
                    try
                    {
                        Document doc = new Document();
                        doc.LoadFromFile(filepath);
                        doc.Encrypt(password);
                        doc.SaveToFile(filepath, Spire.Doc.FileFormat.Docx);
                    }
                    catch (Exception)
                    {

                        if (!Path.GetFileName(filepath).StartsWith("~$"))
                        {
                            errorFiles.Add(filepath);
                        }


                    }
                }

                //iterates through all the pdf files
                foreach (string filepath in Directory.GetFiles(folderpath, "*.pdf"))
                {
                    try
                    {
                        using (PdfDocument pdf = new PdfDocument())
                        {
                            pdf.LoadFromFile(filepath, password);
                            pdf.Security.Encrypt(password, "permission", Spire.Pdf.Security.PdfPermissionsFlags.Print | Spire.Pdf.Security.PdfPermissionsFlags.CopyContent, Spire.Pdf.Security.PdfEncryptionKeySize.Key128Bit);
                            pdf.SaveToFile(filepath, Spire.Pdf.FileFormat.PDF);
                        }

                    }
                    catch (Exception)
                    {

                        if (!Path.GetFileName(filepath).StartsWith("~$"))
                        {
                            errorFiles.Add(filepath);
                            //SimpleErrorPage(filepath, opcode, "The file may already be encrypted, please check or try again");
                        }

                    }
                }


                if (errorFiles.Count > 0)
                {
                    SimpleErrorPage("", opcode, $"The following file(s) may already be encrypted, please check or try again\n\n{string.Join(Environment.NewLine, errorFiles)}");

                }

                SimpleErrorPage(folderpath, "Message", "Password has been added to the files.");
                goMainPage();


            }
            // removing password form files
            else if (opcode == "R")
            {
                // iterates through docx files in directory
                foreach (string filepath in Directory.GetFiles(folderpath, "*.docx"))
                {
                    try
                    {
                        Document doc = new Document();
                        doc.LoadFromFile(filepath, Spire.Doc.FileFormat.Docx, password);
                        doc.RemoveEncryption();
                        doc.SaveToFile(filepath, Spire.Doc.FileFormat.Docx);
                    }
                    catch (Exception)
                    {
                        if (!Path.GetFileName(filepath).StartsWith("~$"))
                        {
                            errorFiles.Add(filepath);
                        }
                        //ErrorPage(filepath, opcode);


                    }
                }

                //iterates through pdf files
                foreach (string filepath in Directory.GetFiles(folderpath, "*.pdf"))
                {
                    try
                    {
                        PdfDocument pdf = new PdfDocument();
                        pdf.LoadFromFile(filepath, password);
                        pdf.Security.Encrypt(string.Empty, string.Empty, Spire.Pdf.Security.PdfPermissionsFlags.Default, Spire.Pdf.Security.PdfEncryptionKeySize.Key128Bit, "permission");
                        pdf.SaveToFile(filepath, Spire.Pdf.FileFormat.PDF);

                    }
                    catch (Exception)
                    {
                        try
                        {
                            PdfDocument pdf = new PdfDocument();
                            pdf.LoadFromFile(filepath, password);
                            pdf.Security.Encrypt(string.Empty, string.Empty, Spire.Pdf.Security.PdfPermissionsFlags.Default, Spire.Pdf.Security.PdfEncryptionKeySize.Key128Bit, password);
                            pdf.SaveToFile(filepath, Spire.Pdf.FileFormat.PDF);
                        }
                        catch
                        {
                            //ErrorPage(filepath, opcode);
                            if (!Path.GetFileName(filepath).StartsWith("~$"))
                            {
                                errorFiles.Add(filepath);
                            }
                        }
                    }
                }

                // to deal with error files
                if (errorFiles.Count > 0)
                {

                    ErrorPage(errorFiles, opcode);
                }
                SimpleErrorPage(folderpath, "Message", "Password has been removed from all of the files.");
                goMainPage();
            }
            else
            {
                SimpleErrorPage("", opcode, "An Unexpected error occured. Try Again.");
                goMainPage();
            }
        }

        private void _Buttons_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

        }
    }
}