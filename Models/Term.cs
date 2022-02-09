using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseRegisterSystem.Models
{
    public partial class Term
    {
        public Term()
        {
            Classes = new HashSet<Class>();
        }

        public long Id { get; set; }

        [Display(Name = "Kì học")]
        public string Name { get; set; } = null!;
        public string StartDate { get; set; } = null!;
        public string EndDate { get; set; } = null!;

        public virtual ICollection<Class> Classes { get; set; }
    }
}
