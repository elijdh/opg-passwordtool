using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.IO;
using Spire.Pdf;
using Word = Microsoft.Office.Interop.Word;
using Window = System.Windows.Window;
using Path = System.IO.Path;
using System.Windows.Media.Imaging;

namespace passwordTool
{
    /// <summary>
    /// Interaction logic for errorPassword.xaml
    /// </summary>
    /// 

    public partial class ErrorPassword : System.Windows.Controls.Page
    {

        private List<string> files;
        
        private string newpassword;
        private string opcode;



        public ErrorPassword(List<string> files, string opcode)
        {
            InitializeComponent();
            this.files = files;
            this.opcode = opcode;


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

        // to be able to go back to popup.xaml page
        private void Button_Click_Back(object sender, RoutedEventArgs e)
        {
            var popUp = System.Windows.Application.Current.Windows.OfType<PopUp>().FirstOrDefault();

            if (popUp != null)
            {
                popUp.ButtonPanel.Visibility = Visibility.Visible; // Ensure button panel is shown
                popUp.Show();
                popUp.Activate(); // Bring the window to the front if necessary
            }

            // Optionally close or hide the current window
            this.Visibility = Visibility.Hidden;

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


        // closing and minimizing buttons
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


        //for any errors 
        private void ShowErrorMessage(string message)
        {

             ErrorMessageTextBlock.Text = $"ERROR: {message}";

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

        


        //  checks if its a docs file
        bool IsDocxfile(string filepath) {

            byte[] docxSignature = new byte[] { 0x50, 0x4B, 0x03, 0x04 };
            using (FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read))
            {
                byte[] buffer = new byte[4];
                fs.Read(buffer, 0, buffer.Length);
                return buffer.SequenceEqual(docxSignature);

            }
        }

        // check if its a pdf file
        bool IsPdfFile(string filepath)
        {

            byte[] PdfSignature = new byte[] { 0x25, 0x50, 0x44, 0x46 };
            using (FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read))
            {
                byte[] buffer = new byte[4];
                fs.Read(buffer, 0, buffer.Length);
                return buffer.SequenceEqual(PdfSignature);

            }
        }


        // same func as in performing ops page
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

        // when continue - FIX THIS
        void Button_Run_code(object sender, RoutedEventArgs e)
        {
            string newpassword = passwordTextBox.Text;
            List<string> errorfiles = new List<string>();

            // use this code to try again

            if (string.IsNullOrEmpty(newpassword) || newpassword == "New Password")
            {
                ShowErrorMessage("No password entered. Enter a valid password");
            }
            else
            {
                if (opcode=="O")
                {
                    //openeing all the word docs
                    foreach (string file in files)
                    {
                        if (file.EndsWith(".docx", StringComparison.OrdinalIgnoreCase) || IsDocxfile(file))
                        {
                            OpenWordDocument(file, newpassword, opcode, errorfiles);
                        }
                        else if (file.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase) && IsDocxfile(file))
                        {
                            // code for pdf
                            try
                            {

                              
                            }
                            catch (Exception)
                            {
                                // !!!! just uncomment below to deal wihth exceptions
                                //if (!Path.GetFileName(file).StartsWith("~$"))
                                //{
                                //    errorfiles.Add(file);
                                //}
                            }
                        }
                    }


                }

                // for removing passwords
                if (opcode=="R")
                {

                    foreach (string file in files)
                    {

                        if (file.EndsWith(".docx", StringComparison.OrdinalIgnoreCase) || IsDocxfile(file))
                        {
                            try
                            {
                                Spire.Doc.Document doc = new Spire.Doc.Document();
                                doc.LoadFromFile(file, Spire.Doc.FileFormat.Docx, newpassword);
                                doc.RemoveEncryption();
                                doc.SaveToFile(file, Spire.Doc.FileFormat.Docx);
                            }
                            catch (Exception)
                            {
                                if (!Path.GetFileName(file).StartsWith("~$"))
                                {
                                    errorfiles.Add(file);
                                }
                                //ErrorPage(filepath, opcode);


                            }

                        }
                        else if (file.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase) && IsDocxfile(file))
                        {
                            try
                            {
                                PdfDocument pdf = new PdfDocument();
                                pdf.LoadFromFile(file, newpassword);
                                pdf.Security.Encrypt(string.Empty, string.Empty, Spire.Pdf.Security.PdfPermissionsFlags.Default, Spire.Pdf.Security.PdfEncryptionKeySize.Key128Bit, "permission");
                                pdf.SaveToFile(file, Spire.Pdf.FileFormat.PDF);

                            }
                            catch (Exception)
                            {
                                try
                                {
                                    PdfDocument pdf = new PdfDocument();
                                    pdf.LoadFromFile(file, newpassword);
                                    pdf.Security.Encrypt(string.Empty, string.Empty, Spire.Pdf.Security.PdfPermissionsFlags.Default, Spire.Pdf.Security.PdfEncryptionKeySize.Key128Bit, newpassword);
                                    pdf.SaveToFile(file, Spire.Pdf.FileFormat.PDF);
                                }
                                catch
                                {
                                    //ErrorPage(filepath, opcode);
                                    if (!Path.GetFileName(file).StartsWith("~$"))
                                    {
                                        errorfiles.Add(file);
                                    }
                                }
                            }
                        }



                    }
                }



                // if still more files left, repeat 
                if (errorfiles.Count > 0)
                {
                    Button_Close(this, null);
                    ErrorPage(errorfiles, opcode);
                }
                else
                {
                    Button_Close(this, null);

                }

            }

        }
    }

}

/* in the else  block
 * try
                {

                    if (filepath.EndsWith(".docx", StringComparison.OrdinalIgnoreCase) || IsDocxfile(filepath))
                    {

                        if (opcode == "O")
                        {
                            Word.Application wordApp = new Word.Application();
                            Word.Document doc = null;
                            doc = wordApp.Documents.Open(filepath, ReadOnly: false, PasswordDocument: newpassword);
                        }
                            
                        if (opcode == "R")
                        {
                            Spire.Doc.Document doc = new Spire.Doc.Document();
                            doc.LoadFromFile(filepath, Spire.Doc.FileFormat.Docx, newpassword);
                            doc.Unprotect();
                            doc.SaveToFile(filepath, Spire.Doc.FileFormat.Docx);
                        }

                    } 
                        
                    if (filepath.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase) || IsPdfFile(filepath))
                    {

                        if (opcode == "R")
                        {
                            PdfDocument pdf = new PdfDocument();
                            pdf.LoadFromFile(filepath, newpassword);
                            pdf.Security.Encrypt(string.Empty, string.Empty, Spire.Pdf.Security.PdfPermissionsFlags.Default, Spire.Pdf.Security.PdfEncryptionKeySize.Key128Bit, "permission");
                            pdf.SaveToFile(filepath, Spire.Pdf.FileFormat.PDF);
                        }
                    }

                }
                catch (Exception)
                {

                    ShowErrorMessage("Something went wrong. Try again.");
                }
            }

            Button_Close(this, null)*/