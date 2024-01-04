using ElevatorSim.Core.Enums;
using ElevatorSim.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSim.ConsoleUI.UIHandlers;
public class ConsoleHelper
{
    public ConsoleHelper()
    {
        // Set console encoding to UTF8 to support unicode characters
        Console.OutputEncoding = Encoding.UTF8;
    }
    public void Write(string message, ConsoleColor? color = null, bool includeNewLine = true)
    {
        if (color.HasValue)
        {
            Console.ForegroundColor = color.Value;
        }

        if (includeNewLine)
        {
            Console.WriteLine(message);
        }
        else
        {
            Console.Write(message);
        }

        if (color.HasValue)
        {
            Console.ResetColor();
        }
    }

    public int PromptForInt(string prompt, ConsoleColor? color = null)
    {
        int value;
        Write(prompt, color);
        
        while (!int.TryParse(Console.ReadLine(), out value))
        {
            Write("Invalid input. Please enter a valid number:", ConsoleColor.Red);
        }
        return value;
    }


    /// <summary>
    /// Writes the simulation setup to the console
    /// </summary>
    public void DisplaySimSetup(IBuildingSim sim, ConsoleColor color = ConsoleColor.Gray)
    {
        Write($"Elevator Count = {sim.ElevatorCount}", color);
        Write($"Floor Count = {sim.FloorCount}", color);
        Write($"Default Elevator Capacity = {sim.DefaultElevatorCapacity}", color);
        Write($"Default Elevator Speed = {sim.DefaultElevatorSpeed}", color);
    }

    public void DisplaySimHeader(IBuildingSim sim)
    {
        Write(new string('-', 80), ConsoleColor.DarkCyan);
        Write($"Elevator Simulation - {sim.FloorCount} Floors", ConsoleColor.DarkMagenta);
    }

    public string GetDirectionSymbol(Direction direction)
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
    public string FormatDestinations(SortedSet<int> floorStops)
    {
        if (floorStops == null || floorStops.Count == 0)
        {
            return "None";
        }
        return string.Join(", ", floorStops);
    }

    /// <summary>
    /// Displays the status of each elevator in the console.
    /// </summary>
    public void DisplayElevatorsStatus(IBuildingSim sim)
    {
        Write(new string('-', 80), ConsoleColor.DarkCyan);
        Write("Elevator Status:", ConsoleColor.DarkCyan);
        Write(" ");
        foreach (var elevator in sim.Manager.Elevators)
        {
            string elevatorDirection = GetDirectionSymbol(elevator.Direction);
            string passengerCount = elevator.CurrentPassengers.Count.ToString();
            string destinations = FormatDestinations(elevator.FloorStops);

            Write($"[{elevator.Name}] Floor {elevator.CurrentFloor} {elevatorDirection}  |  Passengers: {passengerCount}  |  Destinations: {destinations}", ConsoleColor.Yellow);
        }
        Write(" ");
        Write(new string('-', 80), ConsoleColor.DarkCyan);
    }

    public void DisplayActions()
    {
        Write("Actions:", ConsoleColor.DarkCyan);
        Write(" ");
        Write("- Enter 'a' to add a passenger to the simulation", ConsoleColor.Cyan);
        Write("- Enter 'q' to exit the simulation", ConsoleColor.Cyan);
        Write(" ");
        Write(new string('-', 80), ConsoleColor.DarkCyan);
    }
}
