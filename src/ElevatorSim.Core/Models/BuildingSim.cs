using ElevatorSim.Core.Models.Interfaces;
using ElevatorSim.Core.Enums;

namespace ElevatorSim.Core.Models;

public class BuildingSim : IBuildingSim
{
    public int DefaultElevatorCapacity { get; private set; }
    public int DefaultElevatorSpeed { get; private set; }
    public int ElevatorCount { get; private set; }
    public int FloorCount { get; private set; } // not including ground floor (0)
    public IElevatorManager Manager { get; private set; }
    public bool AnyElevatorsMoving
    {
        get
        {
            foreach (var elevator in Manager.Elevators)
            {
                if (elevator.Status != ElevatorStatus.Idle)
                {
                    return true;
                }
            }
            return false;
        }
    }

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

    public void AddPassengersToSim(int originFloor, int destinationFloor, int passengerCount = 1)
    {
        if (originFloor < 0 || originFloor > FloorCount)
        {
            throw new ArgumentException($"Origin floor must be between 0 and {FloorCount}");
        }
        if (destinationFloor < 0 || destinationFloor > FloorCount)
        {
            throw new ArgumentException($"Destination floor must be between 0 and {FloorCount}");
        }
        Manager.AddPassengersToFloor(originFloor, destinationFloor, passengerCount);
    }

    public void ResetSim()
    {
        ElevatorCount = 0;
        FloorCount = 0;
        Manager.Reset();
    }

    public void SetElevatorFloor(int elevatorNum, int floorNum)
    {
        Manager.Elevators[elevatorNum-1].SetFloor(floorNum);
    }

    public async Task MoveElevators()
    {
        await Manager.MoveAllElevators();
    }
}
