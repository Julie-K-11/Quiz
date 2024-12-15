using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace quiz
{
    class QuizUtility
    {
        private QuizManager quizManager;

        public QuizUtility(QuizManager quizManager)
        {
            this.quizManager = quizManager;
        }

        public void CreateQuiz()
        {
            Console.Write("\nВведіть назву вікторини: ");
            var quizName = Console.ReadLine().ToLower(); ;

            var questions = new List<Question>();

            while (true)
            {
                Console.Write("Введіть питання ( введіть 'exit' для завершення): ");
                var questionText = Console.ReadLine();
                if (questionText.ToLower() == "exit")
                {
                    break;
                }

                var options = new List<string>();
                Console.Write("Введіть кількість варіантів відповіді: ");
                var numOptions = int.Parse(Console.ReadLine());
                for (int i = 0; i < numOptions; i++)
                {
                    Console.Write($"Введіть варінт відповіді {i + 1}: ");
                    options.Add(Console.ReadLine());
                }

                Console.Write("Введіть номер варінту з правельною відповіддю (пробіл для розділення): ");
                var correctAnswers = Console.ReadLine().Split(',').Select(int.Parse).ToArray();

                questions.Add(new Question
                {
                    Text = questionText,
                    Options = options,
                    CorrectAnswers = correctAnswers
                });
            }

            quizManager.AddQuiz(quizName, questions);
            Console.WriteLine("Вікторину було створено");
        }

        public void EditQuiz()
        {
            Console.Write("\nВведіть назву вікторини для редагування: ");
            var quizName = Console.ReadLine().ToLower();

            if (!quizManager.QuizExists(quizName))
            {
                Console.WriteLine("Вікторина не знайдена");
                return;
            }

            var questions = quizManager.GetQuizQuestions(quizName);

            while (true)
            {
                Console.Write("Введіть питання ( введіть 'exit' для завершення): ");
                var questionNumber = Console.ReadLine();
                if (questionNumber.ToLower() == "exit")
                {
                    break;
                }

                var questionIndex = int.Parse(questionNumber) - 1;
                if (questionIndex < 0 || questionIndex >= questions.Count)
                {
                    Console.WriteLine("У вікторині лише 20 питань, такого варіанту не існує");
                    continue;
                }

                var question = questions[questionIndex];

                Console.Write("Введіть нове питання: ");
                question.Text = Console.ReadLine();

                var options = new List<string>();
                Console.Write("Введіть кількість варіантів відповіді: ");
                var numOptions = int.Parse(Console.ReadLine());
                for (int i = 0; i < numOptions; i++)
                {
                    Console.Write($"Введіть варінт відповіді  {i + 1}: ");
                    options.Add(Console.ReadLine());
                }
                question.Options = options;

                Console.Write("Введіть номер варінту з правельною відповіддю (пробіл для розділення): ");
                question.CorrectAnswers = Console.ReadLine().Split(',').Select(int.Parse).ToArray();

                questions[questionIndex] = question;
            }

            quizManager.UpdateQuiz(quizName, questions);
            Console.WriteLine("Вікторина була змінена");
        }
    }
}