using Domain.Identity;
using Domain.Slugify;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Forum
{
    public class Thread
    {
        private string _Title;

        public int Id { get; set; }
        [Required] public string Title { get => _Title; set { _Title = value; Slug = value.GenerateSlug(); } }
        [Required] public string Slug { get; set; }
        [Required] public string Content { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime LastUpdated { get; set; } = DateTime.Now;
        public bool Edited { get; set; } = false;
        public DateTime LastEditDate { get; set; } = DateTime.MinValue;
        public long Views { get; set; }

        // Post
        public virtual ICollection<Post> Posts { get; set; }

        // Forum
        public int ForumFK { get; set; }
        public virtual Forum Forum { get; set; }

        // User
        public int AuthorFK { get; set; }
        public virtual User Author { get; set; }
    }
}
