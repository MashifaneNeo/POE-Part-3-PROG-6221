using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ST10449143_PROGPOEPART3
{
    public partial class ChatbotControl : UserControl
    {
        private PreviousWork chatbot;

        public ChatbotControl()
        {
            InitializeComponent();
            chatbot = new PreviousWork(MainWindow.CurrentUserName, DisplayMessage);
        }

        private void DisplayMessage(string message, Brush color)
        {
            var textBlock = new TextBlock
            {
                Text = message,
                Foreground = color,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 4, 0, 4)
            };
            ChatHistoryPanel.Children.Add(textBlock);

            // Update layout and scroll to bottom
            ChatScrollViewer.UpdateLayout();
            ChatScrollViewer.ScrollToEnd();
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessInput();
        }

        private void UserInputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ProcessInput();
            }
        }

        private void ProcessInput()
        {
            var input = UserInputBox.Text.Trim();
            if (!string.IsNullOrWhiteSpace(input))
            {
                chatbot.ProcessInput(input);
                UserInputBox.Clear();
            }
        }
    }
}
