using ElevatorSim.Core.Models.Interfaces;
using ElevatorSim.Core.Enums;

namespace ElevatorSim.Core.Models;

public class StdElevator : IElevator
{
    public StdElevator(string name, int capacity, int timeBetweenFloors = 1000, int startFloor = 0)
    {
        Name = name;
        CapacityLimit = capacity;
        CurrentFloor = startFloor;
        NextStop = null;
        Status = ElevatorStatus.Idle;
        CurrentPassengers = new List<IPassenger>();
        FloorStops = new SortedSet<int>();
        TimeBetweenFloors = timeBetweenFloors;
    }

    public string Name { get; }
    public int CapacityLimit { get; }
    public int TimeBetweenFloors { get; }

    public int CurrentFloor { get; private set; }
    public int? NextStop { get; private set; }
    public ElevatorStatus Status { get; private set; }
    public Direction Direction { get; private set; }
    public List<IPassenger> CurrentPassengers { get; private set; }
    public SortedSet<int> FloorStops { get; private set; }

    public event EventHandler<ElevatorStopEventArgs> StoppedAtFloor;

    /// <summary>
    /// Adds a FloorStop to the FloorStops collection.
    /// Sets NextStop based on standard elevator logic.
    /// - If elevator is idle, sets NextStop to closest floor.
    /// - If elevator is moving, sets NextStop to closest floor in the direction of travel.
    /// - If elevator is moving and there are no more stops in the direction of travel, sets NextStop to closest floor in the opposite direction.
    /// </summary>
    /// <param name="newFloor"></param>
    /// <returns>true if floorstop added, false if not</returns>
    public void AddFloorStop(int newFloor)
    {
        if (FloorStops.Contains(newFloor))
        {
            return; // floor already in collection
        }

        FloorStops.Add(newFloor);


        // Change NextStop based on standard elevator logic:
        if (NextStop is null)
        {
            NextStop = newFloor;
            return;
        }
        
        if (Direction == Direction.Idle)
        {
            // check if newFloor is closer than NextStop
            if (Math.Abs(CurrentFloor - newFloor) < Math.Abs(CurrentFloor - NextStop.Value))
            {
                NextStop = newFloor;
                Direction = CurrentFloor < newFloor ? Direction.Up : Direction.Down;
                //Status = ElevatorStatus.Stopped;
            }
            return;
        }

        // if moving, determine if newFloor is closer than NextStop in the direction of travel
        var newFloorDir = CurrentFloor < newFloor ? Direction.Up : Direction.Down;

        // going up and new floor is below next stop
        if (Direction == Direction.Up &&
            newFloorDir == Direction.Up &&
            newFloor < NextStop)
        {
            NextStop = newFloor;
            return;
        }
        
        // going down and new floor is above next stop
        if (Direction == Direction.Down &&
            newFloorDir == Direction.Down &&
            newFloor > NextStop)
        {
            NextStop = newFloor;
            return;
        }
    }

    public void RemoveFloorStop(int floor)
    {
        FloorStops.Remove(floor);
    }
    public void ClearFloorStops()
    {
        FloorStops.Clear();
    }

    public async Task MoveAsync()
    {
        while (NextStop is not null)
        {
            await MoveToNextStopAsync();
        }
    }

    public async Task MoveToNextStopAsync()
    {
        if (NextStop is null)
        {
            Status = ElevatorStatus.Idle;
            Direction = Direction.Idle;
            return;
        }
        Status = ElevatorStatus.Moving;
        Direction = CurrentFloor < NextStop ? Direction.Up : Direction.Down;
        while (CurrentFloor != NextStop)
        {
            await Task.Delay(TimeBetweenFloors);
            CurrentFloor = Direction == Direction.Up ? CurrentFloor + 1 : CurrentFloor - 1;
        }

        RemoveFloorStop(CurrentFloor); // remove the floor stop we just stopped at
        SetBestNextStop(); // set the next stop based on standard elevator logic
        OnStoppedAtFloor(CurrentFloor);
    }

    public async Task MoveToFloorAsync(int floor)
    {
        Status = ElevatorStatus.Moving;
        Direction = CurrentFloor < floor ? Direction.Up : Direction.Down;

        // move to floor
        while (CurrentFloor != floor)
        {
            await Task.Delay(TimeBetweenFloors);
            CurrentFloor = Direction == Direction.Up ? CurrentFloor + 1 : CurrentFloor - 1;
        }
        RemoveFloorStop(CurrentFloor);
        //Direction = Direction.Idle;
        //Status = ElevatorStatus.Idle;
    }


    public bool LoadPassenger(IPassenger passenger)
    {
        if (CurrentPassengers.Count >= CapacityLimit)
        {
            return false;
        }
        CurrentPassengers.Add(passenger);
        AddFloorStop(passenger.DestinationFloor);
        return true;
    }

    public void UnloadPassengersForThisStop()
    {
        // TODO: Possibly refactor CurrentPassengers to be a dictionary of key=floor and value=List<Passengers>
        var passengersToUnload = CurrentPassengers.Where(x => x.DestinationFloor == CurrentFloor).ToList();
        foreach (var passenger in passengersToUnload)
        {
            CurrentPassengers.Remove(passenger);
        }
    }

    public void ClearPassengers()
    {
        CurrentPassengers.Clear();
    }

    public void Reset()
    {
        CurrentFloor = 0;
        NextStop = null;
        Status = ElevatorStatus.Idle;
        Direction = Direction.Idle;
        CurrentPassengers.Clear();
        FloorStops.Clear();
    }

    public bool IsMovingTowardFloor(int floor)
    {
        if (Direction == Direction.Up && floor > CurrentFloor)
            return true;

        if (Direction == Direction.Down && floor < CurrentFloor)
            return true;

        return false;
    }

    public void SetFloor(int floorNum)
    {
        CurrentFloor = floorNum;
        Direction = Direction.Idle;
        Status = ElevatorStatus.Idle;
    }

    public void SetBestNextStop()
    {
        NextStop = FindBestNextStop();
        if (NextStop is null)
        {
            Status = ElevatorStatus.Idle;
            Direction = Direction.Idle;
            return;
        }
        if (NextStop > CurrentFloor)
        {
            Direction = Direction.Up;
            //Status = ElevatorStatus.Stopped;
        }
        else
        {
            Direction = Direction.Down;
            //Status = ElevatorStatus.Stopped;
        }

    }

    protected virtual void OnStoppedAtFloor(int floorNumber)
    {
        Status = ElevatorStatus.Stopped;
        StoppedAtFloor?.Invoke(this, new ElevatorStopEventArgs { FloorNumber = floorNumber, Direction = Direction});
        
    }

    #region Private Methods

    // TODO: This is less than elegant. Refactor.
    private int? FindBestNextStop()
    {
        var upStops = FloorStops.Where(x => x > CurrentFloor);
        var downStops = FloorStops.Where(x => x < CurrentFloor);

        if (Direction == Direction.Up)
        {
            if (upStops.Any())
                return upStops.Min();
            else if (downStops.Any())
            {
                return downStops.Max();
            }
        }
        else if (Direction == Direction.Down)
        {
            if (downStops.Any())
                return downStops.Max();
            else if (upStops.Any())
            {
                return upStops.Min();
            }
        }
        else if (Direction == Direction.Idle)
        {
            if (upStops.Any())
                return upStops.Min();
            else if (downStops.Any())
            {
                return downStops.Max();
            }
        }

        return null;
    }

    #endregion Private Methods
}
