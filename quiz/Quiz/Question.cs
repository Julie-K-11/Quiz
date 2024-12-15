using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace quiz
{
    class Question
    {
        public string Text { get; set; }
        public List<string> Options { get; set; } = new List<string>();
        public int[] CorrectAnswers { get; set; } = new int[0];

        public bool CheckAnswer(int[] userAnswers)
        {
            if (userAnswers.Length != CorrectAnswers.Length) return false;
            Array.Sort(userAnswers);
            Array.Sort(CorrectAnswers);
            for (int i = 0; i < userAnswers.Length; i++)
            {
                if (userAnswers[i] != CorrectAnswers[i]) return false;
            }
            return true;
        }
    }
}