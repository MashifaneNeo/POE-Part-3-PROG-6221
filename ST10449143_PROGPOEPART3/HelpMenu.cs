using System;
using System.Windows.Media;

namespace ST10449143_PROGPOEPART3
{
    public static class HelpMenu
    {
        public static void Show(Action<string, Brush> output)
        {
            Brush green = Brushes.DarkGreen;
            output("╔═════════════════════════════════════════════════════════════════════╗", green);
            output("║ You can ask me about the following cybersecurity topics:            ║", green);
            output("║  - password, phishing, scam, privacy, encryption                    ║", green);
            output("║  - firewall, antivirus, backup, two-factor authentication           ║", green);
            output("║  - malware, vpn                                                     ║", green);
            output("║                                                                     ║", green);
            output("║ You can also tell me:                                               ║", green);
            output("║  - How you feel about Cybersecurity like:                           ║", green);
            output("║    worried, curious, frustrated, confused,                          ║", green);
            output("║    thankful, anxious, excited, scared,                              ║", green);
            output("║    uninterested and unsure                                          ║", green);
            output("║  - Cybersecurity topics you're interested in                        ║", green);
            output("║  - 'more', 'another', or 'explain' to get another tip on a topic    ║", green);
            output("║  - 'exit' or 'quit' to close the chatbot                            ║", green);
            output("╚═════════════════════════════════════════════════════════════════════╝", green);
        }
    }
}
