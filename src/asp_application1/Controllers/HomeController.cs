﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace asp_application1.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Rules of the game";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Get in touch.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
