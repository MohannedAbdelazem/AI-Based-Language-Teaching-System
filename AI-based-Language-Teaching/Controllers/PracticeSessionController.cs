using AI_based_Language_Teaching.Models;
using AI_based_Language_Teaching.Service;
using Microsoft.AspNetCore.Mvc;

namespace AI_based_Language_Teaching.Controllers
{
    public class PracticeSessionController : Controller
    {
        private readonly IPracticeSessionService _practiceSessionService;

        public PracticeSessionController(IPracticeSessionService practiceSessionService)
        {
            _practiceSessionService = practiceSessionService;
        }

        public async Task<IActionResult> Index()
        {
            var sessions = await _practiceSessionService.GetSessionsAsync();
            return View(sessions);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PracticeSession session)
        {
            if (ModelState.IsValid)
            {
                await _practiceSessionService.StartSessionAsync(session);
                return RedirectToAction(nameof(Index));
            }
            return View(session);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var session = await _practiceSessionService.GetSessionByIdAsync(id);
            if (session == null)
            {
                return NotFound();
            }
            return View(session);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PracticeSession session)
        {
            if (id != session.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _practiceSessionService.EndSessionAsync(id);
                return RedirectToAction(nameof(Index));
            }
            return View(session);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var session = await _practiceSessionService.GetSessionByIdAsync(id);
            if (session == null)
            {
                return NotFound();
            }
            return View(session);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _practiceSessionService.EndSessionAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
