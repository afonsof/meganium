using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MegaSite.Api.ViewModels
{
    public class PostTypeCreateEditVm
    {
        public MultiSelectList BehaviorsMultiselect { get; set; }

        public int Id { get; set; }

        [Required]
        public string SingularName { get; set; }

        [Required]
        public string PluralName { get; set; }

        [Required]
        public string IconId { get; set; }

        public List<string> BehaviorItems { get; set; }

        public string FieldsJson { get; set; }

        public string[] item_Name { get; set; }
        public int[] item_Type { get; set; }
        public string[] item_SelectList { get; set; }
    }
}