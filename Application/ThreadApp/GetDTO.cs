using System;

namespace Application.ThreadApp
{
    public class GetDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool Edited { get; set; }
        public DateTime LastEditDate { get; set; }
        public ForumDTO Forum { get; set; }

        public class ForumDTO
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Slug { get; set; }
            public string Icon { get; set; }
        }

        public UserDTO Author { get; set; }

        public class UserDTO
        {
            public int Id { get; set; }
            public string DisplayName { get; set; }
            public string AvatarUrl { get; set; }
        }
    }
}