using AI_based_Language_Teaching.Models;

namespace AI_based_Language_Teaching.Service
{
    public interface ILessonService
    {
        IEnumerable<Lesson> GetLessonsByCurriculumId(int curriculumId);
        Lesson GetLessonById(int id);
        void CreateLesson(Lesson lesson);
        void UpdateLesson(Lesson lesson);
        void DeleteLesson(int id);
    }
}