using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSim.Core.Services.Interfaces;
public interface IRollingLog
{
    void Add(string message);
    IEnumerable<string> GetEntries();
    IEnumerable<string> GetLastEntries(int numberOfEntries);
}
