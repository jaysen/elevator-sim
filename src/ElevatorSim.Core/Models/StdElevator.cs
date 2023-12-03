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


    public async Task MoveToNextStopAsync()
    {
        throw new NotImplementedException();
    }


    public void AddFloorStop(int floor)
    {
        throw new NotImplementedException();
    }

    public void RemoveFloorStop(int floor)
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

    public void ClearFloorStops()
    {
        throw new NotImplementedException();
    }

    public void Reset()
    {
        throw new NotImplementedException();
    }
}
