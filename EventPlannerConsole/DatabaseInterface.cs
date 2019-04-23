using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
using EventPlannerConsole.Models;

namespace EventPlannerConsole
{
    public class DatabaseInterface
    {
        public string Source { get; set; } = "(LocalDB)\\MSSQLLocalDB";
        public string User { get; set; } = "sa";
        public string Password { get; set; } = "admin";
        public SqlConnection Connection { get; set; }

        // Connect to database
        public void DbConnect()
        {
            try
            {
                // Build connection string
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = Source;   // update me
                builder.UserID = User;              // update me
                builder.Password = Password;      // update me
                builder.InitialCatalog = "EventPlanner";

                // Connect to SQL
                Console.Write("Connecting to SQL Server ... ");
                Connection = new SqlConnection(builder.ConnectionString);
                            
                Connection.Open();

                Console.WriteLine("Done.");

            }

            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            
        }

        // Saves event to DB
        internal void SaveEventCategory(EventCategory newEventCategory)
        {
            var eventID = newEventCategory.EventID;
            var categoryID = newEventCategory.Category.ID;

            string sqlQ = $"INSERT into EventCategory ([EventID],[CategoryID]) VALUES ('{eventID}', '{categoryID}')";
            
            using (SqlCommand command = new SqlCommand(sqlQ, Connection))
            {
                // 4 rows effected raden
                var x = command.ExecuteNonQuery();
            }
        }

        // Saves user to DB
        internal void SaveUser(User user)
        {
            string sqlQ = $"INSERT into [User] ([Name], [Password], [Age], [Admin]) VALUES ('{user.Name}', '{user.Password}', '{user.Age}', '{user.Admin}')";

            using (SqlCommand command = new SqlCommand(sqlQ, Connection))
            {
                // 4 rows effected raden
                var x = command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Gets a ticket from the DB
        /// </summary>
        /// 
        /// <param name="iD">Event ID</param>
        /// <returns>Matching Event</returns>
        internal EventTicket GetActiveTicket(int iD)
        {
            EventTicket ticket = new EventTicket();
            String sqlQ = $"SELECT * FROM [EventTicket] WHERE [EventID] = {iD} AND Active = 1 AND [UserID] IS NULL";

            using (SqlCommand command = new SqlCommand(sqlQ, Connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ticket.ID = reader.GetInt32(0);
                        ticket.EventID = reader.GetInt32(2);
                        ticket.Price = reader.GetDouble(4);
                        ticket.Active = reader.GetBoolean(5);
                    }
                }
            }

            return ticket;
        }

        // Updating the UserID and PurchaseTime for a ticket
        internal void UpdateTicket(EventTicket ticket)
        {
            string sqlQ = $"UPDATE EventTicket SET [UserID] = {ticket.UserID}, [PurchaseTime] = '{ticket.PurchaseTime}' WHERE [ID] = {ticket.ID}";

            using (SqlCommand command = new SqlCommand(sqlQ, Connection))
            {
                // 4 rows effected raden
                var x = command.ExecuteNonQuery();
            }
        }

        // Saves the location to the DB
        internal void SaveLocation(Location newLocation)
        {
            string sqlQ = $"INSERT into Location ([Name],[Adress],[Capacity],[Area]) VALUES ('{newLocation.Name}', '{newLocation.Adress}', '{newLocation.Capacity}', '{newLocation.Area}')";
            
            using (SqlCommand command = new SqlCommand(sqlQ, Connection))
            {
                // 4 rows effected raden
                var x = command.ExecuteNonQuery();
            }
        }

        internal List<Event> GetAllEventsWithAvailableTickets()
        {
            List<Event> events = new List<Event>();
            String sqlQ = "SELECT [Event].ID, [Event].[Name], COUNT(EventTicket.ID) FROM[Event] JOIN[EventTicket] ON EventTicket.EventID = [Event].ID WHERE EventTicket.UserID IS NULL AND Active = 1 GROUP BY[Event].[ID], [Event].[Name];";

            using (SqlCommand command = new SqlCommand(sqlQ, Connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Event newEvent = new Event()
                        {
                            ID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                        };
                        events.Add(newEvent);
                    }
                }
            }

            return events;
        }

