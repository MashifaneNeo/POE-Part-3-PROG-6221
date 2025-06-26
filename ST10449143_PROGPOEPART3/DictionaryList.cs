using System.Collections.Generic;

namespace ST10449143_PROGPOEPART3
{
    public static class DictionaryList
    {
        public static Dictionary<string, string[]> TopicTips = new Dictionary<string, string[]>
        {
            { "passwords", new[] {
                "Use a combination of uppercase, lowercase, numbers, and symbols.",
                "Never reuse the same password across multiple platforms.",
                "Consider using a password manager to generate and store complex passwords."
            }},
            { "phishing", new[] {
                "Avoid clicking on suspicious links in emails or messages.",
                "Verify the sender's email address before responding.",
                "Look for spelling mistakes or urgent language — common signs of phishing."
            }},
            { "malware", new[] {
                "Keep your software and antivirus up to date.",
                "Avoid downloading software from untrusted sources.",
                "Regularly scan your computer for malware."
            }}
            // Add more topics here
        };

        public static Dictionary<string, string> KeywordToTopic = new Dictionary<string, string>
        {
            { "password", "passwords" },
            { "phishing", "phishing" },
            { "malware", "malware" }
            // Add more keywords as needed
        };
    }
}
