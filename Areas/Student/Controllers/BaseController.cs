using Microsoft.AspNetCore.Mvc;
using CourseRegisterSystem.Data;

namespace CourseRegisterSystem.Areas.Student.Controllers;

[Area("Student")]
public class BaseController: Controller
{
    protected readonly MyAppDbContext _context;

    private readonly IConfiguration Configuration;

    protected CourseRegisterSystem.Models.Student? student;

    public BaseController(MyAppDbContext context, IConfiguration configuration) {
        _context = context;
        Configuration = configuration;
    }

    public CourseRegisterSystem.Models.Student GetStudent() {
        if (this.student == null) {
            var studentId = HttpContext.Session.GetString(LoginController.SessionKeyStudentId);
            if (studentId != null) {
                this.student = _context.Students.Find(studentId);
            }   
        }
        return this.student;
    }

    public bool IsLogin() {
        return !String.IsNullOrEmpty(HttpContext.Session.GetString(LoginController.SessionKeyStudentId));
    }

    public void SetViewData() {
        DateTime startDate = DateTime.Parse(Configuration["AppConfig:StartDate"]);
        DateTime expiryDate = DateTime.Parse(Configuration["AppConfig:ExpiryDate"]);
        ViewData["IsLogin"] = IsLogin();
        ViewData["IsActive"] = DateTime.Compare(expiryDate, DateTime.Now) >= 0 &&
                                DateTime.Compare(startDate, DateTime.Now) <= 0;
        ViewData["StudentId"] = HttpContext.Session.GetString(LoginController.SessionKeyStudentId);
    }

    public int GetTermId() {
        return Int32.Parse(Configuration["AppConfig:TermId"]);
    }
}