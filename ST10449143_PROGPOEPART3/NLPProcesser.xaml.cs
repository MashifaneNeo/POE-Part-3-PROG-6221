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
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            switch (result.Intent)
            {
                case NLPProcessor.NlpIntent.AddTask:
                    mainWindow?.taskAssistantControl?.HandleChatCommand($"add task - {result.Title}");
                    mainWindow?.LogActivity($"Task added via NLP: {result.Title}");
                    return $"Task '{result.Title}' added via NLP.";

                case NLPProcessor.NlpIntent.SetReminder:
                    mainWindow?.taskAssistantControl?.HandleChatCommand($"add task - {result.Title}");
                    mainWindow?.taskAssistantControl?.HandleChatCommand("yes, remind me in 3"); // Default 3 days
                    mainWindow?.LogActivity($"Reminder set via NLP for: {result.Title}");
                    return $"Reminder for '{result.Title}' set via NLP.";

                case NLPProcessor.NlpIntent.StartQuiz:
                    mainWindow?.quizControl?.quiz?.StartQuiz();
                    mainWindow?.LogActivity("Quiz started via NLP.");
                    return "Started quiz via NLP.";

                case NLPProcessor.NlpIntent.ViewTasks:
                    return mainWindow?.taskAssistantControl?.HandleChatCommand("show tasks") ?? "No tasks found.";

                case NLPProcessor.NlpIntent.CompleteTask:
                    mainWindow?.taskAssistantControl?.HandleChatCommand($"complete task {result.Title}");
                    mainWindow?.LogActivity($"Task completed via NLP: {result.Title}");
                    return $"Marked task '{result.Title}' as completed via NLP.";

                case NLPProcessor.NlpIntent.DeleteTask:
                    mainWindow?.taskAssistantControl?.HandleChatCommand($"delete task {result.Title}");
                    mainWindow?.LogActivity($"Task deleted via NLP: {result.Title}");
                    return $"Task '{result.Title}' deleted via NLP.";

                case NLPProcessor.NlpIntent.ShowHelp:
                    return "Try commands like: 'add task', 'remind me', 'start quiz', 'show tasks', 'exit'.";

                case NLPProcessor.NlpIntent.Exit:
                    mainWindow?.Close();
                    return "Exiting application...";

                case NLPProcessor.NlpIntent.None:
                    if (result.Title.ToLower().Contains("what have you done"))
                    {
                        var recent = mainWindow?.GetRecentActivity(5);
                        return "Here’s what I’ve done recently:\n" + string.Join("\n", recent ?? new List<string>());
                    }
                    return "I didn't understand that. Try asking for 'help'.";
            }

            return "";
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