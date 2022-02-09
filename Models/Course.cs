using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseRegisterSystem.Models
{
    public partial class Course
    {
        public Course()
        {
            Classes = new HashSet<Class>();
        }
        [Display(Name = "Mã học phần")]
        public string Code { get; set; } = null!;

        [Display(Name = "Tên học phần")]
        public string Name { get; set; } = null!;
        [Display(Name = "Số tín chỉ")]
        public long Credit { get; set; }

        public virtual ICollection<Class> Classes { get; set; }
    }
}
