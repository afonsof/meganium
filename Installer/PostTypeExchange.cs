using System.Collections.Generic;
using Meganium.Api.Entities;

namespace Meganium.Installer
{
    public class PostTypeExchange : PostType
    {
        public List<string> Categories { get; set; }
        public new List<PostExchange> Posts { get; set; }
    }
}
