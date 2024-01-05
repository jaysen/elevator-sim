using ElevatorSim.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSim.Core.Models.Interfaces;

public interface IBuildingSim
{
    int ElevatorCount { get; }
    int FloorCount { get; }
    int DefaultElevatorCapacity { get; }
    int DefaultElevatorSpeed { get; }

    void AddElevatorToSim(IElevator elevator);   
    void SetElevatorFloor(int elevatorNum, int floorNum);
    void AddPassengersToSim(int originFloor, int destinationFloor, int passengerCount = 1);
    void ResetSim();

    bool AnyElevatorsMoving { get; }

    Task MoveElevators();
    
    IElevatorManager Manager { get; }

}
