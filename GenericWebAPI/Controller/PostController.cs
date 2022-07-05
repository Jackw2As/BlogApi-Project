﻿using BlogAPI.Storage.DatabaseModels;
using Domain.Base;
using Domain.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controller
{
    [Authorize()]
    public class PostController : BaseController<Post>
    {
        protected override IRepository<Post> Repository { get; init; }
        public PostController(IRepository<Post> repository)
        {
            Repository = repository;
        }

        [AllowAnonymous]
        public override IActionResult Get(Guid Id)
        {
            return base.Get(Id);
        }
    }
}
