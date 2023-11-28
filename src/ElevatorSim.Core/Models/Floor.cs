
using ElevatorSim.Core.Models.Interfaces;

namespace ElevatorSim.Core.Models;

public class Floor : IFloor
{
    public Queue<IPassenger> UpQueue { get; set; } = new Queue<IPassenger>();
    public Queue<IPassenger> DownQueue { get; set; } = new Queue<IPassenger>();

    public int FloorNumber { get; }

    public void AddPassengerRequest(IPassenger passenger)
    {
        if (passenger.DestinationFloor > FloorNumber)
            UpQueue.Enqueue(passenger);
        else if (passenger.DestinationFloor < FloorNumber)
            DownQueue.Enqueue(passenger);
    }

    public void ClearUpQueue()
    {
        UpQueue.Clear();
    }

    public void ClearDownQueue()
    {
        DownQueue.Clear();
    }

    public void ClearQueues()
    {
        UpQueue.Clear();
    }

}
