using AI_based_Language_Teaching.Models;
using Microsoft.AspNetCore.Mvc;

namespace AI_based_Language_Teaching.Controllers
{
    public class LessonController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LessonController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int curriculumId)
        {
            var lessons = _context.Lessons.Where(l => l.CurriculumId == curriculumId).ToList();
            ViewBag.CurriculumId = curriculumId;
            return View(lessons);
        }

        public IActionResult Details(int id)
        {
            var lesson = _context.Lessons.FirstOrDefault(l => l.Id == id);
            if (lesson == null)
            {
                return NotFound();
            }
            return View(lesson);
        }

        public IActionResult Create(int curriculumId)
        {
            ViewBag.CurriculumId = curriculumId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Lesson lesson)
        {
            if (ModelState.IsValid)
            {
                _context.Lessons.Add(lesson);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index), new { curriculumId = lesson.CurriculumId });
            }
            return View(lesson);
        }

        public IActionResult Edit(int id)
        {
            var lesson = _context.Lessons.FirstOrDefault(l => l.Id == id);
            if (lesson == null)
            {
                return NotFound();
            }
            return View(lesson);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Lesson lesson)
        {
            if (id != lesson.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Lessons.Update(lesson);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index), new { curriculumId = lesson.CurriculumId });
            }
            return View(lesson);
        }

        public IActionResult Delete(int id)
        {
            var lesson = _context.Lessons.FirstOrDefault(l => l.Id == id);
            if (lesson == null)
            {
                return NotFound();
            }
            return View(lesson);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var lesson = _context.Lessons.FirstOrDefault(l => l.Id == id);
            if (lesson != null)
            {
                _context.Lessons.Remove(lesson);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index), new { curriculumId = lesson.CurriculumId });
        }
    }
}
