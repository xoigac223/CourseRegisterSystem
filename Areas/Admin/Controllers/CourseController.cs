using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using CourseRegisterSystem.Data;
using CourseRegisterSystem.Models;
using CourseRegisterSystem.Services;
using Microsoft.EntityFrameworkCore;

namespace CourseRegisterSystem.Areas.Admin.Controllers;

public class CourseController : BaseController
{
    public CourseController(MyAppDbContext context, IConfiguration configuration, ISendMailService sendMailService) : base(context, configuration, sendMailService)
    {

    }

    public async Task<ActionResult> Index(
        string startDate,
        string endDate)
    {
        var enrollments = from e in _context.Enrollments select e;
        enrollments = enrollments.Include(e => e.Class).Include(e => e.Student);
        if (!String.IsNullOrEmpty(startDate) && !String.IsNullOrEmpty(endDate))
        {
            enrollments = enrollments.Where(e => e.CreateAt >= Convert.ToDateTime(startDate, new CultureInfo("en-GB") 
            ) && e.CreateAt <= Convert.ToDateTime(endDate, new CultureInfo("en-GB")));
        }

        return View(await enrollments.AsNoTracking().ToListAsync());
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("ClassId,StudentId")] Enrollment enrollment)
    {
        var enrollmentClass = await _context.Classes.FirstOrDefaultAsync(e => e.Id == enrollment.ClassId);
        var student = await _context.Students.Include(s => s.Enrollments).FirstOrDefaultAsync(s => s.Id.Equals(enrollment.StudentId));
        var e = await _context.Enrollments.FindAsync(enrollment.ClassId, enrollment.StudentId);
        
        if (ModelState.IsValid)
        {
            return View(enrollment);
        } 
        else if (enrollmentClass == null)
        {
            ViewData["create-error"] = "Lớp không tồn tại";
            return View(enrollment);
        } 
        else if (student == null)
        {
            ViewData["create-error"] = "Học sinh không tồn tại";
            return View(enrollment);
        }
        else if (e != null)
        {
            ViewData["create-error"] = "Học sinh đã đăng ký lớp chỉ định";
            return View(enrollment);
        }
        else if (student.Enrollments.Count >= 3)
        {
            ViewData["create-error"] = "Số tín chỉ đăng ký của học sinh " + enrollment.StudentId + " đã vượt quá 3";
            return View(enrollment);
        }

        enrollment.CreateAt = DateTime.Now.Date;
        _context.Add(enrollment);
        await _context.SaveChangesAsync();
        var htmlMessage = "Đăng ký thành công mã lớp " + enrollmentClass.Id + " cho sinh viên " + student.Id;
        await SendMailService.SendEmailAsync(student.Email, "Đăng ký thành công", htmlMessage);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(long? classId, string? studentId)
    {
        var e = await _context.Enrollments.FindAsync(classId, studentId);
        var student = await _context.Students.FindAsync(studentId);
        _context.Enrollments.Remove(e);
        await _context.SaveChangesAsync();
        var htmlMessage = "Hủy đăng ký thành công mã lớp " + classId + " cho sinh viên " + studentId;
        await SendMailService.SendEmailAsync(student.Email, "Hủy đăng ký thành công", htmlMessage);
        return RedirectToAction(nameof(Index));
    }
}