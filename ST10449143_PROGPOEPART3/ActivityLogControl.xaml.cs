using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ST10449143_PROGPOEPART3
{
    public partial class ActivityLogControl : UserControl
    {
        private List<string> fullLog = new List<string>();
        private bool showingAll = false;

        public ActivityLogControl()
        {
            InitializeComponent();
        }

        // Called by MainWindow to update the log
        public void RefreshLog(IEnumerable<string> logEntries)
        {
            fullLog = logEntries.ToList();
            DisplayLog();
        }

        private void DisplayLog()
        {
            LogListView.Items.Clear();

            var logToShow = showingAll ? fullLog : fullLog.Skip(Math.Max(0, fullLog.Count - 7));

            foreach (var entry in logToShow)
            {
                var parts = entry.Split(new[] { " - " }, 2, StringSplitOptions.None);
                LogListView.Items.Add(new
                {
                    Timestamp = parts[0],
                    Message = parts.Length > 1 ? parts[1] : string.Empty
                });
            }

            // Update button label
            ShowMoreButton.Content = showingAll ? "Show Less" : "Show More";
        }

        private void ShowMoreButton_Click(object sender, RoutedEventArgs e)
        {
            showingAll = !showingAll;
            DisplayLog();
        }
    }
}
