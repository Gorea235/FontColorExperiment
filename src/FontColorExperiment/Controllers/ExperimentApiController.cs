using FontColorExperiment.Models;
using FontColorExperiment.Models.Experiment;
using FontColorExperiment.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FontColorExperiment.Controllers
{
    /// <summary>
    /// The experiment API controller.
    /// Handles the users participating in the
    /// experiment.
    /// </summary>
    [Route("api/experiment")]
    public class ExperimentApiController : Controller
    {
        /// <summary>
        /// The path to the sample part-page.
        /// </summary>
        private const string Path_Sample = "../Experiment/_Sample";
        /// <summary>
        /// The path to the question part-page.
        /// </summary>
        private const string Path_Question = "../Experiment/_Question";
        /// <summary>
        /// The path to the finish part-page.
        /// </summary>
        private const string Path_Finish = "../Experiment/_Finish";
        /// <summary>
        /// The path to the error part-page.
        /// </summary>
        private const string Path_Error = "../Experiment/_Error";

        // GET: api/experiment
        /// <summary>
        /// Default path, returns error page.
        /// </summary>
        /// <returns>(IActionResult)</returns>
        [HttpGet]
        public IActionResult Get()
        {
            Main.Log("Debug default get called, returning error page");
            return PartialView(Path_Error);
        }

        // POST: api/experiment/view
        /// <summary>
        /// Returns the current view page for the user.
        /// </summary>
        /// <param name="id">The user ID to get the view for.</param>
        /// <returns>(IActionResult)</returns>
        [HttpPost("view")]
        public IActionResult Post(string id)
        {
            Main.Log("Sending back current view for user {0}", id);
            try
            {
                Tuple<ExperimentCurrentSample, UserStep> current = Main.Manager.CurrentSample(Guid.Parse(id)); // get user & step
                object model = current.Item1;
                string path = "";
                switch (current.Item2) // choose the view to send back
                {
                    case UserStep.Finish:
                        path = Path_Finish;
                        model = id;
                        break;
                    case UserStep.Question:
                    case UserStep.FinalQuestion:
                        path = Path_Question;
                        break;
                    case UserStep.Sample:
                        path = Path_Sample;
                        break;
                }
                return PartialView(path, model); // send view only (without layout)
            }
            catch (Exception ex)
            {
#if DEBUG
                Main.Log(ex);
                return PartialView(Path_Error, ex);
#else
                Main.Log(ex);
                return PartialView(Path_Error);
#endif
            }
        }

        // POST: api/experiment/user/step
        /// <summary>
        /// Moves the current user to the next step and
        /// returns the result.
        /// </summary>
        /// <param name="id">The ID of the user to step.</param>
        /// <returns>(IActionResult)</returns>
        [HttpPost("user/step")]
        public IActionResult User_Step(string id)
        {
            Main.Log("got step request for user {0}", id);
            try
            {
                Main.Manager.StepUser(Guid.Parse(id));
                return Json(new Response(true));
            }
            catch (Exception ex)
            {
                Main.Log(ex);
                return Json(new Response(false, ex));
            }
        }

        // POST: api/experiment/user/answer
        /// <summary>
        /// Applies the answers to current user and current question.
        /// </summary>
        /// <param name="id">The ID of the user to set the answers for.</param>
        /// <param name="answers">The answers to set.</param>
        /// <returns>(IActionResult)</returns>
        [HttpPost("user/answer")]
        public IActionResult User_Answer(string id, string answers)
        {
            // convert answer string to object
            Dictionary<int, object> answerDict = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<int, object>>(answers);
            Main.Log("got {0} answers for current question for user {1}", answerDict.Count, id);
            try
            {
                // apply the answers to the current user
                Main.Manager.ApplyUserAnswers(Guid.Parse(id), answerDict);
                return Json(new Response(true));
            }
            catch (Exception ex)
            {
                Main.Log(ex);
                return Json(new Response(false, ex));
            }
        }

        // api/experiment/user/new
        /// <summary>
        /// Adds a new user to the server and returns the new ID.
        /// </summary>
        /// <returns>(string)</returns>
        [HttpGet("user/new")]
        public string User_New()
        {
            Main.Log("received new user request");
            return Main.Manager.NewUser().Id.ToString();
        }

        // api/experiment/user/exists
        /// <summary>
        /// Checks if a user exists in the server and returns to result.
        /// </summary>
        /// <param name="id">The ID of the user to check.</param>
        /// <returns>(bool)</returns>
        [HttpPost("user/exists")]
        public string User_Exists(string id)
        {
            Main.Log("checking if user {0} exists", id);
            try
            {
                return Main.Manager.UserExists(Guid.Parse(id)).ToString();
            }
            catch (Exception ex)
            {
                Main.Log(ex);
                return false.ToString();
            }
        }
    }
}
