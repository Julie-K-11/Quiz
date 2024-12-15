using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace quiz
{
    class QuizManager : IQuiz
    {
        private Dictionary<string, List<Question>> quizzes = new Dictionary<string, List<Question>>();
        private string questionsPath = "questions.txt";
        private string resultsPath = "results.txt";

        public QuizManager()
        {
            LoadQuizzes();
        }

        public void AddQuiz(string quizName, List<Question> questions)
        {
            if (questions.Count != 20)
            {
                throw new Exception("Вікторина має містити 20 питань");
            }

            quizzes[quizName] = questions;
            SaveQuizzes();
        }

        public void StartQuiz(User user, string quizName)
        {
            if (quizName == "mixed")
            {
                StartMixedQuiz(user);
                return;
            }

            if (!quizzes.ContainsKey(quizName))
            {
                Console.WriteLine("Вікторину не знайдено");
                return;
            }

            var questions = quizzes[quizName];
            Quiz(user, quizName, questions);
        }

        private void StartMixedQuiz(User user)
        {
            var allQuestions = quizzes.Values.SelectMany(quizzes => quizzes).ToList();
            allQuestions = MixedQuestions(allQuestions);
            allQuestions = allQuestions.Take(20).ToList();

            Quiz(user, "mixed", allQuestions);
        }

        private void Quiz(User user, string quizName, List<Question> questions)
        {
            int correctAnswers = 0;
            var startTime = DateTime.Now;

            foreach (var question in questions)
            {
                Console.WriteLine(question.Text);
                for (int i = 0; i < question.Options.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {question.Options[i]}");
                }

                bool validInput = false;
                int[] userAnswers = null;

                while (!validInput)
                {
                    Console.Write("Ваша відповідь (кома для розділення відповідей): ");
                    var input = Console.ReadLine();
                    userAnswers = ParseAnswers(input);

                    if (userAnswers.Any(answer => answer < 1 || answer > question.Options.Count))
                    {
                        Console.WriteLine("Неправильний ввід. Спробуйте ще раз.");
                    }
                    else
                    {
                        validInput = true;
                    }
                }

                if (question.CheckAnswer(userAnswers))
                    correctAnswers++;
            }

            var endTime = DateTime.Now;
            var timeTaken = endTime - startTime;
            var result = new QuizResult
            {
                QuizName = quizName,
                CorrectAnswers = correctAnswers,
                TotalQuestions = questions.Count,
                TimeTaken = $"{timeTaken.Minutes} хвилина {timeTaken.Seconds} секунд"
            };

            user.QuizResults.Add(result);
            SaveResult(user.Login, result);
            Console.WriteLine("Вікторина пройдена!");
            Console.WriteLine($"Ви відповіли коректно {correctAnswers} разів з {questions.Count} питань");
        }

        private List<Question> MixedQuestions(List<Question> questions)
        {
            var random = new Random();
            for (int i = questions.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                var temp = questions[i];
                questions[i] = questions[j];
                questions[j] = temp;
            }
            return questions;
        }

        private void LoadQuizzes()
        {
            if (!File.Exists(questionsPath)) return;

            var lines = File.ReadAllLines(questionsPath);
            string currentQuiz = null;
            List<Question> currentQuestions = null;

            foreach (var line in lines)
            {
                if (line.StartsWith("#"))
                {
                    if (currentQuiz != null)
                    {
                        quizzes[currentQuiz] = currentQuestions;
                    }
                    currentQuiz = line.Substring(1);
                    currentQuestions = new List<Question>();
                }
                else if (currentQuiz != null)
                {
                    var parts = line.Split('|');
                    currentQuestions.Add(new Question
                    {
                        Text = parts[0],
                        Options = new List<string>(parts[1].Split(';')),
                        CorrectAnswers = Array.ConvertAll(parts[2].Split(','), int.Parse)
                    });
                }
            }

            if (currentQuiz != null)
            {
                quizzes[currentQuiz] = currentQuestions;
            }
        }

        private void SaveQuizzes()
        {
            var lines = new List<string>();

            foreach (var quiz in quizzes)
            {
                lines.Add("#" + quiz.Key);
                foreach (var question in quiz.Value)
                {
                    lines.Add($"{question.Text}|{string.Join(";", question.Options)}|{string.Join(",", question.CorrectAnswers)}");
                }
            }
            File.WriteAllLines(questionsPath, lines);
        }

        private int[] ParseAnswers(string input)
        {
            return input.Split(',').Select(int.Parse).ToArray();
        }

        public List<string> GetAllQuizNames()
        {
            return quizzes.Keys.ToList();
        }

        public bool QuizExists(string quizName)
        {
            return quizzes.ContainsKey(quizName);
        }

        public List<Question> GetQuizQuestions(string quizName)
        {
            return quizzes[quizName];
        }

        public void UpdateQuiz(string quizName, List<Question> questions)
        {
            quizzes[quizName] = questions;
            SaveQuizzes();
        }

        public void SaveResult(string login, QuizResult result)
        {
            var lines = File.ReadAllLines(resultsPath).ToList();
            lines.Add($"{login},{result.QuizName},{result.CorrectAnswers},{result.TotalQuestions},{result.TimeTaken}");
            File.WriteAllLines(resultsPath, lines);
        }

        private List<QuizResult> LoadResults(Func<string[], bool> predicate)
        {
            var lines = File.ReadAllLines(resultsPath);
            var results = new List<QuizResult>();

            foreach (var line in lines)
            {
                var parts = line.Split(',');
                if (predicate(parts))
                {
                    results.Add(new QuizResult
                    {
                        QuizName = parts[1],
                        CorrectAnswers = int.Parse(parts[2]),
                        TotalQuestions = int.Parse(parts[3]),
                        TimeTaken = parts[4]
                    });
                }
            }

            return results;
        }

        public List<QuizResult> GetTop20Results(string quizName)
        {
            var results = LoadResults(parts => parts[1] == quizName);
            return results.OrderByDescending(result => result.Score).Take(20).ToList();
        }

        public List<QuizResult> LoadUserResults(string login)
        {
            return LoadResults(parts => parts[0] == login);
        }
    }
}