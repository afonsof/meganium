using System.Collections.Generic;
using MegaSite.Api.Entities;

namespace MegaSite.Installer
{
    public class ConfigExchange
    {
        public Dictionary<string, string> Options { get; set; }
        public List<PostTypeExchange> PostTypes { get; set; }
        public User RootUser { get; set; }
    }
}