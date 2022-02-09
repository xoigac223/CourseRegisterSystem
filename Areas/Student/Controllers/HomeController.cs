using Microsoft.AspNetCore.Mvc;
using CourseRegisterSystem.Data;
using Microsoft.EntityFrameworkCore;
using CourseRegisterSystem.DTO;
using CourseRegisterSystem.Models;

namespace CourseRegisterSystem.Areas.Student.Controllers;

[Area("Student")]
public class HomeController : BaseController
{
    public HomeController(MyAppDbContext context, IConfiguration configuration) : base(context, configuration)
    {

    } 

    public async Task<IActionResult> Index()
    {
        if (IsLogin())
        {
            SetViewData();
            Models.Student currentStudent = GetStudent();
            ViewData["StudentName"] = currentStudent.Name;
            var termId = GetTermId();
            var enrollments = await _context.Enrollments.Include(e => e.Class).Include(e => e.Student).Where(e => e.StudentId.Equals(currentStudent.Id))
                .Where(e => e.Class.TermId.Equals(GetTermId())).ToListAsync();

            List<EnrollmentDto> enrollmentDtos = new List<EnrollmentDto>();
            foreach (var vEnrollment in enrollments)
            {
                enrollmentDtos.Add(ToEnrollmentDto(vEnrollment));
            }
            
            return View(enrollmentDtos);
        }
        return RedirectToAction(actionName: "Index", controllerName: "Login");
    }
    
    public EnrollmentDto ToEnrollmentDto(Enrollment enrollment)
    {
        EnrollmentDto enrollmentDto = new EnrollmentDto();
        enrollmentDto.ClassId = enrollment.ClassId;
        enrollmentDto.CourseId = enrollment.Class.CourseId;
        enrollmentDto.CreateAt = enrollment.CreateAt;
        enrollmentDto.StudentId = enrollment.StudentId;
        enrollmentDto.Status = 0;
        enrollmentDto.CourseName = _context.Courses.FirstOrDefault(c => c.Code.Equals(enrollment.Class.CourseId)).Name;
        return enrollmentDto;
    }

    public IActionResult Logout()
    {
        HttpContext.Session.SetString(LoginController.SessionKeyStudentId, "");
        return RedirectToAction(actionName: "Index", controllerName: "Login");
    }
}
