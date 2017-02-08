using FontColorExperiment.Models.Base;
using FontColorExperiment.Utils;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace FontColorExperiment.Models.Experiment
{
    public class ExperimentData : IExperimentData
    {
        public Color Background { get; set; }
        public Color Foreground { get; set; }
        public int Id { get; set; }
        public Dictionary<int, ExperimentSample> Samples { get; set; }

        [JsonIgnore]
        public System.Windows.Media.SolidColorBrush BackgroundBrush { get => new System.Windows.Media.SolidColorBrush(Background); }
        [JsonIgnore]
        public System.Windows.Media.SolidColorBrush ForegroundBrush { get => new System.Windows.Media.SolidColorBrush(Foreground); }

        public override string ToString()
        {
            return string.Format("#{0:X2}{1:X2}{2:X2}{3:X2} / #{4:X2}{5:X2}{6:X2}{7:X2}",
                Foreground.A, Foreground.R, Foreground.G, Foreground.B,
                Background.A, Background.R, Background.G, Background.B);
        }
    }
}