        // Saves a new category to the DB
        internal void SaveCategory(string name)
        {
            string sqlQ = $"INSERT into Category ([CategoryName]) VALUES ('{name}')";
            
            using (SqlCommand command = new SqlCommand(sqlQ, Connection))
            {
                // 4 rows effected raden
                var x = command.ExecuteNonQuery();
            }
        }

        // Saves EventTicket to DB
        internal void SaveEventTicket(EventTicket _eventTicket)
        {
            var eventID = _eventTicket.EventID;
            var price = _eventTicket.Price;
            var active = _eventTicket.Active;

            string sqlQ = $"INSERT into EventTicket ([EventID],[Price],[Active]) VALUES ('{eventID}', '{price}','{active}')";


            using (SqlCommand command = new SqlCommand(sqlQ, Connection))
            {
                // 4 rows effected raden
                var x = command.ExecuteNonQuery();
            }
        }

        // Get eventID where name is name
        internal int GetEventIdByName(string name)
        {
            String sqlQ = $"SELECT [ID] FROM [Event] WHERE [Event].[Name] = '{name}'";

            int result = 0;

            using (SqlCommand command = new SqlCommand(sqlQ, Connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = reader.GetInt32(0);
                    }
                }
            }

            return result;
        }

        // Saves event to DB
        public void SaveEvent(Event _event)
        {
            var locationID = _event.LocationID;
            var time = _event.Time;
            var name = _event.Name;

            string sqlQ = $"INSERT INTO Event ([Name],[LocationID],[Time]) VALUES ('{name}', '{locationID}','{time}')";
            //string sqlQ = string.Format("INSERT into Event ([Name],LocationID,[Time]) values ('{0}', '{1}','{2}')", name, locationID, time);

            using (SqlCommand command = new SqlCommand(sqlQ, Connection))
            {
                // 4 rows effected
                var x = command.ExecuteNonQuery();
            }



        }

        /// <summary>
        /// Get all users from DB
        /// </summary>
        /// <returns>List of Users</returns>
        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            String sqlQ = "SELECT * FROM [User]";

            using (SqlCommand command = new SqlCommand(sqlQ, Connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        User newUser = new User()
                        {
                            ID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Age = reader.GetInt32(2),
                            Password = reader.GetString(3),
                            Admin = reader.GetBoolean(4)
                        };
                        users.Add(newUser);
                    }
                }
            }

            return users;
        }

        /// <summary>
        /// Get all events from DB
        /// </summary>
        /// <returns>List of events</returns>
        public List<Event> GetAllEvents()
        {
            List<Event> events = new List<Event>();
            String sqlQ = "SELECT * FROM [Event]";

            using (SqlCommand command = new SqlCommand(sqlQ, Connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Event newEvent = new Event()
                        {
                            ID = reader.GetInt32(0),
                            LocationID = reader.GetInt32(1),
                            Name = reader.GetString(2),
                            Time = reader.GetDateTime(3)
                        };

                        events.Add(newEvent);
                    }
                }
            }

            return events;
        }

        /// <summary>
        /// Get all events from DB
        /// </summary>
        /// <returns>List of locations</returns>
        public List<Location> GetAllLocations()
        {
            List<Location> locations = new List<Location>();
            String sqlQ = "SELECT * FROM [Location]";

            using (SqlCommand command = new SqlCommand(sqlQ, Connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Location newLocation = new Location()
                        {
                            ID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Adress = reader.GetString(2),
                            Capacity = reader.GetInt32(3),
                            Area = reader.GetString(4)
                        };                        

                        locations.Add(newLocation);
                    }
                }
            }

            return locations;
        }
        
        /// <summary>
        /// Get all categories
        /// </summary>
        /// <returns>List of categories</returns>
        public List<Category> GetAllCategories()
        {
            List<Category> categories = new List<Category>();
            String sqlQ = "SELECT * FROM [Category]";

            using (SqlCommand command = new SqlCommand(sqlQ, Connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Category newCategory = new Category()
                        {
                            ID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                        };

                        categories.Add(newCategory);
                    }
                }
            }

            return categories;
        }
    }
}
