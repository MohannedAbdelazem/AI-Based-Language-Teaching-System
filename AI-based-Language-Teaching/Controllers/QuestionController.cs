using AI_based_Language_Teaching.Models;
using AI_based_Language_Teaching.Service;
using Microsoft.AspNetCore.Mvc;

namespace AI_based_Language_Teaching.Controllers
{
    public class QuestionController : Controller
    {
        private readonly IQuestionService _questionService;

        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        public async Task<IActionResult> Index()
        {
            var questions = await _questionService.GetQuestionsAsync();
            return View(questions);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Question question)
        {
            if (ModelState.IsValid)
            {
                await _questionService.CreateQuestionAsync(question);
                return RedirectToAction(nameof(Index));
            }
            return View(question);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var question = await _questionService.GetQuestionByIdAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            return View(question);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Question question)
        {
            if (id != question.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _questionService.UpdateQuestionAsync(question);
                return RedirectToAction(nameof(Index));
            }
            return View(question);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var question = await _questionService.GetQuestionByIdAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            return View(question);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _questionService.DeleteQuestionAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

