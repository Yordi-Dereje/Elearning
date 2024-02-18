using E_LearningWebApp.Areas.Identity.Data;
using E_LearningWebApp.Data;
using E_LearningWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace E_LearningWebApp.Repository
{
    public class UserRepository
    {
        private E_LearningDbContext _context;
        public UserRepository(E_LearningDbContext _context)
        {
            this._context = _context;
            
        }
      /*  public Payment AddPayment(Payment payments)
        {
            _context.Payment.Add(payments);
            _context.SaveChanges();
            return payments;
        }*/
        /* public void Updateuser(E_LearningWebAppUser updateuser)
         {
             var updated = _appcontext.E_LearningWebAppUser.Find(p => p.id == updatedCourse.CourseId);
             if (updated != null)
             {
                 _context.Courses.Update(updated);
                 _context.SaveChanges();
             }
         }*/
    }
}
