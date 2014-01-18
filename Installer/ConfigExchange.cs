using System.Collections.Generic;
using Meganium.Api.Entities;

namespace Meganium.Installer
{
    public class ConfigExchange
    {
        public Dictionary<string, object> Options { get; set; }
        public List<PostTypeExchange> PostTypes { get; set; }
        public User RootUser { get; set; }
    }
}