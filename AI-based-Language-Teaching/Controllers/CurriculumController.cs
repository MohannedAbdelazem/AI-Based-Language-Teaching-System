using AI_based_Language_Teaching.Models;
using Microsoft.AspNetCore.Mvc;

namespace AI_based_Language_Teaching.Controllers
{
    public class CurriculumController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CurriculumController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var curricula = _context.Curricula.ToList();
            return View(curricula);
        }

        public IActionResult Details(int id)
        {
            var curriculum = _context.Curricula.FirstOrDefault(c => c.Id == id);
            if (curriculum == null)
            {
                return NotFound();
            }
            return View(curriculum);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Curriculum curriculum)
        {
            if (ModelState.IsValid)
            {
                _context.Curricula.Add(curriculum);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(curriculum);
        }

        public IActionResult Edit(int id)
        {
            var curriculum = _context.Curricula.FirstOrDefault(c => c.Id == id);
            if (curriculum == null)
            {
                return NotFound();
            }
            return View(curriculum);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Curriculum curriculum)
        {
            if (id != curriculum.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Curricula.Update(curriculum);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(curriculum);
        }

        public IActionResult Delete(int id)
        {
            var curriculum = _context.Curricula.FirstOrDefault(c => c.Id == id);
            if (curriculum == null)
            {
                return NotFound();
            }
            return View(curriculum);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var curriculum = _context.Curricula.FirstOrDefault(c => c.Id == id);
            if (curriculum != null)
            {
                _context.Curricula.Remove(curriculum);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

}
}
