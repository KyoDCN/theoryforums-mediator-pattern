using System;
using System.Collections.Generic;
using System.Text;

namespace Application.ForumCategoryApp
{
    public class GetAllDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public List<Forum> Forums { get; set; }

        public class Forum
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Slug { get; set; }
            public string Description { get; set; }
            public string Icon { get; set; }
            public Reply LatestReply { get; set; }

            public class Reply
            {
                public int ThreadId { get; set; }
                public string ThreadTitle { get; set; }
                public string ThreadSlug { get; set; }
                public DateTime PostReplyDate { get; set; }
                public User Author { get; set; }

                public class User
                {
                    public int Id { get; set; }
                    public string DisplayName { get; set; }
                    public string AvatarUrl { get; set; }
                }
            }
        }
    }
}