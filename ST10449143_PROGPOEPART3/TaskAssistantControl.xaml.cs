using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualBasic;

namespace ST10449143_PROGPOEPART3
{
    public partial class TaskAssistantControl : UserControl
    {
        private ObservableCollection<CyberTask> tasks = new ObservableCollection<CyberTask>();        

        public TaskAssistantControl()
        {
            InitializeComponent();
            TasksListView.ItemsSource = tasks; // Bind directly
            UpdateTaskSummary();
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TaskInput.Text))
            {
                MessageBox.Show("Please enter a task description", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var task = new CyberTask
            {
                Title = TaskInput.Text,
                Description = $"Task added on {DateTime.Now:dd MMM yyyy}",
                IsCompleted = false
            };

            // Ask user if they want to add a reminder
            var reminderPrompt = MessageBox.Show("Would you like to set a reminder for this task?", "Set Reminder", MessageBoxButton.YesNo);
            if (reminderPrompt == MessageBoxResult.Yes)
            {
                var input = Microsoft.VisualBasic.Interaction.InputBox("Enter number of days for the reminder:", "Reminder in Days", "3");
                if (int.TryParse(input, out int days))
                {
                    task.ReminderDate = DateTime.Now.AddDays(days);
                }
            }

            tasks.Add(task);
            TaskInput.Clear();
            SetReminderCheck.IsChecked = false;
            ReminderDatePicker.SelectedDate = null;
            UpdateTaskSummary();

            // Log activity
            ((MainWindow)Application.Current.MainWindow)?.LogActivity($"Task added: {task.Title} (Reminder: {task.ReminderDate?.ToShortDateString() ?? "None"})");
        }


        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is CyberTask task)
            {
                tasks.Remove(task);
                UpdateTaskSummary();
            }
        }

        private void TaskComplete_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox cb && cb.DataContext is CyberTask task)
            {
                task.IsCompleted = true;
                UpdateTaskSummary();
                // Refresh list to update UI if needed (optional)
                TasksListView.Items.Refresh();
            }
        }

        private void TaskComplete_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox cb && cb.DataContext is CyberTask task)
            {
                task.IsCompleted = false;
                UpdateTaskSummary();
                TasksListView.Items.Refresh();
            }
        }

        private void UpdateTaskSummary()
        {
            TotalTasksCount.Text = tasks.Count.ToString();
            CompletedTasksCount.Text = tasks.Count(t => t.IsCompleted).ToString();
            PendingTasksCount.Text = tasks.Count(t => !t.IsCompleted).ToString();
            ReminderTasksCount.Text = tasks.Count(t => t.ReminderDate.HasValue).ToString();
        }

        public int TasksCount()
        {
            return tasks.Count;
        }

        /// <summary>
        /// Allows chatbot-style interaction with tasks
        /// </summary>
        public string HandleChatCommand(string input)
        {
            input = input.ToLower().Trim();

            if (input.StartsWith("add task -"))
            {
                var title = input.Substring("add task -".Length).Trim();

                var newTask = new CyberTask
                {
                    Title = title,
                    Description = $"Task: {title}. Make sure this helps improve your cybersecurity habits.",
                    IsCompleted = false
                };

                tasks.Add(newTask);
                UpdateTaskSummary();
                return $"Task added with the description \"{newTask.Description}\". Would you like a reminder?";
            }

            if (input.StartsWith("yes, remind me in"))
            {
                var parts = input.Replace("yes, remind me in", "").Trim().Split(' ');
                if (parts.Length >= 2 && int.TryParse(parts[0], out int numberOfDays))
                {
                    var lastTask = tasks.LastOrDefault();
                    if (lastTask != null)
                    {
                        lastTask.ReminderDate = DateTime.Now.AddDays(numberOfDays);
                        UpdateTaskSummary();
                        return $"Got it! I’ll remind you in {numberOfDays} day(s), on {lastTask.ReminderDate:dd MMM yyyy}.";
                    }
                }

                return "Sorry, I couldn't understand the number of days for the reminder.";
            }

            if (input.StartsWith("show tasks"))
            {
                if (!tasks.Any())
                    return "You have no tasks right now.";

                var taskSummaries = tasks.Select(t =>
                    $"- {t.Title} ({(t.IsCompleted ? "Completed" : "Pending")})" +
                    (t.ReminderDate.HasValue ? $", Reminder: {t.ReminderDate.Value:dd MMM yyyy}" : "")
                );

                return "Here are your tasks:\n" + string.Join("\n", taskSummaries);
            }

            if (input.StartsWith("delete task "))
            {
                var title = input.Replace("delete task", "").Trim();
                var task = tasks.FirstOrDefault(t => t.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
                if (task != null)
                {
                    tasks.Remove(task);
                    UpdateTaskSummary();
                    return $"Task \"{title}\" has been deleted.";
                }
                return $"Could not find a task with the title \"{title}\".";
            }

            if (input.StartsWith("complete task "))
            {
                var title = input.Replace("complete task", "").Trim();
                var task = tasks.FirstOrDefault(t => t.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
                if (task != null)
                {
                    task.IsCompleted = true;
                    UpdateTaskSummary();
                    TasksListView.Items.Refresh();
                    return $"Marked \"{title}\" as completed.";
                }
                return $"Task \"{title}\" not found.";
            }

            return "Sorry, I didn't understand that command. Try something like \"Add task - Update passwords\".";
        }
    }

   

    public class CyberTask
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? ReminderDate { get; set; }
        public bool IsCompleted { get; set; }

        public override string ToString()
        {
            return $"{Title} - {(IsCompleted ? "Completed" : "Pending")}" +
                   (ReminderDate.HasValue ? $" (Remind on {ReminderDate.Value:dd MMM yyyy})" : "");
        }
    }
}
