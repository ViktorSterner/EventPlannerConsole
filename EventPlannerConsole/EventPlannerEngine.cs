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

        /// <summary>
        /// Creates a new DatabaseInterface and connects to MSSQL-DB
        /// </summary>
        public EventPlannerEngine()
        {
            DbInterface = new DatabaseInterface();
            DbInterface.DbConnect();
        }

        /// <summary>
        /// Takes name and password, sends to DbInterface for a check
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns>Bad password or Bad name</returns>
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

        /// <summary>
        /// Takes eventID and price and send to Dbinterface to save in DB
        /// </summary>
        /// <param name="iD">Event ID</param>
        /// <param name="price"></param>
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

        /// <summary>
        /// Takes new eventcategories and send to DbInterface to save to DB
        /// </summary>
        /// <param name="newEventCategory"></param>
        internal void CreateEventCategory(EventCategory newEventCategory)
        {
            DbInterface.SaveEventCategory(newEventCategory);
        }

        /// <summary>
        /// Takes user and send to DbInterface to save to DB
        /// </summary>
        /// <param name="user"></param>
        internal void CreateUser(User user)
        {
            DbInterface.SaveUser(user);

        }

        /// <summary>
        /// Takes category name and send to DbInterface to save new category to DB
        /// </summary>
        /// <param name="name"></param>
        internal void CreateCategory(string name)
        {
            DbInterface.SaveCategory(name);
        }

        /// <summary>
        /// Takes location and send to DbInterface to save to DB
        /// </summary>
        /// <param name="newLocation"></param>
        internal void CreateLocation(Location newLocation)
        {
            DbInterface.SaveLocation(newLocation);
        }

        /// <summary>
        /// Get all events and set the correct location to the event
        /// </summary>
        /// <returns></returns>
        public List<Event> GetEvents()
        {
            var events = DbInterface.GetAllEvents();
            var locations = DbInterface.GetAllLocations();

            foreach (var _event in events)
            {
                _event.Location = locations.FirstOrDefault(x => x.ID == _event.LocationID);
            }

            return events;
        }

        // Get events with available tickets from DB
        public List<Event> GetEventsWithAvailableTickets()
        {
            var events = DbInterface.GetAllEventsWithAvailableTickets();
            
            return events;
        }

        /// <summary>
        /// Takes EventID, checks for tickets and updates the ticket with UserID and PurchaseTime = now
        /// </summary>
        /// <param name="iD">eVENT ID</param>
        internal void BuyTicket(int iD)
        {
            EventTicket ticket = DbInterface.GetActiveTicket(iD);

            ticket.UserID = LoggedInUser.ID;
            ticket.PurchaseTime = DateTime.Now;
            DbInterface.UpdateTicket(ticket);
        }
    }
}
