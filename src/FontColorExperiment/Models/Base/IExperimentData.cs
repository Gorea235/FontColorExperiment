using FontColorExperiment.Utils;
using System.Collections.Generic;

namespace FontColorExperiment.Models.Base
{
    public interface IExperimentData
    {
        int Id { get; set; }
        Color Foreground { get; set; }
        Color Background { get; set; }
        Dictionary<int, Experiment.ExperimentSample> Samples { get; set; }
    }
}
