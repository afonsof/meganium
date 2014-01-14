using System.Collections.Generic;
using System.Web.Mvc;
using Meganium.Api.Entities;

namespace Meganium.Api.ViewModels
{
    public class CategoryVm
    {
        public Category Category { get; set; }
        public IEnumerable<SelectListItem> PostTypeSelect { get; set; }
    }
}