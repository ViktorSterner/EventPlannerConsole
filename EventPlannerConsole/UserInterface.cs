using EventPlannerConsole.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventPlannerConsole
{
    public class UserInterface
    {
        public EventPlannerEngine _eventPlannerEngine { get; set; } = new EventPlannerEngine();

        public UserInterface(EventPlannerEngine engine)
        {
            _eventPlannerEngine = engine;
        }

        public void ShowMenu()
        {
            bool loggedIn = false;

            while (loggedIn == false)
            {
                loggedIn = Login();
            }

            if (loggedIn == true)
            {
                Console.WriteLine("This is menu");
            }
        }

        public bool Login()
        {
            var credentials = new string[2];
            var confirmed = false;

            Console.Write("Enter name:");
            var name = Console.ReadLine();
            credentials [0] = name;

            Console.Write("Enter password:");
            var password = Console.ReadLine();
            credentials [1] = password;

            var result = _eventPlannerEngine.Login(name, password);

            if (result == "Bad name")
            {
                Console.WriteLine("Sorry, unfamiliar name");
            }
            else if (result == "Bad password")
            {
                Console.WriteLine("Sorry, wrong password");
            }
            else
            {
                confirmed = true;
            }

            return confirmed;
        }
    }
}
