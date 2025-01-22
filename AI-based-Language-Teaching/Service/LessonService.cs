using AI_based_Language_Teaching.Models;

namespace AI_based_Language_Teaching.Service
{
    public class LessonService : ILessonService
    {
        private readonly ApplicationDbContext _context;

        public LessonService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Lesson> GetLessonsByCurriculumId(int curriculumId)
        {
            return _context.Lessons.Where(l => l.CurriculumId == curriculumId).ToList();
        }

        public Lesson GetLessonById(int id)
        {
            return _context.Lessons.FirstOrDefault(l => l.Id == id);
        }

        public void CreateLesson(Lesson lesson)
        {
            _context.Lessons.Add(lesson);
            _context.SaveChanges();
        }

        public void UpdateLesson(Lesson lesson)
        {
            _context.Lessons.Update(lesson);
            _context.SaveChanges();
        }

        public void DeleteLesson(int id)
        {
            var lesson = _context.Lessons.FirstOrDefault(l => l.Id == id);
            if (lesson != null)
            {
                _context.Lessons.Remove(lesson);
                _context.SaveChanges();
            }
        }
    }
}
