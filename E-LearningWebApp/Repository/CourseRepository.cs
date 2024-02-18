using E_LearningWebApp.Data;
using E_LearningWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace E_LearningWebApp.Repository
{
    public class CourseRepository
    {
        private E_LearningDbContext _context;

        public CourseRepository( E_LearningDbContext _context)
        {
            this._context = _context;            
        }
        public Courses AddCourse(Courses course)
        {
            _context.Courses.Add(course);
            _context.SaveChanges();
            return course;
        }
        public List<Courses> GetAllCourses()
        {
            return this._context.Courses.AsNoTracking().ToList();
        }
        public Courses GetCourseById(int id)
        {
           var courses = this._context.Courses.Find(id);
            return courses;
        }
        public void UpdateCourse(Courses updatedCourse)
        {
            var updated  = _context.Courses.AsNoTracking().FirstOrDefault(p =>p.CourseId == updatedCourse.CourseId);
            if (updated != null)
            {
                _context.Courses.Update(updated);
                _context.SaveChanges();
            }            
        }
        public void DeleteCourse(Courses courses)
        {
            var deletecourse = _context.Courses.AsNoTracking().FirstOrDefault(p => p.CourseId == courses.CourseId);
            if(deletecourse != null)
            {
                _context.Remove(deletecourse);
                _context.SaveChanges();
            }
           
        }
    }
}
