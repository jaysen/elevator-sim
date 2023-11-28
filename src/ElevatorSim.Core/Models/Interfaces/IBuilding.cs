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

    List<IElevator> Elevators { get; }
    List<IFloor> Floors { get; }

    void AddPassenger(IFloor origin, IFloor destination);

    SortedSet<int> FloorsRequestingUp { get; }
    SortedSet<int> FloorsRequestingDown { get; }

    void AddElevatorFloorStop(IElevator elevator, int floor);
    void RemoveElevatorFloorStop(IElevator elevator, int floor);
    void MoveElevator(IElevator elevator, int floor);


    void ClearElevatorFloorStops(IElevator elevator);
    void ClearAllRequestedFloorStops();
    void Reset();    
}
