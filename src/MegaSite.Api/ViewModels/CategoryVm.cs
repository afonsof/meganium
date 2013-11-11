using System.Collections.Generic;
using System.Web.Mvc;
using MegaSite.Api.Entities;

namespace MegaSite.Api.ViewModels
{
    public class CategoryVm
    {
        public Category Category { get; set; }
        public IEnumerable<SelectListItem> PostTypeSelect { get; set; }
    }
}