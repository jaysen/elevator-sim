using ElevatorSim.Core.Models.Interfaces;
using ElevatorSim.Core.Enums;
using ElevatorSim.Core.Models;
using Moq;

namespace ElevatorSim.Tests.Core.Models;

public class ElevatorManagerTests
{
    private readonly ElevatorManager _elevatorManager;

    public ElevatorManagerTests()
    {
        _elevatorManager = new ElevatorManager();
    }

    [Fact]
    public void Setup_Should_SetupElevatorsAndFloors()
    {
        // Arrange
        int floorCount = 10;
        int elevatorCount = 2;
        int defaultElevatorCapacity = 10;
        int defaultElevatorSpeed = 5;

        // Act
        bool result = _elevatorManager.Setup(floorCount, elevatorCount, defaultElevatorCapacity, defaultElevatorSpeed);

        // Assert
        result.Should().BeTrue();
        _elevatorManager.Elevators.Should().HaveCount(elevatorCount);
        _elevatorManager.Floors.Should().HaveCount(floorCount);
    }

    [Fact]
    public void AddElevator_Should_AddElevatorToList()
    {
        // Arrange
        var elevatorMock = new Mock<IElevator>();
        ElevatorManager elevatorManager = new ElevatorManager();
        elevatorManager.Setup(10, 0, 5, 1000);

        // Act
        bool result = _elevatorManager.AddElevator(elevatorMock.Object);

        // Assert
        result.Should().BeTrue();
        _elevatorManager.Elevators.Should().Contain(elevatorMock.Object);
        _elevatorManager.Elevators.Should().HaveCount(1);
    }

    [Fact]
    public void Reset_Should_ResetElevatorsAndFloors()
    {
        // Arrange
        int floorCount = 10;
        int elevatorCount = 2;
        int defaultElevatorCapacity = 10;
        int defaultElevatorSpeed = 5;
        _elevatorManager.Setup(floorCount, elevatorCount, defaultElevatorCapacity, defaultElevatorSpeed);

        // Act
        _elevatorManager.Reset();

        // Assert
        _elevatorManager.Elevators.Should().HaveCount(0);
        _elevatorManager.Floors.Should().HaveCount(0);
    }

}
