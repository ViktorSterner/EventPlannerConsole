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

                String sql = "SELECT * FROM Location";

                using (SqlCommand command = new SqlCommand(sql, Connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine("\n{0} {1} {2}", reader.GetInt32(0), reader.GetString(1), reader.GetString(2));
                        }
                    }
                }

                Console.WriteLine("Done.");

            }

            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("All done. Press any key to finish...");
            Console.ReadKey(true);
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
            throw new NotImplementedException();
        }
    }
}
