using System.Collections.Generic;

namespace FontColorExperiment.Models.Base
{
    public interface IExperimentSample
    {
        int Id { get; set; }
        string Name { get; set; }
        string SampleText { get; set; }
        Dictionary<int, Question.QuestionData> Questions { get; set; }
    }
}
