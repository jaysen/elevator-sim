using Xunit;
using ElevatorSim.Core.Models;
using ElevatorSim.Core.Enums;
using FluentAssertions;
using System.Linq;
using ElevatorSim.Core.Models.Interfaces;
using Microsoft.VisualStudio.CodeCoverage;
using ElevatorSim.Core.Services.Interfaces;

namespace ElevatorSim.Tests.Integrations;

public class SimDispatchingForPassengers
{
    public BuildingSim Sim { get; private set; }
    public void SetupSim(int floors, int elevators, int capacity, int speed)
    {
        Sim = new(floors, elevators, capacity, speed);
    }

    #region AddPassenger tests

    [Fact]
    public void AddPassengers_WithSecondGoingOtherDirection_Should_DispatchCorrectly()
    {
        // Arrange
        SetupSim(10, 2, 5, 1000);
        int originFloor1 = 1;
        int destinationFloor1 = 6;
        int originFloor2 = 4;
        int destinationFloor2 = 2;

        // Act
        Sim.AddPassengersToSim(originFloor1, destinationFloor1);
        Sim.AddPassengersToSim(originFloor2, destinationFloor2);

        // Assert
        Sim.Manager.Elevators.Count.Should().Be(2, "because there are two elevators in the simulation");

        var elevatorGoingToFirstFloor = Sim.Manager.Elevators.FirstOrDefault(e => e.FloorStops.Contains(originFloor1));
        var elevatorGoingToSecondFloor = Sim.Manager.Elevators.FirstOrDefault(e => e.FloorStops.Contains(originFloor2));

        elevatorGoingToFirstFloor.Should().NotBeNull("because one elevator should be going to the first floor");
        elevatorGoingToSecondFloor.Should().NotBeNull("because one elevator should be going to the fourth floor");
        elevatorGoingToFirstFloor.Should().NotBe(elevatorGoingToSecondFloor, "because the elevators should be different");

        elevatorGoingToFirstFloor.FloorStops.Should().Contain(originFloor1, "because the elevator should have the first floor in its stops");
        elevatorGoingToSecondFloor.FloorStops.Should().Contain(originFloor2, "because the elevator should have the fourth floor in its stops");


    }

    #endregion AddPassenger tests

}
