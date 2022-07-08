﻿using Domain.Base;

namespace Application.ViewModel
{
    public class PostDisplayModel : BaseObject
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}