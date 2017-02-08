using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FontColorExperiment.Controllers
{
    /// <summary>
    /// The experiment controller.
    /// </summary>
    public class ExperimentController : Controller
    {
        // GET: /<controller>/
        /// <summary>
        /// Returns the index page.
        /// </summary>
        /// <returns>(IActionResult)</returns>
        public IActionResult Index()
        {
            return View();
        }

#if DEBUG
        /// <summary>
        /// A test function for returning a page outside the current page folder.
        /// </summary>
        /// <returns>(IActionResult)</returns>
        public IActionResult Tmp()
        {
            return View("../Home/Index");
        }
#endif
    }
}
