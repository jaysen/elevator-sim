using ElevatorSim.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSim.Core.Services.Interfaces;
public interface IBuildingSimFactory
{
    IBuildingSim Create(int floorCount, int elevatorCount, int defaultCapacity, int defaultSpeed);
}
