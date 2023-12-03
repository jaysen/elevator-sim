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
        
        if (Status == ElevatorStatus.Idle)
        {
            // check if newFloor is closer than NextStop
            if (Math.Abs(CurrentFloor - newFloor) < Math.Abs(CurrentFloor - NextStop.Value))
            {
                NextStop = newFloor;
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


    public async Task MoveToNextStopAsync()
    {
        throw new NotImplementedException();
    }


    public void LoadPassenger(IPassenger passenger)
    {
        throw new NotImplementedException();
    }

    public void UnloadPassengersForThisStop()
    {
        throw new NotImplementedException();
    }

    public void ClearPassengers()
    {
        throw new NotImplementedException();
    }



    public void Reset()
    {
        throw new NotImplementedException();
    }



}
