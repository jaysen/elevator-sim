using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ElevatorSim.Core.Models.Interfaces;

public interface IFloor
{
    int FloorNumber { get; }
    Queue<IPassenger> UpQueue { get; }
    Queue<IPassenger> DownQueue { get; }
    List<IElevator> StoppedElevators { get; }

    void ClearUpQueue();
    void ClearDownQueue();

    void AddPassenger(IPassenger passenger);
    void AddElevatorToStoppedElevators(IElevator elevator);
    void RemoveElevatorFromStoppedElevators(IElevator elevator);
    
    void Reset();
}
