using AI_based_Language_Teaching.Models;

namespace AI_based_Language_Teaching.Service
{
    public interface ICurriculumService
    {
        IEnumerable<Curriculum> GetAllCurricula();
        Curriculum GetCurriculumById(int id);
        void CreateCurriculum(Curriculum curriculum);
        void UpdateCurriculum(Curriculum curriculum);
        void DeleteCurriculum(int id);
    }
}
