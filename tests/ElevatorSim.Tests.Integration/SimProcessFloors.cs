using Xunit;
using ElevatorSim.Core.Models;
using ElevatorSim.Core.Enums;
using FluentAssertions;
using System.Linq;
using ElevatorSim.Core.Models.Interfaces;
using Microsoft.VisualStudio.CodeCoverage;
using ElevatorSim.Core.Services.Interfaces;

namespace ElevatorSim.Tests.Integrations;

public class SimProcessingFloorStops
{
    internal BuildingSim? Sim;

    internal void SetupSim(int floors, int elevators, int capacity, int speed)
    {
        Sim = new(floors, elevators, capacity, speed);
    }

    [Fact]
    public async Task PassengerAtLastDestinationGoingTheOtherWay_Should_CorrectlyProcessFloor()
    {
        // Arrange
        SetupSim(25, 2, 5, 1000);
        Sim.SetElevatorFloor(1, 20); // Elevator 1 starts at floor 20
        Sim.AddPassengersToSim(19, 21);

        // Act
        await Sim.MoveElevators();


        // Assert
        var elevator1 = Sim.Manager.Elevators.FirstOrDefault(e => e.CurrentFloor == 21);
        elevator1.Should().NotBeNull("because one elevator should end at floor 21");
    }

}
