using E_LearningWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace E_LearningWebApp.Repository
{
    public class SubCourseRepository
    {
        private E_LearningDbContext _context;
        public SubCourseRepository(E_LearningDbContext _context)
        {
            this._context = _context;
        }
        public SubCourses AddSubCourse(SubCourses course)
        {
            _context.SubCourses.Add(course);
            _context.SaveChanges();
            return course;
        }
        public List<SubCourses> GetAllSubCourse(int courseId)
        {
            /*return this._context.SubCourses.ToList();..itworks dont touch it*/
            return this._context.SubCourses.AsNoTracking().Where(c => c.CourseId == courseId).ToList();
        }
        public SubCourses GetSubCourseById(int id)
        {
            SubCourses? SubCourses = this._context.SubCourses.Find(id);
            return SubCourses;
        }
       /* public IEnumerable<SubCourses> GetSubCourseByCourseId(int courseId)
        {
           was supposted to replace getallsubcourse so we need one methode in the future for that
            return this._context.SubCourses.Where(c => c.CourseId == courseId).ToList();

        }*/
        public void UpdateSubCourse(SubCourses updatedsubCourse)
        {
            var subcourses = _context.SubCourses.AsNoTracking().FirstOrDefaultAsync(subcourse => subcourse.SubCourseId == updatedsubCourse.SubCourseId);

            if (subcourses != null)
            {
                _context.Update(updatedsubCourse);
                _context.SaveChanges();
            }
        }
        public void DeleteSubCourse(SubCourses subcourses)
        {
            var deletecourse = _context.SubCourses.AsNoTracking().FirstOrDefault(p => p.SubCourseId == subcourses.SubCourseId);
            if (deletecourse != null)
            {
                _context.Remove(deletecourse);
                _context.SaveChanges();
            }

        }
       
    }
}
