using ElevatorSim.Core.Models.Interfaces;

namespace ElevatorSim.Core.Models;

public class Passenger : IPassenger
{
    public int DestinationFloor { get; set; }

    public Passenger(int destinationFloor)
    {
        DestinationFloor = destinationFloor;
    }

}
