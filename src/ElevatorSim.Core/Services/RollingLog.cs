using ElevatorSim.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSim.Core.Services;

/// <summary>
/// this Rolling Log is for transient, user-facing messages.
/// </summary>
public class RollingLog(int maxEntries = 10) : IRollingLog
{
    private readonly Queue<string> _logEntries = new Queue<string>();
    private readonly int _maxEntries = maxEntries;

    public void Add(string message)
    {
        lock (_logEntries)
        {
            _logEntries.Enqueue(message);
            if (_logEntries.Count > _maxEntries)
            {
                _logEntries.Dequeue();
            }
        }
    }

    public IEnumerable<string> GetEntries()
    {
        lock (_logEntries)
        {
            return _logEntries.ToList();
        }
    }

    // New method to get the last 'n' entries
    public IEnumerable<string> GetLastEntries(int numberOfEntries)
    {
        lock (_logEntries)
        {
            // If the requested number of entries is more than available, return all entries
            if (numberOfEntries >= _logEntries.Count)
            {
                return _logEntries.ToList();
            }
            else
            {
                // Skip the older entries and take the last 'numberOfEntries' entries
                return _logEntries.Skip(_logEntries.Count - numberOfEntries).ToList();
            }
        }
    }
}
