﻿namespace Bookify.web.Core.Models
{
    public class BaseModel
    {
        public bool IsDeleted { get; set; }
        public string? CreatedById { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string? LastUpdatedOnById { get; set; }
        public DateTime? LastUpdatedOn { get; set; }

        public ApplicationUser? CreatedBy { get; set; }
        public ApplicationUser? LastUpdatedOnBy { get; set; }
    }
}
