using Domain.Forum;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Domain.Identity
{
    public class User : IdentityUser<int>
    {
        public string DisplayName { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime JoinDate { get; set; }
        public string AvatarUrl { get; set; } = "https://png.pngtree.com/svg/20161027/service_default_avatar_182956.png";

        public virtual ICollection<Thread> Threads { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}
