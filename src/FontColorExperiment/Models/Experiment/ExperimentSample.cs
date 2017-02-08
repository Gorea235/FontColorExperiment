using FontColorExperiment.Models.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FontColorExperiment.Models.Experiment
{
    public class ExperimentSample : IExperimentSample
    {
        [Display(Name = "Sample ID")]
        public int Id { get; set; }
        [Display(Name = "Sample Name")]
        public string Name { get; set; }
        [Display(Name = "Sample Text")]
        public string SampleText { get; set; }
        [Display(Name = "Questions For Sample")]
        public Dictionary<int, Question.QuestionData> Questions { get; set; }
    }
}
