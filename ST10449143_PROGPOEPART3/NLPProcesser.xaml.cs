using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
            AddMessage("NLP Processor", "Welcome! I can help you with tasks management and reminders. Look at available" +
                " commands to see what i can help you with.", false);
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
            if (string.IsNullOrWhiteSpace(input)) return;

            AddMessage("You", input, true);
            UserInput.Clear();

            var result = nlpProcessor.AnalyzeInput(input);
            string response = GetResponseForIntent(result, input);
            AddMessage("NLP Processor", response, false);
        }

        private string GetResponseForIntent(NLPProcessor.NlpResult result, string originalInput)
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            if (mainWindow == null) return "Error: Main window not found.";

            switch (result.Intent)
            {
                case NLPProcessor.NlpIntent.AddTask:
                    if (!string.IsNullOrWhiteSpace(result.Title))
                    {
                        mainWindow.taskAssistantControl?.HandleChatCommand($"add task - {result.Title}");
                        mainWindow.LogActivity($"Task added via NLP: {result.Title}");
                        return $"Task '{result.Title}' added via NLP. Would you like to set a reminder for this task?";
                    }
                    return "Please specify the task details.";

                case NLPProcessor.NlpIntent.SetReminder:
                    string reminderTask = result.Title;
                    string reminderCommand;

                    if (originalInput.ToLower().Contains("tomorrow"))
                    {
                        // Set for tomorrow's date
                        reminderCommand = $"yes, remind me on {DateTime.Now.AddDays(1).ToShortDateString()}";
                    }
                    else
                    {
                        reminderCommand = "yes, remind me in 3";
                    }

                    mainWindow.taskAssistantControl?.HandleChatCommand($"add task - {reminderTask}");
                    mainWindow.taskAssistantControl?.HandleChatCommand(reminderCommand);
                    mainWindow.LogActivity($"Reminder set via NLP for: {reminderTask}");
                    return $"Reminder for '{reminderTask}' set via NLP.";

                case NLPProcessor.NlpIntent.ViewTasks:
                    return mainWindow.taskAssistantControl?.HandleChatCommand("show tasks") ?? "No tasks found.";

                case NLPProcessor.NlpIntent.CompleteTask:
                    mainWindow.taskAssistantControl?.HandleChatCommand($"complete task {result.Title}");
                    mainWindow.LogActivity($"Task completed via NLP: {result.Title}");
                    return $"Marked task '{result.Title}' as completed via NLP.";

                case NLPProcessor.NlpIntent.DeleteTask:
                    mainWindow.taskAssistantControl?.HandleChatCommand($"delete task {result.Title}");
                    mainWindow.LogActivity($"Task deleted via NLP: {result.Title}");
                    return $"Task '{result.Title}' deleted via NLP.";

                case NLPProcessor.NlpIntent.StartQuiz:
                    mainWindow.quizControl?.quiz?.StartQuiz();
                    mainWindow.LogActivity("Quiz started via NLP.");
                    return "Started quiz via NLP.";

                case NLPProcessor.NlpIntent.ShowHelp:
                    return "Try commands like: 'add task', 'remind me', 'start quiz', 'show tasks', 'exit'.";

                case NLPProcessor.NlpIntent.Exit:
                    mainWindow.Close();
                    return "Exiting application...";

                case NLPProcessor.NlpIntent.None:
                    if (!string.IsNullOrWhiteSpace(result.Title) &&
                        (result.Title.Contains("what have you done") || result.Title.Contains("summary")))
                    {
                        var recent = mainWindow.GetRecentActivity(5);
                        var cleanSummary = string.Join("\n", recent?.ConvertAll(line => RemoveTimestamp(line)) ?? new List<string>());
                        return "Here's a summary of recent actions:\n" + cleanSummary;
                    }
                    return "I didn't understand that. Try asking for 'help'.";

                default:
                    return "I didn't understand that. Try asking for 'help'.";
            }
        }

        private string RemoveTimestamp(string logEntry)
        {
            int index = logEntry.IndexOf("] ");
            return index >= 0 ? logEntry.Substring(index + 2) : logEntry;
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

        private void ToggleDetails_Click(object sender, RoutedEventArgs e)
        {
            showDetails = !showDetails;
            ConversationHistory.Items.Refresh();
        }
    }
}
