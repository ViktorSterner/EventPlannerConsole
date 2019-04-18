using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventPlannerConsole.Models
{
    public class Accommodation
    {
        public int ID { get; set; }
        public int LocationID { get; set; }
        public Location Location { get; set; }
        public string Name { get; set; }
        public string Adress { get; set; }
        public int Capacity { get; set; }
        public string Type { get; set; }
        public double Price { get; set; }
    }
}
