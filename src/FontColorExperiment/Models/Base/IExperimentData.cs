using FontColorExperiment.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
