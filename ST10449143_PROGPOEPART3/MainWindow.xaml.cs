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

        private PreviousWork chatbot;
        private List<CyberTask> tasks = new List<CyberTask>();
        private CyberTask pendingTask = null;
        private CyberSecurityQuiz cyberQuiz;
        private NLPProcessor nlpProcessor;

        private List<string> userActions = new List<string>(); // General summary
        private Queue<string> activityLog = new Queue<string>(); // Detailed log with timestamps

        public MainWindow()
        {
            InitializeComponent();
            InitializeApplicationComponents();
        }

        private void InitializeApplicationComponents()
        {
            chatbot = new PreviousWork("User", AddMessage);
            cyberQuiz = new CyberSecurityQuiz(AddMessage);
            nlpProcessor = new NLPProcessor();
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string input = InputBox.Text.Trim();
            InputBox.Text = "";

            if (string.IsNullOrWhiteSpace(input)) return;

            AddMessage($"> {input}", Brushes.DarkGray);

            if (cyberQuiz.IsQuizActive)
            {
                cyberQuiz.ProcessAnswer(input);
                LogActivity("Quiz answer processed.");
                return;
            }

            if (pendingTask != null && input.StartsWith("remind me in ", StringComparison.OrdinalIgnoreCase))
            {
                ProcessReminderInput(input);
                return;
            }

            if (TryProcessExplicitTaskCommand(input))
            {
                return;
            }

            if (input.ToLower().Contains("what have you done for me"))
            {
                ShowUserActions();
                return;
            }

            if (input.ToLower().Contains("show activity log"))
            {
                ShowActivityLog();
                return;
            }

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
                    userActions.Add("Quiz started.");
                    LogActivity("Quiz started.");
                    return;

                case NLPProcessor.NlpIntent.ShowHelp:
                    HelpMenu.Show(AddMessage);
                    LogActivity("Help menu shown.");
                    return;

                case NLPProcessor.NlpIntent.Exit:
                    Application.Current.Shutdown();
                    return;
            }

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
                AddMessage("Please specify what task you'd like to add.", Brushes.Red);
                return;
            }

            var task = new CyberTask
            {
                Title = title,
                Description = $" '{title}' has been add" +
                $" to ensure the intergrity of your cybersecurity",
                IsCompleted = false
            };
             
            tasks.Add(task);
            pendingTask = task;

            AddMessage($"Task added: \"{task.Description}\". Would you like a reminder? (e.g., 'Remind me in 3 days')", Brushes.Green);
            userActions.Add($"Task added: '{task.Title}' (no reminder set)");
            LogActivity($"Task added: '{task.Title}'");
        }

        private void HandleDirectReminder(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                AddMessage("Please specify what you'd like me to remind you about.", Brushes.Red);
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
            AddMessage($"Reminder set for '{title}' on {reminderDate:dd MMM yyyy}.", Brushes.SteelBlue);
            userActions.Add($"Reminder set for '{title}' on {reminderDate:dd MMM yyyy}");
            LogActivity($"Reminder created: '{title}' for {reminderDate:dd MMM yyyy}");
        }

        private void ProcessReminderInput(string input)
        {
            if (TryParseReminder(input, out DateTime reminder))
            {
                pendingTask.ReminderDate = reminder;
                AddMessage($"Got it! I'll remind you on {reminder:dd MMM yyyy}.", Brushes.Blue);
                userActions.Add($"Reminder set for '{pendingTask.Title}' on {reminder:dd MMM yyyy}");
                LogActivity($"Reminder added for existing task '{pendingTask.Title}' on {reminder:dd MMM yyyy}");
            }
            else
            {
                AddMessage("Sorry, I couldn't understand the reminder format. Try: 'Remind me in 3 days'", Brushes.Red);
            }

            pendingTask = null;
        }

        private void HandleViewTasks()
        {
            if (tasks.Count == 0)
            {
                AddMessage("You have no tasks. Try: 'Add task - Check password settings'", Brushes.Gray);
            }
            else
            {
                AddMessage("Here are your tasks:", Brushes.DarkGreen);
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
                AddMessage($"Task \"{task.Title}\" marked as completed.", Brushes.Green);
                LogActivity($"Task marked completed: '{task.Title}'");
            }
            else
            {
                AddMessage($"No task found with title \"{title}\".", Brushes.Red);
            }
        }

        private void HandleDeleteTask(string title)
        {
            var task = tasks.Find(t => t.Title.Trim().Equals(title, StringComparison.OrdinalIgnoreCase));
            if (task != null)
            {
                tasks.Remove(task);
                AddMessage($"Task \"{task.Title}\" deleted.", Brushes.Red);
                LogActivity($"Task deleted: '{task.Title}'");
            }
            else
            {
                AddMessage($"No task found with title \"{title}\".", Brushes.Red);
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
                activityLog.Dequeue(); // keep log concise
            }
        }

        private void ShowActivityLog()
        {
            if (activityLog.Count == 0)
            {
                AddMessage("Your activity log is currently empty.", Brushes.Gray);
                return;
            }

            AddMessage("Here's a summary of your recent actions:", Brushes.Orange);
            int count = 1;
            foreach (string entry in activityLog)
            {
                if (count > 5) break;
                AddMessage($"{count++}. {entry}", Brushes.SlateBlue);
            }
        }

        private void ShowUserActions()
        {
            if (userActions.Count == 0)
            {
                AddMessage("I haven't done anything for you yet. Try adding a task or setting a reminder.", Brushes.Gray);
            }
            else
            {
                AddMessage("Here's a summary of your requests:", Brushes.DarkOrange);
                int count = 1;
                foreach (var action in userActions)
                {
                    AddMessage($"{count++}. {action}", Brushes.MediumVioletRed);
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
