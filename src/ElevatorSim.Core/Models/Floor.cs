using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorSim.Core.Models.Interfaces;


namespace ElevatorSim.Core.Models
{
    public class Floor : IFloor
    {
        public Floor(int floorNumber)
        {
            FloorNumber = floorNumber;
            UpQueue = new Queue<IPassenger>();
            DownQueue = new Queue<IPassenger>();
            StoppedElevators = new List<IElevator>();
        }

        public int FloorNumber { get; }
        public Queue<IPassenger> UpQueue { get; }
        public Queue<IPassenger> DownQueue { get; }
        public List<IElevator> StoppedElevators { get; }

        public void ClearUpQueue()
        {
            UpQueue.Clear();
        }

        public void ClearDownQueue()
        {
            DownQueue.Clear();
        }

        public bool AddPassenger(IPassenger passenger)
        {
            if (passenger.DestinationFloor > FloorNumber)
            {
                UpQueue.Enqueue(passenger);
            }
            else if (passenger.DestinationFloor < FloorNumber)
            {
                DownQueue.Enqueue(passenger);
            }
            else
            {
                return false;
            }
            return true;
        }

        public void AddElevatorToStoppedElevators(IElevator elevator)
        {
            StoppedElevators.Add(elevator);
        }

        public void RemoveElevatorFromStoppedElevators(IElevator elevator)
        {
            StoppedElevators.Remove(elevator);
        }

        public void Reset()
        {
            ClearUpQueue();
            ClearDownQueue();
            StoppedElevators.Clear();
        }
    }
}
