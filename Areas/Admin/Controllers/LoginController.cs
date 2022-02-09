using Microsoft.AspNetCore.Mvc;

namespace CourseRegisterSystem.Areas.Admin.Controllers;

[Area("Admin")]
public class LoginController : Controller
{
    public string Index()
    {
        return "Admin Area";
    }
}