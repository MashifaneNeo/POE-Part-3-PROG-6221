using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows;

namespace ST10449143_PROGPOEPART3
{
    public class PreviousWork
    {
        private readonly Dictionary<string, string[]> topicTips = DictionaryList.TopicTips;
        private readonly Dictionary<string, string> keywordToTopic = DictionaryList.KeywordToTopic;

        private readonly Dictionary<string, Queue<int>> shownTipIndices = new Dictionary<string, Queue<int>>();
        private readonly Dictionary<string, int> currentTipIndex = new Dictionary<string, int>();
        private readonly Random random = new Random();

        private string lastTopic = "";
        private string favoriteTopic = "";
        private readonly string userName;

        // Delegate to print messages in the GUI
        private readonly Action<string, Brush> output;

        public PreviousWork(string userName, Action<string, Brush> output)
        {
            this.userName = userName;
            this.output = output;

            foreach (var topic in topicTips.Keys)
            {
                shownTipIndices[topic] = new Queue<int>();
                currentTipIndex[topic] = 0;
            }

            output($"Hello, {userName}! Ask me something about cybersecurity.", Brushes.DarkCyan);
        }

        public void ProcessInput(string input)
        {
            input = input.ToLower().Trim();
            output($"> {input}", Brushes.DarkGray);

            SentimentDetector.DetectSentiment(input, output);

            if (TryRememberUserInfo(input)) return;

            if ((input.Contains("more") || input.Contains("another") || input.Contains("again") ||
                input.Contains("explain") || input.Contains("confused")) && !string.IsNullOrEmpty(lastTopic))
            {
                output(GenerateFollowUpResponse(), Brushes.DarkCyan);
                ProvideTipByTopic(lastTopic);
                return;
            }

            string detectedTopic = DetectTopic(input);

            if (detectedTopic == "exit")
            {
                output($"Goodbye, {userName}! Stay safe online!", Brushes.Green);
                Application.Current.Shutdown();
                return;
            }

            if (detectedTopic == "help")
            {
                HelpMenu.Show(output);
                return;
            }

            if (!string.IsNullOrEmpty(detectedTopic) && topicTips.ContainsKey(detectedTopic))
            {
                lastTopic = detectedTopic;
                ProvideTipByTopic(detectedTopic);
            }
            else
            {
                output($"Sorry {userName}, I didn't understand that. Please ask about a different topic.", Brushes.Red);
            }
        }

        private void ProvideTipByTopic(string topic)
        {
            var tips = topicTips[topic];

            if (shownTipIndices[topic].Count >= tips.Length)
                shownTipIndices[topic].Clear();

            int tipIndex;
            do
            {
                tipIndex = currentTipIndex[topic];
                currentTipIndex[topic] = (currentTipIndex[topic] + 1) % tips.Length;
            }
            while (shownTipIndices[topic].Contains(tipIndex));

            shownTipIndices[topic].Enqueue(tipIndex);
            string tip = tips[tipIndex];

            if (!string.IsNullOrEmpty(favoriteTopic) && topic == favoriteTopic)
            {
                output($"Since you're interested in {favoriteTopic}, here’s a helpful tip to deepen your knowledge:", Brushes.DarkRed);
            }

            Brush[] tipColors = { Brushes.Cyan, Brushes.Blue, Brushes.Magenta, Brushes.Black };
            output(tip, tipColors[random.Next(tipColors.Length)]);
        }

        private string DetectTopic(string input)
        {
            foreach (var pair in keywordToTopic)
            {
                if (input.Contains(pair.Key))
                    return pair.Value;
            }

            if (input.Contains("exit") || input.Contains("quit"))
                return "exit";

            if (input.Contains("help") || input.Contains("what can i ask"))
                return "help";

            return null;
        }

        private bool TryRememberUserInfo(string input)
        {
            string[] phrases = { "i'm interested in ", "i am interested in ", "im interested in " };
            foreach (var phrase in phrases)
            {
                if (input.StartsWith(phrase))
                {
                    string topic = input.Replace(phrase, "").Trim();
                    if (topicTips.ContainsKey(topic))
                    {
                        favoriteTopic = topic;
                        output($"Great! I'll remember that you're interested in {favoriteTopic}.", Brushes.DarkRed);
                        return true;
                    }
                }
            }
            return false;
        }

        private string GenerateFollowUpResponse()
        {
            string[] responses =
            {
                $"Certainly, {userName}. Here's some more information to help you:",
                $"Absolutely! Let me explain further with this tip:",
                $"That’s understandable, {userName}. This should help clarify things:",
                $"Here’s another helpful tip for you, {userName}:",
                $"No problem! Let’s keep learning about this topic:"
            };

            return responses[random.Next(responses.Length)];
        }
    }
}
