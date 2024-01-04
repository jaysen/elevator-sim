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

        for (int i = 0; i <= floorCount; i++)
        {
            Floors.Add(new Floor(i));
        }

        for (int i = 0; i < elevatorCount; i++)
        {
            var name = $"Elevator {i+1}";
            var elevator = new StdElevator(name, defaultElevatorCapacity, defaultElevatorSpeed);
            elevator.StoppedAtFloor += HandleElevatorStop;
            Elevators.Add(elevator);
        }
        return true;
    }

    public bool AddElevator(IElevator elevator)
    {
        Elevators.Add(elevator);
        return true;
    }

    public void DispatchElevatorToFloorAsync(int floorNum, Direction direction)
    {
        var bestElevator = GetBestElevatorToDispatch(floorNum, direction);
        if (bestElevator is null)
        {
            return;
        }
        bestElevator.AddFloorStop(floorNum);
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
            .Where(e => (e.IsMovingTowardFloor(floorNum) && e.Direction == direction) || (e.Status == ElevatorStatus.Idle && e.FloorStops.Count == 0))
            .OrderBy(e => Math.Abs(e.CurrentFloor - floorNum)) // first closest ones
            .ThenBy(e => e.Direction == direction ? 0 : 1) // Prefer ones moving in the right direction
            .ThenBy(e => e.Status == ElevatorStatus.Idle ? 0 : 1) // Prefer idle ones if all else is equal
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
        var direction = floorNum < destinationFloor ? Direction.Up : Direction.Down;
        if (direction == Direction.Up)
        {
            DispatchElevatorToFloorAsync(floorNum, Direction.Up);
            FloorsRequestingUp.Add(floorNum);
        }
        else if (destinationFloor < floorNum)
        {
            DispatchElevatorToFloorAsync(floorNum, Direction.Down);
            FloorsRequestingDown.Add(floorNum);
        }
        return floor.AddPassenger(passenger); 
    }

    public void ProcessFloorStop(IElevator elevator, int floorNum)
    {
        var floor = Floors[floorNum];
        floor.AddElevatorToStoppedElevators(elevator);
        elevator.RemoveFloorStop(floorNum);

        var direction = elevator.Direction;
        if (direction == Direction.Idle || elevator.FloorStops.Count == 0)
        {
            // if the elevator has no next stop go in the direction that most passengers are waiting
            direction = floor.UpQueue.Count > floor.DownQueue.Count ? Direction.Up : Direction.Down;
        }

        // Unload passengers
        elevator.UnloadPassengersForThisStop();

        // Identify which queue of passengers to load into the elevator
        var passengersQueue = direction == Direction.Up ? floor.UpQueue : floor.DownQueue;

        // load passengers from queue while lift not full
        while (passengersQueue.Count > 0 && elevator.CurrentPassengers.Count < elevator.CapacityLimit)
        {
            elevator.LoadPassenger(passengersQueue.Dequeue());
        }

        // Prepare elevator to be moved to its NextStop
        elevator.SetBestNextStop();
        floor.RemoveElevatorFromStoppedElevators(elevator);

        // Request lift if there are still passengers waiting
        if (passengersQueue.Count > 0)
        {
            DispatchElevatorToFloorAsync(floorNum, direction);
        }
    }

    public async Task MoveAllElevators()
    {
        var tasks = Elevators.Where(e => e.Status == ElevatorStatus.Idle)
                     .Select(e => e.MoveAsync());

        await Task.WhenAll(tasks);
    }

    public bool Reset()
    {
        Floors.Clear();
        Elevators.Clear();
        FloorsRequestingDown.Clear();
        FloorsRequestingUp.Clear();
        return true;
    }

    /// <summary>
    /// This method is called when an elevator stops at a floor.
    /// </summary>
    /// <param name="sender">Elevator</param>
    /// <param name="e">ElevatorStopEventArgs</param>
    private void HandleElevatorStop(object sender, ElevatorStopEventArgs e)
    {
        var el = (IElevator)sender;
        ProcessFloorStop(el, e.FloorNumber);
    }

}
