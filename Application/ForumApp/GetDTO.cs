using System;
using System.Collections.Generic;
using System.Text;

namespace Application.ForumApp
{
    public class GetDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
    }
}
