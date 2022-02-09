using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseRegisterSystem.Models
{
    public partial class Class
    {
        public Class()
        {
            Enrollments = new HashSet<Enrollment>();
        }

        [Display(Name = "Mã học phần")]
        public string CourseId { get; set; } = null!;
        public long LecturerId { get; set; }
        public long TermId { get; set; }

        [Display(Name = "Số lượng tối đa")]
        public long Max { get; set; }

        [Display(Name = "Mã lớp")]
        public long Id { get; set; }

        [NotMapped]    
        [Display(Name = "Số lượng")]
        public int Quantity { get; set; }

        public virtual Course Course { get; set; } = null!;
        public virtual Teacher Lecturer { get; set; } = null!;
        public virtual Term Term { get; set; } = null!;
        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}
