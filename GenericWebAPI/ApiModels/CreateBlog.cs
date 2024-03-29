﻿using Domain.Base;

namespace BlogAPI.Application.ApiModels
{
    public class CreateBlog
    {
        public string Name { get; set; }
        public string? Summary { get; set; }
        public CreateBlog()
        {
            Name = String.Empty;
        }
        public CreateBlog(string name, string? summary = null)
        {
            Name = name;
            Summary = summary;
        }
    }
}
