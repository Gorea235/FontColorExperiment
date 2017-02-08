using System;
using System.Collections.Generic;

namespace FontColorExperiment.Models.User
{
    public class UserSimpleData
    {
        public Guid Id { get; set; }
        public Dictionary<int, Dictionary<int, UserResponse>> Answers { get; set; }
        public Dictionary<int, object> FinalAnswers { get; set; }
        public int PreferredSample { get; set; }
        public List<int[]> SampleOrder { get; set; }

        public UserSimpleData() { }
        public UserSimpleData(UserData user)
        {
            Id = user.Id;
            Answers = user.Answers;
            FinalAnswers = user.FinalAnswers;
            SampleOrder = user.SampleOrder;
        }
    }
}
