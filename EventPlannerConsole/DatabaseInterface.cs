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

            //Console.WriteLine("All done. Press any key to finish...");
            //Console.ReadKey(true);
        }

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

        internal void SaveUser(User user)
        {
            string sqlQ = $"INSERT into [User] ([Name], [Password], [Age], [Admin]) VALUES ('{user.Name}', '{user.Password}', '{user.Age}', '{user.Admin}')";

            using (SqlCommand command = new SqlCommand(sqlQ, Connection))
            {
                // 4 rows effected raden
                var x = command.ExecuteNonQuery();
            }
        }

        internal EventTicket GetActiveTicket(int iD)
        {
            EventTicket ticket = new EventTicket();
            String sqlQ = $"SELECT * FROM [EventTicket] WHERE [EventID] = {iD} AND Active = 1";

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

        internal void UpdateTicket(EventTicket ticket)
        {
            string sqlQ = $"UPDATE EventTicket SET [UserID] = {ticket.UserID}, [PurchaseTime] = '{ticket.PurchaseTime}' WHERE [ID] = {ticket.ID}";

            using (SqlCommand command = new SqlCommand(sqlQ, Connection))
            {
                // 4 rows effected raden
                var x = command.ExecuteNonQuery();
            }
        }

        internal void SaveLocation(Location newLocation)
        {
            string sqlQ = $"INSERT into Location ([Name],[Adress],[Capacity],[Area]) VALUES ('{newLocation.Name}', '{newLocation.Adress}', '{newLocation.Capacity}', '{newLocation.Area}')";
            
            using (SqlCommand command = new SqlCommand(sqlQ, Connection))
            {
                // 4 rows effected raden
                var x = command.ExecuteNonQuery();
            }
        }

        internal void SaveCategory(string name)
        {
            string sqlQ = $"INSERT into Category ([CategoryName]) VALUES ('{name}')";
            
            using (SqlCommand command = new SqlCommand(sqlQ, Connection))
            {
                // 4 rows effected raden
                var x = command.ExecuteNonQuery();
            }
        }

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
