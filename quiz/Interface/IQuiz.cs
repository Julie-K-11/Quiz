using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace quiz
{
    interface IQuiz
    {
        void AddQuiz(string quizName, List<Question> questions);
        void StartQuiz(User user, string quizName);
        void SaveResult(string login, QuizResult result);
        List<QuizResult> GetTop20Results(string quizName);
        List<QuizResult> LoadUserResults(string login);
    }
}
