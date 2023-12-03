using ElevatorSim.Core.Models;
using ElevatorSim.Core.Models.Interfaces;
using Moq;

namespace ElevatorSim.Tests.Core.Models;

public class FloorTests
{
    [Fact]
    public void Constructor_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        int floorNumber = 5;

        // Act
        var floor = new Floor(floorNumber);

        // Assert
        floor.FloorNumber.Should().Be(floorNumber);
        floor.UpQueue.Should().BeEmpty();
        floor.DownQueue.Should().BeEmpty();
        floor.StoppedElevators.Should().BeEmpty();
    }

    [Fact]
    public void AddPassenger_ToUpQueue_ShouldAddPassengerWhenDestinationIsHigher()
    {
        // Arrange
        var floor = new Floor(3);
        var passenger = new Passenger(5); // Destination higher than floor number

        // Act
        floor.AddPassenger(passenger);

        // Assert
        floor.UpQueue.Should().ContainSingle().Which.Should().Be(passenger);
    }

    [Fact]
    public void AddPassenger_ToDownQueue_ShouldAddPassengerWhenDestinationIsLower()
    {
        // Arrange
        var floor = new Floor(5);
        var passenger = new Passenger(3); // Destination lower than floor number

        // Act
        floor.AddPassenger(passenger);

        // Assert
        floor.DownQueue.Should().ContainSingle().Which.Should().Be(passenger);
    }

    [Fact]
    public void AddPassenger_SameFloor_ShouldNotAddPassenger()
    {
        // Arrange
        var floor = new Floor(5);
        var passenger = new Passenger(5); // Destination lower than floor number

        // Act
        floor.AddPassenger(passenger);

        // Assert
        floor.UpQueue.Should().BeEmpty();
        floor.DownQueue.Should().BeEmpty();
    }

    [Fact]
    public void ClearUpQueue_ShouldEmptyUpQueue()
    {
        // Arrange
        var floor = new Floor(3);
        floor.AddPassenger(new Passenger(5));
        floor.AddPassenger(new Passenger(7));

        // Act
        floor.ClearUpQueue();

        // Assert
        floor.UpQueue.Should().BeEmpty();
    }

    [Fact]
    public void ClearDownQueue_ShouldEmptyDownQueue()
    {
        // Arrange
        var floor = new Floor(5);
        floor.AddPassenger(new Passenger(3));
        floor.AddPassenger(new Passenger(1));

        // Act
        floor.ClearDownQueue();

        // Assert
        floor.DownQueue.Should().BeEmpty();
    }

    [Fact]
    public void AddElevatorToStoppedElevators_ShouldAddElevator()
    {
        // Arrange
        var floor = new Floor(3);
        var elevatorMock = new Mock<IElevator>();

        // Act
        floor.AddElevatorToStoppedElevators(elevatorMock.Object);

        // Assert
        floor.StoppedElevators.Should().ContainSingle().Which.Should().Be(elevatorMock.Object);
    }

    [Fact]
    public void RemoveElevatorFromStoppedElevators_ShouldRemoveElevator()
    {
        // Arrange
        var floor = new Floor(3);
        var elevatorMock = new Mock<IElevator>();
        floor.AddElevatorToStoppedElevators(elevatorMock.Object);

        // Act
        floor.RemoveElevatorFromStoppedElevators(elevatorMock.Object);

        // Assert
        floor.StoppedElevators.Should().BeEmpty();
    }

    [Fact]
    public void Reset_ShouldClearAllQueuesAndStoppedElevators()
    {
        // Arrange
        var floor = new Floor(3);
        floor.AddPassenger(new Passenger(5));
        floor.AddPassenger(new Passenger(2));
        var elevatorMock = new Mock<IElevator>();
        floor.AddElevatorToStoppedElevators(elevatorMock.Object);

        // Act
        floor.Reset();

        // Assert
        floor.UpQueue.Should().BeEmpty();
        floor.DownQueue.Should().BeEmpty();
        floor.StoppedElevators.Should().BeEmpty();
    }
}
