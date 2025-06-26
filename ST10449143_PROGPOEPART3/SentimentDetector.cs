using System;
using System.Windows.Media;

namespace ST10449143_PROGPOEPART3
{
    public static class SentimentDetector
    {
        public static void DetectSentiment(string input, Action<string, Brush> output)
        {
            if (input.Contains("worried"))
                output("It's okay to feel worried. Here's something that might help:", Brushes.Magenta);
            else if (input.Contains("curious"))
                output("Curiosity is great! Here's something to fuel it:", Brushes.Yellow);
            else if (input.Contains("frustrated"))
                output("Cybersecurity can be frustrating. Here's a helpful tip:", Brushes.Blue);
            else if (input.Contains("thankful") || input.Contains("thanks") || input.Contains("thank you"))
                output("Glad to hear that! Here's another tip you might like:", Brushes.Green);
            else if (input.Contains("anxious"))
                output("Learning helps with anxiety. Here's something reassuring:", Brushes.DarkGoldenrod);
            else if (input.Contains("excited"))
                output("Love your excitement! Here's more to keep it up:", Brushes.Gray);
            else if (input.Contains("scared"))
                output("Fear is normal in cybersecurity. Here's something to ease it:", Brushes.Cyan);
            else if (input.Contains("uninterested"))
                output("That’s okay. Maybe this tip can reignite your interest:", Brushes.Red);
            else if (input.Contains("unsure"))
                output("Perfectly normal to feel unsure. Let's learn together:", Brushes.DarkGreen);
        }
    }
}
