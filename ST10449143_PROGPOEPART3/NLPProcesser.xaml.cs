using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ST10449143_PROGPOEPART3
{
    public partial class NLPProcessorWindow : Window
    {
        private NLPProcessor nlpProcessor = new NLPProcessor();
        private bool showDetails = false;

        public NLPProcessorWindow()
        {
            InitializeComponent();
            AddWelcomeMessage();
        }

        private void AddWelcomeMessage()
        {
            AddMessage("NLP Processor", "Welcome! I can help you with tasks, reminders, and quizzes. Type 'help' to see what I can do.", false);
        }

        private void AddMessage(string speaker, string message, bool isUser)
        {
            var result = isUser ? nlpProcessor.AnalyzeInput(message) : null;

            var messageData = new
            {
                Speaker = speaker,
                Message = message,
                IsUser = isUser,
                IntentDetails = result != null ? $"Intent: {result.Intent}, Title: {result.Title}" : "",
                ShowDetails = showDetails && isUser
            };

            ConversationHistory.Items.Add(messageData);
            ConversationScrollViewer.ScrollToEnd();
        }

        private void ProcessInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return;

            AddMessage("You", input, true);
            UserInput.Clear();

            var result = nlpProcessor.AnalyzeInput(input);

            string response = GetResponseForIntent(result);
            AddMessage("NLP Processor", response, false);
        }

        private string GetResponseForIntent(NLPProcessor.NlpResult result)
        {
            switch (result.Intent)
            {
                case NLPProcessor.NlpIntent.AddTask:
                    return $"I'll add the task: '{result.Title}' to your list.";

                case NLPProcessor.NlpIntent.SetReminder:
                    return $"Reminder set for: '{result.Title}'.";

                case NLPProcessor.NlpIntent.ViewTasks:
                    return "Here are your current tasks...";

                case NLPProcessor.NlpIntent.CompleteTask:
                    return $"Marking task '{result.Title}' as completed.";

                case NLPProcessor.NlpIntent.DeleteTask:
                    return $"Deleting task: '{result.Title}'.";

                case NLPProcessor.NlpIntent.StartQuiz:
                    return "Starting the Cyber Security Quiz...";

                case NLPProcessor.NlpIntent.ShowHelp:
                    return "I understand commands about tasks, reminders, and quizzes. Expand the 'Available Commands' section below to see examples.";

                case NLPProcessor.NlpIntent.Exit:
                    return "Goodbye! Closing the application...";

                case NLPProcessor.NlpIntent.None:
                default:
                    return "I didn't understand that. Try asking for 'help' to see what I can do.";
            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessInput(UserInput.Text);
        }

        private void UserInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && Keyboard.Modifiers != ModifierKeys.Shift)
            {
                ProcessInput(UserInput.Text);
                e.Handled = true;
            }
        }

        // Toggle for showing intent details
        private void ToggleDetails_Click(object sender, RoutedEventArgs e)
        {
            showDetails = !showDetails;
            ConversationHistory.Items.Refresh();
        }
    }
}