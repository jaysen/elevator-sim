using ElevatorSim.Core.Models.Interfaces;
using ElevatorSim.Core.Enums;
using ElevatorSim.Core.Models;
using Moq;

namespace ElevatorSim.Tests.Core.Models;

public class ElevatorManagerTests
{
    private readonly ElevatorManager _manager;

    public ElevatorManagerTests()
    {
        _manager = new ElevatorManager();
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
        bool result = _manager.Setup(floorCount, elevatorCount, defaultElevatorCapacity, defaultElevatorSpeed);

        // Assert
        result.Should().BeTrue();
        _manager.Elevators.Should().HaveCount(elevatorCount);
        _manager.Floors.Should().HaveCount(floorCount);
    }

    [Fact]
    public void AddElevator_Should_AddElevatorToList()
    {
        // Arrange
        var elevatorMock = new Mock<IElevator>();
        ElevatorManager elevatorManager = new ElevatorManager();
        elevatorManager.Setup(10, 0, 5, 1000);

        // Act
        bool result = _manager.AddElevator(elevatorMock.Object);

        // Assert
        result.Should().BeTrue();
        _manager.Elevators.Should().Contain(elevatorMock.Object);
        _manager.Elevators.Should().HaveCount(1);
    }

    [Fact]
    public void Reset_Should_ResetElevatorsAndFloors()
    {
        // Arrange
        int floorCount = 10;
        int elevatorCount = 2;
        int defaultElevatorCapacity = 10;
        int defaultElevatorSpeed = 5;
        _manager.Setup(floorCount, elevatorCount, defaultElevatorCapacity, defaultElevatorSpeed);

        // Act
        _manager.Reset();

        // Assert
        _manager.Elevators.Should().HaveCount(0);
        _manager.Floors.Should().HaveCount(0);
    }


    [Fact]
    public async Task MoveElevatorToFloorAsync_Should_MoveElevatorToFloor()
    {
        // Arrange
        int floorCount = 10;
        int elevatorCount = 2;
        int defaultElevatorCapacity = 10;
        int defaultElevatorSpeed = 0;
        _manager.Setup(floorCount, elevatorCount, defaultElevatorCapacity, defaultElevatorSpeed);
        var elevator = _manager.Elevators[0];

        // Act
        await _manager.MoveElevatorToFloorAsync(elevator, 5);

        // Assert
        elevator.CurrentFloor.Should().Be(5);
    }

    [Fact]
    public void AddPassengerToFloor_Should_AddPassengerToFloor_AndReturnTrue()
    {
        // Arrange
        int floorCount = 10;
        int elevatorCount = 2;
        int defaultElevatorCapacity = 10;
        int defaultElevatorSpeed = 0;
        _manager.Setup(floorCount, elevatorCount, defaultElevatorCapacity, defaultElevatorSpeed);
        var floor = _manager.Floors[0];

        // Act
        bool result = _manager.AddPassengerToFloor(0, 5);

        // Assert
        result.Should().BeTrue();
        floor.UpQueue.Should().HaveCount(1);
        floor.UpQueue.First().DestinationFloor.Should().Be(5);
    }

    #region Elevator Dispatch
    [Fact]
    public async Task GetBestElevatorToDispatch_WhenAllIdle_Should_ReturnCorrectly()
    {
        // Arrange
        int floorCount = 10;
        int elevatorCount = 2;
        int defaultElevatorCapacity = 10;
        int defaultElevatorSpeed = 0;
        _manager.Setup(floorCount, elevatorCount, defaultElevatorCapacity, defaultElevatorSpeed);
        var elevator1 = _manager.Elevators[0];
        var elevator2 = _manager.Elevators[1];

        // Act
        await elevator1.MoveToFloorAsync(5);
        await elevator2.MoveToFloorAsync(10);

        // Assert
        var testElevator = _manager.GetBestElevatorToDispatch(0, Direction.Up);
        testElevator.Should().BeEquivalentTo(elevator1);
    }

    [Fact]
    public async Task GetBestElevatorToDispatch_WhenAllMovingToward_Should_ReturnCorrectly()
    {
        // Arrange
        _manager.Reset();
        int floorCount = 30;
        int elevatorCount = 3;
        int defaultElevatorCapacity = 10;
        int defaultElevatorSpeed = 10;
        _manager.Setup(floorCount, elevatorCount, defaultElevatorCapacity, defaultElevatorSpeed);
        var elevator1 = _manager.Elevators[0];
        var elevator2 = _manager.Elevators[1];
        //var elevator3 = _manager.Elevators[2];

        // Act
        elevator1.AddFloorStop(4);
        await elevator1.MoveToNextStopAsync();
        elevator1.AddFloorStop(16);
        elevator2.AddFloorStop(15);
        var task1 = elevator1.MoveToNextStopAsync();
        var task2 = elevator2.MoveToNextStopAsync();
        var testElevator = _manager.GetBestElevatorToDispatch(20, Direction.Up);
        await Task.WhenAll(task1, task2);

        // Assert
        testElevator.Should().NotBeNull();
        testElevator.Name.Should().Be(elevator1.Name);
    }

    [Fact]
    public async Task GetBestElevatorToDispatch_WhenSomeMovingToward_Should_ReturnFastest()
    {
        // Arrange
        int floorCount = 30;
        int elevatorCount = 3;
        int defaultElevatorCapacity = 10;
        int defaultElevatorSpeed = 10;
        _manager.Setup(floorCount, elevatorCount, defaultElevatorCapacity, defaultElevatorSpeed);
        var elevator1 = _manager.Elevators[0];
        var elevator2 = _manager.Elevators[1];
        var elevator3 = _manager.Elevators[2];
        await Task.WhenAll(elevator1.MoveToFloorAsync(1), elevator3.MoveToFloorAsync(6));
        
        // Act
        var task1 = elevator1.MoveToFloorAsync(10);
        var task2 = elevator2.MoveToFloorAsync(10);
        var testElevator = _manager.GetBestElevatorToDispatch(11, Direction.Up);

        // Assert
        testElevator.Should().Be(elevator3);
        await Task.WhenAll(task1, task2);


    }

    [Fact]
    public async Task DispatchElevatorToFloorAsync_Should_DispatchElevatorToFloor()
    {
        // Arrange
        int floorCount = 10;
        int elevatorCount = 2;
        int defaultElevatorCapacity = 10;
        int defaultElevatorSpeed = 0;
        _manager.Setup(floorCount, elevatorCount, defaultElevatorCapacity, defaultElevatorSpeed);
        var elevator = _manager.Elevators[0];

        // Act
        await _manager.DispatchElevatorToFloorAsync(0, Direction.Up);

        // Assert
        elevator.CurrentFloor.Should().Be(0);
    }

    #endregion GetBestElevatorToDispatch


}
