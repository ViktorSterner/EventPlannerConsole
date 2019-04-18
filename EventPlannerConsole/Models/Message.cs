using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventPlannerConsole.Models
{
    public class Message
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public User User { get; set; }
        public int EventID { get; set; }
        public Event Event { get; set; }
        public string Text { get; set; }
        public DateTime Time { get; set; }
    }
}
