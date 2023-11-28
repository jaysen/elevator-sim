using ElevatorSim.Core.Models.Interfaces;

namespace ElevatorSim.Core.Models;

public class Elevator : IElevator
{
    public int Id { get; }
    public string Name { get; }
    public int CapacityLimit { get; }

    public int CurrentPassengers { get; private set; }
    public int CurrentFloor { get; private set; }
    public Enums.ElevatorStatus Status { get; private set; }
    public Enums.Direction Direction { get; private set; }

    public SortedSet<int> FloorStops { get; private set; }

    public Elevator(int id, string name, int capacityLimit)
    {
        Id = id;
        Name = name;
        CapacityLimit = capacityLimit;

        CurrentPassengers = 0;
        CurrentFloor = 0;
        Status = Enums.ElevatorStatus.Idle;

        FloorStops = [];
    }

    // TODO: Below are in draft - and not properly implemented yet..

    public void MoveToNextStop()
    {
        if (FloorStops.Count == 0)
        {
            Status = Enums.ElevatorStatus.Idle;
            Direction = Enums.Direction.Idle;
            return;
        }

        // TODO: Implement logic to determine which floor to move to next
        //       based on the current floor, direction, and floor stops
    }
    public void MoveToFloor(int floor)
    {
        while (CurrentFloor != floor)
        {
            // TODO: Use the ElevatorSpeed setting to determine how long to sleep
            // sleep for 1 second to simulate moving to the next floor
            Thread.Sleep(1000);
            if (CurrentFloor < floor)
            {
                CurrentFloor++;
            }
            else
            {
                CurrentFloor--;
            }
        }
        FloorStops.Remove(floor);
    }

    public void AddFloorStop(int floor)
    {
        FloorStops.Add(floor);
    }

    public void RemoveFloorStop(int floor)
    {
        FloorStops.Remove(floor);
    }

    public void LoadPassenger(IPassenger passenger)
    {
        CurrentPassengers++;
        AddFloorStop(passenger.DestinationFloor);
    }

    public void UnloadPassengersForThisStop()
    {
        CurrentPassengers--;
        RemoveFloorStop(CurrentFloor);
    }

    public void ClearPassengers()
    {
        CurrentPassengers = 0;
    }

    public void ClearFloorStops()
    {
        FloorStops.Clear();
    }

    public override string ToString()
    {
        return $"Elevator {Id}: {Status} {Direction} - {CurrentFloor} - {CurrentPassengers} - {FloorStops.Count}";
    }
}