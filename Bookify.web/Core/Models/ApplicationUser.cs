﻿using Microsoft.AspNetCore.Identity;

namespace Bookify.web.Core.Models
{
    public class ApplicationUser:IdentityUser
    {
        [MaxLength(100)]
        public string FullName { get; set; } = null!;
        public bool IsDeleted { get; set; }
        public string? CreatedById { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string? LastUpdatedOnById { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
    }
}
