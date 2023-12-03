using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorSim.Core.Enums;
using ElevatorSim.Core.Models.Interfaces;

namespace ElevatorSim.Core.Models;
public class Passenger : IPassenger
{
    private static int _idCounter = 0;
    private static int GetNextId() => ++_idCounter;

    public Passenger(int destinationFloor)
    {
        Id = GetNextId();
        Name = $"Passenger{Id}";
        DestinationFloor = destinationFloor;
    }

    public Passenger(string name, int destinationFloor)
    {
        Id = GetNextId();
        Name = name;
        DestinationFloor = destinationFloor;
    }

    public int Id { get; }
    public string Name { get; }
    public int DestinationFloor { get; }
}
