using ElevatorSim.Core.Models.Interfaces;
using ElevatorSim.Core.Enums;

namespace ElevatorSim.Core.Models;

// Implement the IBuildingSim interface here
public class BuildingSim : IBuildingSim
{
    public int DefaultElevatorCapacity { get; private set; }
    public int DefaultElevatorSpeed { get; private set; }
    public int ElevatorCount { get; private set; }
    public int FloorCount { get; private set; }
    public IElevatorManager Manager { get; private set; }

    public BuildingSim(int floorCount, int elevatorCount, int defaultCapacity, int defaultSpeed)
    {
        FloorCount = floorCount;
        ElevatorCount = elevatorCount;
        DefaultElevatorCapacity = defaultCapacity;
        DefaultElevatorSpeed = defaultSpeed;

        Manager = new ElevatorManager();
        Manager.Setup(floorCount, elevatorCount, defaultCapacity, defaultSpeed);
    }

    public void AddElevatorToSim(IElevator elevator)
    {
        Manager.Elevators.Add(elevator);
        ElevatorCount++;
    }

    public void AddPassengerToSim(int originFloor, int destinationFloor)
    {
        Manager.AddPassengerToFloor(originFloor, destinationFloor);
    }

    public void ResetSim()
    {
        ElevatorCount = 0;
        FloorCount = 0;
        Manager.Reset();
    }
}
