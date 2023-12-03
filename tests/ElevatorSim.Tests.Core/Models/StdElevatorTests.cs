using ElevatorSim.Core.Models;
using ElevatorSim.Core.Enums;

public class StdElevatorTests
{
    [Fact]
    public void Elevator_WhenInitialized_ShouldSetBasicPropertiesCorrectly()
    {
        // Arrange
        var elevator = new StdElevator("Elevator1", 10);

        // Assert
        elevator.Name.Should().Be("Elevator1");
        elevator.CapacityLimit.Should().Be(10);
        elevator.CurrentFloor.Should().Be(0);
        elevator.NextStop.Should().BeNull();
        elevator.Status.Should().Be(ElevatorStatus.Idle);
        elevator.CurrentPassengers.Should().BeEmpty();
        elevator.FloorStops.Should().BeEmpty();
    }

    [Fact]
    public void Elevator_WhenInitialized_ShouldSetOptionalPropertiesCorrectly()
    {
        // Arrange
        var elevator = new StdElevator("Elevator1", 10, 2000, 10);

        // Assert
        elevator.CurrentFloor.Should().Be(10);
        elevator.TimeBetweenFloors.Should().Be(2000);
    }
}
