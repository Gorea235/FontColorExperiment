using FontColorExperiment.Utils;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FontColorExperiment.Models.Experiment
{
    public class ExperimentCurrentSample
    {
        [Display(Name = "Foreground Colour")]
        public Color Background { get; set; }
        [Display(Name = "Background Color")]
        public Color Foreground { get; set; }
        [Display(Name = "Data Id")]
        public int Id { get; set; }
        [Display(Name = "Current Data Sample")]
        public ExperimentSample Sample { get; set; }

        public static ExperimentCurrentSample FromSample(ExperimentData data, int sampleid)
        {
            return new ExperimentCurrentSample() {
                Background = data.Background,
                Foreground = data.Foreground,
                Id = data.Id,
                Sample = data.Samples[sampleid]
            };
        }
    }
}
