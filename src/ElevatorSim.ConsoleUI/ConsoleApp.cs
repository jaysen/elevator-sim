
using ElevatorSim.ConsoleUI.UIHandlers;
using ElevatorSim.Core.Enums;
using ElevatorSim.Core.Models;
using ElevatorSim.Core.Models.Interfaces;
using ElevatorSim.Core.Services;
using ElevatorSim.Core.Services.Interfaces;

internal class ConsoleApp(IBuildingSimFactory simFactory)
{
    private readonly ConsoleHelper _con = new();
    private readonly IBuildingSimFactory _simFactory = simFactory;
    private IBuildingSim? sim;
    private IElevatorManager? manager;

    enum InputState { None, AwaitingFloor, AwaitingDestination }
    private char CommandKey { get; set; }
    private InputState CurrentInputState { get; set; } = InputState.None;
    private int? InitialFloor { get; set; }
    private int? DestinationFloor { get; set; }


    internal async Task RunAsync(string[] args)
    {
        _con.Write("ElevatorSim - Under Construction...", ConsoleColor.Magenta);
        _con.Write("");

        SetupSim(args); //TODO: disabling proper setup while using SetupSimForTesting()
        //SetupSimForTesting();

        if (sim is null || manager is null)
        {
            _con.Write("Sim or manager setup failed. Exiting...", ConsoleColor.Red);
            return;
        }

        //var displayTasks = DisplayElevatorStatusLoop(sim);
        var mainLoopTask = MainLoop(sim);
        var InputTask = InputLoop();
        //var moveTasks = sim.MoveElevators();

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


    private async Task MainLoop(IBuildingSim sim)
    {
        while (CommandKey != 'q' )
        {
            await Task.Delay(50);
            Console.Clear();
            _con.DisplayElevatorsStatus(sim);
            DisplayAndProcessActions();
            sim.MoveElevators();
        }
    }

    private void DisplayAndProcessActions()
    {
        _con.Write("Actions:", ConsoleColor.DarkCyan);
        _con.Write("- Enter 'a' to add a passenger to the simulation", ConsoleColor.Cyan);
        _con.Write("- Enter 'q' to exit the simulation", ConsoleColor.Cyan);
        _con.Write(new string('-', 80), ConsoleColor.DarkCyan);

        if (CurrentInputState == InputState.AwaitingFloor)
        {
            InitialFloor = _con.PromptForInt("Enter the floor number:", ConsoleColor.Green);
            if (InitialFloor.HasValue)
            {
                CurrentInputState = InputState.AwaitingDestination;
            }
        }
        else if (CurrentInputState == InputState.AwaitingDestination)
        {
            DestinationFloor = _con.PromptForInt("Enter the destination floor number:", ConsoleColor.Green);
            if (DestinationFloor.HasValue)
            {
                sim?.AddPassengerToSim(InitialFloor.Value, DestinationFloor.Value);
                InitialFloor = null;
                DestinationFloor = null;
                CurrentInputState = InputState.None; // Reset input state
            }
        }
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
