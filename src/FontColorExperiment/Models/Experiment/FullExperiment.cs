using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FontColorExperiment.Models.Question;
using System.ComponentModel.DataAnnotations;

namespace FontColorExperiment.Models.Experiment
{
    public class FullExperiment : Base.IFullExperiment
    {
        [Display(Name = "Main Page Text")]
        public string MainPageText { get; set; }
        [Display(Name = "All Experiments")]
        public Dictionary<int, ExperimentData> Experiments { get; set; }
        [Display(Name = "Final Questions")]
        public Dictionary<int, QuestionData> FinalQuestions { get; set; }
    }
}
