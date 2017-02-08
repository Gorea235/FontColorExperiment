using System.Collections.Generic;

namespace FontColorExperiment.Models.Question
{
    public class QuestionData : Base.IQuestionData
    {
        public int Id { get; set; }
        public string QuestionString { get; set; }
        public Dictionary<int, string> QuestionStringSecondary { get; set; }
        public Dictionary<int, string> QuestionItems { get; set; }
        public QuestionType Type { get; set; }
    }
}
