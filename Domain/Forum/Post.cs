using Domain.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Forum
{
    public class Post
    {
        public int Id { get; set; }
        [Required] public string Content { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool Edited { get; set; }
        public DateTime LastEditDate { get; set; }

        // Thread
        public int ThreadFK { get; set; }
        public virtual Thread Thread { get; set; }

        // User
        public int AuthorFK { get; set; }
        public virtual User Author { get; set; }
    }
}
