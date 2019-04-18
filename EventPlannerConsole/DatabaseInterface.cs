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

                //String sql = "SELECT * FROM Location";

                //using (SqlCommand command = new SqlCommand(sql, Connection))
                //{
                //    using (SqlDataReader reader = command.ExecuteReader())
                //    {
                //        while (reader.Read())
                //        {
                //            Console.WriteLine("\n{0} {1} {2}", reader.GetInt32(0), reader.GetString(1), reader.GetString(2));
                //        }
                //    }
                //}

                Console.WriteLine("Done.");

            }

            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            //Console.WriteLine("All done. Press any key to finish...");
            //Console.ReadKey(true);
        }

        public List<User> GetUsers()
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

        public List<Event> GetEvents()
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

        public List<Location> GetLocations()
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

        public void SaveEvent(Event _event)
        {
            //var locationID = _event.Location.ID;
            //var time = _event.Time;
            //var name = _event.Name;

            var locationID = 9;
            var time = DateTime.Now;
            var name = "eventNAME";

            string sqlQ = string.Format("INSERT into Event ([Name],LocationID,[Time]) VALUES ('{0}', '{1}','{2}')", name, locationID, time);
        
            using (SqlCommand command = new SqlCommand(sqlQ, Connection))
            {
                // 4 rows effected
                // command.ExecuteNonQuery();
            }

            
        }
    }
}
