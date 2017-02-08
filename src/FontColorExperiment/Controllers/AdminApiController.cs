using FontColorExperiment.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FontColorExperiment.Controllers
{
    /// <summary>
    /// The admin API controller.
    /// Handles the authentication of admins and functions on the
    /// admin page.
    /// </summary>
    [Route("api/admin")]
    public class AdminApiController : Controller
    {
        /// <summary>
        /// Checks if the admin key is valid.
        /// </summary>
        /// <returns>Whether the current request contains a valid admin key.</returns>
        public bool ValidAdmin()
        {
            return Main.Manager.CheckAdminLogon(Guid.Parse(Request.Cookies[Utils.ServerManager.AdminTokenKey]));
        }

        // POST: api/admin/auth
        /// <summary>
        /// Authenticates the user using the given password.
        /// If they are successful, then they are redirected to the admin page.
        /// If the fail, they are redirected to the login page again but with
        /// the 'failed' query set.
        /// </summary>
        /// <param name="pwd">The password the user entered.</param>
        /// <returns>(IActionResult)</returns>
        [HttpPost("auth")]
        public IActionResult Auth(string pwd)
        {
            Main.Log("Authenticating new admin");
            Guid? newid = Main.Manager.LogonAdmin(pwd); // logon user
            if (newid != null) // check if succesful
            {
                // set the cookie and redirect to admin page
                Guid id = (Guid)newid;
                Response.Cookies.Append(Utils.ServerManager.AdminTokenKey, id.ToString());
                Main.Log("Admin successfully authenticated");
                return Redirect("/admin");
            }
            else
            {
                // remove cookie if it exists and redirect to login with 'failed' query
                if (Request.Cookies.ContainsKey(Utils.ServerManager.AdminTokenKey))
                    Response.Cookies.Delete(Utils.ServerManager.AdminTokenKey);
                Main.Log("Admin failed authentication");
                return Redirect("/admin/login?failed=true");
            }
        }

        // POST: api/admin/validate
        /// <summary>
        /// Allows the page to validate the current admin.
        /// Mainly to prevent lock-out after 30 mins.
        /// </summary>
        /// <returns>(IActionResult)</returns>
        [HttpPost("validate")]
        public IActionResult Validate()
        {
            try
            {
                if (ValidAdmin())
                    return Ok();
            }
            catch (Exception ex)
            {
                Main.Log(ex);
            }
            return Unauthorized();
        }

        // POST: api/admin/experimentupload
        /// <summary>
        /// Receives the experiment data file in order to replace the current
        /// file on the server.
        /// </summary>
        /// <param name="file">The file the user uploaded.</param>
        /// <returns>(IActionResult)</returns>
        [HttpPost("experimentupload")]
        public IActionResult ExperimentUpload(IFormFile file)
        {
            Main.Log("Attempting to upload new experiment file from admin {0}", Request.Cookies[Utils.ServerManager.AdminTokenKey]);
            if (!ValidAdmin()) // validate admin
            {
                Main.Log("admin not authenticated");
                return Redirect("/admin/login");
            }
            if (file == null) // check if file is null
            {
                Main.Log("admin didn't upload a file");
                return Redirect("/admin?upload=0");
            }
            string data;
            string tmpPath = Path.GetTempFileName();
            using (FileStream writer = new FileStream(tmpPath, FileMode.Create)) // write data to tmp file
            {
                file.CopyTo(writer);
            }
            data = System.IO.File.ReadAllText(tmpPath); // load string to use as new data
            if (Main.Manager.ValidExperimentData(data)) // check if valid data
            {
                // set data and emit success
                Main.SetExperimentData(data);
                Main.Log("Completed experiment file update");
                return Redirect("/admin?upload=1");
            }
            // ignore data and emit failure
            Main.Log("Experiment file was not valid, ignoring");
            return Redirect("/admin?upload=0");
        }

        // GET: api/admin/logout
        /// <summary>
        /// Logs out the current user.
        /// </summary>
        /// <returns>(IActionResult)</returns>
        [HttpGet("logout")]
        public IActionResult Logout()
        {
            Main.Log("Logging out admin {0}", Request.Cookies[Utils.ServerManager.AdminTokenKey]);
            try
            {
                if (!ValidAdmin()) // check if admin is logged in
                    return BadRequest();
                // logout user
                Main.Manager.LogoutAdmin(Guid.Parse(Request.Cookies[Utils.ServerManager.AdminTokenKey]));
                return Ok();
            }
            catch (Exception ex)
            {
                Main.Log("Unable to logout admin {0}", Request.Cookies[Utils.ServerManager.AdminTokenKey]);
                Main.Log(ex);
                return BadRequest();
            }
        }

        // GET: api/admin/resetusers
        /// <summary>
        /// Resets the user data.
        /// </summary>
        /// <returns>(IActionResult)</returns>
        [HttpGet("resetusers")]
        public IActionResult ResetUsers()
        {
            if (!ValidAdmin()) // check if admin is logged in
                return Unauthorized();
            // reset user data
            Main.Manager.ResetUserData();
            Main.Log("Admin {0} reset all user data", Request.Cookies[Utils.ServerManager.AdminTokenKey]);
            return Ok();
        }

        // GET: api/admin/data
        /// <summary>
        /// Sends a json file of the current user data to the admin.
        /// </summary>
        /// <returns></returns>
        [HttpGet("data")]
        public IActionResult Data()
        {
            if (!ValidAdmin())
                return Unauthorized();
            Main.Log("Sending data file to admin");
            // convert user data to simple form
            List<UserSimpleData> data = new List<UserSimpleData>();
            foreach (UserData user in Main.Manager.CurrentUsers)
                data.Add(new UserSimpleData(user));
            // send data to user
            return File(System.Text.Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(data)), "application/json", "UserAnswers.json");
        }
    }
}
