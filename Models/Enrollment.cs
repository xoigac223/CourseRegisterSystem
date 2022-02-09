using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseRegisterSystem.Models
{
    public partial class Enrollment
    {
        public long ClassId { get; set; }
        public string StudentId { get; set; } = null!;

        [DataType(DataType.Date)]
        public DateTime? CreateAt { get; set; }

        public virtual Class Class { get; set; } = null!;
        public virtual Student Student { get; set; } = null!;
    }
}
