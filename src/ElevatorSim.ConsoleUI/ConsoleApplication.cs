
using ElevatorSim.ConsoleUI.UIHandlers;
using ElevatorSim.Core.Models;
using ElevatorSim.Core.Models.Interfaces;
using ElevatorSim.Core.Services;
using ElevatorSim.Core.Services.Interfaces;

internal class ConsoleApplication
{
    private ConsoleHelper _con = new();
    private readonly IBuildingSimFactory _simFactory;
    public IBuildingSim? Sim { get; set; }
    

    public ConsoleApplication(IBuildingSimFactory simFactory)
    {
        _simFactory = simFactory;
    }
    public void Run(string[] args)
    {
        _con.Write("ElevatorSim - Under Construction...", ConsoleColor.Magenta, true);
        // Parse command-line arguments or prompt for input if they are not provided
        int floors = args.Length > 0 ? int.Parse(args[0]) : _con.PromptForInt("Enter the number of floors:", ConsoleColor.Green);
        int elevators = args.Length > 1 ? int.Parse(args[1]) : _con.PromptForInt("Enter the number of elevators:", ConsoleColor.Green);
        int capacity = args.Length > 2 ? int.Parse(args[2]) : _con.PromptForInt("Enter the default elevator capacity:", ConsoleColor.Green);
        int speed = args.Length > 3 ? int.Parse(args[3]) : _con.PromptForInt("Enter the default elevator speed:", ConsoleColor.Green);

        // Directly instantiate BuildingSim with provided parameters
        Sim = _simFactory.Create(floors, elevators, capacity, speed);
        Sim.AddPassengerToSim(0, floors);

        _con.Write("");
        _con.Write("BuildingSim created!", ConsoleColor.Yellow, true);
        _con.Write($"Elevator Count = {Sim.ElevatorCount}", ConsoleColor.Red);
        _con.Write($"Floor Count = {Sim.FloorCount}", ConsoleColor.Red);
        _con.Write($"Default Elevator Capacity = {Sim.DefaultElevatorCapacity}", ConsoleColor.Red);
        _con.Write($"Default Elevator Speed = {Sim.DefaultElevatorSpeed}", ConsoleColor.Red, true);
    }


}
