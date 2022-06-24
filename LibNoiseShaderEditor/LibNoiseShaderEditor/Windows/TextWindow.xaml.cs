using LibNoiseShaderEditor.Helpers;
using System;
using System.Text;
using System.Windows;

namespace LibNoiseShaderEditor.Windows
{
    /// <summary>
    /// Logique d'interaction pour TextWindow.xaml
    /// </summary>
    public partial class TextWindow : Window
    {
        public TextWindow(string text, string extension = null)
        {
            InitializeComponent();

            textBox.Text = text;
            DefaultExtension = extension ?? "txt";
        }

        public string DefaultExtension { get; set; }

        private void SaveAsButton_Click(object sender, RoutedEventArgs e)
        {
            string fileName = FileDialogHelpers.GetSaveFileName(this, DefaultExtension);

            if (fileName is null)
            {
                return;
            }

            try
            {
                System.IO.File.WriteAllText(fileName, textBox.Text, Encoding.UTF8);
                Xceed.Wpf.Toolkit.MessageBox.Show("Saved", Title);
            }
            catch (Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.ToString(), GlobalConstants.XceedMessageBoxTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
