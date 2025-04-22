using AI_based_Language_Teaching.Models;
using AI_based_Language_Teaching.Service;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AI_based_Language_Teaching.Controllers
{
    public class ProgressController : Controller
    {
        private readonly IProgressService _progressService;

        public ProgressController(IProgressService progressService)
        {
            _progressService = progressService;
        }

        public  IActionResult Details(string userId)
        {
            var progress =  _progressService.GetProgressByUserId(userId);
            if (progress == null)
            {
                return NotFound();
            }
            return View(progress);
        }

        public IActionResult Edit(string userId)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Progress progress)
        {
            if (ModelState.IsValid)
            {
                 _progressService.UpdateProgress(progress);
                return RedirectToAction("Details", new { userId = progress.UserId });
            }
            return View(progress);
        }
    }
}
