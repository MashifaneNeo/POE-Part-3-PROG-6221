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

            if (input.StartsWith("add task") || input.StartsWith("create task") || input.StartsWith("set task"))
            {
                string title = input.Substring(input.IndexOf("task") + 4).Trim(new[] { '-', ' ', ':' });
                return new NlpResult { Intent = NlpIntent.AddTask, Title = title };
            }

            if (input.StartsWith("remind me to") || input.StartsWith("set reminder to") || input.StartsWith("reminder to"))
            {
                string title = input.Substring(input.IndexOf("to") + 2).Trim();
                return new NlpResult { Intent = NlpIntent.SetReminder, Title = title };
            }

            if (input.StartsWith("view tasks") || input.StartsWith("show tasks") || input.StartsWith("list tasks"))
            {
                return new NlpResult { Intent = NlpIntent.ViewTasks };
            }

            if (input.StartsWith("complete task") || input.StartsWith("finish task") || input.StartsWith("mark task"))
            {
                string title = input.Substring(input.IndexOf("task") + 4).Trim();
                return new NlpResult { Intent = NlpIntent.CompleteTask, Title = title };
            }

            if (input.StartsWith("delete task") || input.StartsWith("remove task") || input.StartsWith("cancel task"))
            {
                string title = input.Substring(input.IndexOf("task") + 4).Trim();
                return new NlpResult { Intent = NlpIntent.DeleteTask, Title = title };
            }

            if (input.Contains("start quiz") || input.Contains("begin quiz"))
            {
                return new NlpResult { Intent = NlpIntent.StartQuiz };
            }

            if (input == "help" || input == "show help")
            {
                return new NlpResult { Intent = NlpIntent.ShowHelp };
            }

            if (input == "exit" || input == "quit" || input == "close")
            {
                return new NlpResult { Intent = NlpIntent.Exit };
            }

            return new NlpResult { Intent = NlpIntent.None };
        }
    }
}
