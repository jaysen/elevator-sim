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


        // if all moving in the opposite direction,
        // return the elevator that can turn around first
        var elevatorsMovingAway = Elevators.Where(e => e.Direction != direction).ToList();
        if (elevatorsMovingAway.Any())
        {
            //return the one that can turn around first
            if (direction == Direction.Up)  
            {
                // request up, elevator moving down
                // so choose the elevator with the highest minimum floor stop 
                return elevatorsMovingAway.OrderByDescending(e => e.FloorStops.Min).First();
            }
            else
            {
                // request down, elevator moving up
                // so choose the elevator with the lowest maximum floor stop
                return elevatorsMovingAway.OrderByDescending(e => e.FloorStops.Max).First();
            }
        }

        return null;
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
