using System;
using System.Collections.Generic;
using System.Text;

namespace WordPressNet.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ParentId { get; set; }
        public string Slug { get; set; }
    }
}
