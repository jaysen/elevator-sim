
using ElevatorSim.ConsoleUI.UIHandlers;
using ElevatorSim.Core.Enums;
using ElevatorSim.Core.Models;
using ElevatorSim.Core.Models.Interfaces;
using ElevatorSim.Core.Services;
using ElevatorSim.Core.Services.Interfaces;
using System.Drawing;

internal class ConsoleApp(IBuildingSimFactory simFactory)
{
    private readonly ConsoleHelper _con = new();
    private readonly IBuildingSimFactory _simFactory = simFactory;
    private IBuildingSim? sim;
    private IElevatorManager? manager;

    enum InputState { None, AwaitingFloor, AwaitingDestination}
    private char CommandKey { get; set; }
    private InputState CurrentInputState { get; set; } = InputState.None;
    private int? InitialFloor { get; set; }
    private int? DestinationFloor { get; set; }

    private string FloorError { get; set; } 

    internal async Task RunAsync(string[] args)
    {
        _con.Write("ElevatorSim - v1.0", ConsoleColor.Magenta);
        _con.Write("");

        SetupSim(args);
        //SetupSimForTesting(); // if you want initial passengers and elevator set for testing

        if (sim is null || manager is null)
        {
            _con.Write("Sim or manager setup failed. Exiting...", ConsoleColor.Red);
            return;
        }

        var mainLoopTask = MainLoop();
        var InputTask = InputLoop();

        await Task.WhenAny(mainLoopTask, InputTask);

        _con.Write("Simulation ending.", ConsoleColor.DarkCyan);
    }

    private async Task InputLoop()
    {
        while (CommandKey != 'q')
        {
            CommandKey = Console.ReadKey(true).KeyChar;  // Use true to not display the entered character.
            if (CommandKey == 'a' && CurrentInputState == InputState.None)
            {
                CurrentInputState = InputState.AwaitingFloor;
            }
            // other input states are handled in the DisplayAndProcessActions() method
        }
    }


    private async Task MainLoop()
    {
        while (CommandKey != 'q' )
        {
            await Task.Delay(50);
            Console.Clear();
            _con.DisplaySimHeader(sim);
            _con.DisplayElevatorsStatus(sim);
            DisplayAndProcessActions();
            sim.MoveElevators();
        }
    }

    private void DisplayAndProcessActions()
    {
        _con.DisplayActions();

        if (CurrentInputState == InputState.AwaitingFloor)
        {
            if (FloorError != "")
            {
                _con.Write(FloorError, ConsoleColor.Red);
            }
            int? floor = _con.PromptForInt("Enter the passenger's origin floor number:", ConsoleColor.Green);
            if (floor.HasValue && IsValidFloor(floor.Value))
            {
                InitialFloor = floor;
                CurrentInputState = InputState.AwaitingDestination;
                FloorError = "";
            }
            else
            {
                FloorError = $"Invalid floor number. Please enter a number between 0 and {sim.FloorCount}!";
            }
        }
        else if (CurrentInputState == InputState.AwaitingDestination)
        {
            if (FloorError != "")
            {
                _con.Write(FloorError, ConsoleColor.Red);
            }
            int? destination = _con.PromptForInt("Enter the destination floor number:", ConsoleColor.Green);
            if (destination.HasValue && IsValidFloor(destination.Value) && destination != InitialFloor)
            {
                DestinationFloor = destination;
                sim?.AddPassengerToSim(InitialFloor.Value, DestinationFloor.Value);

                // Reset for the next input
                FloorError = "";
                InitialFloor = null;
                DestinationFloor = null;
                CurrentInputState = InputState.None;
            }
            else if (destination == InitialFloor)
            {
                FloorError = "Destination floor cannot be the same as the initial floor. Please enter a different floor!";
            }
            else
            {
                FloorError = $"Invalid floor number. Please enter a number between 1 and {sim.FloorCount}!";
            }
        }
        _con.Write(" ");
    }

    private bool IsValidFloor(object value)
    {
        return value is int floor && floor >= 0 && floor <= sim.FloorCount;
    }

    private async Task DisplayElevatorStatusLoop(IBuildingSim sim)
    {
        var moving = true;
        while (moving)
        {
            await Task.Delay(100);
            Console.Clear();
            _con.DisplayElevatorsStatus(sim);
            moving = sim.AnyElevatorsMoving;
        }
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
        _con.Write("Simulation setup:", ConsoleColor.DarkCyan);

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
        _con.DisplaySimSetup(sim, ConsoleColor.Yellow);
        return true;
    }

    private void SetupSimForTesting()
    {
        SetupSim(["30", "4", "10", "1000"]);

        sim.SetElevatorFloor(3, 20); // Elevator 1 starts at floor 20
        sim.SetElevatorFloor(2, 30); // Elevator 2 starts at floor 30

        sim.AddPassengerToSim(1, 6);
        sim.AddPassengerToSim(4, 2);
        sim.AddPassengerToSim(8, 22);
        sim.AddPassengerToSim(14, 22);
        sim.AddPassengerToSim(15, 7);
        sim.AddPassengerToSim(19, 21);
        sim.AddPassengerToSim(26, 20);
    }


}
