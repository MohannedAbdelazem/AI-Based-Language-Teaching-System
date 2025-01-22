using AI_based_Language_Teaching.Models;
using AI_based_Language_Teaching.Service;
using Microsoft.AspNetCore.Mvc;

namespace AI_based_Language_Teaching.Controllers
{
    public class AnswerController : Controller
    {
        private readonly IAnswerService _answerService;

        public AnswerController(IAnswerService answerService)
        {
            _answerService = answerService;
        }

        public async Task<IActionResult> Index()
        {
            var answers = await _answerService.GetAnswersAsync();
            return View(answers);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Answer answer)
        {
            if (ModelState.IsValid)
            {
                await _answerService.CreateAnswerAsync(answer);
                return RedirectToAction(nameof(Index));
            }
            return View(answer);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var answer = await _answerService.GetAnswerByIdAsync(id);
            if (answer == null)
            {
                return NotFound();
            }
            return View(answer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Answer answer)
        {
            if (id != answer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _answerService.UpdateAnswerAsync(answer);
                return RedirectToAction(nameof(Index));
            }
            return View(answer);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var answer = await _answerService.GetAnswerByIdAsync(id);
            if (answer == null)
            {
                return NotFound();
            }
            return View(answer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _answerService.DeleteAnswerAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
