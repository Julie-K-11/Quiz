using System;
using System.Collections.Generic;
using System.Linq;

namespace quiz
{
    class Program
    {
        static void Main(string[] args)
        {
            UserManager userManager = new UserManager();
            QuizManager quizManager = new QuizManager();
            QuizUtility quizUtility = new QuizUtility(quizManager);

            User currentUser = null;

            while (true)
            {
                Console.WriteLine("1. Реєстрація");
                Console.WriteLine("2. Вхід");
                Console.WriteLine("3. Створити/Редагувати Вікторину");
                Console.WriteLine("4. Вихід");
                Console.Write("Введіть цифу, відповідно до бажаної дії : ");
                Console.WriteLine();

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        currentUser = RegisterUser(userManager);
                        if (currentUser != null)
                        {
                            StartQuizMenu(currentUser, quizManager, userManager);
                        }
                        break;
                    case "2":
                        currentUser = LoginUser(userManager);
                        if (currentUser != null)
                        {
                            StartQuizMenu(currentUser, quizManager, userManager);
                        }
                        break;
                    case "3":
                        CreateOrEditQuiz(quizUtility);
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Такої опції не існує\n");
                        break;
                }
            }
        }

        static User RegisterUser(UserManager userManager)
        {
            Console.Write("\nВведіть логін: ");
            var login = Console.ReadLine();
            Console.Write("Введіть пароль: ");
            var password = Console.ReadLine();
            Console.Write("Введіть дату народження (dd.mm.yyyy): ");
            var dateOfBirth = DateTime.Parse(Console.ReadLine());

            try
            {
                var user = userManager.Register(login, password, dateOfBirth);
                Console.WriteLine("Реєстрацію завершено");
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        static User LoginUser(UserManager userManager)
        {
            Console.Write("\nВведіть логін: ");
            var login = Console.ReadLine();
            Console.Write("Введіть пароль: ");
            var password = Console.ReadLine();

            try
            {
                var user = userManager.Login(login, password);
                Console.WriteLine("Вхід виконано успішно");
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        static void StartQuizMenu(User user, QuizManager quizManager, UserManager userManager)
        {
            while (true)
            {
                Console.WriteLine("\n1. Почати Вікторину");
                Console.WriteLine("2. Дивитися минулі результати");
                Console.WriteLine("3. Топ 20");
                Console.WriteLine("4. Редагування профілю");
                Console.WriteLine("5. Вихід");
                Console.Write("Введіть цифу, відповідно до бажаної дії : ");
                Console.WriteLine();

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        StartNewQuiz(user, quizManager);
                        break;
                    case "2":
                        ViewPastResults(user, quizManager);
                        break;
                    case "3":
                        ViewTop20(user, quizManager);
                        break;
                    case "4":
                        EditSettings(user, userManager);
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Такої опції не існує");
                        break;
                }
            }
        }

        static void StartNewQuiz(User user, QuizManager quizManager)
        {
            Console.WriteLine("\nДоступні Вікторини:");
            foreach (var quizName in quizManager.GetAllQuizNames())
            {
                Console.WriteLine($"- {quizName}");
            }
            Console.WriteLine("- mixed");

            Console.Write("\nВведіт Назву Вікторини: ");
            var selectedQuizName = Console.ReadLine().ToLower();

            quizManager.StartQuiz(user, selectedQuizName);

        }

        static void ViewPastResults(User user, QuizManager quizManager)
        {
            Console.WriteLine("\nРезультати минулих вікторин:");
            var results = quizManager.LoadUserResults(user.Login);
            foreach (var result in results)
            {
                Console.WriteLine($"Вікторина: {result.QuizName}, час: {result.TimeTaken}, успішність: {result.Score}%");
            }
        }

        static void ViewTop20(User user, QuizManager quizManager)
        {
            Console.Write("\nВведіть назву вікторини для Топ 20: ");
            var quizName = Console.ReadLine().ToLower();

            if (quizName != "mixed" && !quizManager.QuizExists(quizName))
            {
                Console.WriteLine("Такої вікторини не існує");
                return;
            }

            var topResults = quizManager.GetTop20Results(quizName);
            Console.WriteLine($"Топ 20 для {quizName}:");
            foreach (var result in topResults)
            {
                Console.WriteLine($"Успішність: {result.Score}%, час: {result.TimeTaken}");
            }
        }

        static void EditSettings(User user, UserManager userManager)
        {
            Console.Write("\nВведіть новий пароль: ");
            var newPassword = Console.ReadLine();
            Console.Write("Ввідь нову дату народження (dd.mm.yyyy): ");
            var newDateOfBirth = DateTime.Parse(Console.ReadLine());

            user.Password = newPassword;
            user.DateOfBirth = newDateOfBirth;
            userManager.UpdateUser(user);

            Console.WriteLine("Інформацію змінено");
        }

        static void CreateOrEditQuiz(QuizUtility quizUtility)
        {
            Console.WriteLine("\n1. Створити Вікторину");
            Console.WriteLine("2. Редагувати Вікторину");
            Console.Write("Введіть цифу, відповідно до бажаної дії : ");
            Console.WriteLine();

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    quizUtility.CreateQuiz();
                    break;
                case "2":
                    quizUtility.EditQuiz();
                    break;
                default:
                    Console.WriteLine("Такої опції не існує\n");
                    break;
            }
        }
    }
}