using EventPlannerConsole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventPlannerConsole
{
    public class EventPlannerEngine
    {
        public User LoggedInUser { get; set; }
        public DatabaseInterface DbInterface { get; set; }

        public EventPlannerEngine()
        {
            DbInterface = new DatabaseInterface();
            DbInterface.DbConnect();
        }

        public string Login(string name, string password)
        {
            var loginManager = new LoginManager(DbInterface);
            var outcome = "";

            if (loginManager.UserExists(name))
            {
                var userToCheck = loginManager.CorrectPassword(name, password);

                if (userToCheck != null)
                {
                    LoggedInUser = userToCheck;
                }
                else
                {

                    outcome = "Bad password";
                }
            }
            else
            {
                outcome = "Bad name";
            }

            return outcome;

        }

        /// <summary>
        /// Saves in DB and return event ID from DB
        /// </summary>
        /// <param name="_event"></param>
        /// <returns>Event ID</returns>
        public int CreateEvent(Event _event)
        {
            // Check if legit
            DbInterface.SaveEvent(_event);
            
            return DbInterface.GetEventIdByName(_event.Name);
        }

        internal void CreateEventTicket(int iD, double price)
        {
            EventTicket newEventTicket = new EventTicket()
            {
                EventID = iD,
                Price = price,
                Active = true
            };

            DbInterface.SaveEventTicket(newEventTicket);
        }

        internal void CreateEventCategory(EventCategory newEventCategory)
        {
            DbInterface.SaveEventCategory(newEventCategory);
        }

        internal void CreateCategory(string name)
        {
            DbInterface.SaveCategory(name);
        }

        internal void CreateLocation(Location newLocation)
        {
            DbInterface.SaveLocation(newLocation);
        }

        public List<Event> GetEvents()
        {
            var events = DbInterface.GetAllEvents();
            var locations = DbInterface.GetLocations();

            foreach (var _event in events)
            {
                _event.Location = locations.FirstOrDefault(x => x.ID == _event.LocationID);
            }

            return events;
        }
    }
}
