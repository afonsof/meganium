using System.Collections.Generic;
using MegaSite.Api.Entities;

namespace MegaSite.Installer
{
    public class PostTypeExchange : PostType
    {
        public List<string> Categories { get; set; }
        public new List<PostExchange> Posts { get; set; }
    }
}
