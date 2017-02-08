using FontColorExperiment.Models.Question;
using System.Collections.Generic;

namespace FontColorExperiment.Models.Experiment
{
    public class FullExperiment : Base.IFullExperiment
    {
        public string MainPageText { get; set; }
        public Dictionary<int, ExperimentData> Experiments { get; set; }
        public Dictionary<int, QuestionData> FinalQuestions { get; set; }
    }
}
