using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FontColorExperiment.Models.Base
{
    public interface IFullExperiment
    {
        string MainPageText { get; set; }
        Dictionary<int, Experiment.ExperimentData> Experiments { get; set; }
        Dictionary<int, Question.QuestionData> FinalQuestions { get; set; }
    }
}
