using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ST10449143_PROGPOEPART3;

namespace ST10449143_PROGPOEPart3
{
    public partial class MainWindow : Window
    {
        public class CyberTask
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public DateTime? ReminderDate { get; set; }
            public bool IsCompleted { get; set; }

            public override string ToString()
            {
                return $"{(IsCompleted ? "[✔] " : "[ ] ")}{Title} - {Description}" +
                       (ReminderDate.HasValue ? $" (Reminder: {ReminderDate.Value:dd MMM yyyy})" : "");
            }
        }

        private string userName = null;
        private bool awaitingName = true;
        private bool isShowingFullActivity = false;

        private PreviousWork chatbot;
        private List<CyberTask> tasks = new List<CyberTask>();
        private CyberTask pendingTask = null;
        private CyberSecurityQuiz cyberQuiz;
        private NLPProcessor nlpProcessor;

        private List<string> userActions = new List<string>();
        private Queue<string> activityLog = new Queue<string>();

        public MainWindow()
        {
            InitializeComponent();
            InitializeApplicationComponents();
            AskForUserName();
        }

        private void InitializeApplicationComponents()
        {            
            cyberQuiz = new CyberSecurityQuiz(AddMessage);
            nlpProcessor = new NLPProcessor();
        }

        private void AskForUserName()
        {
            AddMessage("Hello! Welcome to the" +
                " Cybersecurity chatbot. Before we begin, may I know your name?", Brushes.DarkSlateBlue);
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string input = InputBox.Text.Trim();
            InputBox.Text = "";

            if (string.IsNullOrWhiteSpace(input)) return;

            AddMessage($"> {input}", Brushes.DarkGray);

            if (awaitingName)
            {
                userName = input;
                awaitingName = false;
                AddMessage($"Nice to meet you, {userName}! I'm your Cybersecurity Assistant. I'm here to aid you in matters related to cybersecurity.", Brushes.MediumSeaGreen);
                return;
            }

            // Stop the quiz
            if (input.Equals("stop quiz", StringComparison.OrdinalIgnoreCase))
            {
                cyberQuiz.StopQuiz();
                LogActivity($"Quiz paused by {userName} at question {cyberQuiz.QuestionsAnswered + 1}.");
                return;
            }

            // Continue the quiz
            if (input.Equals("continue quiz", StringComparison.OrdinalIgnoreCase))
            {
                cyberQuiz.ResumeQuiz();
                LogActivity($"Quiz resumed by {userName} at question {cyberQuiz.QuestionsAnswered + 1}.");
                return;
            }

            // If quiz is active, process the answer
            if (cyberQuiz.IsQuizActive)
            {
                cyberQuiz.ProcessAnswer(input);
                LogActivity($"Quiz answer processed by {userName}. Question {cyberQuiz.QuestionsAnswered}.");
                return;
            }

            // Reminder after task
            if (pendingTask != null && input.StartsWith("remind me in ", StringComparison.OrdinalIgnoreCase))
            {
                ProcessReminderInput(input);
                return;
            }

            if (TryProcessExplicitTaskCommand(input))
            {
                return;
            }

            // Show user action history
            if (input.Equals("what have you done for me", StringComparison.OrdinalIgnoreCase))
            {
                ShowUserActions();
                return;
            }

            // Show activity log preview
            if (input.Equals("show activity log", StringComparison.OrdinalIgnoreCase))
            {
                isShowingFullActivity = false;
                ShowActivityLog();
                return;
            }

            // Show full log if "show more"
            if (input.Equals("show more", StringComparison.OrdinalIgnoreCase) && activityLog.Count > 7)
            {
                isShowingFullActivity = true;
                ShowActivityLog();
                return;
            }

            // NLP-based commands
            var nlpResult = nlpProcessor.AnalyzeInput(input);

            switch (nlpResult.Intent)
            {
                case NLPProcessor.NlpIntent.SetReminder:
                    HandleDirectReminder(nlpResult.Title);
                    return;

                case NLPProcessor.NlpIntent.AddTask:
                    HandleAddTask(nlpResult.Title);
                    return;

                case NLPProcessor.NlpIntent.ViewTasks:
                    HandleViewTasks();
                    return;

                case NLPProcessor.NlpIntent.CompleteTask:
                    HandleCompleteTask(nlpResult.Title);
                    return;

                case NLPProcessor.NlpIntent.DeleteTask:
                    HandleDeleteTask(nlpResult.Title);
                    return;

                case NLPProcessor.NlpIntent.StartQuiz:
                    cyberQuiz.StartQuiz();
                    userActions.Add($"Quiz started by {userName}.");
                    LogActivity($"Quiz started by {userName}.");
                    return;

                case NLPProcessor.NlpIntent.ShowHelp:
                    HelpMenu.Show(AddMessage);
                    LogActivity($"Help menu shown to {userName}.");
                    return;

                case NLPProcessor.NlpIntent.Exit:
                    Application.Current.Shutdown();
                    return;
            }

            // If no match, pass to chatbot
            chatbot.ProcessInput(input);
        }


