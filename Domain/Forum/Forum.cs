﻿using Domain.Slugify;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Forum
{
    public class Forum
    {
        private string _Title;

        public int Id { get; set; }
        [Required] public string Title { get => _Title; set { _Title = value; Slug = value.GenerateSlug(); } }
        [Required] public string Slug { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }

        public virtual ICollection<Thread> Threads { get; set; }

        public int ForumCategoryFK { get; set; }
        public virtual ForumCategory ForumCategory { get; set; }
    }
}
