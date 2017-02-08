using FontColorExperiment.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FontColorExperiment.Models.Base
{
    public interface IUserData
    {
        Guid Id { get; set; }
        DateTime SampleStartTime { get; set; }
        Dictionary<int, Dictionary<int, UserResponse>> Answers { get; set; }
        Dictionary<int, object> FinalAnswers { get; set; }
        int CurrentSample { get; set; }
        List<int[]> SampleOrder { get; set; }
        UserStep Step { get; set; }
    }
}
