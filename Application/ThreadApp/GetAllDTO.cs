using System;
using System.Collections.Generic;
using System.Text;

namespace Application.ThreadApp
{
    public class GetAllDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public UserDTO Author { get; set; }

        public class UserDTO
        {
            public int Id { get; set; }
            public string DisplayName { get; set; }
            public string AvatarUrl { get; set; }
        }

        public DateTime CreatedOn { get; set; }
        public int Replies { get; set; }
        public long Views { get; set; }
        public DateTime LastReplyDate { get; set; }
    }
}