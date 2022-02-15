using Microsoft.AspNetCore.Mvc;
using CourseRegisterSystem.Data;
using CourseRegisterSystem.Models;
using CourseRegisterSystem.Services;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CourseRegisterSystem.Areas.Admin.Controllers;

[Area("Admin")]
public class HomeController : BaseController
{
    public HomeController(MyAppDbContext context, IConfiguration configuration, ISendMailService sendMailService) : base(context, configuration, sendMailService)
    {

    } 
    public IActionResult Index()
    {
        Config config = GetConfig();
        PopulateTermsDropDownList(config.TermId);
        return View(config);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Index([Bind("TermId,StartDate,ExpiryDate")] Config config)
    {
        if (ModelState.IsValid)
        {
            config.SaveConfig();
        }
        PopulateTermsDropDownList(config.TermId);
        return View(config);
    }
    
    private void PopulateTermsDropDownList(object selectedTerm)
    {
        var termsQuery = from t in _context.Terms orderby t.Name select t;
        ViewBag.TermId = new SelectList(termsQuery, "Id", "Name", selectedTerm);
    }
}