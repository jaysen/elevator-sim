using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorSim.Core.Models;

namespace ElevatorSim.Core.Models.Interfaces;

public interface IFloor
{
    Queue<IPassenger> UpQueue { get; }
    Queue<IPassenger> DownQueue { get; }

    int FloorNumber { get; }

    void ClearUpQueue();
    void ClearDownQueue();
    void ClearQueues();


}
