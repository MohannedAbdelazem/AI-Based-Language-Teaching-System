using AI_based_Language_Teaching.Models;

namespace AI_based_Language_Teaching.Service
{
    public class CurriculumService : ICurriculumService
    {
        private readonly ApplicationDbContext _context;

        public CurriculumService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Curriculum> GetAllCurricula()
        {
            return _context.Curricula.ToList();
        }

        public Curriculum GetCurriculumById(int id)
        {
            return _context.Curricula.FirstOrDefault(c => c.Id == id);
        }

        public void CreateCurriculum(Curriculum curriculum)
        {
            _context.Curricula.Add(curriculum);
            _context.SaveChanges();
        }

        public void UpdateCurriculum(Curriculum curriculum)
        {
            _context.Curricula.Update(curriculum);
            _context.SaveChanges();
        }

        public void DeleteCurriculum(int id)
        {
            var curriculum = _context.Curricula.FirstOrDefault(c => c.Id == id);
            if (curriculum != null)
            {
                _context.Curricula.Remove(curriculum);
                _context.SaveChanges();
            }
        }
    }
}