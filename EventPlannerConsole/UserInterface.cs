using EventPlannerConsole.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventPlannerConsole
{
    public class UserInterface
    {
        public EventPlannerEngine _eventPlannerEngine { get; set; } = new EventPlannerEngine();

        public void ShowMenu()
        {
            bool loggedIn = false;
            string answer = "";

            answer = StartUpMenu();
            
            if (answer == "1")
            {
                CreateUser();
            }

            while (loggedIn == false)
            {
                loggedIn = Login();
            }

            if (loggedIn == true)
            {
                if (_eventPlannerEngine.LoggedInUser.Admin == true)
                {
                    ShowAdminMenu();
                }
                else
                {
                    ShowUserMenu();
                }
            }
        }

        private void CreateUser()
        {

            User user = new User();
            Console.WriteLine($"---Create new user---");
            Console.Write("User name: ");
            user.Name = Console.ReadLine();
            Console.WriteLine("User password: ");
            user.Password = Console.ReadLine();
            Console.WriteLine("User age: ");
            user.Age = int.Parse(Console.ReadLine());

            user.Admin = false;
            _eventPlannerEngine.CreateUser(user);

            Console.WriteLine("User created!");
        }

        private string StartUpMenu()
        {
            Console.WriteLine("---Välkommen---");
            Console.WriteLine("1. Skapa ny användare");
            Console.WriteLine("2. Logga in");
            string answer = Console.ReadLine();

            return answer;
        }

        private void ShowUserMenu()
        {
            Console.WriteLine("--- User menu ---");
            Console.WriteLine("1. Show all events");
            Console.WriteLine("2. Buy Ticket");

            var input = Convert.ToInt32(Console.ReadLine());

            switch (input)
            {
                case 1:
                    ShowAllEvents();
                    break;
                case 2:
                    BuyTicket();
                    break;
                default:
                    break;
            }
        }

        private void BuyTicket()
        {
            Console.WriteLine("Pick event");
            ShowAllEvents();
            var events = _eventPlannerEngine.GetEvents();
            int answer = int.Parse(Console.ReadLine());

            Event pickedEvent = events[answer - 1];

            _eventPlannerEngine.BuyTicket(pickedEvent.ID);
        }

        private void ShowAdminMenu()
        {
            Console.WriteLine("--- Admin menu ---");
            Console.WriteLine("1. Create new event");
            Console.WriteLine("2. Show all events");
            Console.WriteLine("3. Create Tickets");

            var input = Convert.ToInt32(Console.ReadLine());

            switch (input)
            {
                case 1:
                    CreateEvent();
                    break;
                case 2:
                    ShowAllEvents();
                    break;
                case 3:
                    //CreateEventTickets();
                    break;
                default:
                    break;
            }
        }

        public bool Login()
        {
            var credentials = new string[2];
            var confirmed = false;
            Console.WriteLine("---Login---");
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
                Console.WriteLine("Correct login");
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

            newEvent.Location = SelectLocationFromDb();
            newEvent.LocationID = newEvent.Location.ID;

            newEvent.ID = _eventPlannerEngine.CreateEvent(newEvent);

            CreateEventCategory(newEvent.ID);

            Console.WriteLine("Skriv yes för fler kategorier");

            string answer = Console.ReadLine();

            while (answer.ToLower() == "yes")
            {
                switch (answer)
                {
                    case "yes":
                        CreateEventCategory(newEvent.ID);
                        break;
                    case "no":
                        break;
                    default:
                        break;
                }

                Console.WriteLine("Skriv yes för fler kategorier");

                answer = Console.ReadLine();
            }

            CreateEventTickets(newEvent.ID);

            Console.WriteLine("Event skapat!");
            return newEvent;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iD">Event ID</param>
        public void CreateEventTickets(int iD)
        {
            Console.WriteLine($"Create ticket for event: {iD}.");
            Console.Write("Price:");
            double price = Convert.ToDouble(Console.ReadLine()); // Är float i DB
            Console.Write("Amount:");
            int amount = Convert.ToInt32(Console.ReadLine());

            for (int i = 0; i < amount; i++)
            {
                _eventPlannerEngine.CreateEventTicket(iD, price);
            }
        }

        // Om man inte vet vilket ID och vill välja från lista
        private void CreateEventTickets()
        {
            throw new NotImplementedException();
        }

        public void CreateEventCategory(int eventId)
        {
            EventCategory newEventCategory = new EventCategory();
            int i = 0;

            Console.Write("Välj ett kategori, eller skriv [ny] för att skapa en ny");

            var categoryList = _eventPlannerEngine.DbInterface.GetAllCategories();

            foreach (var category in categoryList)
            {
                Console.WriteLine($"{i + 1}: {category.Name}");
                i++;
            }

            string answer = Console.ReadLine();
            int intAnswer = 0;

            if (answer.ToLower() == "ny")
            {
                CreateNewCategory();
                CreateEventCategory(eventId);
            }
            else
            {
                intAnswer = int.Parse(answer);
                newEventCategory.Category = categoryList[intAnswer - 1];

                newEventCategory.EventID = eventId;
                _eventPlannerEngine.CreateEventCategory(newEventCategory);
            }

        }

        private void CreateNewCategory()
        {
            Console.WriteLine($"Create new category");
            Console.Write("Category name:");
            string name = Console.ReadLine();
            _eventPlannerEngine.CreateCategory(name);
            Console.WriteLine("Category created!");
        }

        public Location SelectLocationFromDb()
        {
            int i = 0;
            Location theLocation = new Location();
            Console.WriteLine("Välj ett plats, eller skriv [ny] för att skapa en ny plats");

            var locationList = _eventPlannerEngine.DbInterface.GetAllLocations();

            foreach (var location in locationList)
            {
                Console.WriteLine($"{i + 1}: {location.Name}");
                i++;
            }

            string answer = Console.ReadLine();
            int intAnswer = 0;

            if (answer.ToLower() == "ny")
            {
                CreateNewLocation();
                theLocation = SelectLocationFromDb();
            }
            else
            {
                intAnswer = int.Parse(answer);
                theLocation = locationList[intAnswer - 1];
            }

            return theLocation;
        }

        private void CreateNewLocation()
        {
            Console.WriteLine($"Create new location");
            Console.Write("Location name:");
            string name = Console.ReadLine();
            Console.Write("Adress:");
            string adress = Console.ReadLine();
            Console.Write("Capacity:");
            int capacity = Convert.ToInt32(Console.ReadLine());
            Console.Write("Area:");
            string area = Console.ReadLine();

            var newLocation = new Location()
            {
                Name = name,
                Adress = adress,
                Capacity = capacity,
                Area = area
            };

            _eventPlannerEngine.CreateLocation(newLocation);
            Console.WriteLine("Location created!");
        }

        public void ShowAllEvents()
        {
            var events = _eventPlannerEngine.GetEvents();
            int i = 1;

            foreach (var _event in events)
            {
                Console.WriteLine($"{i}. {_event.Name}\n{_event.Time}\n{_event.Location.Name} - {_event.Location.Adress}");
                Console.WriteLine("--------");
                i++;
            }
        }


    }
}
