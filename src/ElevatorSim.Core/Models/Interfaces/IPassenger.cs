namespace ElevatorSim.Core.Models.Interfaces;

public interface IPassenger
{
    int Id { get; }
    string Name { get; }
    int DestinationFloor { get; }

    // double Weight { get; }

}