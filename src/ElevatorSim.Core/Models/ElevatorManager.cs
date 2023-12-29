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
        var bestElevator = GetBestElevatorToDispatch(floorNum, direction);
        if (bestElevator is null)
        {
            return;
        }
        bestElevator.AddFloorStop(floorNum);
        await bestElevator.MoveToNextStopAsync();
    }

    public IElevator? GetBestElevatorToDispatch(int floorNum, Direction direction)
    {
        // Case 1: Elevators stopped or idle on floorNum
        var stoppedOrIdleOnFloor = Elevators
            .Where(e => e.CurrentFloor == floorNum && (e.Status == ElevatorStatus.Idle || e.Status == ElevatorStatus.Stopped))
            .FirstOrDefault();

        if (stoppedOrIdleOnFloor != null)
        {
            return stoppedOrIdleOnFloor;
        }

        // Case 2: Elevators either idle, or moving towards the floor and heading in the right direction
        var movingTowardsFloorInRightDirection = Elevators
            .Where(e => e.IsMovingTowardFloor(floorNum) && e.Direction == direction || e.Status == ElevatorStatus.Idle)
            .OrderBy(e => Math.Abs(e.CurrentFloor - floorNum)) // Then closest ones
            .FirstOrDefault();
        if (movingTowardsFloorInRightDirection != null)
        {
            return movingTowardsFloorInRightDirection;
        }


        // whats left are elevators moving toward the floor but going in the wrong direction, and those moving away.
        // Case 3: Elevators that have to turn around to get to the floor

        // order elevators by how long it will take to turn around and get to the floor
        // if the elevator is going up, then the distance before turning around is the distance from the current floor to the highest floor stop
        // if the elevator is going down, then the distance before turning around is the distance from the current floor to the lowest floor stop
        // the total distance is the distance before turning around + the distance between the turn around stop and the requested floor

        //TODO: This case doesn't yet handle the case where the elevator is already needing to turn around and go past the requested floor counter to requested direction.

        var elevatorsTurningAround = Elevators
            .Select(e => new
            {
                Elevator = e,
                Distance = e.Direction == Direction.Up ? 
                                        e.FloorStops.Max - e.CurrentFloor + Math.Abs(e.FloorStops.Max - floorNum) :
                                        e.CurrentFloor - e.FloorStops.Min + Math.Abs(floorNum - e.FloorStops.Min)
            })
            .OrderBy(e => e.Distance)
            .ToList();

        return elevatorsTurningAround.FirstOrDefault()?.Elevator;

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
