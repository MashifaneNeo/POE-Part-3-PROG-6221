Cybersecurity Awareness Assistant – GUI User Guide

Overview

The Cybersecurity Awareness Assistant is a C# WPF application that provides cybersecurity education and task management through a natural language chatbot interface. It allows users to:

* Add and manage cybersecurity tasks
* Set and view reminders
* Start and complete cybersecurity quizzes
* Track past actions and view a detailed activity log
* Interact with the chatbot using natural language commands

---

 Getting Started

Running the Application

1. Open the solution in Visual Studio.
2. Build and run the project.
3. The main window will launch, displaying the chat interface.

---
Task Management

Add a Task

Use one of the following commands to add a new cybersecurity-related task:

* `Add task Check my privacy settings`
* `Create task Enable two-factor authentication`

The assistant will respond with a message confirming the task and ask if you want to set a reminder.


Tasks will appear in the main window with an indicator of whether each task is complete and whether a reminder is set.

Complete a Task

Mark a task as completed using:

* `Complete task Check my privacy settings`
* `Mark task Enable two-factor authentication as done`

The assistant will confirm the task is completed.

 Delete a Task

To remove a task:

* `Delete task Check my privacy settings`
* `Remove task Enable two-factor authentication`

---

 Setting Reminders

 After Adding a Task

After adding a task, the bot will prompt:

> Task added: "Review: Check my privacy settings". Would you like a reminder? (e.g., 'Remind me in 3 days')

Reply with:

* `Remind me in 3 days`

This will set a reminder for that task.

 Direct Reminder

You can also directly create a reminder without adding a task first:

* `Remind me to update my password tomorrow`
* `Set a reminder to review my firewall in 2 days`

The assistant will extract the title and schedule the reminder accordingly.

---

 Quiz Feature

 Starting the Quiz

Begin a cybersecurity quiz by typing:

* `Start quiz`
* `Quiz time`
* `Begin quiz`

The quiz will start immediately. Answer each question as prompted.

 Quiz Activity

The assistant logs the start and end of quizzes in the activity log.

---

 Viewing Past Actions

 What Have You Done for Me?

To view a summary of recent user-initiated actions:

* `What have you done for me`
* `Show my actions`

This includes task additions, reminders set, quiz started, etc., in short phrases.

Show Activity Log

To view a detailed log:

* `Show activity log`

This will display the last five significant actions, including the exact time they were recorded.

Examples of logged actions:

* ` - Task added: 'Check software updates'`
* `- Reminder created: 'Enable encryption' for 25 Jun 2025`

---

Help and Exit

 Getting Help

To see the help menu:

* `Show help`
* `What can I ask`
* `Help`

Exiting the Application

To close the application:

* `Exit`
* `Quit`
* `Close bot`

---

 Notes

* The assistant uses simple NLP logic to detect intent from your input.
* You must phrase task and reminder inputs clearly to ensure proper understanding.
* Task titles must be unique for accurate tracking and deletion.
* The activity log stores the last 50 actions; the display is limited to 5 entries by default for readability.

---

Summary of Supported Commands

| Intent        | Example Input                             |
| ------------- | ----------------------------------------- |
| Add Task      | `Add task Check password strength`        |
| View Tasks    | `View tasks`                              |
| Complete Task | `Complete task Check password strength`   |
| Delete Task   | `Delete task Check password strength`     |
| Set Reminder  | `Remind me to update antivirus in 3 days` |
| Start Quiz    | `Start quiz`                              |
| Help Menu     | `Show help`                               |
| Exit          | `Exit`                                    |
| Show Actions  | `What have you done for me`               |
| Activity Log  | `Show activity log`                       |

---

Developer Information

* Built in C# using WPF
* NLP handled through keyword-based matching in `NLPProcessor`
* Tasks stored in memory (`List<CyberTask>`)
* Activity log stored in a fixed-size queue (`Queue<string>`)
* GUI dynamically updates using `TextBlock` controls

---
Github link: https://github.com/MashifaneNeo/POE-Part-3-PROG-6221.git
YouTube video link: 
