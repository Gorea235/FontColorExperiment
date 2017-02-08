using FontColorExperiment.Models.Experiment;
using FontColorExperiment.Models.User;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FontColorExperiment.Utils
{
    /// <summary>
    /// The main data manager class.
    /// Handles all user and experiment data, and also the
    /// loading/saving of the server state.
    /// </summary>
    public class ServerManager
    {
        /// <summary>
        /// A basic container class to enable proper saving/loading of server state.
        /// </summary>
        private class ServerManagerStore
        {
            public Dictionary<Guid, UserData> Users { get; set; }
            public FullExperiment Experiment { get; set; }
        }

        /// <summary>
        /// The admin password. Nothing complex is needed due to being
        /// an enclosed experiment (and not being open source when it 
        /// was initially used).
        /// </summary>
        private const string AdminPassword = "admin password"; // changed from actual used
        /// <summary>
        /// The maximum time an admin can be logged on for before being
        /// logged out.
        /// </summary>
        private readonly TimeSpan MaxAdminLogonCheckTime = TimeSpan.FromMinutes(30);
        /// <summary>
        /// The admin token name.
        /// </summary>
        public const string AdminTokenKey = "admin-token";
        /// <summary>
        /// The random number generator used for orderin the sample texts/
        /// </summary>
        private static Random m_sortRng = new Random();

        /// <summary>
        /// A dictionary of users current in the server.
        /// </summary>
        private Dictionary<Guid, UserData> m_users = new Dictionary<Guid, UserData>();
        /// <summary>
        /// A list of users currently in the server.
        /// </summary>
        public List<UserData> CurrentUsers { get => m_users.Values.ToList(); }
        /// <summary>
        /// The current experiment data.
        /// </summary>
        private FullExperiment m_experiment = new FullExperiment();
        /// <summary>
        /// The current experiment data.
        /// </summary>
        public FullExperiment CurrentExperiment { get => m_experiment; }
        /// <summary>
        /// Whether to raise the changed event.
        /// </summary>
        private bool m_changeEventFire = true;
        /// <summary>
        /// A dictionary of the current admins logged in.
        /// </summary>
        private Dictionary<Guid, DateTime> m_admins = new Dictionary<Guid, DateTime>();

        /// <summary>
        /// The changed event. Raised when an item in the server changes.
        /// </summary>
        public event EventHandler Changed;
        /// <summary>
        /// A helper function that raises the changed event.
        /// </summary>
        private void OnChange()
        {
            if (m_changeEventFire)
                Changed?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Constructs an empty server manager
        /// </summary>
        public ServerManager() { }
        /// <summary>
        /// Constructs a new server manager containing the given data.
        /// </summary>
        /// <param name="users">The users in the server.</param>
        /// <param name="experiment">The experiment data.</param>
        private ServerManager(Dictionary<Guid, UserData> users, FullExperiment experiment)
        {
            m_users = users;
            m_experiment = experiment;
        }

        #region User

        /// <summary>
        /// Creates a new user, sets up their sample ordering,
        /// adds them to the server and returns the new user.
        /// </summary>
        /// <returns>The new created user.</returns>
        public UserData NewUser()
        {
            // first three colors with first text samples
            List<int[]> dataOrderLeft = new List<int[]>();
            // same as previous but in oppisite colour order and with other text sample
            List<int[]> dataOrderRight = new List<int[]>();
            // the storage of the answers
            Dictionary<int, Dictionary<int, UserResponse>> answerStore = new Dictionary<int, Dictionary<int, UserResponse>>();
            // the current answer object being built
            Dictionary<int, UserResponse> currentAnswer;
            int left, right;
            foreach (ExperimentData data in m_experiment.Experiments.Values)
            {
                if (m_sortRng.NextDouble() <= .5) // randomly orders the 2 text samples in the colours
                {
                    left = 0;
                    right = 1;
                }
                else
                {
                    left = 1;
                    right = 0;
                }
                // adds the sample id to the lists
                dataOrderLeft.Add(new[] { data.Id, left });
                // (adds the sample in reverse order)
                dataOrderRight.Insert(0, new[] { data.Id, right });
                // creates the answer storge object and adds it to the store
                currentAnswer = new Dictionary<int, UserResponse>
                {
                    { 0, new UserResponse() },
                    { 1, new UserResponse() }
                };
                answerStore.Add(data.Id, currentAnswer);
            }
            // appends the right list to the left creating the full ordering
            dataOrderLeft.AddRange(dataOrderRight);
            // creates the new user with the data
            UserData user = new UserData()
            {
                Id = Guid.NewGuid(),
                SampleStartTime = DateTime.Now,
                SampleOrder = dataOrderLeft,
                Answers = answerStore
            };
            // adds the new user to the server
            m_users.Add(user.Id, user);
            Main.Log("Generated new user with id {0}", user.Id);
            OnChange(); // raised changed event
            return user; // return user
        }

        /// <summary>
        /// Returns whether the specified user exists.
        /// </summary>
        /// <param name="userid">The ID to check.</param>
        /// <returns>Whether the given ID exists in the server.</returns>
        public bool UserExists(Guid userid)
        {
            return m_users.ContainsKey(userid);
        }

        /// <summary>
        /// Returns the current sample data and step for the given user.
        /// </summary>
        /// <param name="userid">The ID of the current user.</param>
        /// <returns>A tuple containing the sample data and the current step.</returns>
        public Tuple<ExperimentCurrentSample, UserStep> CurrentSample(Guid userid)
        {
            Main.Log("Returning current sample for user {0}", userid);
            UserData user = m_users[userid]; // get current user
            switch (user.Step)
            {
                case UserStep.FinalQuestion:
                    return Tuple.Create(
                        new ExperimentCurrentSample()
                        {
                            Sample = new ExperimentSample()
                            {
                                Questions = m_experiment.FinalQuestions
                            }
                        },
                    user.Step);
                case UserStep.Finish:
                    return Tuple.Create<ExperimentCurrentSample, UserStep>(null, user.Step);
                default:
                    //if (user.SampleStartTime == DateTime.MinValue)
                    //    user.SampleStartTime = DateTime.Now;
                    int[] currentSample = user.CurrentSampleOrder;
                    return Tuple.Create(
                        ExperimentCurrentSample.FromSample(
                            m_experiment.Experiments[currentSample[0]],
                            currentSample[1]),
                        user.Step);
            }
        }

        /// <summary>
        /// Sets the answers for the current sample for the user.
        /// </summary>
        /// <param name="userid">The ID of the user to apply the answers to.</param>
        /// <param name="answers">The answers to apply to the user.</param>
        public void ApplyUserAnswers(Guid userid, Dictionary<int, object> answers)
        {
            Main.Log("Applying {0} answers for user {1}", answers.Keys.Count, userid);
            UserData user = m_users[userid]; // get user
            switch (user.Step)
            {
                case UserStep.Question:
                    user.CurrentAnswer.Answers = answers;
                    break;
                case UserStep.FinalQuestion:
                    user.FinalAnswers = answers;
                    break;
            }
            OnChange();
        }

        /// <summary>
        /// Moves the user to the next step of the experiment.
        /// </summary>
        /// <param name="userid">The user of the user to step.</param>
        public void StepUser(Guid userid)
        {
            Main.Log("Stepping user {0} to next step", userid);
            UserData user = m_users[userid];
            switch (user.Step)
            {
                case UserStep.Finish:
                    break;
                case UserStep.Question:
                    user.CurrentSample++;
                    if (user.CurrentSample >= user.SampleOrder.Count)
                        user.Step = UserStep.FinalQuestion;
                    else
                    {
                        user.SampleStartTime = DateTime.Now; // set start view time
                        user.Step = UserStep.Sample;
                    }
                    break;
                case UserStep.Sample:
                    user.CurrentAnswer.TimeTaken = DateTime.Now - user.SampleStartTime; // set the time taken to read the sample
                    user.Step = UserStep.Question;
                    break;
                case UserStep.FinalQuestion:
                    user.Step = UserStep.Finish;
                    break;
            }
            OnChange();
        }

        #endregion

        #region Admin

        /// <summary>
        /// Log on the admin using the given password.
        /// Returns the ID of the admin token, or null
        /// if the password was incorrect.
        /// </summary>
        /// <param name="givenPass"></param>
        /// <returns></returns>
        public Guid? LogonAdmin(string givenPass)
        {
            if (givenPass == AdminPassword)
            {
                Guid id = Guid.NewGuid();
                m_admins.Add(id, DateTime.Now);
                return id;
            }
            return null;
        }

        /// <summary>
        /// Checks if the admin token is valid and resets the logon timeout.
        /// </summary>
        /// <param name="id">The ID of the admin to check.</param>
        /// <returns>Whether the ID is vaild.</returns>
        public bool CheckAdminLogon(Guid id)
        {
            if (m_admins.ContainsKey(id) && DateTime.Now - m_admins[id] < MaxAdminLogonCheckTime)
            {
                m_admins[id] = DateTime.Now;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Logs out the admin with the given ID.
        /// </summary>
        /// <param name="id">The admin to logout.</param>
        public void LogoutAdmin(Guid id)
        {
            if (m_admins.ContainsKey(id))
                m_admins.Remove(id);
        }

        #endregion

        #region Data Loading/Saving

        /// <summary>
        /// Loads the experiment from the specified path into the server data.
        /// </summary>
        /// <param name="path">The path of the experiment data to load.</param>
        public void LoadExperiment(string path)
        {
            m_experiment = JsonConvert.DeserializeObject<FullExperiment>(File.ReadAllText(path));
            Main.Log("Loaded experiment from {0}", path);
            OnChange();
        }

        /// <summary>
        /// Creates a new server manager object from the save file.
        /// </summary>
        /// <param name="path">The path of the server data file.</param>
        /// <returns>The loaded server manager.</returns>
        public static ServerManager FromFile(string path)
        {
            ServerManagerStore store = JsonConvert.DeserializeObject<ServerManagerStore>(File.ReadAllText(path));
            return new ServerManager(store.Users, store.Experiment);
        }

        /// <summary>
        /// Saves the current server data to the specified file.
        /// </summary>
        /// <param name="path">The file to save to.</param>
        public void SaveState(string path)
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(new ServerManagerStore()
            {
                Users = m_users,
                Experiment = m_experiment
            }));
        }

        /// <summary>
        /// Clears all of the current user data.
        /// </summary>
        public void ResetUserData()
        {
            m_users.Clear();
            OnChange();
        }

        /// <summary>
        /// Replaces the current experiment data with the new string data.
        /// Also replaces the experiment data file.
        /// </summary>
        /// <param name="path">The experiment data file.</param>
        /// <param name="data">The data to load into the server.</param>
        public void ReplaceExperimentData(string path, string data)
        {
            Main.Log("Replacing experiment data");
            m_changeEventFire = false;
            ResetUserData();
            File.WriteAllText(path, data);
            LoadExperiment(path);
            m_changeEventFire = true;
            OnChange();
        }

        /// <summary>
        /// Checks if the given string is valid experiment data that can be used.
        /// </summary>
        /// <param name="data">The string to check.</param>
        /// <returns>Whether the string is valid experiment data.</returns>
        public bool ValidExperimentData(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
                return false;
            try
            {
                JsonConvert.DeserializeObject<FullExperiment>(data);
                return true;
            }
            catch (Exception ex)
            {
                Main.Log("Experiment data validation failed");
                Main.Log(ex);
                return false;
            }
        }

        #endregion

    }
}
