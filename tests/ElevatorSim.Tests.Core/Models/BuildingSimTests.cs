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
        building.Manager.Floors.Count.Should().Be(floorCount);
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

    // Placeholder for AddPassenger test
    // [Fact]
    // public void AddPassenger_ShouldAddPassengerToFloor()
    // {
    //     // Test implementation here
    // }

    // Placeholder for DispatchElevator test
    // [Fact]
    // public void DispatchElevator_ShouldSendElevatorToFloor()
    // {
    //     // Test implementation here
    // }

    // Placeholder for Reset test
    // [Fact]
    // public void Reset_ShouldResetBuildingState()
    // {
    //     // Test implementation here
    // }
}
