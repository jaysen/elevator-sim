using ElevatorSim.Core.Models;
using ElevatorSim.Core.Enums;

namespace ElevatorSim.Tests.Core.Models;

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

    #region AddFloorStop_When_Idle: 
    [Fact]
    public void AddFloorStop_ShouldAddFloorToFloorStops()
    {
        // Arrange
        var elevator = new StdElevator("Elevator1", 10, 0);
        // Act
        elevator.AddFloorStop(5);
        // Assert
        elevator.FloorStops.Should().Contain(5);
    }

    [Fact]
    public void AddFloorStop_ShouldUpdateNextStop()
    {
        // Arrange
        var elevator = new StdElevator("Elevator1", 10, 0);
        // Act
        elevator.AddFloorStop(5);
        // Assert
        elevator.NextStop.Should().Be(5);
    }


    [Fact]
    public void AddFloorStops_WhenIdle_ShouldChangeNextStopToCloserFloor()
    {
        // Arrange
        var elevator = new StdElevator("Elevator1", 10, 0, 5);

        // Act
        elevator.AddFloorStop(10);
        elevator.AddFloorStop(4);

        // Assert
        elevator.NextStop.Should().Be(4);
    }

    [Fact]
    public void AddFloorStops_WhenIdle_ShouldChangeNextStopToCloserFloor2()
    {
        // Arrange
        var elevator = new StdElevator("Elevator1", 10, 0, 5);

        // Act
        elevator.AddFloorStop(9);
        elevator.AddFloorStop(6);

        // Assert
        elevator.NextStop.Should().Be(6);
    }


    [Fact]
    public void AddFloorStops_WhenIdle_ShouldNotChangeNextStopToFurtherFloor()
    {
        // Arrange
        var elevator = new StdElevator("Elevator1", 10, 0, 5);

        // Act
        elevator.AddFloorStop(7);
        elevator.AddFloorStop(12);

        // Assert
        elevator.NextStop.Should().Be(7);
    }

    [Fact]
    public void AddFloorStops_WhenIdle_ShouldNotChangeNextStopToFurtherFloor2()
    {
        // Arrange
        var elevator = new StdElevator("Elevator1", 10, 0, 5);

        // Act
        elevator.AddFloorStop(6);
        elevator.AddFloorStop(2);

        // Assert
        elevator.NextStop.Should().Be(6);
    }

    #endregion AddFloorStop_When_Idle:

}
