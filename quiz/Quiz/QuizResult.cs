using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace quiz
{
    class QuizResult
    {
        public string QuizName { get; set; }
        public int CorrectAnswers { get; set; }
        public int TotalQuestions { get; set; }
        public string TimeTaken { get; set; }
        public double Score => (double)CorrectAnswers / TotalQuestions * 100;
    }
}
