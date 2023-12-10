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

    #region RemoveFloorStop_ClearFloorStops
    [Fact]
    public void RemoveFloorStop_WhenCalled_ShouldRemoveFloorFromFloorStops()
    {
        // Arrange
        var elevator = new StdElevator("Elevator1", 10, 0);
        elevator.AddFloorStop(5);
        elevator.AddFloorStop(15);

        // Act
        elevator.RemoveFloorStop(5);
        // Assert
        elevator.FloorStops.Should().NotContain(5);
    }

    [Fact]
    public void ClearFloorStops_WhenCalled_ShouldClearFloorStops()
    {
        // Arrange
        var elevator = new StdElevator("Elevator1", 10, 0);
        elevator.AddFloorStop(5);
        elevator.AddFloorStop(15);

        // Act
        elevator.ClearFloorStops();
        // Assert
        elevator.FloorStops.Should().BeEmpty();
    }

    #endregion RemoveFloorStop_ClearFloorStops

    #region MoveElevator and FindNextStop while moving

    [Fact]
    public async Task MoveToFloor_WhenCalled_ShouldMoveElevatorToFloor()
    {
        // Arrange
        var elevator = new StdElevator("Elevator1", 10, 0, 0);

        // Act
        await elevator.MoveToFloorAsync(5);
        // Assert
        elevator.CurrentFloor.Should().Be(5);
    }

    [Fact]
    public async Task MoveToNextStop_WhenCalled_ShouldMoveElevatorToNextStop()
    {
        // Arrange
        var elevator = new StdElevator("Elevator1", 10, 0, 0);
        elevator.AddFloorStop(5);
        elevator.AddFloorStop(15);

        // Act
        await elevator.MoveToNextStopAsync();
        // Assert
        elevator.CurrentFloor.Should().Be(5);
    }

    [Fact]
    public async Task FindNextStop_WhenNone_ShouldClearNextStop()
    {
        // Arrange
        var elevator = new StdElevator("Elevator1", 10, 0, 0);
        elevator.AddFloorStop(5);
        // Act
        await elevator.MoveToNextStopAsync();
        // Assert
        elevator.NextStop.Should().BeNull();
    }
    [Fact]
    public async Task FindNextStop_WhenSameDirection_ShouldUpdateNextStop()
    {
        // Arrange
        var elevator = new StdElevator("Elevator1", 10, 0, 0);
        elevator.AddFloorStop(5);
        elevator.AddFloorStop(15);
        elevator.AddFloorStop(35);

        // Act
        await elevator.MoveToNextStopAsync();
        // Assert
        elevator.NextStop.Should().Be(15);
    }

    [Fact]
    public async Task FindNextStop_WhenOtherDirection_ShouldNotUpdateNextStop()
    {
        // Arrange
        var elevator = new StdElevator("Elevator1", 10, 0, 5); // start floor is 5
        elevator.AddFloorStop(10);
        elevator.AddFloorStop(1);

        // Act
        await elevator.MoveToNextStopAsync();
        // Assert
        elevator.NextStop.Should().Be(10);
    }

    [Fact]
    public async Task FindNextStop_WhenOtherDirection_ShouldNotUpdateNextStop2()
    {
        // Arrange
        var elevator = new StdElevator("Elevator1", 10, 0, 5); // start floor is 5
        elevator.AddFloorStop(10);
        elevator.AddFloorStop(15);
        elevator.AddFloorStop(4);

        // Act
        await elevator.MoveToNextStopAsync();
        // Assert
        elevator.NextStop.Should().Be(10);
    }

    #endregion MoveElevator and FindNextStop while moving

    #region AddFloorStop_WhenMoving

    [Fact]
    // should not update next-stop when the new stop is in different direction than elevator is moving
    public async Task AddFloorStop_WhenMoving_ShouldUpdateNextStopCorrectly()
    {
        // Arrange
        var elevator = new StdElevator("Elevator1", 10, 0, 10); // start floor is 10 
        elevator.AddFloorStop(13);
        elevator.AddFloorStop(16);

        // Act
        await elevator.MoveToNextStopAsync(); // move to 13
        elevator.AddFloorStop(12);

        // Assert
        // 12 is in opposite direction, so next stop should not change, even though it is closer
        elevator.NextStop.Should().Be(16);
    }

    [Fact]
    // should not update next-stop when the new stop is in different direction than elevator is moving
    public async Task AddFloorStop_WhenMoving_ShouldUpdateNextStopCorrectly2()
    {
        // Arrange
        var elevator = new StdElevator("Elevator1", 10, 0, 10); // start floor is 10 
        elevator.AddFloorStop(13);
        elevator.AddFloorStop(18);

        // Act
        await elevator.MoveToNextStopAsync(); // move to 13
        elevator.AddFloorStop(17);

        // Assert
        // 17 is in same direction, so next stop should change to 17 from 18
        elevator.NextStop.Should().Be(17);

    }

    #endregion AddFloorStop_WhenMoving

    #region CurrentPassengers
    [Fact]
    public void LoadPassenger_WhenCalled_ShouldAddPassengerToCurrentPassengers()
    {
        // Arrange
        var elevator = new StdElevator("Elevator1", 10, 0);
        var passenger = new Passenger(5);

        // Act
        elevator.LoadPassenger(passenger);

        // Assert
        elevator.CurrentPassengers.Should().Contain(passenger);
    }

    [Fact]
    public void LoadPassenger_WhenCalled_ShouldAddPassengerDestinationFloorStops()
    {
        // Arrange
        var elevator = new StdElevator("Elevator1", 10, 0);
        var passenger = new Passenger(5);

        // Act
        elevator.LoadPassenger(passenger);

        // Assert
        elevator.FloorStops.Should().Contain(passenger.DestinationFloor);
    }

    [Fact]
    public async void UnloadPassengersForThisStop_ShouldRemovePassengersWithDestinationFloorMatchingCurrentFloor()
    {
        // Arrange
        var elevator = new StdElevator("Elevator1", 10, 0);
        var passenger1 = new Passenger(5);
        var passenger2 = new Passenger(10);
        var passenger3 = new Passenger(15);
        elevator.LoadPassenger(passenger1); 
        elevator.LoadPassenger(passenger2);
        elevator.LoadPassenger(passenger3);

        // Act
        await elevator.MoveToNextStopAsync(); // move to 5
        elevator.UnloadPassengersForThisStop(); // unload passenger1

        // Assert
        elevator.CurrentPassengers.Should().NotContain(passenger1);
    }

    [Fact]
    public void ClearPassengers_ShouldRemoveAllPassengers()
    {
        // Arrange
        var elevator = new StdElevator("Elevator1", 10, 0);
        var passenger1 = new Passenger(5);
        var passenger2 = new Passenger(10);
        var passenger3 = new Passenger(15);
        elevator.LoadPassenger(passenger1);
        elevator.LoadPassenger(passenger2);
        elevator.LoadPassenger(passenger3);

        // Act
        elevator.ClearPassengers();

        // Assert
        elevator.CurrentPassengers.Should().BeEmpty();
    }
    #endregion CurrentPassengers

    #region Reset
    [Fact]
    public void Reset_ShouldResetElevatorToInitialState()
    {
        // Arrange
        var elevator = new StdElevator("Elevator1", 10, 0);
        var passenger1 = new Passenger(5);
        var passenger2 = new Passenger(10);
        var passenger3 = new Passenger(15);
        elevator.LoadPassenger(passenger1);
        elevator.LoadPassenger(passenger2);
        elevator.LoadPassenger(passenger3);

        // Act
        elevator.Reset();

        // Assert
        elevator.CurrentFloor.Should().Be(0);
        elevator.NextStop.Should().BeNull();
        elevator.Status.Should().Be(ElevatorStatus.Idle);
        elevator.CurrentPassengers.Should().BeEmpty();
        elevator.FloorStops.Should().BeEmpty();
    }

    #endregion Reset

    [Fact]
    public async void IsMovingTowardFloor_ShouldReturnCorrectly()
    {
        // Arrange
        var elevator = new StdElevator("Elevator1", 10, 5, 0); // start floor is 0
        await elevator.MoveToFloorAsync(5);

        // Act
        var result = elevator.IsMovingTowardFloor(11);

        // Assert
        result.Should().BeTrue();
    }
}
