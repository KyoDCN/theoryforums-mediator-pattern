using System;

namespace Application.PostApp
{
    public class GetDTO
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool Edited { get; set; }
        public DateTime LastEditDate { get; set; }
        public UserDTO Author { get; set; }

        public class UserDTO
        {
            public int Id { get; set; }
            public string DisplayName { get; set; }
            public string AvatarUrl { get; set; }
        }
    }
}