using System;
using System.Collections.Generic;

namespace CourseRegisterSystem.Models
{
    public partial class Admin
    {
        public string Username { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
    }
}
