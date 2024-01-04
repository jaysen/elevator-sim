using Xunit;
using ElevatorSim.Core.Models;
using ElevatorSim.Core.Enums;
using FluentAssertions;
using System.Linq;
using Moq;
using ElevatorSim.Core.Models.Interfaces;

namespace ElevatorSim.Tests.Core.Models;

public class BuildingSimTests
{
    [Fact]
    public void Constructor_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        int floorCount = 10;
        int elevatorCount = 3;
        int defaultCapacity = 5;
        int defaultElevatorSpeed = 1000;

        // Act
        var building = new BuildingSim(floorCount, elevatorCount, defaultCapacity, defaultElevatorSpeed);

        // Assert
        building.FloorCount.Should().Be(floorCount);
        building.ElevatorCount.Should().Be(elevatorCount);
        building.DefaultElevatorCapacity.Should().Be(defaultCapacity);
        building.DefaultElevatorSpeed.Should().Be(defaultElevatorSpeed);
        building.Manager.Elevators.Count.Should().Be(elevatorCount);
        building.Manager.Floors.Count.Should().Be(floorCount+1); // +1 for ground floor
    }

    [Fact]
    public void AddElevator_ShouldAddElevatorToList()
    {
        // Arrange
        var building = new BuildingSim(10, 2, 5, 1000);
        var elevatorMock = new Mock<IElevator>();

        // Act
        building.AddElevatorToSim(elevatorMock.Object);

        // Assert
        building.Manager.Elevators.Should().Contain(elevatorMock.Object);
        building.ElevatorCount.Should().Be(3);
    }

    [Fact]
    public void Setup_WithCustomElevators_ShouldInitializeCorrectly()
    {
        // Arrange
        var customElevators = new[]
        {
            new Mock<IElevator>().Object,
            new Mock<IElevator>().Object
        };
        int defaultCapacity = 5;
        int defaultSpeed = 1000;
        int floorCount = 10;

        // Act
        var sim = new BuildingSim(floorCount, 0, defaultCapacity, defaultSpeed);
        foreach (var elevator in customElevators)
        {
            sim.AddElevatorToSim(elevator);
        }

        // Assert
        sim.ElevatorCount.Should().Be(customElevators.Length);
        sim.FloorCount.Should().Be(floorCount);
        sim.Manager.Elevators.Should().BeEquivalentTo(customElevators);
    }

    #region AddPassenger tests

    [Fact]
    public void AddPassenger_ShouldAddPassengerToFloor_And_ReturnTrue()
    {
        // Arrange
        var building = new BuildingSim(10, 2, 5, 1000);
        int originFloor = 5;
        int destinationFloor = 3;

        // Act
        building.AddPassengersToSim(originFloor, destinationFloor);

        // Assert
        building.Manager.Floors[originFloor].DownQueue
            .Should().ContainSingle().Which
            .Should().BeOfType<Passenger>()
            .Which.DestinationFloor.Should().Be(destinationFloor);
    }
    [Fact]
    public void AddPassenger_ToDestinationFloor_ShouldNotAddPassengerToFloor()
    {
        // Arrange
        var building = new BuildingSim(10, 2, 5, 1000);
        int originFloor = 5;
        int destinationFloor = 5;

        // Act
        building.AddPassengersToSim(originFloor, destinationFloor);

        // Assert
        building.Manager.Floors[originFloor].DownQueue
            .Should().BeEmpty();
        building.Manager.Floors[originFloor].UpQueue
            .Should().BeEmpty();
    }

    [Fact]
    public void AddPassenger_WithDestinationAboveTop_ShouldThrowException()
    {
        // Arrange
        var floorCount = 2;
        var building = new BuildingSim(floorCount, 1, 5, 1000);
        int originFloor = 1;
        int destinationFloor = 3;

        // add passenger should throw exception
        building.Invoking(b => b.AddPassengersToSim(originFloor, destinationFloor))
            .Should().Throw<ArgumentException>()
            .WithMessage($"Destination floor must be between 0 and {floorCount}");
    }
    [Fact]
    public void AddPassenger_WithOriginAboveTop_ShouldThrowException()
    {
        // Arrange
        var floorCount = 2;
        var building = new BuildingSim(floorCount, 1, 5, 1000);
        int badOriginFloor = 3;
        int destinationFloor = 2;

        // add passenger should throw exception
        building.Invoking(b => b.AddPassengersToSim(badOriginFloor, destinationFloor))
            .Should().Throw<ArgumentException>()
            .WithMessage($"Origin floor must be between 0 and {floorCount}");
    }
    [Fact]
    public void AddPassenger_WithNegativeOrigin_ShouldThrowException()
    {
        // Arrange
        var floorCount = 2;
        var building = new BuildingSim(floorCount, 1, 5, 1000);
        int originFloor = -1;
        int destinationFloor = 2;

        // add passenger should throw exception
        building.Invoking(b => b.AddPassengersToSim(originFloor, destinationFloor))
            .Should().Throw<ArgumentException>()
            .WithMessage($"Origin floor must be between 0 and {floorCount}");
    }

    #endregion AddPassenger tests

}
