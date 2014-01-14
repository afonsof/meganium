using System;

namespace Meganium.Site.Areas.Extension.Models
{
    public class RsvpOptions
    {
        public DateTime OpenDate { get; set; }
        public DateTime Deadline { get; set; }
        public string GreetingMessage { get; set; }
        public DateTime YesVerbiage { get; set; }
        public DateTime NoVerbiage { get; set; }
        public DateTime ThankYouMessage { get; set; }
    }
}