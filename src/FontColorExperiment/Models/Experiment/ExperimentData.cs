using FontColorExperiment.Models.Base;
using FontColorExperiment.Utils;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FontColorExperiment.Models.Experiment
{
    public class ExperimentData : IExperimentData
    {
        [Display(Name = "Background Colour")]
        public Color Background { get; set; }
        [Display(Name = "Foreground Color")]
        public Color Foreground { get; set; }
        [Display(Name = "Data Id")]
        public int Id { get; set; }
        [Display(Name = "Data samples")]
        public Dictionary<int, ExperimentSample> Samples { get; set; }
    }
}
