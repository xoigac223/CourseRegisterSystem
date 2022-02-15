using CourseRegisterSystem.Data;
using CourseRegisterSystem.Models;
using CourseRegisterSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourseRegisterSystem.Areas.Admin.Controllers;

[Area("Admin")]
public class BaseController : Controller
{
    protected readonly MyAppDbContext _context;
    
    private readonly IConfiguration Configuration;

    protected readonly ISendMailService SendMailService;
    
    public BaseController(MyAppDbContext context, IConfiguration configuration, ISendMailService sendMailService) {
        _context = context;
        Configuration = configuration;
        SendMailService = sendMailService;
    }

    public Config GetConfig()
    {
        DateTime startDate = DateTime.Parse(Configuration["AppConfig:StartDate"]);
        DateTime expiryDate = DateTime.Parse(Configuration["AppConfig:ExpiryDate"]);
        long termId = Int32.Parse(Configuration["AppConfig:TermId"]);
        return new Config(termId, startDate, expiryDate);
    }
}