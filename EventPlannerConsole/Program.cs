using EventPlannerConsole.Models;
using System;
using System.Data.SqlClient;

namespace EventPlannerConsole
{
    class Program
    {
        private static UserInterface ui = new UserInterface();

        static void Main(string[] args)
        {
            // Set your database login in DatabaesInterface.cs
            
            ui.ShowMenu();    

            Console.ReadLine();
        }
    }
}

