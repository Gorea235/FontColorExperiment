using FontColorExperiment.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
