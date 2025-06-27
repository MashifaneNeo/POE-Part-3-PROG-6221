using System.Windows;

namespace ST10449143_PROGPOEPART3
{
    public partial class WelcomeDialog : Window
    {
        public string UserName => NameTextBox.Text.Trim();

        public WelcomeDialog()
        {
            InitializeComponent();
        }

        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(UserName))
            {
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Please enter your name", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}