        private bool TryProcessExplicitTaskCommand(string input)
        {
            if (input.StartsWith("complete task ", StringComparison.OrdinalIgnoreCase))
            {
                string title = input.Substring("complete task ".Length).Trim();
                HandleCompleteTask(title);
                return true;
            }

            if (input.StartsWith("delete task ", StringComparison.OrdinalIgnoreCase))
            {
                string title = input.Substring("delete task ".Length).Trim();
                HandleDeleteTask(title);
                return true;
            }

            if (input.StartsWith("add task ", StringComparison.OrdinalIgnoreCase))
            {
                string title = input.Substring("add task ".Length).Trim();
                HandleAddTask(title);
                return true;
            }

            if (input.Equals("view tasks", StringComparison.OrdinalIgnoreCase))
            {
                HandleViewTasks();
                return true;
            }

            return false;
        }

        private void HandleAddTask(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                AddMessage($"{userName}, please specify what task you'd like to add.", Brushes.Red);
                return;
            }

            var task = new CyberTask
            {
                Title = title,
                Description = $"'{title}' has been added to strengthen your cybersecurity.",
                IsCompleted = false
            };

            tasks.Add(task);
            pendingTask = task;

            AddMessage($"Thanks, {userName}. Task added: \"{task.Description}\". Would you like a reminder? (e.g., 'Remind me in 3 days')", Brushes.Green);
            userActions.Add($"Task added: '{task.Title}' by {userName}");
            LogActivity($"Task added by {userName}: '{task.Title}'");
        }

        private void HandleDirectReminder(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                AddMessage($"{userName}, please specify what you'd like me to remind you about.", Brushes.Red);
                return;
            }

            DateTime reminderDate;
            if (title.Contains("tomorrow"))
            {
                reminderDate = DateTime.Now.AddDays(1);
                title = title.Replace("tomorrow", "").Trim();
            }
            else if (TryExtractDays(title, out int days, out string cleanTitle))
            {
                reminderDate = DateTime.Now.AddDays(days);
                title = cleanTitle;
            }
            else
            {
                reminderDate = DateTime.Now.AddDays(1);
            }

            var reminder = new CyberTask
            {
                Title = title,
                Description = $"Reminder: {title}",
                ReminderDate = reminderDate,
                IsCompleted = false
            };

            tasks.Add(reminder);
            AddMessage($"{userName}, your reminder for '{title}' has been set for {reminderDate:dd MMM yyyy}.", Brushes.SteelBlue);
            userActions.Add($"Reminder set for '{title}' on {reminderDate:dd MMM yyyy}");
            LogActivity($"Reminder created by {userName}: '{title}' for {reminderDate:dd MMM yyyy}");
        }

        private void ProcessReminderInput(string input)
        {
            if (TryParseReminder(input, out DateTime reminder))
            {
                pendingTask.ReminderDate = reminder;
                AddMessage($"Got it, {userName}! I'll remind you on {reminder:dd MMM yyyy}.", Brushes.Blue);
                userActions.Add($"Reminder set for '{pendingTask.Title}' on {reminder:dd MMM yyyy}");
                LogActivity($"Reminder added by {userName} for existing task '{pendingTask.Title}' on {reminder:dd MMM yyyy}");
            }
            else
            {
                AddMessage($"{userName}, I couldn't understand the reminder format. Try: 'Remind me in 3 days'", Brushes.Red);
            }

            pendingTask = null;
        }

        private void HandleViewTasks()
        {
            if (tasks.Count == 0)
            {
                AddMessage($"{userName}, you have no tasks. Try: 'Add task - Check password settings'", Brushes.Gray);
            }
            else
            {
                AddMessage($"Here are your tasks, {userName}:", Brushes.DarkGreen);
                foreach (var task in tasks)
                {
                    AddMessage(task.ToString(), task.IsCompleted ? Brushes.Gray : Brushes.DarkBlue);
                }
            }
        }

        private void HandleCompleteTask(string title)
        {
            var task = tasks.Find(t => t.Title.Trim().Equals(title, StringComparison.OrdinalIgnoreCase));
            if (task != null)
            {
                task.IsCompleted = true;
                AddMessage($"Great job, {userName}! Task \"{task.Title}\" marked as completed.", Brushes.Green);
                LogActivity($"Task marked completed by {userName}: '{task.Title}'");
            }
            else
            {
                AddMessage($"Sorry {userName}, no task found with title \"{title}\".", Brushes.Red);
            }
        }

        private void HandleDeleteTask(string title)
        {
            var task = tasks.Find(t => t.Title.Trim().Equals(title, StringComparison.OrdinalIgnoreCase));
            if (task != null)
            {
                tasks.Remove(task);
                AddMessage($"Okay {userName}, task \"{task.Title}\" deleted.", Brushes.Red);
                LogActivity($"Task deleted by {userName}: '{task.Title}'");
            }
            else
            {
                AddMessage($"No task found with title \"{title}\", {userName}.", Brushes.Red);
            }
        }

        private bool TryParseReminder(string input, out DateTime reminderDate)
        {
            reminderDate = DateTime.MinValue;
            input = input.ToLower().Trim().Replace(".", "").Replace("?", "");

            if (!input.StartsWith("remind me in")) return false;

            string numberPart = input.Replace("remind me in", "")
                                     .Replace("days", "")
                                     .Replace("day", "")
                                     .Trim();

            return int.TryParse(numberPart, out int days) &&
                   (reminderDate = DateTime.Now.AddDays(days)) != DateTime.MinValue;
        }

        private bool TryExtractDays(string input, out int days, out string cleanTitle)
        {
            days = 0;
            cleanTitle = input;

            string lower = input.ToLower();
            int index = lower.IndexOf("in ");
            if (index != -1)
            {
                string afterIn = lower.Substring(index + 3);
                string[] parts = afterIn.Split(' ');
                if (parts.Length > 0 && int.TryParse(parts[0], out days))
                {
                    cleanTitle = lower.Replace($"in {days} days", "")
                                      .Replace($"in {days} day", "")
                                      .Replace($"in {days}", "").Trim();
                    return true;
                }
            }

            return false;
        }

        private void LogActivity(string description)
        {
            string logEntry = $"{description}";
            activityLog.Enqueue(logEntry);

            if (activityLog.Count > 50)
            {
                activityLog.Dequeue();
            }
        }

        private void ShowActivityLog()
        {
            if (activityLog.Count == 0)
            {
                AddMessage($"{userName}, your activity log is currently empty.", Brushes.Gray);
                return;
            }

            // Show quiz progress if relevant
            if (cyberQuiz.QuestionsAnswered > 0 && cyberQuiz.QuestionsAnswered < cyberQuiz.Questions.Count)
            {
                AddMessage($"{userName}, you have answered {cyberQuiz.QuestionsAnswered} out of {cyberQuiz.Questions.Count} quiz questions so far.", Brushes.Teal);
            }

            AddMessage($"Here are your {(isShowingFullActivity ? "complete" : "recent")} activities, {userName}:", Brushes.Orange);

            var logArray = activityLog.ToArray();
            int countToShow = isShowingFullActivity ? logArray.Length : Math.Min(7, logArray.Length);

            for (int i = 0; i < countToShow; i++)
            {
                AddMessage($"{i + 1}. {logArray[i]}", Brushes.SlateBlue);
            }

            if (!isShowingFullActivity && activityLog.Count > 7)
            {
                AddMessage("Type 'show more' to see the full activity history.", Brushes.DarkBlue);
            }
        }

        private void ShowUserActions()
        {
            if (userActions.Count == 0)
            {
                AddMessage($"{userName}, I haven't done anything for you yet. Try adding a task or setting a reminder.", Brushes.Gray);
            }
            else
            {
                AddMessage($"Here's what I've done for you recently, {userName}:", Brushes.DarkOrange);
                int count = 1;
                for (int i = userActions.Count - 1; i >= 0 && count <= 5; i--, count++)
                {
                    AddMessage($"{count}. {userActions[i]}", Brushes.MediumVioletRed);
                }
            }
        }

        private void AddMessage(string message, Brush color = null)
        {
            Dispatcher.Invoke(() =>
            {
                var textBlock = new TextBlock
                {
                    Text = message,
                    Foreground = color ?? Brushes.Black,
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(0, 5, 0, 0)
                };
                ChatPanel.Children.Add(textBlock);
                ChatScrollViewer.ScrollToEnd();
            });
        }
    }
}
