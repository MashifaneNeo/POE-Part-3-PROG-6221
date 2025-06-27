 Cybersecurity Assistant GUI

 Overview

The Cybersecurity Assistant is a WPF-based desktop application designed to help users improve their cybersecurity awareness. It offers interactive features including:

* A Task Assistant for managing cybersecurity-related tasks.
* A Cybersecurity Quiz to test users' knowledge.
* An Activity Log to track user actions and quiz progress.
* An integrated Chatbot that uses keyword detection, sentiment analysis, and memory to provide helpful cybersecurity tips and guidance.

---
Features

* User Authentication: Prompts users for their name on startup.
* Tabbed Interface: Easy navigation between Task Assistant, Quiz, Activity Log, and Chatbot.
* Cybersecurity Quiz:

  * Start, pause, resume, and submit answers.
  * Displays feedback and tracks scores.
  * Logs quiz activity.
*  Activity Log:

  * Logs application start, quiz events, and user actions.
  * Displays recent activities with a toggle for full view.
*  Chatbot:

  * Natural language processing with keyword detection.
  * Sentiment detection to personalize responses.
  * Memory of user preferences (favorite topics).
  * Provides randomized helpful tips on cybersecurity topics.
*  NLP Processor Launch: Button to open NLP processing window (optional feature).
*  Status Bar: Displays current user information.

---
 Usage

1. When the application starts, enter your name in the welcome dialog.
2. Navigate between tabs to access different features:

   * Task Assistant: Manage cybersecurity tasks.
   *  Cybersecurity Quiz: Take quizzes and monitor your progress.
   *  Activity Log: View logged actions and quiz events.
   *  Chatbot: Interact with the assistant using natural language.
3. Use the Open NLP Processor button to launch the NLP window.
4. Use the Exit button to safely close the application.

---

  Project Structure

* MainWindow\.xaml & MainWindow\.xaml.cs: Main UI and application logic controller.
* TaskAssistantControl.xaml & .cs: Task assistant user control.
* CyberSecurityQuizControl.xaml & .cs: Quiz interface and logic.
* ActivityLogControl.xaml & .cs: Displays logged activities.
* ChatbotControl.xaml & .cs: Chatbot UI embedded in the tab.
* ChatbotWindow\.xaml & .cs: Optional standalone chatbot window (if used).
* PreviousWork.cs: Chatbot logic including keyword detection, sentiment analysis, tip management, and memory.
* DictionaryList.cs: Data for chatbot topics and keywords.
* HelpMenu.cs: Help info displayed by chatbot.
* SentimentDetector.cs: Simple sentiment recognition logic.

---

 Dependencies

* .NET Framework 4.7.2 or later
* WPF (Windows Presentation Foundation)
* No external libraries required.

---

Github link: https://github.com/MashifaneNeo/POE-Part-3-PROG-6221.git
YouTube video link: https://youtu.be/vvuSIzFxzRE
