using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace ST10449143_PROGPOEPART3
{
    public partial class MainWindow : Window
    {
        public static string CurrentUserName { get; private set; } = "Guest";

        private TaskAssistantControl taskAssistantControl;
        private CyberSecurityQuiz cyberQuiz;
        private CyberSecurityQuizControl quizControl;
        private ActivityLogControl activityLogControl;
        private DispatcherTimer clockTimer;

        private Queue<string> activityLog = new Queue<string>();

        public MainWindow()
        {
            InitializeComponent();
            ShowWelcomeDialog();              // Launch welcome dialog first
            InitializeApplicationComponents();
            AddActivityLogEntry($"User '{CurrentUserName}' started the application");
        }

        private void ShowWelcomeDialog()
        {
            var welcomeDialog = new WelcomeDialog();
            bool? result = welcomeDialog.ShowDialog();

            if (result == true)
            {
                CurrentUserName = welcomeDialog.UserName;
            }
            else
            {
                Close(); // Exit if user cancels or closes the dialog
            }
        }

        private void InitializeApplicationComponents()
        {
            taskAssistantControl = new TaskAssistantControl();
            TaskAssistantTab.Content = taskAssistantControl;

            cyberQuiz = new CyberSecurityQuiz(AddMessage);
            quizControl = new CyberSecurityQuizControl();
            QuizTab.Content = quizControl;

            activityLogControl = new ActivityLogControl();
            ActivityLogTab.Content = activityLogControl;

            UpdateStatusBar();
        }

        // Add entries to the activity log queue and refresh UI
        private void AddActivityLogEntry(string entry)
        {
            activityLog.Enqueue($"{DateTime.Now:HH:mm:ss} - {entry}");

            // Keep only last 50 entries
            if (activityLog.Count > 50)
                activityLog.Dequeue();

            // Refresh ActivityLogControl
            activityLogControl.RefreshLog(activityLog);
            UpdateStatusBar();
        }

        private void UpdateStatusBar()
        {
            Dispatcher.Invoke(() =>
            {
                StatusText.Text = $"User: {CurrentUserName} | Tasks: {taskAssistantControl?.TasksCount() ?? 0} | Quiz Score: {cyberQuiz?.Score ?? 0}/{cyberQuiz?.Questions.Count ?? 0}";
            });
        }

        private void MainTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl && MainTabControl.SelectedItem == ActivityLogTab)
            {
                activityLogControl.RefreshLog(activityLog);
            }
        }

        private void OpenNLPProcessor_Click(object sender, RoutedEventArgs e)
        {
            var nlpWindow = new NLPProcessorWindow();
            nlpWindow.Owner = this;
            nlpWindow.Show();
        }

        private void StartClock()
        {
            clockTimer = new DispatcherTimer();
            clockTimer.Interval = TimeSpan.FromSeconds(1);
            clockTimer.Tick += (s, e) =>
            {
                TimeText.Text = $"Last update: {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
            };
            clockTimer.Start();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to exit?", "Exit Confirmation",
                                         MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes)
            {
                e.Cancel = true; // Cancel close
            }
        }


        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddMessage(string message, System.Windows.Media.Brush color)
        {
            Dispatcher.Invoke(() =>
            {
                StatusText.Text = message;
            });
        }
    }
}
