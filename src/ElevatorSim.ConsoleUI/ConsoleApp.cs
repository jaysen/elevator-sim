
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

    enum InputState { None, AwaitingNumberOfPassengers, AwaitingFloor, AwaitingDestination}
    private char CommandKey { get; set; }
    private InputState CurrentInputState { get; set; } = InputState.None;
    private int NumPassengers { get; set; } = 1;
    private int? InitialFloor { get; set; }
    private int? DestinationFloor { get; set; }
    private string InputError { get; set; }


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
            if (CommandKey == 'm' && CurrentInputState == InputState.None)
            {
                CurrentInputState = InputState.AwaitingNumberOfPassengers;
            }
            // other input states are handled in the ProcessActionInputs() method
        }
    }


    private async Task MainLoop()
    {
        while (CommandKey != 'q' )
        {
            await Task.Delay(20);
            Console.Clear();
            _con.DisplaySimHeader(sim);
            _con.DisplayElevatorsStatus(sim);
            _con.DisplayActions();
            ProcessActionInputs();
            sim.MoveElevators();
        }
    }

    private void ProcessActionInputs()
    {
        if (CurrentInputState == InputState.AwaitingNumberOfPassengers)
        {
            if (InputError != "")
            {
                _con.Write(InputError, ConsoleColor.Red);
            }
            NumPassengers = _con.PromptForInt("Enter the number of passengers to add:", ConsoleColor.Green);
            if (NumPassengers > 0)
            {
                CurrentInputState = InputState.AwaitingFloor;
                InputError = "";
            }
            else
            {
                InputError = "Invalid number of passengers. Please enter a number greater than 0";
            }
        }
        else if (CurrentInputState == InputState.AwaitingFloor)
        {
            if (InputError != "")
            {
                _con.Write(InputError, ConsoleColor.Red);
            }
            int? floor = _con.PromptForInt("Enter the passenger's origin floor number:", ConsoleColor.Green);
            if (floor.HasValue && IsValidFloor(floor.Value))
            {
                InitialFloor = floor;
                CurrentInputState = InputState.AwaitingDestination;
                InputError = "";
            }
            else
            {
                InputError = $"Invalid floor number. Please enter a number between 0 and {sim.FloorCount}!";
            }
        }
        else if (CurrentInputState == InputState.AwaitingDestination)
        {
            if (InputError != "")
            {
                _con.Write(InputError, ConsoleColor.Red);
            }
            int? destination = _con.PromptForInt("Enter the destination floor number:", ConsoleColor.Green);
            if (destination.HasValue && IsValidFloor(destination.Value) && destination != InitialFloor)
            {
                DestinationFloor = destination;
                sim.AddPassengersToSim(InitialFloor.Value, DestinationFloor.Value, numPassengers);

                // Reset for the next input
                InputError = "";
                InitialFloor = null;
                DestinationFloor = null;
                CurrentInputState = InputState.None;
                NumPassengers = 1;
            }
            else if (destination == InitialFloor)
            {
                InputError = "Destination floor cannot be the same as the initial floor. Please enter a different floor!";
            }
            else
            {
                InputError = $"Invalid floor number. Please enter a number between 1 and {sim.FloorCount}!";
            }
        }
        _con.Write(" ");
    }

    private bool IsValidFloor(object value)
    {
        return value is int floor && floor >= 0 && floor <= sim.FloorCount;
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

        sim.AddPassengersToSim(1, 6);
        sim.AddPassengersToSim(4, 2);
        sim.AddPassengersToSim(8, 22);
        sim.AddPassengersToSim(14, 22);
        sim.AddPassengersToSim(15, 7);
        sim.AddPassengersToSim(19, 21);
        sim.AddPassengersToSim(26, 20);
    }


}
