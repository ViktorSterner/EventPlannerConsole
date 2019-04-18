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

        public LoginManager(DatabaseInterface _dbInterface)
        {
            dbInterface = _dbInterface;
        }

        public bool UserExists(string name)
        {
            var users = dbInterface.GetUsers();
            var result = users.Any(x => x.Name == name);            

            return result;
        }

        public User CorrectPassword(string name, string password)
        {
            var users = dbInterface.GetUsers();
            User user = users.FirstOrDefault(x => x.Name == name);

            if (user.Password == password)
            {
                return user;
            }

            return null;
        }
    }
}
