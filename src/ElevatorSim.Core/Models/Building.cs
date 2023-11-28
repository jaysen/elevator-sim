using ElevatorSim.Core.Enums;
using ElevatorSim.Core.Models.Interfaces;


public class Building : IBuilding
{
    public Building()
    {
        FloorsRequestingDown = [];
        FloorsRequestingUp = [];
        Elevators = [];
        Floors = [];
    }
    public int ElevatorCount { get; }
    public int FloorCount { get; }

    public List<IElevator> Elevators { get; }
    public List<IFloor> Floors { get; }

    public void AddPassenger(IFloor origin, IFloor destination)
    {
        // Implementation goes here
    }
    public void ClearAllRequestedFloorStops()
    {
        // Implementation goes here
    }

    public void Reset()
    {
        // Implementation goes here
    }

    // Building Controller 

    public SortedSet<int> FloorsRequestingUp { get; }
    public SortedSet<int> FloorsRequestingDown { get; }

    public void AddElevatorFloorStop(IElevator elevator, int floor)
    {
        // Implementation goes here
    }

    public void RemoveElevatorFloorStop(IElevator elevator, int floor)
    {
        // Implementation goes here
    }

    public void MoveElevator(IElevator elevator, int floor)
    {
        // Implementation goes here
    }

    public void ClearElevatorFloorStops(IElevator elevator)
    {
        // Implementation goes here
    }


}
