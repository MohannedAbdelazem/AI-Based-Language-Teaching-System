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

        public  IActionResult Index()
        {
            var answers =  _answerService.GetAnswers();
            return View(answers);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public  IActionResult Create(Answer answer)
        {
            if (ModelState.IsValid)
            {
                 _answerService.CreateAnswer(answer);
                return RedirectToAction(nameof(Index));
            }
            return View(answer);
        }

        public  IActionResult Edit(int id)
        {
            var answer =  _answerService.GetAnswerById(id);
            if (answer == null)
            {
                return NotFound();
            }
            return View(answer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public  IActionResult Edit(int id, Answer answer)
        {
            if (id != answer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                 _answerService.UpdateAnswer(answer);
                return RedirectToAction(nameof(Index));
            }
            return View(answer);
        }

        public  IActionResult Delete(int id)
        {
            var answer =  _answerService.GetAnswerById(id);
            if (answer == null)
            {
                return NotFound();
            }
            return View(answer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public  IActionResult DeleteConfirmed(int id)
        {
             _answerService.DeleteAnswer(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
