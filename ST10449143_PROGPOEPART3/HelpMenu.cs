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
            output("║ You can ask me about the following cybersecurity topics:                                    ║", green);
            output("║  - password, phishing, scam, privacy, encryption                                       ║", green);
            output("║  - firewall, antivirus, backup, two-factor authentication                              ║", green);
            output("║  - malware, vpn                                                                  ║", green);
            output("║                                                                                     ║", green);
            output("║ You can make use of new features such as                                            ║", green);
            output("║  - Adding tasks                                                          ║", green);
            output("║  - Setting reminders                                                        ║", green);
            output("║  - Taking a quiz to test your Cybersecurity knowledge                              ║", green);
            output("║  - Keep track of your scores and activites                                    ║", green);
            output("║  - Ask the chatbot a wide variety of questions                                       ║", green);            
            output("║  - 'exit' or 'quit' to close the chatbot                                                       ║", green);
            output("╚═════════════════════════════════════════════════════════════════════╝", green);
        }
    }
}
