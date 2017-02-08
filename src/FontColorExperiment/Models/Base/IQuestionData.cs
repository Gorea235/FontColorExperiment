using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FontColorExperiment.Models.Base
{
    public interface IQuestionData
    {
        int Id { get; set; }
        string QuestionString { get; set; }
        Dictionary<int, string> QuestionStringSecondary { get; set; }
        Dictionary<int, string> QuestionItems { get; set; }
        Question.QuestionType Type { get; set; }
    }
}
