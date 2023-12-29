using ElevatorSim.Core.Models.Interfaces;
using ElevatorSim.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorSim.Core.Services.Interfaces;

namespace ElevatorSim.Core.Services;
public class StdBuildingSimFactory : IBuildingSimFactory
{
    public IBuildingSim Create(int floorCount, int elevatorCount, int defaultCapacity, int defaultSpeed)
    {
        return new BuildingSim(floorCount, elevatorCount, defaultCapacity, defaultSpeed);
    }
}
