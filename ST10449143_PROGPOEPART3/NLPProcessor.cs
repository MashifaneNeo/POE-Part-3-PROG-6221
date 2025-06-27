using System;

namespace ST10449143_PROGPOEPART3
{
    public class NLPProcessor
    {
        public enum NlpIntent
        {
            None,
            AddTask,
            SetReminder,
            ViewTasks,
            CompleteTask,
            DeleteTask,
            StartQuiz,
            ShowHelp,
            Exit
        }

        public class NlpResult
        {
            public NlpIntent Intent { get; set; }
            public string Title { get; set; }
        }

        public NlpResult AnalyzeInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return new NlpResult { Intent = NlpIntent.None };

            input = input.ToLower().Trim();

            // Add Task variations
            if (input.Contains("add task") || input.Contains("create task") || input.Contains("set task") ||
                input.Contains("add a task") || input.Contains("add task to") || input.Contains("add task:"))
            {
                // Extract task title after keyword "task"
                int idx = input.IndexOf("task");
                string title = idx >= 0 && idx + 4 < input.Length ? input.Substring(idx + 4).Trim(new[] { '-', ':', ' ' }) : "";
                return new NlpResult { Intent = NlpIntent.AddTask, Title = title };
            }

            // Set Reminder variations
            if (input.Contains("remind me to") || input.Contains("set a reminder to") || input.Contains("reminder to") ||
                input.Contains("remind me about"))
            {
                int idx = input.IndexOf("to");
                string title = idx >= 0 && idx + 2 < input.Length ? input.Substring(idx + 2).Trim() : "";
                return new NlpResult { Intent = NlpIntent.SetReminder, Title = title };
            }

            // View Tasks
            if (input.Contains("view tasks") || input.Contains("show tasks") || input.Contains("list tasks"))
            {
                return new NlpResult { Intent = NlpIntent.ViewTasks };
            }

            // Complete Task
            if (input.Contains("complete task") || input.Contains("finish task") || input.Contains("mark task"))
            {
                int idx = input.IndexOf("task");
                string title = idx >= 0 && idx + 4 < input.Length ? input.Substring(idx + 4).Trim() : "";
                return new NlpResult { Intent = NlpIntent.CompleteTask, Title = title };
            }

            // Delete Task
            if (input.Contains("delete task") || input.Contains("remove task") || input.Contains("cancel task"))
            {
                int idx = input.IndexOf("task");
                string title = idx >= 0 && idx + 4 < input.Length ? input.Substring(idx + 4).Trim() : "";
                return new NlpResult { Intent = NlpIntent.DeleteTask, Title = title };
            }

            // Start Quiz
            if (input.Contains("start quiz") || input.Contains("begin quiz") || input.Contains("take quiz"))
            {
                return new NlpResult { Intent = NlpIntent.StartQuiz };
            }

            // Help
            if (input == "help" || input == "show help")
            {
                return new NlpResult { Intent = NlpIntent.ShowHelp };
            }

            // Exit commands
            if (input == "exit" || input == "quit" || input == "close")
            {
                return new NlpResult { Intent = NlpIntent.Exit };
            }

            // Special case for summary requests
            if (input.Contains("what have you done") || input.Contains("show me what you did") || input.Contains("summary"))
            {
                return new NlpResult { Intent = NlpIntent.None, Title = input };
            }

            return new NlpResult { Intent = NlpIntent.None };
        }
    }
}

