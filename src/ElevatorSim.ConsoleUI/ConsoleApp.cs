
using ElevatorSim.ConsoleUI.UIHandlers;
using ElevatorSim.Core.Enums;
using ElevatorSim.Core.Models;
using ElevatorSim.Core.Models.Interfaces;
using ElevatorSim.Core.Services;
using ElevatorSim.Core.Services.Interfaces;

internal class ConsoleApp
{
    private ConsoleHelper _con = new();
    private bool _argsUsed = false;
    private readonly IBuildingSimFactory _simFactory;
    private IBuildingSim? sim;
    private IElevatorManager? manager;
    

    public ConsoleApp(IBuildingSimFactory simFactory)
    {
        _simFactory = simFactory;
    }
    public void Run(string[] args)
    {
        _con.Write("ElevatorSim - Under Construction...", ConsoleColor.Magenta);
        _con.Write("");

        SetupSim(args);

        DisplayElevatorStatus();

    }

    /// <summary>
    /// Displays the status of each elevator in the console.
    /// </summary>
    private void DisplayElevatorStatus()
    {
        _con.Write(new string('-', 80), ConsoleColor.DarkCyan);
        _con.Write("Elevator Status:", ConsoleColor.DarkCyan);

        foreach (var elevator in manager.Elevators)
        {
            string elevatorDirection = GetDirectionSymbol(elevator.Direction);
            string passengerCount = elevator.CurrentPassengers.Count.ToString();
            string destinations = FormatDestinations(elevator.FloorStops);

            _con.Write($"[{elevator.Name}] Floor {elevator.CurrentFloor} {elevatorDirection}  |  Passengers: {passengerCount}  |  Destinations: {destinations}", ConsoleColor.Gray);
        }

        _con.Write(new string('-', 80), ConsoleColor.DarkCyan);
    }

    private string GetDirectionSymbol(Direction direction)
    {
        return direction switch
        {
            Direction.Up => "↑",
            Direction.Down => "↓",
            _ => "-"
        };
    }

    /// <summary>
    /// Formats the destinations of an elevator into a string.
    /// </summary>
    /// <param name="floorStops">A sorted set of floor stops.</param>
    /// <returns>A formatted string of destinations.</returns>
    private string FormatDestinations(SortedSet<int> floorStops)
    {
        if (floorStops == null || floorStops.Count == 0)
        {
            return "None";
        }
        return string.Join(", ", floorStops);
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
        _con.Write("Simulation setup:", ConsoleColor.Cyan);

        // Parse command-line arguments or prompt for input if they are not provided
        int floors = args.Length > 0 ? int.Parse(args[0]) : _con.PromptForInt("Enter the number of floors:", ConsoleColor.Green);
        int elevators = args.Length > 1 ? int.Parse(args[1]) : _con.PromptForInt("Enter the number of elevators:", ConsoleColor.Green);
        int capacity = args.Length > 2 ? int.Parse(args[2]) : _con.PromptForInt("Enter the default elevator capacity:", ConsoleColor.Green);
        int speed = args.Length > 3 ? int.Parse(args[3]) : _con.PromptForInt("Enter the default elevator speed:", ConsoleColor.Green);

        // Directly instantiate BuildingSim with provided parameters
        sim = _simFactory.Create(floors, elevators, capacity, speed);
        manager = sim?.Manager;

        if (sim is null || manager is null)
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
        _con.Write($"Elevator Count = {sim.ElevatorCount}", color);
        _con.Write($"Floor Count = {sim.FloorCount}", color);
        _con.Write($"Default Elevator Capacity = {sim.DefaultElevatorCapacity}", color);
        _con.Write($"Default Elevator Speed = {sim.DefaultElevatorSpeed}", color);

    }
}
