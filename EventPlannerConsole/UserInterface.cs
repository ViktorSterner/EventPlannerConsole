using EventPlannerConsole.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventPlannerConsole
{
    public class UserInterface
    {
        public EventPlannerEngine _eventPlannerEngine { get; set; } = new EventPlannerEngine();

        //public UserInterface(EventPlannerEngine engine)
        //{
        //    _eventPlannerEngine = engine;
        //}

        public void ShowMenu()
        {
            bool loggedIn = false;

            while (loggedIn == false)
            {
                loggedIn = Login();
            }

            if (loggedIn == true)
            {
                if (_eventPlannerEngine.LoggedInUser.Admin == true)
                {
                    Console.WriteLine("This is admin menu");
                }
                else
                {
                    Console.WriteLine("This is normal menu");
                }
            }
        }

        public bool Login()
        {
            var credentials = new string[2];
            var confirmed = false;

            Console.Write("Enter name:");
            var name = Console.ReadLine();
            credentials[0] = name;

            Console.Write("Enter password:");
            var password = Console.ReadLine();
            credentials[1] = password;

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

        public Event CreateEvent()
        {
            Event newEvent = new Event();

            Console.Write("Välj namn på Evenemanget: ");
            newEvent.Name = Console.ReadLine();
            Console.Write("Välj Tid för evenemanget: ");
            newEvent.Time = DateTime.Parse(Console.ReadLine());

            newEvent.Location = GetEventLocation();

            CreateEventCategory();

            CreateEventTickets();


            return newEvent;

        }

        private EventCategory CreateEventCategory(int eventId)
        {
            EventCategory theEventCategory = new EventCategory();
            int i = 0;

            Console.Write("Välj ett kategori, eller skapa en ny kategori (För att skapa en ny kategori, skriv [ny]): ");

            var categoryList = _eventPlannerEngine.dbInterface.GetCategories();

            foreach (var category in categoryList)
            {
                Console.WriteLine($"{i + 1}: {category.Name}");
                i++;
            }

            string answer = Console.ReadLine();
            int intAnswer = 0;

            if (answer.ToLower() == "ny")
            {
                //CreateNewCategory();
                CreateEventCategory(eventId);
            }
            else
            {
                intAnswer = int.Parse(answer);
                theEventCategory.Category = categoryList[intAnswer - 1];

                theEventCategory.EventID = eventId;

            }

            return theEventCategory;    
        }

        private Location GetEventLocation()
        {
            int i = 0;
            Location theLocation = new Location();
            Console.Write("Välj ett plats, eller skapa en ny plats (För att skapa en ny plats, skriv [ny]): ");

            var locationList = _eventPlannerEngine.dbInterface.GetLocations();
            foreach (var location in locationList)
            {
                Console.WriteLine($"{i + 1}: {location.Name}");
                i++;
            }

            string answer = Console.ReadLine();
            int intAnswer = 0;

            if (answer.ToLower() == "ny")
            {
                //CreateNewLocation();
                GetEventLocation();
            }
            else
            {
                intAnswer = int.Parse(answer);
                theLocation = locationList[intAnswer - 1];
            }

            return theLocation;
        }

    }
}
