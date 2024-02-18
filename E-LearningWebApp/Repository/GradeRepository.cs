using E_LearningWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace E_LearningWebApp.Repository
{
    public class GradeRepository
    {
        private E_LearningDbContext _context;
        public GradeRepository(E_LearningDbContext _context)
        {
            this._context = _context;
        }
        public Grade AddCourse(Grade gr)
        {
            _context.Grades.Add(gr);
            _context.SaveChanges();
            return gr;
        }
        /*public List<Grade> GetAllGrade(int id)
        {
            return this._context.Grades.AsNoTracking().ToList().Find(id);
        }*/
        public Courses GetCourseById(int id)
        {
            var courses = this._context.Courses.Find(id);
            return courses;
        }
    }
}
