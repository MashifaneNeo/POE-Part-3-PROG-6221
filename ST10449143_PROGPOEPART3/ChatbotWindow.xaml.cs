using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ST10449143_PROGPOEPART3
{
    public partial class ChatbotWindow : Window
    {
        private PreviousWork chatbot;

        public ChatbotWindow()
        {
            InitializeComponent();
            chatbot = new PreviousWork("User", DisplayMessage);
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
