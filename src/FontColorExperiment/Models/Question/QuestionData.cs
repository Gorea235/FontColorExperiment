using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FontColorExperiment.Models.Question
{
    public class QuestionData : Base.IQuestionData
    {
        [Display(Name = "Question ID")]
        public int Id { get; set; }
        [Display(Name = "Question Text")]
        public string QuestionString { get; set; }
        [Display(Name = "Secondary Question Texts")]
        public Dictionary<int, string> QuestionStringSecondary { get; set; }
        [Display(Name = "Question Items")]
        public Dictionary<int, string> QuestionItems { get; set; }
        [Display(Name = "Question Type")]
        public QuestionType Type { get; set; }
    }
}
