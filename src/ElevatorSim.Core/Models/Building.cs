using ElevatorSim.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorSim.Core.Models.Interfaces;

namespace ElevatorSim.Core.Models
{
    public class Building : IBuilding
    {
        public Building(int floorCount, int elevatorCount, int defaultCapacity, int defaultElevatorSpeed)
        {
            Elevators = new List<IElevator>();
            Floors = new List<IFloor>();
            Setup(floorCount, elevatorCount, defaultCapacity, defaultElevatorSpeed);
            FloorsRequestingUp = new SortedSet<int>();
            FloorsRequestingDown = new SortedSet<int>();
            DefaultElevatorCapacity = defaultCapacity;
            DefaultElevatorSpeed = defaultElevatorSpeed;
        }

        public Building(int floorCount, List<IElevator> elevators)
        {
            Elevators = new List<IElevator>();
            Floors = new List<IFloor>();
            Setup(floorCount, elevators);
            FloorsRequestingUp = new SortedSet<int>();
            FloorsRequestingDown = new SortedSet<int>();
        }

        public int ElevatorCount { get; private set; }
        public int FloorCount { get; private set; }
        public int DefaultElevatorCapacity { get; private set; }
        public int DefaultElevatorSpeed { get; private set; }
        public List<IElevator> Elevators { get; private set; }
        public List<IFloor> Floors { get; private set; }
        public SortedSet<int> FloorsRequestingUp { get; private set; }
        public SortedSet<int> FloorsRequestingDown { get; private set; }

        public void Setup(int floorCount, int elevatorCount, int defaultElevatorCapacity = 10, int defaultElevatorSpeed = 1000)
        {
            ElevatorCount = elevatorCount;
            FloorCount = floorCount;

            for (int i = 0; i < floorCount; i++)
            {
                Floors.Add(new Floor(i));
            }

            for (int i = 0; i < elevatorCount; i++)
            {
                var name = $"Elevator {i}";
                Elevators.Add(new StdElevator(name, defaultElevatorCapacity, defaultElevatorSpeed));
            }
        }

        public void Setup(int floorCount, List<IElevator> elevators)
        {
            ElevatorCount = elevators.Count;
            Elevators = elevators;

            FloorCount = floorCount;
            for (int i = 0; i < floorCount; i++)
            {
                Floors.Add(new Floor(i));
            }
        }

        public void AddElevator(IElevator elevator)
        {
            Elevators.Add(elevator);
            ElevatorCount++;
        }

        public void AddPassenger(int originFloor, int destinationFloor)
        {
            // Implementation here
        }

        public void DispatchElevator(int floor, Direction direction)
        {
            // Implementation here
        }

        public void Reset()
        {
            // Implementation here
        }
    }
}
