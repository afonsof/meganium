using System.Web.Mvc;

namespace Meganium.Api.ViewModels
{
    public class ImportVm
    {
        public string PluginName { get; set; }
        public SelectList Items { get; set; }
    }
}