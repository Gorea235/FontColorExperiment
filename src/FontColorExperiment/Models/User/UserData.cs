using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FontColorExperiment.Models.User
{
    public class UserData : Base.IUserData
    {
        [Display(Name = "User ID")]
        public Guid Id { get; set; }
        [Display(Name = "User Experiment Sample Start Time")]
        public DateTime SampleStartTime { get; set; }
        [Display(Name = "User Answers")]
        public Dictionary<int, Dictionary<int, UserResponse>> Answers { get; set; }
        [Display(Name = "Final User Answers")]
        public Dictionary<int, object> FinalAnswers { get; set; }
        [Display(Name = "Current Sample")]
        public int CurrentSample { get; set; }
        [Display(Name = "Sample Order")]
        public List<int[]> SampleOrder { get; set; }
        public UserStep Step { get; set; }

        [JsonIgnore]
        public UserResponse CurrentAnswer
        {
            get { return Answers[CurrentSampleOrder[0]][CurrentSampleOrder[1]]; }
            set { Answers[CurrentSampleOrder[0]][CurrentSampleOrder[1]] = value; }
        }

        [JsonIgnore]
        [Display(Name = "Current User Sample Order")]
        public int[] CurrentSampleOrder { get { return SampleOrder[CurrentSample]; } }
    }
}
