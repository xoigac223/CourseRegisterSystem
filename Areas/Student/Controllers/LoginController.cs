using Microsoft.AspNetCore.Mvc;
using CourseRegisterSystem.Data;
using utils;

namespace CourseRegisterSystem.Areas.Student.Controllers;

[Area("Student")]
public class LoginController : BaseController
{
    public const string SessionKeyStudentId = "_StudentId";

    public LoginController(MyAppDbContext context, IConfiguration configuration) : base(context, configuration)
    {

    } 

    public IActionResult Index()
    {
        SetViewData();
        if (!IsLogin()) return View();
        else return RedirectToAction(actionName: "Index", controllerName: "Home");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index([Bind("Id, Password")] CourseRegisterSystem.Models.Student user)
    {
        if (ModelState.IsValid)
        {
            var student = await _context.Students.FindAsync(user.Id);

            if (student != null) {
                if (Helper.ComputeSha256Hash(user.Password).Equals(student.PasswordHash)) {
                    HttpContext.Session.SetString(SessionKeyStudentId, user.Id);
                    return RedirectToAction(actionName: "Index", controllerName: "Home");
                } 
            } 
        }
        SetViewData();
        ViewData["login-error"] = "Thông tin đăng nhập không chính xác";
        return View();
    }
}