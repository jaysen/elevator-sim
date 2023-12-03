using ElevatorSim.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSim.Core.Models.Interfaces;

public interface IBuilding
{
    int ElevatorCount { get; }
    int FloorCount { get; }
    int DefaultElevatorCapacity { get; }
    int DefaultElevatorSpeed { get; }

    List<IElevator> Elevators { get; }
    List<IFloor> Floors { get; }
    SortedSet<int> FloorsRequestingUp { get; }
    SortedSet<int> FloorsRequestingDown { get; }

    void Setup(int floorCount, int elevatorCount, int defaultElevatorCapacity, int defaultElevatorSpeed);
    void Setup(int floorCount, List<IElevator> elevators);

    void AddElevator(IElevator elevator);

    void AddPassenger(int originFloor, int destinationFloor);

    void DispatchElevator(int floor, Direction direction);

    void Reset();    


}
