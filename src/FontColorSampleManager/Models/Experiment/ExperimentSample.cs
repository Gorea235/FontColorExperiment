using FontColorExperiment.Models.Base;
using FontColorExperiment.Utils;
using System.Collections.Generic;

namespace FontColorExperiment.Models.Experiment
{
    public class ExperimentSample : IExperimentSample
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SampleText { get; set; }
        public Dictionary<int, Question.QuestionData> Questions { get; set; }
    }
}
