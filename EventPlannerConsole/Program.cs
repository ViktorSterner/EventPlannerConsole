using EventPlannerConsole.Models;
using System;
using System.Data.SqlClient;

namespace EventPlannerConsole
{
    class Program
    {
        private static DatabaseInterface dbInterface = new DatabaseInterface();
        //    private static EventPlannerEngine engine = new EventPlannerEngine();
        private static UserInterface ui = new UserInterface();

        static void Main(string[] args)
        {
            ui.CreateEvent();

            Console.ReadLine();
        }
    }
}

