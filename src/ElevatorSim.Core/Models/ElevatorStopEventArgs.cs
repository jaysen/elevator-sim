using ElevatorSim.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSim.Core.Models;
public class ElevatorStopEventArgs : EventArgs
{
    public int FloorNumber { get; set; }
    public Direction Direction { get; set; }

}