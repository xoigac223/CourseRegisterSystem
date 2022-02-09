using Microsoft.AspNetCore.Mvc;
using CourseRegisterSystem.Data;
using CourseRegisterSystem.DTO;
using Microsoft.EntityFrameworkCore;
using CourseRegisterSystem.Models;

namespace CourseRegisterSystem.Areas.Student.Controllers;

[Area("Student")]
public class CourseController: BaseController
{
    public CourseController(MyAppDbContext context, IConfiguration configuration) : base(context, configuration)
    {

    }   

    public async Task<IActionResult> List(string currentCourseId, string currentClassId, string courseId, string classId, int? pageNumber)
    {
        if (IsLogin()) {

            SetViewData();
            
            var termId = GetTermId();

            var term = await _context.Terms.FindAsync(Convert.ToInt64(termId));

            if (term != null) {
                ViewData["TermName"] = term.Name;
            }

            if (courseId != null || classId != null)
            {
                pageNumber = 1;
            } else {
                courseId = currentCourseId;
                classId = currentClassId;
            }

            ViewData["CurrentCourseId"] = courseId;
            ViewData["CurrentClassId"] = classId;

            var classes = _context.Classes.Include(c => c.Course)
            .Include(c => c.Term).Include(c => c.Lecturer).Where(c => c.TermId.Equals(termId));
            
            if (!String.IsNullOrEmpty(courseId)) 
            {
                classes = classes.Where(c => c.CourseId.Contains(courseId));
            }

            if (!String.IsNullOrEmpty(classId)) 
            {
                classes = classes.Where(c => c.Id.Equals(Int32.Parse(classId)));
            }

            int pageSize = 8;

            return View(await PaginatedList<CourseRegisterSystem.Models.Class>.CreateAsync(classes.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        return RedirectToAction(actionName: "Index", controllerName: "Login");
    }

    public async Task<IActionResult> Register()
    {
        if (IsLogin()) {

            SetViewData();

            var currentStudent = GetStudent();
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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Class>>> GetClasses() {
        var classes = _context.Classes.Include(c => c.Course).Where(c => c.TermId.Equals(GetTermId()));
        return await classes.ToListAsync();
    }

    [HttpPost]
    public JsonResult InsertEnrollment(List<Enrollment> enrollments) {
        foreach (var vEnrollment in enrollments)
        {
            vEnrollment.CreateAt = DateTime.Now.Date;
            _context.Enrollments.Add(vEnrollment);
        }
        int result = _context.SaveChanges();
        return Json(result);
    }
    
    [HttpPost]
    public JsonResult RemoveEnrollment(List<Enrollment> enrollments) {
        foreach (var vEnrollment in enrollments)
        {
            _context.Enrollments.Remove(vEnrollment);
        }
        int result = _context.SaveChanges();
        return Json(result);
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
}