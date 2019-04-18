using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace EventPlannerConsole
{
    public class DatabaseInterface
    {
        public void DbConnect()
        {
            try
            {
                // Build connection string
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "(LocalDB)\\MSSQLLocalDB";   // update me
                builder.UserID = "sa";              // update me
                builder.Password = "admin";      // update me
                builder.InitialCatalog = "EventPlanner";

                // Connect to SQL
                Console.Write("Connecting to SQL Server ... ");
                SqlConnection connection = new SqlConnection(builder.ConnectionString);

                connection.Open();
                Console.WriteLine("Done.");

            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("All done. Press any key to finish...");
            Console.ReadKey(true);
        }
        
    }
}
