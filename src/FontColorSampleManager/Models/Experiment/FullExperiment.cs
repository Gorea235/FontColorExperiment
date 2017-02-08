using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FontColorExperiment.Models.Question;

namespace FontColorExperiment.Models.Experiment
{
    public class FullExperiment : Base.IFullExperiment
    {
        public string MainPageText { get; set; }
        public Dictionary<int, ExperimentData> Experiments { get; set; }
        public Dictionary<int, QuestionData> FinalQuestions { get; set; }
    }
}
