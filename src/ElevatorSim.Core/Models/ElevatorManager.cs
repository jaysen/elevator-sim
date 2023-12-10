using ElevatorSim.Core.Models.Interfaces;
using ElevatorSim.Core.Enums;

namespace ElevatorSim.Core.Models;

// Implement the IElevatorManager interface
public class ElevatorManager : IElevatorManager
{

    public List<IElevator> Elevators { get; }
    public List<IFloor> Floors { get; }

    public SortedSet<int> FloorsRequestingUp { get; }
    public SortedSet<int> FloorsRequestingDown { get; }

    public ElevatorManager()
    {
        Elevators = [];
        Floors = [];
        FloorsRequestingUp = [];
        FloorsRequestingDown = [];
    }

    public bool Setup(int floorCount, int elevatorCount, int defaultElevatorCapacity, int defaultElevatorSpeed)
    {

        for (int i = 0; i < floorCount; i++)
        {
            Floors.Add(new Floor(i));
        }

        for (int i = 0; i < elevatorCount; i++)
        {
            var name = $"Elevator {i}";
            Elevators.Add(new StdElevator(name, defaultElevatorCapacity, defaultElevatorSpeed));
        }
        return true;
    }

    public bool AddElevator(IElevator elevator)
    {
        Elevators.Add(elevator);
        return true;
    }

    public async Task DispatchElevatorToFloorAsync(int floorNum, Direction direction)
    {
        throw new NotImplementedException();
    }


    public async Task<bool> MoveElevatorToFloorAsync(IElevator elevator, int floorNum)
    {
        await elevator.MoveToFloorAsync(floorNum);
        return elevator.CurrentFloor == floorNum;
    }

    public bool AddPassengerToFloor(int floorNum, int destinationFloor)
    {
        var floor = Floors[floorNum];
        var passenger = new Passenger(destinationFloor);
        return floor.AddPassenger(passenger); 
    }

    public Task<bool> ProcessFloorStop(IElevator elevator, int floorNum)
    {
        throw new NotImplementedException();
    }



    public bool Reset()
    {
        Floors.Clear();
        Elevators.Clear();
        FloorsRequestingDown.Clear();
        FloorsRequestingUp.Clear();
        return true;
    }



}
