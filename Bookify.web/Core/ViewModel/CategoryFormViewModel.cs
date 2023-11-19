﻿using Microsoft.AspNetCore.Mvc;

namespace Bookify.web.Core.ViewModel
{
    public class CategoryFormViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="enter name")]
        [MaxLength(50, ErrorMessage = "name should be less than 50 chr.")]
        [Remote("AllowItem",null,ErrorMessage ="this name is allready exist")]
        public string Name { get; set; } = null!;
    }
}
