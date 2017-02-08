using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FontColorExperiment.Controllers
{
    /// <summary>
    /// The admin section controller.
    /// Returns the admin and admin login pages.
    /// </summary>
    public class AdminController : Controller
    {
        // GET: /<controller>/
        /// <summary>
        /// Returns the index page.
        /// </summary>
        /// <param name="upload">The upload result query.</param>
        /// <returns>(IActionResult)</returns>
        public IActionResult Index(int upload = -1)
        {
            Main.Log("Checking admin login and handing back admin page");
            try
            {
                if (Request.Cookies.ContainsKey(Utils.ServerManager.AdminTokenKey) &&
                    Main.Manager.CheckAdminLogon(Guid.Parse(Request.Cookies[Utils.ServerManager.AdminTokenKey])))
                    return View(new Models.AdminPageData(Main.Manager.CurrentUsers, Main.Manager.CurrentExperiment, upload, Main.LogStore));
            }
            catch (Exception ex)
            {
                Main.Log(ex);
            }
            return Redirect("/admin/login");
        }

        // GET: /admin/login
        /// <summary>
        /// Returns the login page.
        /// </summary>
        /// <param name="failed">The failed result query.</param>
        /// <returns>(IActionResult)</returns>
        public IActionResult Login(bool failed)
        {
            Main.Log("Showing admin login page");
            if (Request.Cookies.ContainsKey(Utils.ServerManager.AdminTokenKey))
                Response.Cookies.Delete(Utils.ServerManager.AdminTokenKey);
            return View(failed);
        }
    }
}
