
using ElevatorSim.ConsoleUI.UIHandlers;
using ElevatorSim.Core.Models;
using ElevatorSim.Core.Models.Interfaces;
using ElevatorSim.Core.Services;
using ElevatorSim.Core.Services.Interfaces;

internal class ConsoleApp
{
    private ConsoleHelper _con = new();
    private bool _argsUsed = false;
    private readonly IBuildingSimFactory _simFactory;
    public IBuildingSim? Sim { get; set; }
    

    public ConsoleApp(IBuildingSimFactory simFactory)
    {
        _simFactory = simFactory;
    }
    public void Run(string[] args)
    {
        _con.Write("ElevatorSim - Under Construction...", ConsoleColor.Magenta);
        _con.Write("");

        SetupSim(args);

    }

    /// <summary>
    /// Sets up the simulation based on command-line arguments or user input
    /// </summary>
    /// <param name="args">
    /// Command-line arguments:
    /// floors: int (number of floors in the building)
    /// elevators: int (number of elevators in the building)
    /// capacity: int (default elevator capacity)
    /// speed: int (default elevator time to traverse one floor - in ms)
    /// </param>
    /// <returns>bool: True if sim setup, false otherwise</returns>
    private bool SetupSim(string[] args)
    {
        _argsUsed = args.Length > 0;
        _con.Write("Sim Setup:", ConsoleColor.Cyan);

        // Parse command-line arguments or prompt for input if they are not provided
        int floors = args.Length > 0 ? int.Parse(args[0]) : _con.PromptForInt("Enter the number of floors:", ConsoleColor.Green);
        int elevators = args.Length > 1 ? int.Parse(args[1]) : _con.PromptForInt("Enter the number of elevators:", ConsoleColor.Green);
        int capacity = args.Length > 2 ? int.Parse(args[2]) : _con.PromptForInt("Enter the default elevator capacity:", ConsoleColor.Green);
        int speed = args.Length > 3 ? int.Parse(args[3]) : _con.PromptForInt("Enter the default elevator speed:", ConsoleColor.Green);

        // Directly instantiate BuildingSim with provided parameters
        Sim = _simFactory.Create(floors, elevators, capacity, speed);

        if (Sim is null)
        {
            _con.Write("Sim setup failed. Exiting...", ConsoleColor.Red);
            return false;
        }

        _con.Write("");
        DisplaySimSetup(ConsoleColor.Yellow);
        return true;
    }

    /// <summary>
    /// Writes the simulation setup to the console
    /// </summary>
    private void DisplaySimSetup(ConsoleColor color = ConsoleColor.Gray)
    {
        _con.Write($"Elevator Count = {Sim.ElevatorCount}", color);
        _con.Write($"Floor Count = {Sim.FloorCount}", color);
        _con.Write($"Default Elevator Capacity = {Sim.DefaultElevatorCapacity}", color);
        _con.Write($"Default Elevator Speed = {Sim.DefaultElevatorSpeed}", color);

    }
}
