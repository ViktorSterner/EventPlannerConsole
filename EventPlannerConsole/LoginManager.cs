using EventPlannerConsole.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventPlannerConsole
{
    public class LoginManager
    {
        public bool UserExists(string name)
        {
            // om nån i databasen har det här namnet return true
            return false;
        }

        public User CorrectPassword(string name, string password)
        {
            // kolla om det matchar och skicka tillbaka rätt User annars null
            return new User();
        }
    }
}
