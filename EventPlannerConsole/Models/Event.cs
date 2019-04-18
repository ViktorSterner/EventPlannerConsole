using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventPlannerConsole.Models
{
    public class Event
    {
        public int ID { get; set; }
        public int LocationID { get; set; }
        public Location Location { get; set; }
        public string Name { get; set; }
        public DateTime Time { get; set; }
    }
}
