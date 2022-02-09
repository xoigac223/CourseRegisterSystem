using System.ComponentModel.DataAnnotations;

namespace CourseRegisterSystem.Models
{
    public partial class Teacher
    {
        public Teacher()
        {
            Classes = new HashSet<Class>();
        }

        public long Id { get; set; }

        [Display(Name = "Tên giáo viên")]
        public string Name { get; set; } = null!;

        public virtual ICollection<Class> Classes { get; set; }
    }
}
