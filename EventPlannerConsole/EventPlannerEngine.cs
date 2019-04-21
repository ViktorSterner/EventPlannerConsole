using EventPlannerConsole.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventPlannerConsole
{
    public class EventPlannerEngine
    {
        public User LoggedInUser { get; set; }
        public DatabaseInterface dbInterface { get; set; }

        public EventPlannerEngine()
        {
            dbInterface = new DatabaseInterface();
            dbInterface.DbConnect();
        }

        public string Login(string name, string password)
        {
            var loginManager = new LoginManager(dbInterface);
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
            dbInterface.SaveEvent(_event);
            
            return dbInterface.GetEventIdByName(_event.Name);
        }

        internal void CreateEventTicket(int iD, double price)
        {
            EventTicket newEventTicket = new EventTicket()
            {
                ID = iD,
                Price = price,
                Active = true
            };

            dbInterface.SaveEventTicket(newEventTicket);
        }
    }
}
