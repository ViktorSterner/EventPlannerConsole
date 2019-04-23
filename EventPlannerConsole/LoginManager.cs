using EventPlannerConsole.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace EventPlannerConsole
{
    public class LoginManager
    {
        public DatabaseInterface dbInterface { get; set; }

        // Set DbInterface 
        public LoginManager(DatabaseInterface _dbInterface)
        {
            dbInterface = _dbInterface;
        }

        // Checks if user with name exists in DB and returns bool
        public bool UserExists(string name)
        {
            var users = dbInterface.GetAllUsers();
            var result = users.Any(x => x.Name == name);            

            return result;
        }

        // Checks if user with name and password exists and returns that user, else null
        public User CorrectPassword(string name, string password)
        {
            var users = dbInterface.GetAllUsers();
            User user = users.FirstOrDefault(x => x.Name == name && x.Password == password);

            if (user != null)
            {
                return user;
            }

            return null;
        }
    }
}
