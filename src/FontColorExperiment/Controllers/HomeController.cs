using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FontColorExperiment.Controllers
{
    /// <summary>
    /// The home index controller.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Returns the index page of the site.
        /// </summary>
        /// <returns>(IActionResult)</returns>
        public IActionResult Index()
        {
            return View((object)(Main.Manager.CurrentExperiment.MainPageText ?? ""));
        }

        /// <summary>
        /// Returns the error page of the site.
        /// </summary>
        /// <returns>(IActionResult)</returns>
        public IActionResult Error()
        {
            return View();
        }
    }
}
