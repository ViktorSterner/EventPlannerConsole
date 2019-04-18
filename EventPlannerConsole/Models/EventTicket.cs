using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventPlannerConsole.Models
{
    public class EventTicket
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public User User { get; set; }
        public int EventID { get; set; }
        public Event Event { get; set; }
        public bool Active { get; set; }
        public DateTime PurchaseTime { get; set; }
        public double Price { get; set; }
    }
}
