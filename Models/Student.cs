using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseRegisterSystem.Models
{
    public partial class Student
    {
        public Student()
        {
            Enrollments = new HashSet<Enrollment>();
        }

        [Display(Name = "Mã số sinh viên")]
        [Required]
        public string Id { get; set; } = null!;

        [Display(Name = "Mật khẩu")]
        [Required]
        [NotMapped]
        public string Password { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}
