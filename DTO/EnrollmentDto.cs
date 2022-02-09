using System.ComponentModel.DataAnnotations;
using CourseRegisterSystem.Models;

namespace CourseRegisterSystem.DTO;

public class EnrollmentDto
{
    [Display(Name = "Mã lớp")]
    public long ClassId { get; set; }
    
    public string StudentId { get; set; }
    
    [Display(Name = "Ngày đăng ký")]
    [DataType(DataType.Date)]
    public DateTime? CreateAt { get; set; }
    
    [Display(Name = "Mã học phần")]
    public string? CourseId { get; set; }
    
    [Display(Name = "Tên học phần")]
    public string? CourseName { get; set; }
    public int Status { get; set; }
}