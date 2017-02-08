using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FontColorExperiment.Models.User
{
    public class UserResponse
    {
        [Display(Name = "User Answers")]
        public Dictionary<int, object> Answers { get; set; }
        [Display(Name = "Read Times")]
        public TimeSpan TimeTaken { get; set; }
    }
}
