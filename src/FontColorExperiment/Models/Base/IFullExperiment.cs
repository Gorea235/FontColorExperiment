using System.Collections.Generic;

namespace FontColorExperiment.Models.Base
{
    public interface IFullExperiment
    {
        string MainPageText { get; set; }
        Dictionary<int, Experiment.ExperimentData> Experiments { get; set; }
        Dictionary<int, Question.QuestionData> FinalQuestions { get; set; }
    }
}
