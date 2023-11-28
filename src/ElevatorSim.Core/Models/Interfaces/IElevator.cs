using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSim.Core.Models.Interfaces;

public interface IElevator
{
    int Id { get; }
    string Name { get; }
    
    int CapacityLimit { get; }

    int CurrentPassengers { get; }
    int CurrentFloor { get; }
    Enums.ElevatorStatus Status { get; }
    Enums.Direction Direction { get; }

    SortedSet<int> FloorStops { get; }
    
    void MoveToNextStop();
    void MoveToFloor(int floor);
    void AddFloorStop(int floor);
    void RemoveFloorStop(int floor);

    void LoadPassenger(IPassenger passenger);
    void UnloadPassengersForThisStop();
    
    void ClearPassengers();
    void ClearFloorStops();

}
