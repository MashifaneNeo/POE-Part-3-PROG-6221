using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ST10449143_PROGPOEPART3
{
    public partial class CyberSecurityQuizControl : UserControl
    {
        public CyberSecurityQuiz quiz;

        public CyberSecurityQuizControl()
        {
            InitializeComponent();
            quiz = new CyberSecurityQuiz(AddQuizMessage);
        }

        private void AddQuizMessage(string message, Brush color)
        {
            Dispatcher.Invoke(() =>
            {
                // Show status and feedback messages here
                QuizStatusText.Text = message;
                QuizStatusText.Foreground = color;
            });
        }

        private void StartQuizButton_Click(object sender, RoutedEventArgs e)
        {
            quiz.StartQuiz();
            LoadCurrentQuestion();
            StartQuizButton.IsEnabled = false;
            PauseQuizButton.IsEnabled = true;
            SubmitButton.IsEnabled = true;
            ResumeQuizButton.IsEnabled = false;
            UpdateQuizProgress();
        }

        private void PauseQuizButton_Click(object sender, RoutedEventArgs e)
        {
            quiz.StopQuiz();
            PauseQuizButton.IsEnabled = false;
            ResumeQuizButton.IsEnabled = true;
            SubmitButton.IsEnabled = false;
            QuizStatusText.Text = "Quiz paused.";
            QuizStatusText.Foreground = Brushes.Orange;
        }

        private void ResumeQuizButton_Click(object sender, RoutedEventArgs e)
        {
            quiz.ResumeQuiz();
            PauseQuizButton.IsEnabled = true;
            ResumeQuizButton.IsEnabled = false;
            SubmitButton.IsEnabled = true;
            LoadCurrentQuestion();
            QuizStatusText.Text = "Quiz resumed.";
            QuizStatusText.Foreground = Brushes.Green;
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (OptionsList.SelectedIndex >= 0)
            {
                quiz.ProcessAnswer((OptionsList.SelectedIndex + 1).ToString());
                LoadCurrentQuestion();
                UpdateQuizProgress();
            }
            else
            {
                QuizStatusText.Text = "Please select an answer!";
                QuizStatusText.Foreground = Brushes.Red;
            }
        }

        private void LoadCurrentQuestion()
        {
            if (quiz.CurrentQuestionIndex < quiz.Questions.Count && quiz.IsQuizActive)
            {
                var question = quiz.Questions[quiz.CurrentQuestionIndex];
                QuestionText.Text = $"Question {quiz.CurrentQuestionIndex + 1}: {question.QuestionText}";
                OptionsList.ItemsSource = question.Options;
                OptionsList.SelectedIndex = -1;
            }
            else
            {
                QuestionText.Text = "Quiz completed!";
                OptionsList.ItemsSource = null;
                SubmitButton.IsEnabled = false;
                PauseQuizButton.IsEnabled = false;
                ResumeQuizButton.IsEnabled = false;
            }
        }

        private void UpdateQuizProgress()
        {
            QuizProgress.Maximum = quiz.Questions.Count;
            QuizProgress.Value = quiz.QuestionsAnswered;
            ProgressText.Text = $"{quiz.Score}/{quiz.QuestionsAnswered} ({(double)quiz.Score / quiz.Questions.Count:P0})";
        }
    }
}
