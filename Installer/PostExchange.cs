using System.Collections.Generic;
using Meganium.Api.Entities;

namespace Meganium.Installer
{
    public class PostExchange : Post
    {
        public new List<string> Categories { get; set; }
    }
}