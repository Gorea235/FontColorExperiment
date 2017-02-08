using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FontColorExperiment.Models
{
    public class AdminPageData
    {
        public List<User.UserData> Users { get; set; }
        public Experiment.FullExperiment Experiment { get; set; }
        public int UploadSuccess { get; set; }
        public string LogData { get; set; }

        public AdminPageData(List<User.UserData> users, Experiment.FullExperiment experiment, int success, string log)
        {
            Users = users;
            Experiment = experiment;
            UploadSuccess = success;
            LogData = log;
        }
    }
}
