using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace ST10449143_PROGPOEPART3
{
    public class CyberSecurityQuiz
    {
        public List<QuizQuestion> Questions { get; private set; }
        public int CurrentQuestionIndex { get; private set; }
        public int Score { get; private set; }
        public bool IsQuizActive { get; private set; }
        public bool IsQuizPaused { get; private set; } = false;
        public Action<string, Brush> AddMessage { get; set; }

        public int QuestionsAnswered => CurrentQuestionIndex;

        public CyberSecurityQuiz(Action<string, Brush> addMessageCallback)
        {
            AddMessage = addMessageCallback;
            InitializeQuestions();
        }

        private void InitializeQuestions()
        {
            Questions = new List<QuizQuestion>
            {
                new QuizQuestion(
                    "What is the most secure password practice?",
                    new List<string> { "Using the same password for all accounts", "Using personal information in passwords", "Using long, random passwords with special characters", "Writing passwords on sticky notes" },
                    2,
                    "Long, random passwords with special characters are hardest to guess or crack."
                ),
                new QuizQuestion(
                    "What is phishing?",
                    new List<string> { "A fishing hobby", "A type of malware", "A fraudulent attempt to obtain sensitive information", "A hardware device" },
                    2,
                    "Phishing is when attackers pretend to be trustworthy entities to steal sensitive data."
                ),
                new QuizQuestion(
                    "True or False: Public WiFi networks are always safe to use for banking.",
                    new List<string> { "True", "False" },
                    1,
                    "False. Public WiFi networks are often unsecured and should not be used for sensitive transactions."
                ),
                new QuizQuestion(
                    "What does 2FA stand for?",
                    new List<string> { "Two Factor Authentication", "Two File Access", "Two Form Application", "Two Frequency Amplification" },
                    0,
                    "2FA stands for Two Factor Authentication, which adds an extra layer of security."
                ),
                new QuizQuestion(
                    "What should you do if you receive a suspicious email?",
                    new List<string> { "Click on links to verify", "Reply to the sender", "Delete it without opening", "Forward it to your IT department" },
                    3,
                    "Forward suspicious emails to your IT department for verification."
                ),
                new QuizQuestion(
                    "Which of these is NOT a common characteristic of a strong password?",
                    new List<string> { "At least 12 characters long", "Contains your birth year", "Includes uppercase and lowercase letters", "Contains special characters" },
                    1,
                    "Personal information like birth years should never be used in passwords."
                ),
                new QuizQuestion(
                    "What is the purpose of a VPN?",
                    new List<string> { "To increase internet speed", "To encrypt internet traffic", "To block all ads", "To change your computer's operating system" },
                    1,
                    "VPNs encrypt your internet traffic to protect your privacy."
                ),
                new QuizQuestion(
                    "True or False: You should always open email attachments from unknown senders.",
                    new List<string> { "True", "False" },
                    1,
                    "False. Email attachments from unknown senders may contain malware."
                ),
                new QuizQuestion(
                    "What is social engineering?",
                    new List<string> { "A type of computer virus", "Manipulating people to gain access to confidential information", "A new programming language", "A hardware component" },
                    1,
                    "Social engineering exploits human psychology rather than technical hacking techniques."
                ),
                new QuizQuestion(
                    "How often should you update your software?",
                    new List<string> { "Only when new features are added", "Never", "As soon as updates are available", "Once every 5 years" },
                    2,
                    "Software updates often contain critical security patches that should be installed promptly."
                ),
                new QuizQuestion(
                    "What is ransomware?",
                    new List<string> { "A type of insurance", "Malware that encrypts files until a ransom is paid", "A security protocol", "A backup solution" },
                    1,
                    "Ransomware locks your files and demands payment for their release."
                ),
                new QuizQuestion(
                    "True or False: Using biometric authentication (like fingerprints) is more secure than passwords alone.",
                    new List<string> { "True", "False" },
                    0,
                    "True. Biometrics add an extra layer of security as they're unique to each individual."
                ),
                new QuizQuestion(
                    "What should you do before disposing of an old computer?",
                    new List<string> { "Throw it in the trash", "Sell it as-is", "Perform a factory reset", "Completely wipe the hard drive" },
                    3,
                    "Simply resetting isn't enough - you should securely wipe all data to prevent recovery."
                ),
                new QuizQuestion(
                    "What is a zero-day exploit?",
                    new List<string> { "A 24-hour sale on security software", "An attack that exploits unknown vulnerabilities", "A security protocol", "A type of firewall" },
                    1,
                    "Zero-day exploits target vulnerabilities before developers have had time to patch them."
                ),
                new QuizQuestion(
                    "True or False: HTTPS in a website URL guarantees the site is completely safe.",
                    new List<string> { "True", "False" },
                    1,
                    "False. HTTPS only means the connection is encrypted, not that the site itself is trustworthy."
                ),
                new QuizQuestion(
                    "What is the principle of least privilege?",
                    new List<string> { "Giving users maximum access to all systems", "Only providing the minimum access needed to perform a job", "A type of encryption", "A password policy" },
                    1,
                    "This principle limits potential damage from accidents or malicious actions."
                ),
                new QuizQuestion(
                    "What is the best way to handle multiple online accounts?",
                    new List<string> { "Use the same password for all", "Use a password manager", "Write them down in a notebook", "Memorize them all" },
                    1,
                    "Password managers securely store and generate unique passwords for each account."
                ),
                new QuizQuestion(
                    "True or False: Antivirus software makes you completely immune to all cyber threats.",
                    new List<string> { "True", "False" },
                    1,
                    "False. Antivirus is important but doesn't replace good security practices."
                ),
                new QuizQuestion(
                    "What is a firewall used for?",
                    new List<string> { "To block unauthorized network access", "To speed up internet connection", "To organize files", "To create backups" },
                    0,
                    "Firewalls monitor and control incoming and outgoing network traffic."
                ),
                new QuizQuestion(
                    "What is the most common way malware is spread?",
                    new List<string> { "Through email attachments", "Through social media links", "Through infected USB drives", "All of the above" },
                    3,
                    "Malware can spread through all these methods, making awareness crucial."
                )
            };
        }

        public void StartQuiz()
        {
            CurrentQuestionIndex = 0;
            Score = 0;
            IsQuizActive = true;
            IsQuizPaused = false;
            DisplayCurrentQuestion();
        }


        public void ResumeQuiz()
        {
            if (!IsQuizActive && CurrentQuestionIndex < Questions.Count)
            {
                IsQuizActive = true;
                IsQuizPaused = false;
                DisplayCurrentQuestion();
            }
            else
            {
                AddMessage("There's no quiz to continue.", Brushes.Gray);
            }
        }

        public void StopQuiz()
        {
            if (IsQuizActive)
            {
                IsQuizPaused = true;
                IsQuizActive = false;
                AddMessage("Quiz paused. Type 'continue quiz' to resume.", Brushes.Orange);
            }
            else
            {
                AddMessage("Quiz is not currently running.", Brushes.Gray);
            }
        }

        public void DisplayCurrentQuestion()
        {
            if (CurrentQuestionIndex < Questions.Count)
            {
                var question = Questions[CurrentQuestionIndex];
                AddMessage($"Question {CurrentQuestionIndex + 1}: {question.QuestionText}", Brushes.DarkBlue);

                for (int i = 0; i < question.Options.Count; i++)
                {
                    AddMessage($"{i + 1}. {question.Options[i]}", Brushes.Black);
                }
                AddMessage("Type the number of your answer:", Brushes.DarkBlue);
            }
            else
            {
                EndQuiz();
            }
        }

        public void ProcessAnswer(string input)
        {
            if (!IsQuizActive)
            {
                AddMessage("Quiz is paused. Type 'continue quiz' to resume.", Brushes.Gray);
                return;
            }

            if (int.TryParse(input, out int answer) &&
                answer >= 1 &&
                answer <= Questions[CurrentQuestionIndex].Options.Count)
            {
                bool isCorrect = (answer - 1) == Questions[CurrentQuestionIndex].CorrectAnswerIndex;

                if (isCorrect)
                {
                    Score++;
                    AddMessage("✅ Correct! " + Questions[CurrentQuestionIndex].Explanation, Brushes.Green);
                }
                else
                {
                    AddMessage("❌ Incorrect. " + Questions[CurrentQuestionIndex].Explanation, Brushes.Red);
                }

                CurrentQuestionIndex++;
                DisplayCurrentQuestion();
            }
            else
            {
                AddMessage("Please enter a valid option number.", Brushes.Red);
            }
        }

        private void EndQuiz()
        {
            IsQuizActive = false;
            IsQuizPaused = false;
            double percentage = (double)Score / Questions.Count * 100;
            AddMessage($"Quiz complete! Your score: {Score}/{Questions.Count} ({percentage:0}%)", Brushes.Purple);

            if (percentage >= 80)
                AddMessage("Excellent! You're a cybersecurity expert!", Brushes.Gold);
            else if (percentage >= 50)
                AddMessage("Good job! Keep learning about cybersecurity.", Brushes.Blue);
            else
                AddMessage("Consider reviewing cybersecurity basics for better protection.", Brushes.Orange);
        }
    }

    public class QuizQuestion
    {
        public string QuestionText { get; }
        public List<string> Options { get; }
        public int CorrectAnswerIndex { get; }
        public string Explanation { get; }

        public QuizQuestion(string question, List<string> options, int correctAnswerIndex, string explanation)
        {
            QuestionText = question;
            Options = options;
            CorrectAnswerIndex = correctAnswerIndex;
            Explanation = explanation;
        }
    }
}