using System.Collections.Generic;
using MegaSite.Api.Entities;

namespace MegaSite.Installer
{
    public class PostExchange : Post
    {
        public new List<string> Categories { get; set; }
    }
}