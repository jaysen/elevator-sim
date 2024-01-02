using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorSim.Core.Enums;

namespace ElevatorSim.Core.Models.Interfaces;

public interface IElevator
{
    string Name { get; }
    int CapacityLimit { get; }
    int TimeBetweenFloors { get; } // in milliseconds - how long it takes to move between a floor
    int CurrentFloor { get; }
    int? NextStop { get; }
    ElevatorStatus Status { get; }
    Direction Direction { get; }
    List<IPassenger> CurrentPassengers { get; }
    SortedSet<int> FloorStops { get; }

    event EventHandler<ElevatorStopEventArgs> StoppedAtFloor;

    void SetFloor(int floorNum);
    Task MoveAsync();
    Task MoveToNextStopAsync();
    Task MoveToFloorAsync(int floorNum, ElevatorStatus endStatus = ElevatorStatus.Idle);
    bool IsMovingTowardFloor(int floorNum);

    void AddFloorStop(int floor);
    void RemoveFloorStop(int floor);

    bool LoadPassenger(IPassenger passenger);
    void UnloadPassengersForThisStop();
    
    void ClearPassengers();
    void ClearFloorStops();

    void Reset();


}
