using System;
using System.Collections.Generic;

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
            input = input.ToLower().Trim().Replace(".", "").Replace("?", "");

            // Prioritize task management commands
            if (IsTaskCommand(input, out var taskResult)) return taskResult;

            // Then check for other commands
            if (IsGeneralCommand(input, out var generalResult)) return generalResult;

            return new NlpResult { Intent = NlpIntent.None };
        }

        private bool IsTaskCommand(string input, out NlpResult result)
        {
            result = new NlpResult { Intent = NlpIntent.None };

            // Reminder command should come BEFORE AddTask
            if (input.StartsWith("remind me to") || input.StartsWith("set a reminder to"))
            {
                result.Intent = NlpIntent.SetReminder;
                result.Title = ExtractAfter(input, new[] { "remind me to", "set a reminder to" });
                return true;
            }

            // Delete task commands (highest priority for task management)
            if (input.Contains("delete task") || input.Contains("remove task") ||
                input.Contains("erase task") || input.Contains("cancel task"))
            {
                result.Intent = NlpIntent.DeleteTask;
                result.Title = ExtractAfter(input, new[] { "delete task", "remove task", "erase task", "cancel task" });
                return true;
            }

            // Complete task commands
            if (input.Contains("complete task") || input.Contains("mark task") ||
                input.Contains("finish task") || input.Contains("task done"))
            {
                result.Intent = NlpIntent.CompleteTask;
                result.Title = ExtractAfter(input, new[] { "complete task", "mark task", "finish task", "task done" });
                return true;
            }

            // Add task commands
            if (input.Contains("add task") || input.Contains("create task") ||
                input.Contains("new task") || input.Contains("set task") ||
                input.Contains("remind me to") || input.Contains("set a reminder to"))
            {
                result.Intent = NlpIntent.AddTask;
                result.Title = ExtractAfter(input, new[] { "add task", "create task", "new task", "set task", "remind me to", "set a reminder to" });
                return true;
            }

            // View tasks commands
            if (input.Contains("view tasks") || input.Contains("show tasks") ||
                input.Contains("list tasks") || input.Contains("my tasks"))
            {
                result.Intent = NlpIntent.ViewTasks;
                return true;
            }

            return false;
        }

        private bool IsGeneralCommand(string input, out NlpResult result)
        {
            result = new NlpResult { Intent = NlpIntent.None };

            if (input.Contains("start quiz") || input.Contains("begin quiz") || input.Contains("quiz time"))
            {
                result.Intent = NlpIntent.StartQuiz;
                return true;
            }

            if (input.Contains("help") || input.Contains("what can i ask") || input.Contains("show help"))
            {
                result.Intent = NlpIntent.ShowHelp;
                return true;
            }

            if (input.Contains("exit") || input.Contains("quit") || input.Contains("close bot"))
            {
                result.Intent = NlpIntent.Exit;
                return true;
            }

            return false;
        }

        private string ExtractAfter(string input, string[] phrases)
        {
            foreach (var phrase in phrases)
            {
                if (input.Contains(phrase))
                {
                    int index = input.IndexOf(phrase);
                    return input.Substring(index + phrase.Length).Trim();
                }
            }
            return string.Empty;
        }
    }
}